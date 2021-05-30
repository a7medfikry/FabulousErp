using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.Models;
using FabulousErp;
using FabulousErp.Bussiness;
using FabulousModels.Inventory;
using WebGrease.Css.Extensions;

namespace Inventory.Controllers
{
    public class Inv_transfer_itemsController : Controller
    {
        private DBContext db = new DBContext();

        public ActionResult Transfer()
        {
            ViewBag.AllowTransfer = db.Inv_movment_GS.ToList().DefaultIfEmpty(new Inv_movment_GS { Allow_Transfer = false }).FirstOrDefault().Allow_Transfer.ToString().ToLower();

            ViewBag.From_store = ViewBag.To_store = new SelectList(db.Inv_store, "Id", "Store_id");
            ViewBag.From_site = ViewBag.To_site = new SelectList(new List<string>());
            ViewBag.Item_id = new SelectList(new List<string>());
            ViewBag.UOM_id = new SelectList(new List<string>());

            return View();
        }
        [HttpPost]
        public ActionResult Transfer(List<Inv_po_item_transfer> Items,List<Inv_receive_item_serial> Serial)
        {
            List<Gr_store> Grs = new List<Gr_store>();
            List<Go_store> Go = new List<Go_store>();
            //Inv_po_item_transfer Trans = new Inv_po_item_transfer();
            Inv_receive_po Po = new Inv_receive_po();
            Inv_sales_invoice Sales = new Inv_sales_invoice();
            int Max = db.Inv_movment_GS.FirstOrDefault().Next_transfer_no;
            db.Inv_movment_GS.FirstOrDefault().Next_transfer_no = Max + 1;
          //  List<PoAndSales> PoAndSales = new List<PoAndSales>();

            string Desc = "";
            if (string.IsNullOrWhiteSpace(Items.FirstOrDefault().Desc))
            {
                Desc = "Description";
            }
            else
            {
                Desc = Items.FirstOrDefault().Desc;
            }
            Inv_receive_po ToStorePo = new Inv_receive_po
            {
                Creation_date = DateTime.Now,
                Currency_id = FabulousErp.Business.GetCompanyId(),
                Doc_date = Items.FirstOrDefault().Transfer_date,
                Doc_type = Doc_type.Transfer,
                Gr_num = Max,
                Store_id = Items.FirstOrDefault().To_store_id,
                Site_id = Items.FirstOrDefault().To_site_id,
            };
            ToStorePo.Desc = Desc;
            db.Inv_receive_po.Add(ToStorePo);
            db.SaveChanges();
            List<Inv_receive_po_items> MainPoItems = new List<Inv_receive_po_items>();
            foreach (Inv_po_item_transfer Transfer in Items)
            {
                int ItemUOm = db.Inv_item.Find(Transfer.Item_id).Unit_of_measure_id.Value;
                ItemDetails ThisItems =
                    InvBus.CalcItemDetails(Transfer.Item_id, Transfer.Transfer_qty, Transfer.From_store_id, null, false, ItemUOm);

                List<Inv_receive_po_items> PoItems = new List<Inv_receive_po_items>();
                Inv_receive_po_items AddedItem = new Inv_receive_po_items();

                foreach (InvSalesPo i in ThisItems.Po_inv)
                {
                    Inv_receive_po_items ThuiItem = db.Inv_receive_po_items.FirstOrDefault(x => x.Item_id == i.item_id && x.Receive_po_id == i.Po_id);

                    Inv_receive_po_items MyItems = new Inv_receive_po_items
                    {
                        Deduct_amount= 0,//ThuiItem.Deduct_amount,
                        Deduct_id= ThuiItem.Deduct_id,
                        Discount= ThuiItem.Discount,
                        Fright= ThuiItem.Fright,
                        Item_id= ThuiItem.Item_id,
                        Item_name= ThuiItem.Item_name,
                        Net_amount= ThuiItem.Net_amount,
                        Quantity= InvBus.CalcItemEq(ThuiItem.Item_id, Transfer.UOM_id, Transfer.Transfer_qty),
                        Receive_po_id= ThuiItem.Receive_po_id,
                        Site_id= ThuiItem.Site_id,
                        Table_vat_amount= 0,//ThuiItem.Table_vat_amount,
                        Table_vat_id= ThuiItem.Table_vat_id,
                        Total_after_vat_table= 0,//ThuiItem.Total_after_vat_table,
                        Unit_price=Transfer.Total/ (decimal)InvBus.CalcItemEq(ThuiItem.Item_id, Transfer.UOM_id, Transfer.Transfer_qty)/* Transfer.Unit_price*/,
                        UOM_id= Transfer.UOM_id,
                        Vat_amount= 0,
                        Vat_id= ThuiItem.Vat_id
                    };

                    Transfer.Po_id = i.Po_id;
                    PoItems.Add((Inv_receive_po_items)MyItems);
                }
                foreach (List<Inv_receive_po_items> i in PoItems.GroupBy(x => x.Item_id).Select(x => x.ToList()).ToList())
                {
                    AddedItem = i.FirstOrDefault();
                    AddedItem.Quantity = InvBus.CalcItemEq(i.FirstOrDefault().Item_id, Transfer.UOM_id, Transfer.Transfer_qty);
                }
                Transfer.Transfer_num = Max;
                db.Inv_po_item_transfer.Add(Transfer);

                db.SaveChanges();


                MainPoItems.Add(AddedItem);

                //ToStorePo.Items = new List<Inv_receive_po_items>();
                //ToStorePo.Items.Add(AddedItem);


                db.SaveChanges();
                foreach (InvSalesPo i in ThisItems.Po_inv)
                {
                    db.Inv_transfer_relation.Add(new FabulousDB.Models.Inventory.Inv_transfer_relation
                    {
                        Item_id = i.item_id,
                        Main_po_id = i.Po_id,
                        Receive_po_id = ToStorePo.Id,
                        Quantity =InvBus.CalcItemEq(i.item_id,Transfer.UOM_id, i.Qty),
                        Transfer_id = Transfer.Id
                    });
                }
                db.SaveChanges();
                List<Inv_receive_item_serial> TransSerial= db.Inv_receive_item_serial.Where(x => x.Po_id == Transfer.Main_po).ToList()
                    .Where(x => Serial.Any(z => z.Serial == x.Serial)).ToList();
                TransSerial.ForEach(x => x.Po_id = ToStorePo.Id);
                db.SaveChanges();
              //  PoAndSales.Add(new Controllers.PoAndSales { Po = ToStorePo.Id });
            }
            MainPoItems.ForEach(x => x.Receive_po_id = ToStorePo.Id);
            db.Inv_receive_po_items.AddRange(MainPoItems);
            db.SaveChanges();
            return Json(ToStorePo.Id);
        }
      
