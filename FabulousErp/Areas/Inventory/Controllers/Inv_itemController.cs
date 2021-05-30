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
using FabulousErp;
using FabulousErp.Bussiness;
using FabulousModels.Inventory;

namespace Inventory.Controllers
{
    public class Inv_itemController : Controller
    {
        private DBContext db = new DBContext();
        #region Crud
        // GET: Inventory/Inv_item
        public ActionResult Index(bool HasRecipe=false)
        {
            List<Inv_item> Res = new List<Inv_item>();
            if (HasRecipe)
            {
                Res = db.Inv_item
              .Join(db.Inv_item_recipe,I=>I.Id,R=>R.Main_item_id,(I,R)=>new {I,R })
              .Select(x=>x.I).Include(i => i.Inv_item_group)
              .Include(i => i.Unit_of_measure).ToList();
            }
            else
            {
                Res = db.Inv_item
               .GroupJoin(db.Inv_item_recipe, I => I.Id, R=> R.Main_item_id, (I, R) => new { I, R=R.DefaultIfEmpty() }).Where(x=>x.R.Any(z=>z==null)).Select(x=>x.I)
               .Include(i => i.Inv_item_group)
               .Include(i => i.Unit_of_measure).ToList();
            }
           
            return View(Res.Distinct());
        }
        
        // GET: Inventory/Inv_item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item inv_item = db.Inv_item.Find(id);
            if (inv_item == null)
            {
                return HttpNotFound();
            }
            return View(inv_item);
        }

