using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.Models;


namespace FixedAssets.Controllers
{
    public class Assets_classController : Controller
    {
        private DBContext db = new DBContext();
        private DBContext MainCont = new DBContext();

        // GET: Assets_class
        public ActionResult Index()
        {
            var assets_class = db.Assets_class;
            return View(assets_class.ToList());
        }
        // GET: Assets_class/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_class assets_class = db.Assets_class.Find(id);
            ViewBag.Id = id;
            if (assets_class == null)
            {
                return HttpNotFound();
            }
            return View(assets_class);
        }

        // GET: Assets_class/Create
        public ActionResult Create()
        {
            return View();
        }
        public JsonResult GetDebitCreditAccount(int AssetsClass)
        {
            try
            {
                int CostAccount = db.Assets_accounts.FirstOrDefault(x => x.Class_id == AssetsClass).Cost_account;
                int AccruedAccont = db.Assets_accounts.FirstOrDefault(x => x.Class_id == AssetsClass).Accrued.Value;
                C_CreateAccount_Table DebitAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == CostAccount).ToList().FirstOrDefault();
                C_CreateAccount_Table CreditAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == AccruedAccont).ToList().FirstOrDefault();

                return Json(db.Assets_class.Where(x => x.Id == AssetsClass).ToList().Select(x => new
                {
                    Debit = CostAccount,
                    Credit = AccruedAccont,
                    DebitAccNum = DebitAcc.AccountID,
                    DebitAccName = DebitAcc.AccountName,
                    CreditAccName = CreditAcc.AccountName,
                    CreditAccNum = CreditAcc.AccountID,

                }
                ), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDebitCreditAccountByAssetsId(int AssetsId)
        {
            try
            {
                int AssetsClass = db.Assets.Find(AssetsId).Assets_class_id;
                int CostAccount = db.Assets_accounts.FirstOrDefault(x => x.Class_id == AssetsClass).Cost_account;
                int AccruedAccont = db.Assets_accounts.FirstOrDefault(x => x.Class_id == AssetsClass).Accrued.Value;
                C_CreateAccount_Table DebitAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == CostAccount).ToList().FirstOrDefault();
                C_CreateAccount_Table CreditAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == AccruedAccont).ToList().FirstOrDefault();

                return Json(db.Assets_class.Where(x => x.Id == AssetsClass).ToList().Select(x => new
                {
                    Debit = CostAccount,
                    Credit = AccruedAccont,
                    DebitAccNum = DebitAcc.AccountID,
                    DebitAccName = DebitAcc.AccountName,
                    CreditAccName = CreditAcc.AccountName,
                    CreditAccNum = CreditAcc.AccountID,

                }
                ), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDebitCreditAccountByAssetsIdRevaluate(int AssetsId,decimal Old_acq,decimal Adjustment)
        {
            try
            {
                
                int AssetsClass = db.Assets.Find(AssetsId).Assets_class_id;
                int CostAccount = db.Assets_accounts.FirstOrDefault(x => x.Class_id == AssetsClass).Cost_account;
                int RevaluateAccont = db.Assets_accounts.FirstOrDefault(x => x.Class_id == AssetsClass).Revaluation_account.Value;
                C_CreateAccount_Table DebitAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == CostAccount).ToList().FirstOrDefault();
                C_CreateAccount_Table CreditAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == RevaluateAccont).ToList().FirstOrDefault();

                if ((Old_acq - Adjustment) < 0)
                {
                    return Json(db.Assets_class.Where(x => x.Id == AssetsClass).ToList().Select(x => new
                    {
                        Debit = CostAccount,
                        Credit = RevaluateAccont,
                        DebitAccNum = DebitAcc.AccountID,
                        DebitAccName = DebitAcc.AccountName,
                        CreditAccName = CreditAcc.AccountName,
                        CreditAccNum = CreditAcc.AccountID
                    }), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(db.Assets_class.Where(x => x.Id == AssetsClass).ToList().Select(x => new
                    {
                        Debit = RevaluateAccont,
                        Credit = CostAccount,
                        DebitAccNum = CreditAcc.AccountID,
                        DebitAccName = CreditAcc.AccountName,
                        CreditAccName = DebitAcc.AccountName,
                        CreditAccNum = DebitAcc.AccountID
                    }), JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDisposalDebitCreditAccountByAssetsId(int AssetsId, decimal Disposal_amount, DateTime TransactionDate)
        {
            try
            {
                Asset asset = db.Assets.Find(AssetsId);
                int AssetsClass = asset.Assets_class_id;
                decimal AcqustionCost = asset.Acquisation_cost;
                decimal RenwalAmount = asset.Fixed_assets_renewal.Where(x => x.Renwal_date >= TransactionDate).Sum(x => x.Renewal_amount).Value;
                decimal OldDepreication = DeprecationsController.CalculateOldDeprication(AssetsId, TransactionDate);
                decimal AcqusectionRenwal = (AcqustionCost + RenwalAmount);
                decimal DisposalEquation = AcqusectionRenwal - OldDepreication;

                Assets_accounts Assets_account = db.Assets_accounts.FirstOrDefault(x => x.Class_id == AssetsClass);
                int accumlated_depreciation = Assets_account.Deprecation_accumulated_account;
                int AccruedAccont = 0;
                if (Assets_account.Accrued.HasValue)
                {
                    AccruedAccont = Assets_account.Accrued.Value;
                }
                int CostAccount = Assets_account.Cost_account;

                C_CreateAccount_Table DebitAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == accumlated_depreciation).ToList().FirstOrDefault();
                C_CreateAccount_Table DebitAcc2 = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == AccruedAccont).ToList().DefaultIfEmpty(new C_CreateAccount_Table { }).FirstOrDefault();
                C_CreateAccount_Table CreditAcc = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == CostAccount).ToList().FirstOrDefault();


                if (DisposalEquation == Disposal_amount)
                {

                    AccountTbl[] Result = new AccountTbl[]
                   {
                         new AccountTbl
                        {
                            DebitAmount = OldDepreication,
                            CreditAmount = AcqusectionRenwal,

                            CreditAccId=CreditAcc.C_AID,
                            DebitAccId = DebitAcc.C_AID,




                            DebitAccNum = DebitAcc.AccountID,
                            DebitAccName = DebitAcc.AccountName,



                            CreditAccName = CreditAcc.AccountName,
                            CreditAccNum = CreditAcc.AccountID
                        },
                         new AccountTbl
                        {

                            DebitAmount = Disposal_amount,
                            CreditAmount=(decimal)0,


                            CreditAccId=0,
                            DebitAccId = DebitAcc2.C_AID,



                            DebitAccNum = DebitAcc2.AccountID,
                            DebitAccName = DebitAcc2.AccountName,

                            CreditAccName = "",
                            CreditAccNum = ""
                        }


                    };
                    CheckNegativeCredit(Result);
                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                else if (DisposalEquation < Disposal_amount)
                {
                    int ProfitAccount = 0;

                    if (Assets_account.Profit_account.HasValue)
                    {
                        ProfitAccount = Assets_account.Profit_account.Value;

                    }

                    C_CreateAccount_Table CreditAcc2 = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == ProfitAccount).ToList().DefaultIfEmpty(new C_CreateAccount_Table { }).FirstOrDefault();
                    AccountTbl[] Result = new AccountTbl[]
                     {
                         new AccountTbl
                        {
                            DebitAmount = OldDepreication,
                            CreditAmount = AcqusectionRenwal,

                            CreditAccId=CreditAcc.C_AID,
                            DebitAccId = DebitAcc.C_AID,

                            DebitAccNum = DebitAcc.AccountID,
                            DebitAccName = DebitAcc.AccountName,



                            CreditAccName = CreditAcc.AccountName,
                            CreditAccNum = CreditAcc.AccountID,



                        },
                        new AccountTbl
                        {
                            DebitAmount = Disposal_amount,
                            CreditAmount = AcqusectionRenwal - (OldDepreication + Disposal_amount),

                            CreditAccId=CreditAcc2.C_AID,
                            DebitAccId=DebitAcc2.C_AID,

                              DebitAccNum = DebitAcc2.AccountID,
                            DebitAccName = DebitAcc2.AccountName,

                              CreditAccName = CreditAcc2.AccountName,
                            CreditAccNum = CreditAcc2.AccountID,

                        }
                    };
                    CheckNegativeCredit(Result);

                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                else if (DisposalEquation > Disposal_amount)
                {
                    int LoseAccount = 0;

                    if (Assets_account.Lose_account.HasValue)
                    {
                        LoseAccount = Assets_account.Lose_account.Value;
                    }

                    C_CreateAccount_Table DebitAcc3 = MainCont.C_CreateAccount_Tables.Where(z => z.C_AID == LoseAccount).ToList().DefaultIfEmpty(new C_CreateAccount_Table { }).FirstOrDefault();
                    AccountTbl[] Result = new AccountTbl[]
                    {
                        new AccountTbl
                        {
                            DebitAmount = OldDepreication,
                            CreditAmount = AcqusectionRenwal,

                            CreditAccId=CreditAcc.C_AID,
                            DebitAccId = DebitAcc.C_AID,

                            DebitAccNum = DebitAcc.AccountID,
                            DebitAccName = DebitAcc.AccountName,

                            CreditAccName = CreditAcc.AccountName,
                            CreditAccNum = CreditAcc.AccountID
                        },
                        new AccountTbl
                        {
                            DebitAmount = Disposal_amount,
                            CreditAmount=0,

                            DebitAccId=DebitAcc2.C_AID,
                            CreditAccId=0,


                            DebitAccNum = DebitAcc2.AccountID,
                            DebitAccName = DebitAcc2.AccountName,

                            CreditAccName="",
                            CreditAccNum=""
                        },
                        new AccountTbl
                        {
                            DebitAmount = AcqusectionRenwal - (OldDepreication + Disposal_amount),
                            CreditAmount=0,

                            CreditAccId=0,
                            DebitAccId=DebitAcc3.C_AID,

                              DebitAccNum = DebitAcc3.AccountID,
                            DebitAccName = DebitAcc3.AccountName,
                               CreditAccName="",
                            CreditAccNum=""
                        }
                    };
                    CheckNegativeCredit(Result);

                    return Json(Result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("");
                }


            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        private AccountTbl[] CheckNegativeCredit(AccountTbl[] Result)
        {
            foreach (AccountTbl R in Result)
            {
                if (R.CreditAmount < 0)
                {
                    R.CreditAmount = -R.CreditAmount;
                }
            }
            return Result;
        }

        // POST: Assets_class/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Class_id,Description,Deprecation_method,Deperecation_rate,Active")] Assets_class assets_class)
        {
            if (ModelState.IsValid)
            {
                db.Assets_class.Add(assets_class);
                db.SaveChanges();
                db.Stoking_assets.Add(new Stoking_assets
                {
                    Assets_class_id = assets_class.Id
                });
                db.SaveChanges();
                return Json(assets_class.Id);
            }
            return View(assets_class);
        }

        // GET: Assets_class/Edit/5
        public ActionResult Edit(int? id, bool IsPartial = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_class assets_class = db.Assets_class.Find(id);
            if (IsPartial)
            {
                ViewBag.IsPartial = IsPartial;
            }
            if (assets_class == null)
            {
                return HttpNotFound();
            }
            return View(assets_class);
        }

        // POST: Assets_class/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Class_id,Description,Deprecation_method,Deperecation_rate,Active")] Assets_class assets_class)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assets_class).State = EntityState.Modified;
                db.Entry(assets_class).Property("Class_id").IsModified = false;
                db.SaveChanges();
                return Json(assets_class.Id);
            }
            return View(assets_class);
        }

        // GET: Assets_class/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Assets_class assets_class = db.Assets_class.Find(id);
            if (assets_class == null)
            {
                return HttpNotFound();
            }
            ViewBag.Error = TempData["Error"];

            ViewBag.Id = id;

            return View(assets_class);
        }

        // POST: Assets_class/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assets_class assets_class = db.Assets_class.Find(id);
            if (assets_class.Assets.Any())
            {
                TempData["Error"] = "you can't delete this assets class because it has assets attached to it ";
                return RedirectToAction("Delete", new {id= id });
            }
            Business.Business.GetDeleteAssetsClass(assets_class);
            db.Assets_class.Remove(assets_class);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                try
                {
                    string St = ex.InnerException.InnerException.ToString();

                    int pFrom = St.IndexOf("table") + "table".Length;
                    int pTo = St.LastIndexOf("column");

                    String result = St.Substring(pFrom, pTo - pFrom).Replace("\"dbo.", "").Replace("\"", "");
                    TempData["Error"] = $"This Assets Class Has {result} can't be deleted";
                    return RedirectToAction("Delete");

                }
                catch
                {
                    return RedirectToAction("Delete");
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteWithAll(int id)
        {
            try
            {
                db.Deleted_assets_class.Add(Business.Business.GetDeleteAssetsClass(db.Assets_class.Find(id)));

                Assets_class assetClass = db.Assets_class.FirstOrDefault(x => x.Id == id);
                if (db.Assets.Any(x => x.Assets_class_id == id))
                {
                    db.Assets.RemoveRange(db.Assets.Where(x => x.Assets_class_id == id));
                }
                if (db.Assets_accounts.Any(x => x.Class_id == id))
                {
                    db.Assets_accounts.RemoveRange(db.Assets_accounts.Where(x => x.Class_id == id));
                }
                if (db.Assets_main.Any(x => x.Assets_class_id == id))
                {
                    db.Assets_main.RemoveRange(db.Assets_main.Where(x => x.Assets_class_id == id));
                }
                if (db.Depreication_assets_connection.Any(x => x.Assets_class_id == id))
                {
                    db.Depreication_assets_connection.RemoveRange(db.Depreication_assets_connection
                        .Where(x => x.Assets_class_id == id));
                }
                if (db.New_assets_transaction.Any(x => x.Assets_class_id == id))
                {
                    db.New_assets_transaction.RemoveRange(db.New_assets_transaction
                        .Where(x => x.Assets_class_id == id));
                }
                if (db.Stoking_assets.Any(x => x.Assets_class_id == id))
                {
                    db.Stoking_assets.RemoveRange(db.Stoking_assets
                        .Where(x => x.Assets_class_id == id));
                }
                db.Assets_class.Remove(assetClass);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                try
                {
                    string St = ex.InnerException.ToString();

                    int pFrom = St.IndexOf("table") + "table".Length;
                    int pTo = St.LastIndexOf("column");

                    String result = St.Substring(pFrom, pTo - pFrom).Replace("\"dbo.", "").Replace("\"", "");
                    TempData["Error"] = $"This Assets Has {result} can't be deleted";
                    return RedirectToAction("Delete");
                }
                catch
                {

                }
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    public class AccountTbl
    {
        public decimal DebitAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public int DebitAccId { get; set; }
        public int CreditAccId { get; set; }
        public string DebitAccNum { get; set; }
        public string DebitAccName { get; set; }
        public string CreditAccName { get; set; }
        public string CreditAccNum { get; set; }
    }
}
