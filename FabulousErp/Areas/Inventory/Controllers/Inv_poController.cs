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

namespace Inventory.Controllers
{
    public class Inv_poController : Controller
    {
        private DBContext db = new DBContext();
        #region CRUD
        // GET: Inventory/Inv_po
        public ActionResult Index()
        {
            var inv_po = db.Inv_po.Include(i => i.Currency).Include(i => i.Pr_no).Include(i => i.Vendore);
            return View(inv_po.ToList());
        }
        // GET: Inventory/Inv_po/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po inv_po = db.Inv_po.Find(id);
            if (inv_po == null)
            {
                return HttpNotFound();
            }
            return View(inv_po);
        }
        string CompanyId = FabulousErp.Business.GetCompanyId();

        // GET: Inventory/Inv_po/Create
        public ActionResult Create()
        {
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x=>x.CompanyID== CompanyId), "CurrencyID", "ISOCode");
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "PR_no");
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id");
            return View();
        }

        // POST: Inventory/Inv_po/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Po_type,Pr_no_id,Date,Vendore_id,Currency_id,System_rate,Transaction_rate,Difference")] Inv_po inv_po)
        {
            if (ModelState.IsValid)
            {
                int Max = 1;
                try
                {
                    Max = db.Inv_po.Max(x => x.Po_num) + 1;
                }
                catch
                {

                }
                inv_po.Po_num = Max;
                db.Inv_po.Add(inv_po);
                db.SaveChanges();
                return Json(new {Id= inv_po.Id,Po_num=Max });
            }

            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode", inv_po.Currency_id);
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "PR_no", inv_po.Pr_no_id);
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_po.Vendore_id);
            return View(inv_po);
        }

        // GET: Inventory/Inv_po/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po inv_po = db.Inv_po.Find(id);
            if (inv_po == null)
            {
                return HttpNotFound();
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode", inv_po.Currency_id);
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "PR_no", inv_po.Pr_no_id);
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_po.Vendore_id);
            return View(inv_po);
        }

        // POST: Inventory/Inv_po/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Po_type,Pr_no_id,Date,Vendore_id,Currency_id,System_rate,Transaction_rate,Difference")] Inv_po inv_po)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_po).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Currency_id = new SelectList(db.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyId), "CurrencyID", "ISOCode", inv_po.Currency_id);
            ViewBag.Pr_no_id = new SelectList(db.Inv_purchase_request, "Id", "PR_no", inv_po.Pr_no_id);
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_po.Vendore_id);
            return View(inv_po);
        }

        // GET: Inventory/Inv_po/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po inv_po = db.Inv_po.Find(id);
            if (inv_po == null)
            {
                return HttpNotFound();
            }
            return View(inv_po);
        }

        // POST: Inventory/Inv_po/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_po inv_po = db.Inv_po.Find(id);
            db.Inv_po.Remove(inv_po);
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
        #endregion
        public JsonResult GetPoItems(int? StoreId=null,int? SiteId=null,bool GetAll=false)
        {
            List<ItemD> List = new List<ItemD>();
            if (!GetAll)
            {
                if (SiteId != null)
                {
                    List = db.Inv_receive_po_items.Include(x => x.Item)
                        .Include(x => x.Receive_po)
                       .Where(x => x.Receive_po.Site_id == SiteId)
                       .Select(x => new ItemD { Id = x.Item_id, Item_id = x.Item.Item_id, Site_id = x.Site_id, Po_id = x.Receive_po_id }).ToList();
                }
                else if (StoreId != null)
                {
                    List = db.Inv_receive_po_items.Include(x => x.Item)
                        .Include(x => x.Receive_po)
                        .Where(x => x.Receive_po.Store_id == StoreId)
                        .Select(x => new ItemD { Id = x.Item_id, Item_id = x.Item.Item_id, Site_id = x.Site_id, Po_id = x.Receive_po_id }).ToList();
                }
            }
            else
            {
                if (SiteId != null)
                {
                    List = db.Inv_item
                        .Include(x=>x.Item_store_site)
                       .Where(x => x.Item_store_site.Any(z=>z.Site_id== SiteId))
                       .Select(x => new ItemD { Id = x.Id, Item_id = x.Item_id, Site_id = SiteId, Po_id = 0 }).ToList();
                }
                else if (StoreId != null)
                {
                    List = db.Inv_item
                        .Include(x => x.Item_store_site)
                       .Where(x => x.Item_store_site.Any(z => z.Store_id == StoreId))
                        .Select(x => new ItemD { Id = x.Id, Item_id = x.Item_id, Site_id = SiteId, Po_id = 0 }).ToList();
                }
            }
           
            return Json(List.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()));
        }
    }
    public class ItemD
    {
        public int Id { get; set; }
        public string Item_id { get; set; }
        public int? Site_id { get; set; }
        public int? Po_id { get; set; }
    }
}
