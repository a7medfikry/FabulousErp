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
using FabulousModels.Inventory;

namespace Inventory.Controllers
{
    public class Inv_sales_invoiceController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_sales_invoice
        public ActionResult Index()
        {
            var inv_sales_invoice = db.Inv_sales_invoice.Include(i => i.Currency).Include(i => i.Site).Include(i => i.Store);
            return View(inv_sales_invoice.ToList());
        }

        // GET: Inventory/Inv_sales_invoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales_invoice inv_sales_invoice = db.Inv_sales_invoice.Find(id);
            if (inv_sales_invoice == null)
            {
                return HttpNotFound();
            }
            return View(inv_sales_invoice);
        }

        // GET: Inventory/Inv_sales_invoice/Create
        public ActionResult Create()
        {
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "ISOCode");
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id");
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id");
            return View();
        }

        // POST: Inventory/Inv_sales_invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Doc_type,Doc_no,Batch_id,Transaction_date,Posting_date,Sales_person,Currency_id,Customer_id,So_no,Store_id,Site_id")] Inv_sales_invoice inv_sales_invoice)
        {
            if (ModelState.IsValid)
            {
                db.Inv_sales_invoice.Add(inv_sales_invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "ISOCode", inv_sales_invoice.Currency_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id", inv_sales_invoice.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_sales_invoice.Store_id);
            return View(inv_sales_invoice);
        }

        // GET: Inventory/Inv_sales_invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales_invoice inv_sales_invoice = db.Inv_sales_invoice.Find(id);
            if (inv_sales_invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "ISOCode", inv_sales_invoice.Currency_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id", inv_sales_invoice.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_sales_invoice.Store_id);
            return View(inv_sales_invoice);
        }

        // POST: Inventory/Inv_sales_invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Doc_type,Doc_no,Batch_id,Transaction_date,Posting_date,Sales_person,Currency_id,Customer_id,So_no,Store_id,Site_id")] Inv_sales_invoice inv_sales_invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_sales_invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables, "CurrencyID", "ISOCode", inv_sales_invoice.Currency_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id", inv_sales_invoice.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_sales_invoice.Store_id);
            return View(inv_sales_invoice);
        }

        // GET: Inventory/Inv_sales_invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales_invoice inv_sales_invoice = db.Inv_sales_invoice.Find(id);
            if (inv_sales_invoice == null)
            {
                return HttpNotFound();
            }
            return View(inv_sales_invoice);
        }

        // POST: Inventory/Inv_sales_invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_sales_invoice inv_sales_invoice = db.Inv_sales_invoice.Find(id);
            db.Inv_sales_invoice.Remove(inv_sales_invoice);
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
        public JsonResult GetSalesItemsDetils(int SalesId,int ItemId)
        {
            Inv_sales_invoice_items I = db.Inv_sales_invoice_items.Where(x => x.Sales_invoice_id == SalesId && x.Item_id == ItemId)
                       .Include(x=>x.Sales_invoice.Po_recive)
                       .FirstOrDefault();

           
            decimal CostPrice = db.Inv_sales_invoice_items.FirstOrDefault(x=>x.Item_id==ItemId&&x.Sales_invoice_id
            == SalesId).Cost_price.Value;
            
            return Json(new { Qty =InvBus.CalcItemEq(ItemId,I.UOM_id, I.Quantity), Unit_price=  InvBus.CalcItemEqUnitPrice(I.Item_id, I.UOM_id.Value,I.Quantity, I.Unit_price,true), I.Total, CostPrice= (decimal)InvBus.CalcItemEq(I.Item_id, I.UOM_id.Value,I.Quantity)*CostPrice,I.Discount,I.Fright,I.UOM_id }) ;
        }
        public JsonResult GetCustomerInvoice(int CustomerId,bool IsInstallment=false)
        {
            IEnumerable<Inv_sales_invoice> Invs =CalcCustomerInvoice(CustomerId);

            if (!IsInstallment)
            {
                return Json(Invs.Select(x => new { x.Id, 
                    Trx = x.Receivable.Trans_doc_type.Trx_num }).ToList());
            }
            else
            {
                return Json(Invs
                    .Where(x => !x.Installment_contract_invoice.Any(z => z.Invoice_id == x.Id))
                    .Select(x => new { x.Id,
                        Trx = x.Receivable.Trans_doc_type.Trx_num }).ToList());
            }


            //List<InvoiceItems> AvItem = InvBus.GetItemAvaliableByCustomer(CustomerId);
            //AvItem.ForEach(x => x.Sales_items = null);
            //return Json(AvItem);

        }

        public static List<Inv_sales_invoice> CalcCustomerInvoice(int CustomerId)
        {
           return MyDbContext.Instance.Inv_sales_invoice.Include(x => x.Receivable)
                            .Include(x => x.Receivable.Trans_doc_type)
                            .Include(x => x.Inv_sales_item.Select(z => z.Serials))
                            .Include(x => x.Installment_contract_invoice)
                           .Where(x => x.Receivable.Vendor_id == CustomerId).ToList();
        } 
        public static List<Inv_sales_invoice> CalcCustomerInvoice()
        {
            return MyDbContext.Instance.Inv_sales_invoice.Include(x => x.Receivable)
                            .Include(x => x.Receivable.Trans_doc_type)
                            .Include(x => x.Inv_sales_item.Select(z => z.Serials))
                            .Include(x => x.Installment_contract_invoice).ToList();
        }
        public static dynamic CustomerInvoiceTrx()
        {
            return MyDbContext.Instance.Inv_sales_invoice.Include(x => x.Receivable)
                            .Include(x => x.Receivable.Trans_doc_type)
                            .Include(x => x.Inv_sales_item.Select(z => z.Serials))
                            .Include(x => x.Installment_contract_invoice).ToList().Select(x => new {
                             Id=    x.Id, 
                             Trx = x.Receivable!=null? x.Receivable.Trans_doc_type.Trx_num:InvBus.GetFullPayTrxNum(x.Id)
                                }).ToList();
        }
        public ActionResult GetSalesInvoice(int Id)
        {
            Inv_sales_invoice InvSales = db.Inv_sales_invoice.Where(x => x.Id == Id).Include(x => x.Store)
                   .Include(x => x.Site)
                   .Include(x => x.Profit_user)
                   .Include(x => x.Receivable)
                   .Include(x => x.Receivable.Trans_doc_type)
                   .Include(x => x.Receivable.Vendor)
                   .Include(x => x.Shipping_method)
                   .Include(x => x.Payment_terms).FirstOrDefault();
            ViewBag.Id = Id;
            ViewBag.PostringNumber = InvSales.Posting_number;
            ViewBag.Jn =Business.GetJournalEntry(InvSales.Posting_number);
            return View(InvSales);
        }
        public JsonResult GetSalesCostPrice(int ItemId,int StoreId)
        {
            return Json(db.Inv_receive_po_items.Include(x => x.Receive_po)
                .Where(x => x.Receive_po.Doc_type != Doc_type.Return && x.Item_id == ItemId
                && x.Receive_po.Store_id==StoreId)
                .ToList().LastOrDefault().Unit_price);
        }
        public JsonResult GetInvTotal(int[] Inv)
        {
          return  Json(db.Inv_sales_invoice.Where(x => Inv.Any(z => z == x.Id))
                 .Include(x => x.Inv_sales_item).Sum(x => x.Inv_sales_item.Sum(z => z.Total))
                 );
        }
    }
}