        // GET: Inventory/Inv_item/Create
        public ActionResult Create(int? Id, bool HasRecipe = false)
        {
            Inv_item I = db.Inv_item.Where(x => x.Id == Id).Include(x => x.Deduct_tax)
                .Include(x=>x.Inv_item_option).ToList().DefaultIfEmpty(new Inv_item { Inv_item_option=new List<Inv_item_option> { new Inv_item_option { } } }).FirstOrDefault();
            ViewBag.Id = Id;
            ViewBag.HasRecipe = HasRecipe;
            ViewBag.Validation_method = new SelectList(Enum.GetValues(typeof(Validation_method))
               .Cast<Validation_method>().Select(x => new { Text = x.ToString(), Value = (int)x }).ToList()
               , "Value", "Text", (int)I.Validation_method);

            ViewBag.Item_group_id = new SelectList(db.Inv_item_group, "Id", "Item_group_id", I.Item_group_id);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", I.Unit_of_measure_id);
            ViewBag.Vat_Item_type = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Text = FabulousErp.BusController.Translate("3- Material") , Value = "3" },
                new SelectListItem{Text = FabulousErp.BusController.Translate( "4- Service") , Value = "4" },
                new SelectListItem{Text = FabulousErp.BusController.Translate("5- Equipment") , Value = "5" },
                new SelectListItem{Text = FabulousErp.BusController.Translate("6- Parts of Equipment") , Value = "6" },
                new SelectListItem{Text = FabulousErp.BusController.Translate("7- Exemptions") , Value = "7" }
            }, "Value", "Text", I.Vat_Item_type);

            ViewBag.Tax_type_id = new SelectList(new List<SelectListItem>
                                     {
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("1- General") , Value = "1" },
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("2- Tax Table") , Value = "2" }
                                     }, "Value", "Text", I.Tax_type_id);

            ViewBag.Tax_table_type_id = new SelectList(new List<SelectListItem>
                                     {
                                         new SelectListItem{Text =FabulousErp.BusController.Translate("0- None") , Value = "0" },
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("1- Table First") , Value = "1" },
                                         new SelectListItem{Text = FabulousErp.BusController.Translate("2- Table Second") , Value = "2" }
                                     }, "Value", "Text", I.Tax_table_type_id);
            ViewBag.Cost_center_id = new SelectList(db.C_CostCenter_Tables.ToList(), "C_CostCenterID", "C_CostCenterName");

            int Max = 0;
            foreach (string ThisI in db.Inv_item.Select(x => x.Item_id))
            {
                if (FabulousErp.Business.GetDigitsIgnoreChars(ThisI) != 0)
                {
                    Max = FabulousErp.Business.GetDigitsIgnoreChars(ThisI);
                }
            }
         
            return View(I);
        }
        public JsonResult DuplicateId(string Item_id,int Id)
        {
            if (Id == 0)
            {
                return Json(db.Inv_item.Any(x => x.Item_id == Item_id.Trim()));
            }
            return Json(db.Inv_item.Any(x => x.Item_id == Item_id.Trim() && x.Id != Id));
        }
        public ActionResult AddRecipe(int? ItemId=null)
        {
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            List<Inv_item_recipe> Res = db.Inv_item_recipe
                .Where(x => x.Main_item_id == ItemId)
                .Include(x=>x.Recipe_item).Include(x=>x.Recipe_item.Unit_of_measure).ToList();
            return View(Res);
        }
        [HttpPost]
        public ActionResult AddRecipe(List<Inv_item_recipe> Item_recipe)
        {
            db.Inv_item_recipe.AddRange(Item_recipe.Where(x=>x.Id==0));
            db.SaveChanges();
            return View();
        }
        public JsonResult RmRecipeItem(int Id)
        {
            try
            {
                db.Inv_item_recipe.Remove(db.Inv_item_recipe.Find(Id));
                db.SaveChanges();
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
      
        }
        // POST: Inventory/Inv_item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_item inv_item, List<int> DeductTaxIds)
        {
            if (DeductTaxIds == null)
            {
                DeductTaxIds = new List<int>();
            }
            if (ModelState.IsValid)
            {
                if (inv_item.Id == 0)
                {
                    db.Inv_item.Add(inv_item);
                    db.SaveChanges();
                    foreach (int i in DeductTaxIds)
                    {
                        db.Inv_item_deduct_tax.Add(new Inv_item_deduct_tax
                        {
                            Deduct_id = i,
                            item_id = inv_item.Id
                        });
                    }
                    db.SaveChanges();
                }
                else
                {
                    db.Entry(inv_item).State = EntityState.Modified;
                    db.SaveChanges();
                    db.Inv_item_deduct_tax.RemoveRange(db.Inv_item_deduct_tax.Where(x => x.item_id == inv_item.Id));
                    foreach (int i in DeductTaxIds)
                    {
                        db.Inv_item_deduct_tax.Add(new Inv_item_deduct_tax
                        {
                            Deduct_id = i,
                            item_id = inv_item.Id
                        });
                    }
                    db.SaveChanges();
                }

                return Json(inv_item.Id);
            }

            ViewBag.Item_group_id = new SelectList(db.Inv_item_group, "Id", "Item_group_id", inv_item.Item_group_id);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_item.Unit_of_measure_id);
            return View(inv_item);
        }

        // GET: Inventory/Inv_item/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item inv_item = db.Inv_item.Find(id);
            if (inv_item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Item_group_id = new SelectList(db.Inv_item_group, "Id", "Item_group_id", inv_item.Item_group_id);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_item.Unit_of_measure_id);
            return View(inv_item);
        }

        // POST: Inventory/Inv_item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Item_id,Description,Short_description,Item_group_id,Type,Unit_of_measure_id,Sales_tax_group_id,Purchase_tax_group_id")] Inv_item inv_item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Item_group_id = new SelectList(db.Inv_item_group, "Id", "Item_group_id", inv_item.Item_group_id);
            ViewBag.Unit_of_measure_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_item.Unit_of_measure_id);
            return View(inv_item);
        }

        // GET: Inventory/Inv_item/Delete/5
        public ActionResult Delete(int? id, string Msg = "")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item inv_item = db.Inv_item.Find(id);
            if (inv_item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Msg = Msg;
            return View(inv_item);
        }

        // POST: Inventory/Inv_item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Inv_item inv_item = db.Inv_item.Include(x => x.Item_gl_account).Include(x=>x.Item_store_site).FirstOrDefault(x => x.Id == id);
                db.Inv_item_gl_accounts.RemoveRange(inv_item.Item_gl_account);
                db.Inv_item_store_sites.RemoveRange(inv_item.Item_store_site);
                db.Inv_item.Remove(inv_item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Delete", new { id = id, Msg = "Some Thing Went Wrong...." });

            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
#endregion

        public JsonResult GetTaxIds(int ItemId,int PorS=1)
        {
            try
            {
                Inv_item Item = db.Inv_item.Include(x => x.Deduct_tax).Include(x=>x.Deduct_tax.Select(z=>z.Deduct)).Include(x => x.Vat).Include(x => x.Tbl_vat).FirstOrDefault(x => x.Id == ItemId);

                int DeductTaxPurch = db.C_TaxSetting_Tables.ToList().Where(x => x.TG_ID == PorS && Item.Deduct_tax.Any(z => z.Deduct_id == x.CT_ID)).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax.C_TaxSetting_table { CT_ID=0 }).FirstOrDefault().CT_ID;
                int? DeductTaxCAID = db.C_TaxSetting_Tables.ToList().Where(x => x.TG_ID == PorS && Item.Deduct_tax.Any(z => z.Deduct_id == x.CT_ID)).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax.C_TaxSetting_table { C_AID = 0 }).FirstOrDefault().C_AID;

                double? DeductTaxPer = db.C_TaxSetting_Tables.ToList().Where(x => Item.Deduct_tax.Any(z => z.Deduct_id == x.CT_ID&&z.Deduct.TG_ID==PorS))
                    .ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax.C_TaxSetting_table { C_Taxpercentage=0 }).FirstOrDefault().C_Taxpercentage;
                
                return Json(new
                {
                    Vat_id = Item.Vat_id,
                    Item.Tbl_vat_Id,
                    DeductId = DeductTaxPurch,
                    Item.Vat_Item_type,
                    Item.Tax_type_id,
                    Item.Tax_table_type_id,
                    Item.Unit_of_measure_id,

                    TblVatPer = (Item.Tbl_vat != null) ? Item.Tbl_vat.C_Taxpercentage : 0,
                    VatPer = (Item.Vat != null) ? Item.Vat.C_Taxpercentage : 0,
                    DeductPer = (DeductTaxPer != null) ? DeductTaxPer : 0,

                    VatAccId = (Item.Vat != null) ? Item.Vat.C_AID : 0,
                    TblVatAccId = (Item.Tbl_vat != null) ? Item.Tbl_vat.C_AID : 0,
                    DedAccId = DeductTaxCAID,
                });
            }
            catch
            {
                return Json(-1);
            }
            
        }

        public JsonResult GetInventoryAccount(int ItemId, Doc_type Doc_type=Doc_type.Invoice,bool IsSales=false)
        {
            C_CreateAccount_Table Inv = new C_CreateAccount_Table();
            C_CreateAccount_Table VarianceInv = new C_CreateAccount_Table();
            if (Doc_type == Doc_type.Invoice
                || (IsSales && Doc_type == Doc_type.Return))
            {
                Inv = db.Inv_item_gl_accounts.Include(x => x.Inventory)
              .FirstOrDefault(x => x.Item_id == ItemId).Inventory;
            }
            else
            {
                Inv = db.Inv_item_gl_accounts.Include(x => x.Inventory_returne)
                        .FirstOrDefault(x => x.Item_id == ItemId).Inventory_returne;
                VarianceInv = db.Inv_item_gl_accounts.Include(x => x.Variancies)
                              .FirstOrDefault(x => x.Item_id == ItemId).Variancies;
            }

            return Json(new { AC_Id = Inv.C_AID, AName = Inv.AccountName, AId = Inv.AccountID 
                , VAC_Id = VarianceInv.C_AID, VAName = VarianceInv.AccountName , VAId= VarianceInv.AccountID
            });
        }  
        public JsonResult GetDamageAccount(int ItemId)
        {
            C_CreateAccount_Table Inv = new C_CreateAccount_Table();
            Inv = db.Inv_item_gl_accounts.Include(x => x.Damage)
            .FirstOrDefault(x => x.Item_id == ItemId).Damage;
            return Json(new { AC_Id = Inv.C_AID, AName = Inv.AccountName, AId = Inv.AccountID 
            });
        }   
        public JsonResult GetCostOfAccount(int ItemId, Doc_type Doc_type)
        {

            C_CreateAccount_Table Inv = new C_CreateAccount_Table();

            Inv = db.Inv_item_gl_accounts.Include(x => x.Cost_of_GS)
             .FirstOrDefault(x => x.Item_id == ItemId).Cost_of_GS;

            return Json(new { AC_Id = Inv.C_AID, AName = Inv.AccountName, AId = Inv.AccountID });
        }

        public JsonResult GetItemUnitDecimalNumber(int ItemId)
        {
            try
            {
                return Json(db.Inv_item.Include(x => x.Unit_of_measure).FirstOrDefault(x => x.Id == ItemId).Unit_of_measure.Quantity_dicimal);
            }
            catch
            {
                return Json(0);
            }
        }
        public JsonResult GetItemName(int Item_id, int ST = 1)
        {
            string ItemName = string.Empty;
            ItemName =Business.GetItemBySt(ST, db.Inv_item.Find(Item_id));

            //if (string.IsNullOrEmpty(ItemName))
            //{
            //    ItemName = db.Inv_item.Find(Item_id).Description;
            //} 
            //if (string.IsNullOrEmpty(ItemName))
            //{
            //    ItemName = db.Inv_item.Find(Item_id).Item_id;
            //}

            return Json(ItemName);
        }
        public JsonResult ItemHasPassword(int Item_id)
        {
            return Json(db.Inv_item.Find(Item_id).Has_password);
        }
        public JsonResult ItemCheckPassword(int Item_id,string Password)
        {
            return Json(db.Inv_item.Any(x=>x.Id==Item_id&&x.Password==Password));
        }
        public JsonResult GetItemUnitMeasure(int Item_id)
        {
            try
            {
                return Json(new
                {
                    Unit = db.Inv_item.Include(x => x.Unit_of_measure)
            .FirstOrDefault(x => x.Id == Item_id).Unit_of_measure.Unit_id,
                    Unit_id = db.Inv_item.Include(x => x.Unit_of_measure)
            .FirstOrDefault(x => x.Id == Item_id).Unit_of_measure.Id
                });
            }
            catch (Exception e)
            {
                return Json(new { Unit = "", Unit_id = "" });
            }
        
        } 
        public JsonResult GetItemUnitMeasureSelect(int Item_id)
        {
            try
            {
                int? ItemUOM = db.Inv_item.Find(Item_id).Unit_of_measure_id;
                List<Unit_of_measure> UOM = db.Unit_of_measures.Include(x => x.Equivalante).Where(x => x.Id == ItemUOM).ToList();
                List<Unit_of_measure> EOM = UOM.Where(x => x.Equivalante != null).Select(x => x.Equivalante).ToList();
               
                EOM.AddRange(db.Unit_of_measures.Include(x => x.Equivalante).Where(x => x.Equivalante_id == ItemUOM).ToList());

                EOM.AddRange(db.Unit_of_measures.Include(x => x.Equivalante).ToList().Where(x => EOM.Any(z=>z.Id==x.Equivalante_id)).ToList());

                EOM.ForEach(x => {
                    x.Equivalante_quantity = InvBus.CalcItemEq(Item_id, x.Id, 1);
                    //  x.Action = UOM.Find(z => x.Id == z.Equivalante_id.Value).Action; 
                });
                UOM.ForEach(x => { x.Equivalante_quantity =  1; /*x.Action = 0;*/ });
                UOM.AddRange(EOM);
                UOM = UOM.Distinct().ToList();
                return Json(UOM.Select(x=>new {Id=x.Id,Name=x.Unit_id,MainUnit= ItemUOM,Qty=x.Equivalante_quantity/*,Action=(int)x.Action*/ }).ToList());
            }
            catch (Exception e)
            {
                return Json(new { Unit = "", Unit_id = "" });
            }
        }
        public JsonResult GetStoreItems(int StoreId)
        {
            try
            {
                return Json(db.Inv_item.Where(x => x.Item_store_site.Any(z => z.Store_id == StoreId)).Select(x => new { x.Id,Name= x.Item_id }));
            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult GetSiteItems(int SiteId,int ST=0)
        {
            try
            {
                return Json(db.Inv_item.Where(x => x.Item_store_site.Any(z => z.Site_id == SiteId)&&x.Inactive==false).ToList().Select(x => new { x.Id,Name=Business.GetItemBySt(ST,x) }));
            }
            catch
            {
                return Json(null);
            }
        } 
        public JsonResult GetPurchaseItems(int Id,int ST=0)
        {
            try
            {
                return Json(db.Inv_receive_po_items.Where(x => x.Receive_po_id == Id)
                    .Include(x => x.Item).Include(x => x.Item_serial)
                    .ToList().Select(x => new { x.Item.Id, Name = Business.GetItemBySt(ST, x.Item) }));

                //List<InvoiceItems> Items = InvBus.GetItemAvaliableByPoId(Id,true);
                //List<IdItemId> Res = new List<IdItemId>() ;
                //foreach (Inv_receive_po_items I in Items.SelectMany(x=>x.Purchase_items))
                //{
                //    Res.Add(new IdItemId { Id =I.Item_id, ItemId =db.Inv_item.Find(I.Item_id).Item_id});
                //}
                //return Json(Res);
            }
            catch (Exception ex)
            {
                return Json(new IdItemId { Id = 0, ItemId =""});
            }
        }
        public JsonResult GetSalesItems(int Id,int ST=0)
        {
            try
            {
                return Json(db.Inv_sales_invoice_items.Where(x => x.Sales_invoice_id==Id)
                    .Include(x=>x.Item).Include(x=>x.Serials).ToList().Select(x => new { x.Item.Id,Name=Business.GetItemBySt(ST,x.Item) }));
            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult GetGroupItems(int GroupId)
        {
            try
            {
                return Json(db.Inv_item.Where(x => x.Item_group_id==GroupId).Select(x => new { x.Id,Name= x.Item_id }));
            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult HasProp(int ItemId)
        {
            try
            {
                Inv_item I = db.Inv_item.Find(ItemId);
                return Json(new { I.Has_serial,I.Has_warranty,I.Has_expiry_date });
            }
            catch
            {
                return Json(false);
            }
        }
    }
    public class IdItemId
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
    }
}
