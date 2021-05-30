using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using FabulousDB.Models;
using FabulousErp.Payable.Models;
using FabulousDB.DB_Context;
using FabulousErp.Receivable.Models;
using FabulousDB.DB_Context;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.Tax;
using FabulousModels.Inventory;
using FabulousErp.Bussiness;

namespace Inventory.Controllers
{
    public class Inv_receive_po_itemsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_receive_po_items
        public ActionResult Index()
        {
            var inv_receive_po_items = db.Inv_receive_po_items.Include(i => i.Item).Include(i => i.UOM);
            return View(inv_receive_po_items.ToList());
        }

        // GET: Inventory/Inv_receive_po_items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_receive_po_items inv_receive_po_items = db.Inv_receive_po_items.Find(id);
            if (inv_receive_po_items == null)
            {
                return HttpNotFound();
            }
            return View(inv_receive_po_items);
        }

        // GET: Inventory/Inv_receive_po_items/Create
        public ActionResult Create(bool Sales = false)
        {
            if (Sales == false)
            {
                ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
                ViewBag.ShowUnitPrice = "";
            }
            else
            {
                ViewBag.Item_id = new SelectList(db.Inv_receive_po_items.Include(x => x.Item).ToList().GroupBy(x => x.Item_id).Select(x => new { Id = x.FirstOrDefault().Item_id, Item_id = x.FirstOrDefault().Item.Item_id }), "Id", "Item_id");
              
                ViewBag.ShowCostPrice = db.Inv_sales_GS.ToList().DefaultIfEmpty().FirstOrDefault()
                       .Show_cost_price.ToString().ToLower().Replace("false", "hide");
                if (Request["Doc_type"]== "Return")
                {
                    ViewBag.ShowCostPrice = "";
                }
            }
            ViewBag.UOM_id = new SelectList(new List<int>());// new SelectList(db.Unit_of_measures, "Id", "Unit_id");
            ViewBag.Deduct_id = db.C_TaxSetting_Tables.Select(x => new TaxCodeByGroup_DTO
            {
                CT_ID = x.CT_ID,
                C_Taxpercentage = x.C_Taxpercentage,
                C_Taxcode = x.C_Taxcode
            }).ToList();
            ViewBag.Table_vat_id = db.C_TaxSetting_Tables.Select(x => new TaxCodeByGroup_DTO
            {
                CT_ID = x.CT_ID,
                C_Taxpercentage = x.C_Taxpercentage,
                C_Taxcode = x.C_Taxcode
            }).ToList();
            ViewBag.Vat_id = db.C_TaxSetting_Tables.Select(x => new TaxCodeByGroup_DTO
            {
                CT_ID = x.CT_ID,
                C_Taxpercentage = x.C_Taxpercentage,
                C_Taxcode = x.C_Taxcode
            }).ToList();
            ViewBag.Sales = Sales;
            ViewBag.Site_id = new SelectList(new List<string> { });
       
            return View();
        }

        public JsonResult GetItemDetails(int ItemId, float Qty, float? SoldItems, int StoreId, bool GetAvaliable = false, int? UOM = null)
        {
            ItemDetails I = InvBus.CalcItemDetails(ItemId, Qty, StoreId, SoldItems, GetAvaliable, UOM);
            float EqQty = 1;
            try
            {
                EqQty = InvBus.CalcItemEq(ItemId, UOM.Value);
            }
            catch
            {

            }
            return Json(new { CostPrice = I.TotalPrice.ToString(FabulousErp.Business.GetDecimalNumber()), POs = I.Po_inv, Avaliable = I.Avaliable, EqQty = EqQty });
        }
        public JsonResult GetUnitPruce(int ItemId, int StoreId, int UOM, float Qty)
        {
            List<IGrouping<int?, Inv_receive_po_items>> I = db.Inv_receive_po_items
                     .Where(x => x.Item_id == ItemId && x.Receive_po.Store_id == StoreId).OrderBy(x => x.Receive_po_id)
                     .GroupBy(x => x.Receive_po_id).ToList();

            decimal UP = 0;
            decimal Total = 0;
            InvBus.GetUnitPrice(I, ItemId, Qty, StoreId, ref UP, ref Total, UOM, 0);
            return Json(new
            {
                UP = UP,
                Total = Total,
                EqQty = InvBus.CalcItemEq(ItemId, UOM)
            });
        }
        public JsonResult GetCalcPurchasDetails(int ItemId, int RPoId, int StoreId, float Qty, bool ChangeQty = false, int? UOM_id = null)
        {
            PurchasemDetails I = CalcPurchasDetails(ItemId, RPoId, StoreId, Qty, ChangeQty, UOM_id);
            return Json(I);
        }
        //<summary>
        //Calculate Qty Unit Price For Sale and Get Avaliable Items
        //</summary>
        public static PurchasemDetails CalcPurchasDetails(int Item, int RPoId, int StoreId, float Qty = 0, bool ChangeQty = false, int? UOM_id = null)
        {
            using (DBContext db = new DBContext())
            {
                PurchasemDetails Res = db.Inv_receive_po_items.Where(x => x.Item_id == Item && x.Receive_po_id == RPoId)
                    .Include(x => x.Receive_po).Include(x => x.Receive_po.Po_sales).Include(x => x.Item).Include(x => x.Item.Item_store_site)
                    .ToList().Select(x => new PurchasemDetails
                    {
                        Qty = InvBus.CalcItemDetails(Item, (Qty == 0 ? x.Quantity : Qty), StoreId, null, true, null/*((ChangeQty)?UOM_id:x.UOM_id)*/).Po_inv.Sum(z => z.Qty),
                        UnitPrice = InvBus.CalcItemDetails(Item, (Qty == 0 ? x.Quantity : Qty), StoreId, null, true, null /*((ChangeQty) ? UOM_id : x.UOM_id)*/).CostPrice,
                        CostPrice = InvBus.CalcItemDetails(Item, (Qty == 0 ? x.Quantity : Qty), StoreId, null, true, null /*((ChangeQty) ? UOM_id : x.UOM_id)*/).TotalPrice,
                        Discount = x.Discount,
                        Fright = x.Fright,
                        UOMId = ((ChangeQty) ? UOM_id : x.UOM_id)
                    }).FirstOrDefault();

                if (Res.CostPrice == -1)
                {
                    Res.CostPrice = 0;
                    Res.Qty = 0;
                    Res.Qty = 0;
                    Res.Discount = 0;
                    Res.Fright = 0;
                }
                else
                {
                    if (Qty == 0)
                    {
                        //Res.CostPrice = Res.CostPrice * (decimal)Res.Qty;
                    }
                    else
                    {
                        if (Qty > Res.Qty)
                        {
                            Qty = Res.Qty;
                        }
                        Res.CostPrice = Res.UnitPrice * (decimal)Res.Qty;
                        Res.Qty = Qty;
                    }
                }
                if (!ChangeQty)
                {
                    Res.Qty = InvBus.CalcItemEq(Item, Res.UOMId.Value, Res.Qty, false);
                    //Res.UnitPrice = InvBus.CalcItemEqUnitPrice(Item, Res.UOMId.Value, Res.Qty, Res.UnitPrice, true);
                }
                return Res;
            }
        }
        public JsonResult CreateList(List<Inv_receive_po_items> inv_po_items, Doc_type Doc_type, List<Inv_receive_item_serial> Serials = null)
        {
            bool AllService = false;
            if (Serials == null)
            {
                Serials = new List<Inv_receive_item_serial>();
            }
            if (Doc_type == Doc_type.Return)
            {
                inv_po_items.ForEach(x => x.Unit_price = x.Cost_price.Value / (decimal)x.Quantity);
            }

            decimal Tax = 0;

            List<Inv_receive_po_items> ThisItems = new List<Inv_receive_po_items>();
            ThisItems.AddRange(inv_po_items);



            int InvrecivePo = inv_po_items.FirstOrDefault().Receive_po_id.Value;
            foreach (Inv_receive_po_items i in ThisItems)
            {
                if (db.Inv_item.Find(i.Item_id).Martial_or_service == MartialService.Service)
                {
                    inv_po_items.Remove(i);
                }
                else
                {
                    if (i.UOM_id != db.Inv_item.Find(i.Item_id).Unit_of_measure_id && i.UOM_id != null)
                    {
                        i.Quantity = InvBus.CalcItemEq(i.Item_id, i.UOM_id.Value, i.Quantity);
                        i.Unit_price = InvBus.CalcItemEqUnitPrice(i.Item_id, i.UOM_id.Value, i.Quantity, i.Unit_price);

                        //if (Doc_type != Doc_type.Return)
                        //{
                        //}
                        //else
                        //{
                        //    i.Unit_price = Unit_of_measureController.CalcItemEqUnitPrice(i.Item_id, i.UOM_id.Value, i.Quantity, i.Unit_price,false);
                        //}
                        // i.UOM_id = db.Inv_item.Find(i.Item_id).Unit_of_measure_id;
                    }
                }
            }

            if (!inv_po_items.Any())
            {
                Inv_receive_po ThisInv = db.Inv_receive_po.Find(InvrecivePo);
                db.Inv_store.Find(ThisInv.Store_id).Next_gr_no = db.Inv_store.Find(ThisInv.Store_id).Next_gr_no - 1;
                db.Inv_receive_po.Remove(ThisInv);
                AllService = true;
            }
            else
            {
                db.Inv_receive_po_items.AddRange(inv_po_items);
            }




            db.SaveChanges();
            int ItemId = 0;
            string Error = "";
            string DuplicateError = "";
            bool valid = false;
            try
            {
                foreach (Inv_receive_po_items i in inv_po_items)
                {
                    ItemId = i.Item_id;
                    string ItemName = "";
                    try
                    {
                        ItemName = db.Inv_item.Find(i.Item_id).Item_id;
                    }
                    catch
                    {

                    }
                    if (Doc_type == Doc_type.Invoice)
                    {
                        Serials.Where(x => x.Item_id == i.Item_id)
                        .ToList()
                        .ForEach(x =>
                        {
                            x.Po_id = InvrecivePo;
                            x.Item_id = i.Id;
                        });
                        db.Inv_receive_item_serial.AddRange(Serials);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            db.Exceptions_Tables.Add(new FabulousDB.DB_Tabels.Important.Exceptions_Table
                            {
                                Exception = Newtonsoft.Json.JsonConvert.SerializeObject(Serials.Where(x => x.Item_id == i.Item_id).ToList()) + "" + e.ToString(),
                                URL = "Inv Purchase"
                            });
                            valid = false;
                            Error += $"Error In Serial Number Entry in Item {ItemName} <br/>";
                        }
                    }
                    else
                    {
                        List<Inv_receive_item_serial> ThisSerials = db.Inv_receive_item_serial.ToList().Where(x => Serials.Any(z => z.Serial == x.Serial)).ToList();
                        ThisSerials.ForEach(x => x.Sold = false);
                        db.Inv_old_receive_item_serial.AddRange(ThisSerials.Select(x => new Inv_old_receive_item_serial
                        {
                            Serial_item_id = x.Id,
                            Old_po_id = x.Po_id,
                            Transfer_date = DateTime.Now
                        }));
                        List<Inv_sales_item_serial> SalesSerials = db.Inv_sales_item_serial.ToList().Where(x => Serials.Any(z => z.Serial == x.Serial)).ToList();
                        ThisSerials.ForEach(x => x.Po_id = inv_po_items.FirstOrDefault().Receive_po_id.Value);

                        db.Inv_sales_item_serial.RemoveRange(SalesSerials);
                        db.SaveChanges();
                        //foreach (List<Inv_receive_po_items> PoItems in inv_po_items.GroupBy(x=>x.Receive_po_id).Select(x=>x.ToList()))
                        //{
                        //    foreach (Inv_receive_po_items PI in PoItems)
                        //    {
                        //        int SalesId = db.Inv_sales_receivs_pos.Where(x => x.Receive_po_id == PI.Receive_po_id && x.Item_id == PI.Item_id).FirstOrDefault().Sales_id;
                        //        if (db.Inv_sales_invoice_items.Where(x => x.Sales_invoice_id == SalesId && x.Item_id == PI.Item_id)
                        //            .FirstOrDefault().Quantity == SalesSerials.Count())
                        //        {
                        //            db.Inv_sales_invoice.Remove(db.Inv_sales_invoice.Find(SalesId));
                        //            try
                        //            {
                        //                db.SaveChanges();
                        //            }
                        //            catch (Exception e)
                        //            {
                        //            }
                        //        }
                        //    }
                        //}
                    }

                }
            }
            catch (Exception e)
            {
                return Json(new { valid = valid, error = Error });
            }
            return Json(new { AllService = AllService });
        }
        public JsonResult CreateListSales(List<Inv_sales_invoice_items> inv_po_items
           , Doc_type Doc_type, List<Inv_sales_item_serial> Serials = null,bool FullPay=false)
        {
            bool AllService = false;
            if (Serials == null)
            {
                Serials = new List<Inv_sales_item_serial>();
            }
            if (Doc_type == Doc_type.Invoice)
            {
                if (Serials == null)
                {
                    Serials = new List<Inv_sales_item_serial>();
                }
                decimal Tax = 0;
                List<Inv_sales_invoice_items> ThisItems = new List<Inv_sales_invoice_items>();
                ThisItems.AddRange(inv_po_items);
                int ThisInvoiceId = inv_po_items.FirstOrDefault().Sales_invoice_id.Value;
                foreach (Inv_sales_invoice_items i in ThisItems)
                {
                    if (db.Inv_item.Find(i.Item_id).Martial_or_service == MartialService.Service)
                    {
                        inv_po_items.Remove(i);
                    }
                    else
                    {
                        i.Unit_price = InvBus.CalcItemEqUnitPrice(i.Item_id, i.UOM_id.Value, i.Quantity, i.Unit_price);
                        i.Cost_price = (i.Cost_price / (decimal)InvBus.CalcItemEq(i.Item_id, i.UOM_id.Value, i.Quantity));
                    }
                }

                if (!inv_po_items.Any())
                {
                    Inv_sales_invoice ThisInv = db.Inv_sales_invoice.Find(ThisInvoiceId);
                    db.Inv_store.Find(ThisInv.Store_id).Next_goods_no = db.Inv_store.Find(ThisInv.Store_id).Next_goods_no - 1;
                    db.Inv_sales_invoice.Remove(ThisInv);
                    AllService = true;
                }
                else
                {
                    db.Inv_sales_invoice_items.AddRange(inv_po_items);
                }


                db.SaveChanges();

                try
                {
                    foreach (Inv_sales_invoice_items i in inv_po_items)
                    {
                        Serials.Where(x => x.Item_id == i.Item_id).ToList()
                            .ForEach(x =>
                            {
                                x.Sales_id = ThisInvoiceId;
                                x.Item_id = i.Id;
                            });
                        List<Inv_receive_item_serial> IRI = db.Inv_receive_item_serial
                            .ToList().Where(x => Serials.Any(z => z.Serial == x.Serial))
                            .ToList();
                        IRI.ForEach(x => x.Sold = true);
                        if (Doc_type.Invoice == Doc_type)
                            db.Inv_sales_item_serial.AddRange(Serials);
                        else
                        {
                            List<Inv_receive_item_serial> PurSer = db
                                .Inv_receive_item_serial.ToList().Where(x => Serials.Any(z => z.Serial == x.Serial)).ToList();
                            PurSer.ForEach(x => x.Sold = true);
                        }
                    }

                    db.SaveChanges();
                }
                catch (Exception e)
                {

                }
                db.SaveChanges();
                return Json(new { AllService = AllService });
            }
            else
            {
                foreach (Inv_sales_invoice_items i in inv_po_items)
                {
                    //int InvItemId = db.Inv_receive_po_items.Where(x => x.Item_id == i.Item_id
                    //&& x.Receive_po_id == i.Po_id.Value).FirstOrDefault().Id;

                    ItemDetails ThisPoDetails = InvBus.CalcItemDetails(i.Item_id, i.Quantity, db.Inv_sales_invoice.Find(i.Sales_invoice_id).Store_id);
                    foreach (InvSalesPo ThisI in ThisPoDetails.Po_inv)
                    {
                        db.Inv_sales_receivs_pos.Add(new Inv_sales_receivs_pos
                        {
                            Receive_po_id = ThisI.Po_id,
                            Quantity = ThisI.Qty,
                            Item_id = ThisI.item_id,
                            Sales_id = i.Sales_invoice_id.Value
                        });

                    }
                    i.Unit_price = InvBus.CalcItemEqUnitPrice(i.Item_id, i.UOM_id.Value, i.Quantity, i.Unit_price);

                    i.Cost_price = (i.Cost_price / (decimal)InvBus.CalcItemEq(i.Item_id, i.UOM_id.Value, i.Quantity));

                }
                List<Inv_receive_item_serial> PurSer = db
                             .Inv_receive_item_serial.ToList().Where(x => Serials.Any(z => z.Serial == x.Serial)).ToList();
                PurSer.ForEach(x => x.Sold = true);
                db.Inv_sales_invoice_items.AddRange(inv_po_items);
                db.SaveChanges();
                try
                {
                    foreach (Inv_sales_invoice_items i in inv_po_items)
                    {
                        Serials.Where(x => x.Item_id == i.Item_id).ToList()
                            .ForEach(x =>
                            {
                                x.Sales_id = inv_po_items.FirstOrDefault().Sales_invoice_id.Value;
                                x.Item_id = i.Id;
                            });
                        List<Inv_receive_item_serial> IRI = db.Inv_receive_item_serial
                            .ToList().Where(x => Serials.Any(z => z.Serial == x.Serial))
                            .ToList();
                        IRI.ForEach(x => x.Sold = true);
                        db.Inv_sales_item_serial.AddRange(Serials);
                    }
                    db.SaveChanges();
                }
                catch
                {

                }

                return Json(new { AllService = AllService });
            }

        }

        public JsonResult GetItemsSerials(int ItemId, Doc_type Doc, int? PoId = null, bool Sales = false)
        {
            List<Seriales> Serials = new List<Seriales>();
            if (Sales && Doc == Doc_type.Return)
            {
                Serials = db.Inv_sales_item_serial.Include(x => x.Item).Where(x => x.Item.Item_id == ItemId && x.Sales_id == PoId.Value)
                    .ToList().Select(x => new Seriales
                    {
                        Serial = x.Serial,
                        //ExDate = x.Expiry_date,
                        SalesEx=x.Expiry_date?.ToShortDateString()
                    }).ToList();
            }
            else
            {
                if (PoId.HasValue && Doc != Doc_type.Return)
                {
                    Serials = db.Inv_receive_item_serial.Include(x => x.Item)
                        .Where(x => x.Item.Item_id == ItemId && x.Sold == false && x.Po_id == PoId.Value)
                        .Include(x => x.Expiry)
                        .ToList().Select(x => new Seriales
                        {
                            Serial = x.Serial,
                            ExDate = x.Expiry.Select(z => z.Date.ToShortDateString()).ToList()
                        }).ToList();
                }
                else
                {
                    Serials = db.Inv_receive_item_serial.Include(x => x.Item)
                        .Where(x => x.Item.Item_id == ItemId && x.Sold == false)
                        .ToList().Select(x => new Seriales
                        {
                            Serial = x.Serial,
                            ExDate = x.Expiry.Select(z => z.Date.ToShortDateString()).ToList()
                        }).ToList();
                }
            }

            return Json(Serials);
        }
        public JsonResult CheckSerial(string Serial, int? ItemId = null)
        {
            if (!ItemId.HasValue)
            {
                return Json(db.Inv_receive_item_serial.Any(x => x.Serial == Serial));
            }
            else
            {
                return Json(db.Inv_receive_item_serial.Include(x => x.Item).Any(x => x.Serial == Serial && x.Item.Item_id == ItemId));
            }
        }
        // POST: Inventory/Inv_receive_po_items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Item_id,UOM_id,Quantity,Unit_price,Total,Amount_system_currency,Discount,Net_amount,Table_vat_id,Table_vat_amount,Vat_id,Vat_amount,Deduct_id,Deduct_amount,Receive_po_id")] Inv_receive_po_items inv_receive_po_items)
        {
            if (ModelState.IsValid)
            {
                db.Inv_receive_po_items.Add(inv_receive_po_items);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Deduct_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Deduct_id);
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_receive_po_items.Item_id);
            ViewBag.Table_vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Table_vat_id);
            ViewBag.UOM_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_receive_po_items.UOM_id);
            ViewBag.Vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Vat_id);
            return View(inv_receive_po_items);
        }

        // GET: Inventory/Inv_receive_po_items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_receive_po_items inv_receive_po_items = db.Inv_receive_po_items.Find(id);
            if (inv_receive_po_items == null)
            {
                return HttpNotFound();
            }
            ViewBag.Deduct_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Deduct_id);
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_receive_po_items.Item_id);
            ViewBag.Table_vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Table_vat_id);
            ViewBag.UOM_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_receive_po_items.UOM_id);
            ViewBag.Vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Vat_id);
            return View(inv_receive_po_items);
        }

        // POST: Inventory/Inv_receive_po_items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Item_id,UOM_id,Quantity,Unit_price,Total,Amount_system_currency,Discount,Net_amount,Table_vat_id,Table_vat_amount,Vat_id,Vat_amount,Deduct_id,Deduct_amount,Receive_po_id")] Inv_receive_po_items inv_receive_po_items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_receive_po_items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Deduct_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Deduct_id);
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_receive_po_items.Item_id);
            ViewBag.Table_vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Table_vat_id);
            ViewBag.UOM_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_receive_po_items.UOM_id);
            ViewBag.Vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_receive_po_items.Vat_id);
            return View(inv_receive_po_items);
        }

        // GET: Inventory/Inv_receive_po_items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_receive_po_items inv_receive_po_items = db.Inv_receive_po_items.Find(id);
            if (inv_receive_po_items == null)
            {
                return HttpNotFound();
            }
            return View(inv_receive_po_items);
        }

        // POST: Inventory/Inv_receive_po_items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_receive_po_items inv_receive_po_items = db.Inv_receive_po_items.Find(id);
            db.Inv_receive_po_items.Remove(inv_receive_po_items);
            db.SaveChanges();
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
        public JsonResult GetItemQty(int ItemId, int SiteId)
        {
            return Json(db.Inv_receive_po_items.Where(x => x.Item_id == ItemId && x.Site_id == SiteId).Sum(x => x.Quantity));
        }
        public JsonResult GetPoItemQty(int ItemId, int Store)
        {
            return Json(InvBus.GetItemAvaliableStore(ItemId, Store));
        }
        public JsonResult GetItemQtyUnitPrice(int ItemId, int StoreId, float Qty)
        {
            ItemDetails I = InvBus.CalcItemDetails(ItemId, Qty, StoreId, null);
            return Json(I.CostPrice);
        }
        public PartialViewResult GetReceivePoItems(int Po)
        {
            List<Inv_receive_po_items> Inv_receive_po_items = db.Inv_receive_po_items.Include(x => x.Receive_po).Where(x => x.Receive_po_id == Po)
                .Include(i => i.Item)
                .Include(x => x.Item.Vat).Include(x => x.Item.Deduct_tax)
                .Include(x => x.Item.Deduct_tax.Select(z => z.Deduct))
                .Include(x => x.Item.Tbl_vat).Include(i => i.UOM)
                .Include(x => x.Item.Vat)
                .Include(x => x.Item.Item_gl_account).Include(x => x.Item.Item_gl_account.Select(z => z.Inventory)).ToList();
            Inv_receive_po_items.ForEach(x =>
            {
                x.Quantity = InvBus.CalcItemEq(x.Item.Id, x.UOM_id, x.Quantity, false);
                x.Unit_price = InvBus.CalcItemEqUnitPrice(x.Item_id, x.UOM_id.HasValue ? x.UOM_id.Value : x.Item.Unit_of_measure_id.Value, x.Quantity, x.Unit_price, true);
            });
            return PartialView(Inv_receive_po_items.ToList());
        }
        public PartialViewResult GetSalesItems(int SalesId)
        {
            List<Inv_sales_invoice_items> Sales = db.Inv_sales_invoice_items.Include(x => x.Sales_invoice).Where(x => x.Sales_invoice_id == SalesId).Include(i => i.Item)
                .Include(x => x.Item.Vat).Include(x => x.Item.Deduct_tax)
                .Include(x => x.Item.Deduct_tax.Select(z => z.Deduct))
                .Include(x => x.Item.Tbl_vat).Include(i => i.UOM)
                .Include(x => x.Item.Vat)
                .Include(x => x.Item.Item_gl_account).Include(x => x.Item.Item_gl_account.Select(z => z.Inventory)).ToList();

            //Sales.ForEach(x => x.Quantity = InvBus.CalcItemEq(x.Item.Id, x.UOM_id,x.Quantity, false));
            return PartialView(Sales.ToList());
        }
        public JsonResult AdjustPoItem(int PoId, int NewQty)
        {
            db.Inv_receive_po_items.Find(PoId).Quantity = NewQty;
            db.SaveChanges();
            return Json(1);
        }
    }


}
