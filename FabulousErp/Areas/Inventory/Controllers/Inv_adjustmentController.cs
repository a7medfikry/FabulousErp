using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousModels.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Inventory.Controllers
{
    public class Inv_adjustmentController : Controller
    {
        // GET: Inventory/Inv_po_adjustment
        DBContext db = new DBContext();
        public ActionResult Index()
        {
            ViewBag.Po = new SelectList(db.Inv_receive_po, "Id", "Gr_num");
            return View();
        }


        public ActionResult Adjustment()
        {
            ViewBag.AllowAdjust = db.Inv_movment_GS.ToList().DefaultIfEmpty(new Inv_movment_GS { Allow_adjustment = false }).FirstOrDefault().Allow_adjustment.ToString().ToLower();
            ViewBag.Site_id = new SelectList(new List<string> { });
            ViewBag.Store_id = new SelectList(db.Inv_store.ToList(), "Id", "Store_id");

            ViewBag.Item_id = new SelectList(new List<string>());
            ViewBag.UOM_id = new SelectList(new List<string>());

            return View();
        }
        [HttpPost]
        public ActionResult PostAdjustment(
                List<Inv_item_adjustment> Items
            //, List<Inv_receive_item_serial> Serial
            )
        {
            int AdjustNum = 0;
            if (!InvBus.HasGs())
            {
                return Json(-1);
            }
            List<Gr_store> Grs = new List<Gr_store>();
            List<Go_store> Go = new List<Go_store>();
            //Inv_item_adjustment Trans = new Inv_item_adjustment();
            Inv_receive_po Po = new Inv_receive_po();
            Inv_sales_invoice Sales = new Inv_sales_invoice();
            int Max = db.Inv_movment_GS.FirstOrDefault().Next_transfer_no;
            db.Inv_movment_GS.FirstOrDefault().Next_transfer_no = Max + 1;

            foreach (Inv_item_adjustment Adjustment in Items)
            {
                int ItemUOm = db.Inv_item.Find(Adjustment.Item_id).Unit_of_measure_id.Value;

                //ItemDetails ThisItems =
                //Inv_receive_po_itemsController.CalcItemDetails(Transfer.Item_id, Transfer.Transfer_qty, Transfer.From_store_id, null, false, ItemUOm);
                //List<Inv_receive_po_items> PoItems = new List<Inv_receive_po_items>();

                string Desc = "";

                Inv_receive_po InvItem = new Inv_receive_po
                {
                    Creation_date = DateTime.Now,
                    Currency_id = FabulousErp.Business.GetCompanyId(),
                    Doc_date = Adjustment.Transaction_date,
                    Doc_type = Doc_type.Adjustment,
                    Gr_num = Max,
                    Store_id = Adjustment.Store_id,
                    Site_id = Adjustment.Site_id,
                    Transaction_date = Adjustment.Transaction_date,
                    Posting_date = Adjustment.Posting_date,
                };
                InvItem.Items = new List<Inv_receive_po_items>();
                Desc = Adjustment.Desc;
                if (string.IsNullOrWhiteSpace(Desc))
                {
                    Desc = "Description";
                }
                InvItem.Desc = Desc;
                db.Inv_receive_po.Add(InvItem);
                db.SaveChanges();
                if (Adjustment.Earn_qty != 0)
                {
                    Inv_receive_po_items EarnedItem = new Inv_receive_po_items
                    {
                        Cost_price = Adjustment.Earn_amount,
                        //Deduct_amount = ThuiItem.Deduct_amount,
                        //Deduct_id = ThuiItem.Deduct_id,
                        //Discount = ThuiItem.Discount,
                        //Fright = ThuiItem.Fright,
                        Item_id = Adjustment.Item_id,
                        //Item_name = ThuiItem.Item_name,
                        //Net_amount = ThuiItem.Net_amount,
                        Quantity = InvBus.CalcItemEq(Adjustment.Item_id, Adjustment.UOM_id, Adjustment.Earn_qty),
                        // Receive_po_id = ThuiItem.Receive_po_id,
                        Site_id = Adjustment.Site_id,
                        // Table_vat_amount = ThuiItem.Table_vat_amount,
                        // Table_vat_id = ThuiItem.Table_vat_id,
                        //Total = (decimal)Adjustment.Earn_qty* Adjustment.Earn_amount,
                        // Total_after_vat_table = ThuiItem.Total_after_vat_table,
                        Unit_price = Adjustment.Earn_amount / (decimal)InvBus.CalcItemEq(Adjustment.Item_id, Adjustment.UOM_id, Adjustment.Earn_qty),
                        UOM_id = Adjustment.UOM_id,
                        //Vat_amount = ThuiItem.Vat_amount,
                        //Vat_id = ThuiItem.Vat_id
                    };


                    InvItem.Items.Add(EarnedItem);

                }
                Adjustment.Po_id = InvItem.Id;

                int ThisAdjust = db.Inv_movment_GS.FirstOrDefault().Next_adjustment_no;
                Adjustment.Adjustment_num = ThisAdjust;
                db.Inv_movment_GS.FirstOrDefault().Next_adjustment_no += 1;
                db.SaveChanges();





                ItemDetails ThisItems =
                    InvBus.CalcItemDetails(
                        Adjustment.Item_id,
                          InvBus.CalcItemEq(Adjustment.Item_id, Adjustment.UOM_id, Adjustment.Damage_qty + Adjustment.Loss_qty),
                        Adjustment.Store_id,
                        null, false, ItemUOm);

                db.Inv_item_adjustment.Add(Adjustment);
                AdjustNum = Adjustment.Adjustment_num;
                db.SaveChanges();
                foreach (InvSalesPo i in ThisItems.Po_inv)
                {
                    db.Inv_adjustment_relation.Add(
                        new Inv_adjustment_relation
                        {
                            Item_id = i.item_id,
                            Main_po_id = i.Po_id,
                            Receive_po_id = InvItem.Id,
                            Quantity = i.Qty,
                            Adjustment_id = Adjustment.Id
                        });
                }
                db.SaveChanges();
                //List<Inv_receive_item_serial> TransSerial = db.Inv_receive_item_serial.Where(x => x.Po_id == Transfer.Main_po).ToList()
                //    .Where(x => Serial.Any(z => z.Serial == x.Serial)).ToList();
                //TransSerial.ForEach(x => x.Po_id = ToStorePo.Id);
                //db.SaveChanges();
            }
            List<PoAndSales> PoAndSales = new List<PoAndSales>();
            return Json(new { AdjustNum = AdjustNum });
        }
    }
}