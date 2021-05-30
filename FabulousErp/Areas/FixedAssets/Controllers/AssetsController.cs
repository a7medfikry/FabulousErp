using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.Models;
using FabulousErp.Controllers.API;

namespace FixedAssets.Controllers
{
    public class AssetsController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext MDB = new DBContext();
        // GET: Assets
        public ActionResult Index()
        {
            List<Asset> assets = db.Assets.Include(a => a.Assets_class).Include(a => a.Assets_main)
                .ToList().Select(x=> {
                    x.Acquisation_cost = x.Fixed_assets_revaluate.ToList().DefaultIfEmpty(new Fixed_assets_revaluate { Adjustment_cost = x.Acquisation_cost }).ToList().LastOrDefault().Adjustment_cost.Value;
                    return x;
                }).ToList();

            ViewBag.Count = db.Books.Count();
            ViewBag.Error = TempData["Error"];
            try
            {
                foreach (Asset A in assets)
                {
                    try
                    {
                        A.Transaction_date = Convert.ToDateTime(MDB.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == A.New_assets_transaction.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_TransactionDate = DateTime.Now.ToShortDateString().ToString() })
                  .FirstOrDefault().C_TransactionDate);

                    }
                    catch
                    {

                    }
                  
                }
                

            }
            catch
            {

            }
            return View(assets.ToList());
        }

        // GET: Assets/Details/5
        public ActionResult Details(int? id,bool IsNumber=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);

            if (IsNumber)
            {
                asset = db.Assets.FirstOrDefault(x => x.Assets_number == id.ToString());
            }
            if (asset == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = asset.Id;
            return View(asset);
        }

        // GET: Assets/Create

   
        public ActionResult GetAssetWithNumberOfUnit(DateTime TransactionDate)
        {
            ViewBag.Period = new SelectList(db.Deprecation_periods, "Id", "text");
            int Depreication_method = (int)Deprecation_method.Number_of_units;
            if (db.Assets.Any(x => x.Deprecation_method == Depreication_method))
            {
                List<Number_of_deprecation_units> AssetsDepRecation = db.Assets.Where(x => x.Deprecation_method == Depreication_method
                &&x.Start_use<= TransactionDate).Select(x => new Number_of_deprecation_units
                {
                    Asset_id = x.Id,
                    Asset_description = x.Description,
                    Deprecation_unit = 0
                }).ToList();
                if (AssetsDepRecation.Any())
                {
                    return View(AssetsDepRecation);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAssetsClass(int Id)
        {
            Assets_class ThisClass = db.Assets_class.Find(Id);
            return Json(new { Deprecation_method = ThisClass.Deprecation_method, Deprecation_rate = ThisClass.Deperecation_rate });
        }
        public JsonResult GetDebitCreditAccount(int Id)
        {
            try
            {
                return Json(db.Assets.Where(x => x.Id == Id).Select(x => new { Debit = x.Assets_class.Assets_accounts.FirstOrDefault().Cost_account, Credit = x.Assets_class.Assets_accounts.FirstOrDefault().Accrued }));
            }
            catch
            {
                return Json("");
            }
        }
       public JsonResult CheckAssets_number(string Assets_number)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Assets_number))
                {
                    return Json(db.Assets.Any(x => x.Assets_number == Assets_number));
                }
                else
                {
                    return Json(false);
                }
            }
            catch
            {
                return Json(false);
            }
        }
        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult ValidateCreate(Asset asset)
        {
            ModelState["Assets_transaction_id"].Errors.Clear();
            if (!ModelState.IsValid)
            {
                string Errors = string.Join(",", ModelState.Where(x => x.Value.Errors.Count() > 0).ToList().Select(x => x.Value.Errors.FirstOrDefault().ErrorMessage));
                return Json(Errors);
            }
            else
            {
                return Json(1);
            }
        }
        public JsonResult CheckSerialNumber(List<Assets_part_serial> PartsSerial)
        {
            try
            {
                if (!PartsSerial.Any())
                {
                    return Json(false);
                }
                else
                {
                    bool SerialExist = false;
                    foreach (Assets_part_serial i in PartsSerial.Where(x => x.Serial != null))
                    {
                        if (PartsSerial.ToArray().Any(x => x.Serial == i.Serial && x.Id != i.Id))
                        {
                            ViewBag.ErrRes += $"Serial {i.Serial} is Duplicated  ";
                            SerialExist = true;
                        }
                        else if(db.Assets_part_serial.Any(x => x.Serial == i.Serial && x.Id != i.Id))
                        {
                            ViewBag.ErrRes += $"Serial {i.Serial} Already Exist ";
                            SerialExist = true;
                        }
                    }
                    return Json(new { status = SerialExist, msg = ViewBag.ErrRes });
                }
            }
            catch
            {
                return Json(false);
            }
        }
        public ActionResult Create(int? Id)
        {
            if (!db.Books.Any())
            {
                TempData["Error"] = "There are no Book you can't create assets";
                return RedirectToAction("Index");
            }
            ViewBag.Purchase_types = new SelectList(Enum.GetValues(typeof(Purchase_type)).Cast<Purchase_type>().ToList().
                Select(x => new { Id = (int)x, Value = x.ToString() }), "Id", "Value");

            ViewBag.Assets_transaction_id = new SelectList(Enumerable.Empty<New_assets_transaction>().ToList(),"Id", "Reference");
            #region detectJEPer
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            if (string.IsNullOrEmpty(companyID))
            {
                return Redirect("/");
            }
            using (FabulousDB.DB_Context.DBContext DB = new FabulousDB.DB_Context.DBContext())
            {
                var detectJEPer =FabulousErp.Business.GetPostingSetup();
                //check Journal entry Per Transaction or Batch

                if (detectJEPer != null)
                {
                    ViewBag.FJEPer = "B1";//detectJEPer.CreateJEPer;
                    ViewBag.EPD = detectJEPer.EditPostingDate;
                    detectJEPer.CreateJEPer = "";
                    if (detectJEPer.CreateJEPer == "B2")
                    {
                        //ViewBag.BatchAction = detectJEPer.ExistingBatch;
                        ViewBag.PostDateType = detectJEPer.PostingDataFrom;
                    }
                }
                else
                {
                    ViewBag.FJEPer = "NoPS";
                }
                ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();
                ViewBag.FJEPer = "NoPS";

            }
            ViewBag.FromTCGE = false;
            #endregion

            ViewBag.Assets_class_id = new SelectList(db.Assets_class.Where(x => x.Active == false), "Id", "Description");
            ViewBag.Assets_main_id = new SelectList(db.Assets_main.Where(x => x.Inactive == false), "Id", "Assets_number");
            ViewBag.Book_id = new SelectList(db.Books, "Id", "Description");
            if (Request["Setting"] == "Edit" || Request["Setting"] == null)
            {
                ViewBag.Assets_transaction_id = null;
            }
            try
            {
                ViewBag.CanAddTransaction = db.Deprecation_Setting.FirstOrDefault().Can_add_assets_info;
            }
            catch
            {
                ViewBag.CanAddTransaction = false;
            }
            return View(new Asset {Date_of_orgin=DateTime.Now,Start_use=DateTime.Now,Start_derecation_date=DateTime.Now,Use_life=DateTime.Now });
        }

        public JsonResult GetTransactions(Purchase_type P)
        {
            if (P == Purchase_type.Direct)
            {
                return Json(db.New_assets_transaction.Include(x => x.Assets).Where(x => !x.Assets.Any() && x.IsVoid == false).ToList().Select(x => new { x.Id, x.Reference }));
            }
            else if (P == Purchase_type.Payable)
            {
                using (DBContext dbP = new DBContext())
                {
                    return Json(dbP.Payable_transactions.Where(x=>x.Fixed_assets_trx==true).ToList().Select(x => new { x.Id, Reference= x.Desc }));
                }
            }
            return Json("");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Asset asset, List<Assets_part_serial> PartsSerial)
        {
            if (ModelState.IsValid)
            {
                decimal Transaction_Acquesation_cost = db.New_assets_transaction.Where(x => x.Id == asset.Assets_transaction_id).ToList().DefaultIfEmpty(new New_assets_transaction { Acquesation_cost = asset.Acquisation_cost }).FirstOrDefault().Acquesation_cost.Value;
                asset.Acquisation_cost = Transaction_Acquesation_cost;
                if (string.IsNullOrWhiteSpace(asset.Assets_number))
                {
                    int Count = 1;
                    asset.Assets_number = (db.Assets.Count() + Count).ToString();
                    while (db.Assets.Any(x => x.Assets_number == asset.Assets_number))
                    {
                        asset.Assets_number = (db.Assets.Count() + Count).ToString();
                        Count++;
                    }
                }
                if (asset.Purchase_types == (int)Purchase_type.Payable)
                {
                    asset.Payable_Inv_trans = asset.Assets_transaction_id;
                    asset.Assets_transaction_id = null;
                }
                asset.Creation_date = DateTime.Now;
                db.Assets.AddOrUpdate(asset);
                db.SaveChanges();
               
                
               
                if (PartsSerial != null)
                {
                    PartsSerial = PartsSerial.Where(x => x.Serial != null).ToList();
                    PartsSerial.ForEach(x => x.Assets_id = asset.Id);
                    PartsSerial.Where(x => string.IsNullOrWhiteSpace(x.Serial)).ToList().ForEach(x => x.Serial = null);
                    db.Assets_part_serial.AddRange(PartsSerial);
                    if (PartsSerial.Any())
                    {
                        foreach (Assets_part_serial i in PartsSerial)
                        {
                            db.Stoking_assets.Add(new Stoking_assets
                            {
                                Assets_id = asset.Id,
                                Assets_class_id = asset.Assets_class_id,
                                Company_id = (string)FabulousErp.Business.GetCompanyId(),
                                Serial = i.Serial,
                                Added_date = DateTime.Now
                            });
                        }
                    }
                    else
                    {
                        db.Stoking_assets.Add(new Stoking_assets
                        {
                            Assets_id = asset.Id,
                            Assets_class_id = asset.Assets_class_id,
                            Company_id = (string)FabulousErp.Business.GetCompanyId(),
                            Added_date = DateTime.Now
                        });
                    }
                   
                }
                else
                {
                    if (!db.Stoking_assets.Any(x => x.Assets_id == asset.Id))
                    {
                        db.Stoking_assets.Add(new Stoking_assets
                        {
                            Assets_id = asset.Id,
                            Assets_class_id = asset.Assets_class_id,
                            Company_id = (string)FabulousErp.Business.GetCompanyId(),
                                Added_date = DateTime.Now

                        });
                    }
                }
                db.SaveChanges();


                return Json(asset.Assets_number);
            }
            string Errors = string.Join(",", ModelState.Where(x => x.Value.Errors.Count() > 0).ToList().Select(x => x.Value.Errors.FirstOrDefault().ErrorMessage));

            MDB.Exceptions_Tables.Add(new FabulousDB.DB_Tabels.Important.Exceptions_Table
            {
                Exception = Errors,
                URL="/FixedAssets/Assets/Create"
            });
            return Json(Errors);
        }
        public ActionResult GetAssetsParts(int? id)
        {
            try
            {
                return Json(db.Assets.Find(id).Assets_part_serial);

            }
            catch
            {
                return Json(0);
            }
        }
        // GET: Assets/Edit/5
        public ActionResult Edit(int? id,bool IsPartial=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsPartial = IsPartial;
            ViewBag.Assets_class_id = new SelectList(db.Assets_class.Where(x => x.Active == false), "Id", "Description", asset.Assets_class_id);
            ViewBag.Assets_main_id = new SelectList(db.Assets_main.Where(x => x.Inactive == false), "Id", "Assets_number", asset.Assets_main_id);
            ViewBag.Book_id = new SelectList(db.Books, "Id", "Description", asset.Book_id);

            return View(asset);
        }


        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Asset asset, List<Assets_part_serial> PartsSerial)
        {
            ModelState["Assets_transaction_id"].Errors.Clear();
            if (ModelState.IsValid)
            {
                Asset DbAssets= db.Assets.Find(asset.Id);
                DbAssets.Description = asset.Description;
                DbAssets.Foreign_name = asset.Foreign_name;
                DbAssets.Book_id = asset.Book_id;
                DbAssets.Scrap_value = asset.Scrap_value;
                if (PartsSerial != null)
                {
                    PartsSerial.Where(x => string.IsNullOrWhiteSpace(x.Serial)).ToList().ForEach(x => x.Serial = null);

                    foreach (Assets_part_serial i in PartsSerial)
                    {
                        if (i.Id == 0)
                        {
                            db.Assets_part_serial.Add(i);
                        }
                        else
                        {
                            db.Entry(i).State = EntityState.Modified;
                            db.Entry(i).Property(x => x.Assets_id).IsModified = false;
                        }
                    }
                }
                //db.Entry(DbAssets).State = EntityState.Unchanged;
                //db.Entry(DbAssets).Property(x => x.Description).IsModified = true;
                //db.Entry(DbAssets).Property(x => x.Book_id).IsModified = true;
                //db.Entry(DbAssets).Property(x => x.Foreign_name).IsModified = true;
                //db.Entry(DbAssets).Property(x => x.Scrap_value).IsModified = true;
                //if (db.Deprecation_Setting.FirstOrDefault().Change_deprecation_method)
                //{
                //    db.Entry(asset).Property("Book_id").IsModified = true;
                //}
                db.SaveChanges();

                return Json(asset.Id);
            }
            else
            {
               
                string Error =string.Join(" ", ModelState.Where(x => x.Value.Errors.Count() > 0)
                    .Select(x => x.Value.Errors.FirstOrDefault().ErrorMessage).ToList());
                ModelState.AddModelError(string.Empty, Error);
            }
            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description", asset.Assets_class_id);
            ViewBag.Assets_main_id = new SelectList(db.Assets_main, "Id", "Description", asset.Assets_main_id);
            ViewBag.Book_id = new SelectList(db.Books, "Id", "Description");

            return View(asset);
        }

        // GET: Assets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            if (asset.Assets_transaction_id.HasValue)
            {
                ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();
                ViewBag.PostingNum = MDB.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == asset.New_assets_transaction.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_PostingNumber = 0 })
                    .FirstOrDefault().C_PostingNumber;
                ViewBag.Currency = asset.New_assets_transaction.Currency_id;

                ViewBag.TransactionDate = MDB.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == asset.New_assets_transaction.Gl_transaction_id).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { C_TransactionDate = DateTime.Now.ToShortDateString().ToString() })
                    .FirstOrDefault().C_TransactionDate;


            }
            string Has = " This Assets Has ";
            if (db.Deprecation_number_of_units.Any(x => x.Asset_id == id))
            {
                Has += " Deprecation_number_of_units ";
            }
            if (db.Additional_information.Any(x => x.Assets_id == id))
            {
                Has += " Additional_information ";

            }
            if (db.Depreication_assets_id_connection.Any(x => x.Assets_id == id))
            {
                Has += " Depreication ";

            }
            if (db.Fixed_assets_disposel.Any(x => x.Assets_id == id))
            {
                Has += " Fixed_assets_disposel ";

            }
            if (db.Fixed_assets_renewal.Any(x => x.Assets_id == id))
            {
                Has += " Fixed_assets_renewal ";

            }
            if (db.Fixed_assets_revaluate.Any(x => x.Assets_id == id))
            {
                Has += " Fixed_assets_revaluate ";

            }
            if (db.Stoking_assets.Any(x => x.Assets_id == id))
            {
                Has += " Stoking_assets ";

            }
            if (db.Deprecation_record.Any(x => x.Asset.Id == id))
            {
                Has += " Deprecations ";

            }
            Has += " are you sure you want to delete with all the related Info";
            ViewBag.Error = Has;
            ViewBag.Id = id;
            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset asset = db.Assets.Include(x => x.Assets_part_serial).FirstOrDefault(x => x.Id == id);
            int? TransactionId = asset.Assets_transaction_id;
            if (db.Depreication_assets_id_connection.Any(x => x.Assets_id == id))
            {
                TempData["Error"] = $"This Assets Has Depreication can't be deleted";
                return RedirectToAction("Delete");
            }
            db.Deleted_assets.Add(Business.Business.GetDeleteAssets(asset));
            List<Deleted_assets_serial> DelAsSerial = asset.Assets_part_serial.Select(x => new Deleted_assets_serial
            {
                Assets_id=x.Assets_id,
                Part_number=x.Part_number,
                Serial=x.Serial
            }).ToList();
            db.Deleted_assets_serial.AddRange(DelAsSerial);

            db.Assets.Remove(asset);
            try
            {

                db.SaveChanges();
                //if (TransactionId.HasValue)
                //{
                //    db.New_assets_transaction.Find(TransactionId.Value).IsVoid = true;
                //    db.SaveChanges();
                //    int GlTransaction = db.New_assets_transaction.Find(TransactionId.Value).Gl_transaction_id.Value;
                // //   VoidThisTransaction(GlTransaction, (string)FabulousErp.Business.GetCompanyId());
                //}

            }
            catch (SqlException ex)
            {

            }
            catch (Exception ex)
            {
                try
                {
                    string St = ex.InnerException.InnerException.ToString();

                    int pFrom = St.IndexOf("table") + "table".Length;
                    int pTo = St.LastIndexOf("column");

                    String result = St.Substring(pFrom, pTo - pFrom).Replace("\"dbo.", "").Replace("\"", "");
                    TempData["Error"] = $"This Assets Has {result} can't be deleted";
                    return RedirectToAction("Delete",new {id=id });

                }
                catch
                {
                    return RedirectToAction("Index", new { Setting = "Edit" });
                }

            }
            return RedirectToAction("Index", new { Setting = "Edit" });
        }



        [HttpPost]
        public ActionResult DeleteWithAll(int id)
        {
            int? GlTransaction = 0;
            try
            {
                db.Deleted_assets.Add(Business.Business.GetDeleteAssets(db.Assets.Find(id)));
                Asset asset = db.Assets.Include(x=>x.Assets_part_serial).Where(x => x.Id == id).FirstOrDefault();
                GlTransaction = asset.New_assets_transaction.Gl_transaction_id;
                int? TransactionId = asset.Assets_transaction_id;
                if (db.Deprecation_number_of_units.Any(x => x.Asset_id == id))
                {
                    db.Deprecation_number_of_units.RemoveRange(db.Deprecation_number_of_units.Where(x => x.Asset_id == id));
                }
                if (db.Additional_information.Any(x => x.Assets_id == id))
                {
                    db.Additional_information.RemoveRange(db.Additional_information.Where(x => x.Assets_id == id));
                }
                if (db.Depreication_assets_id_connection.Any(x => x.Assets_id == id))
                {
                    db.Depreication_assets_id_connection.RemoveRange(db.Depreication_assets_id_connection.Where(x => x.Assets_id == id));
                }
                if (db.Fixed_assets_disposel.Any(x => x.Assets_id == id))
                {
                    db.Fixed_assets_disposel.RemoveRange(db.Fixed_assets_disposel.Where(x => x.Assets_id == id));
                }
                if (db.Fixed_assets_renewal.Any(x => x.Assets_id == id))
                {
                    db.Fixed_assets_renewal.RemoveRange(db.Fixed_assets_renewal.Where(x => x.Assets_id == id));
                }
                if (db.Fixed_assets_revaluate.Any(x => x.Assets_id == id))
                {
                    db.Fixed_assets_revaluate.RemoveRange(db.Fixed_assets_revaluate.Where(x => x.Assets_id == id));
                }
                if (db.Stoking_assets.Any(x => x.Assets_id == id))
                {
                    db.Stoking_assets.RemoveRange(db.Stoking_assets.Where(x => x.Assets_id == id));
                }
                if (db.Deprecation_record.Any(x => x.Asset.Id == id))
                {
                    db.Deprecations.RemoveRange(db.Deprecations.Where(x => x.Deprecation_record.Any(z => z.Asset_id == id)));
                    db.Deprecation_record.RemoveRange(db.Deprecation_record.Where(x => x.Asset_id == id));
                }
                List<Deleted_assets_serial> DelAsSerial = asset.Assets_part_serial.Select(x => new Deleted_assets_serial
                {
                    Assets_id = x.Assets_id,
                    Part_number = x.Part_number,
                    Serial = x.Serial
                }).ToList();
                db.Deleted_assets_serial.AddRange(DelAsSerial);

                db.Assets.Remove(asset);
                db.SaveChanges();
                //if (TransactionId.HasValue)
                //{
                //    db.New_assets_transaction.Find(TransactionId.Value).IsVoid = true;
                //    db.SaveChanges();
                //    int GlTransaction = db.New_assets_transaction.Find(TransactionId.Value).Gl_transaction_id.Value;
                //    VoidThisTransaction(GlTransaction, (string)FabulousErp.Business.GetCompanyId());
                //}

            }
            catch (Exception ex)
            {
                try
                {
                    string St = ex.InnerException.InnerException.ToString();

                    int pFrom = St.IndexOf("table") + "table".Length;
                    int pTo = St.LastIndexOf("column");

                    String result = St.Substring(pFrom, pTo - pFrom).Replace("\"dbo.", "").Replace("\"", "");
                    TempData["Error"] = $"This Assets Has {result} can't be deleted";
                    return RedirectToAction("Delete");

                }
                catch
                {
                    return RedirectToAction("Delete");

                }

            }
            int JrNumber = Business.Business.GetPotinNumber(GlTransaction);
            return Json(JrNumber);



        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetAccquistion(int AssetsId)
        {
            try
            {
                Asset ThisAsset = db.Assets.Include(x=>x.Fixed_assets_revaluate).FirstOrDefault(x=>x.Id==AssetsId);
                return Json(ThisAsset.Fixed_assets_revaluate.ToList().DefaultIfEmpty(new Fixed_assets_revaluate {Adjustment_cost= ThisAsset.Acquisation_cost }).ToList().LastOrDefault().Adjustment_cost);
            }
            catch
            {
                return Json(0);
            }
        }
    }
}