        public ActionResult TrasferJv(int ItemId, float Qty, int? FromStore = null, int? FromSite = null, int? ToStore = null, int? ToSite = null)
        {
            int? FromAcc = 0;
            int? ToAcc = 0;
            decimal UnitPrice = InvBus.CalcItemDetails(ItemId, Qty, FromStore.Value).CostPrice;
            string FromAccName = "";
            string ToAccName = "";

            if (ToStore == null && ToSite != null)
            {
                ToStore = db.Inv_store_site.Find(ToSite).Store_id;
            }
            if (FromStore == null && FromSite != null)
            {
                FromAcc = db.Inv_store_site.Find(FromSite).Store_id;
            }

            ToAcc = db.Inv_store.Find(ToStore).Store_gl_account_id;
            FromAcc = db.Inv_store.Find(FromSite).Store_gl_account_id;
            FromAccName = db.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == FromAcc).AccountName;
            ToAccName = db.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == ToAcc).AccountName;
            string FromAccNameId = db.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == FromAcc).AccountID;
            string ToAccNameId = db.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == ToAcc).AccountID;

            string companyID = FabulousErp.Business.GetCompanyId();
            var Res = new JvHeaderDet
            {
                ShowHeader = new JvHead
                {
                    ISO = db.CurrenciesDefinition_Tables.Where(z => z.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" }).FirstOrDefault().ISOCode,
                    DocType = "SED"
                },
                ShowTransactions = new List<JvManyAccountFormate>
                {
                    new JvManyAccountFormate
                    {
                        AID=FromAcc.Value,
                        Debit= (decimal)Qty*UnitPrice,
                        Credit= 0,
                        AccountID =FromAccNameId,
                        AccountName =FromAccName,
                        Orginal_debit=(decimal)Qty*UnitPrice,
                        Orginal_curr="EGP"
                    },
                    new JvManyAccountFormate
                    {
                        AID=ToAcc.Value,
                        Debit=0,
                        Credit= (decimal)Qty*UnitPrice,
                        AccountID=ToAccNameId,
                        AccountName =ToAccName,
                        Orginal_credit=(decimal)Qty*UnitPrice,
                        Orginal_curr="EGP"
                    },
                }
            };
            return Json(Res);
        }
        // GET: Inventory/Inv_transfer_items

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult Inquery()
        {
            ViewBag.Transfer_num = new SelectList(db.Inv_po_item_transfer.GroupBy(x => x.Transfer_num).SelectMany(x => x.ToList())
                .Select(x => new {x.Transfer_num }).Distinct(), "Transfer_num", "Transfer_num");
            return View();
        }
        public ActionResult InqueryRes(int Transfer_num)
        {
            List<Inv_po_item_transfer> Trans = db.Inv_po_item_transfer.Where(x => x.Transfer_num == Transfer_num)
                .Include(x => x.Item)
                .Include(x => x.Relations)
                .Include(x => x.Relations.Select(z => z.Receive_po))
                .Include(x => x.Relations.Select(z => z.Receive_po.Items))
                .Include(x => x.UOM)
                .Include(x => x.From_store)
                .Include(x => x.From_site)
                .Include(x => x.To_store)
                .Include(x => x.To_site)
                .ToList();
            return View(Trans);
        }
    }
    public class Gr_store
    {
        public int Gr { get; set; }
        public int Store { get; set; }
        public int Po { get; set; }
    }
    public class Go_store
    {
        public int Go { get; set; }
        public int Store { get; set; }
        public int Sales { get; set; }
    } 
    public class PoAndSales
    {
        public int Po { get; set; }
        public int Sales { get; set; }
        public int AdjustNum { get; set; }
    }
}
