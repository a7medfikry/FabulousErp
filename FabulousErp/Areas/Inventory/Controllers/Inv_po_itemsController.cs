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
    public class Inv_po_itemsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_po_items
        public ActionResult Index()
        {
            var inv_po_items = db.Inv_po_items.Include(i => i.Deduct).Include(i => i.Inv_po).Include(i => i.Item).Include(i => i.Table_vat).Include(i => i.UOM).Include(i => i.Vat);
            return View(inv_po_items.ToList());
        }
       
        // GET: Inventory/Inv_po_items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po_items inv_po_items = db.Inv_po_items.Find(id);
            if (inv_po_items == null)
            {
                return HttpNotFound();
            }
            return View(inv_po_items);
        }

        // GET: Inventory/Inv_po_items/Create
        public ActionResult Create()
        {
            ViewBag.Po_id = new SelectList(db.Inv_po, "Id", "Currency_id");
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");

            ViewBag.Deduct_id = db.C_TaxSetting_Tables.ToList();//, "CT_ID", "C_Taxcode");
            ViewBag.Table_vat_id = db.C_TaxSetting_Tables.ToList();//, "CT_ID", "C_Taxcode");
            ViewBag.Vat_id = db.C_TaxSetting_Tables.ToList();//, "CT_ID", "C_Taxcode");

            ViewBag.UOM_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id");
            return View();
        }

        // POST: Inventory/Inv_po_items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Item_id,UOM_id,Quantity,Unit_price,Total,Amount_system_currency,Discount,Net_amount,Table_vat_id,Table_vat_amount,Vat_id,Vat_amount,Deduct_id,Deduct_amount,Po_id")] Inv_po_items inv_po_items)
        {
            if (ModelState.IsValid)
            {
                db.Inv_po_items.Add(inv_po_items);
                db.SaveChanges();
                return Json(1);
            }

            ViewBag.Deduct_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Deduct_id);
            ViewBag.Po_id = new SelectList(db.Inv_po, "Id", "Currency_id", inv_po_items.Po_id);
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_po_items.Item_id);
            ViewBag.Table_vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Table_vat_id);
            ViewBag.UOM_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_po_items.UOM_id);
            ViewBag.Vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Vat_id);
            return View(inv_po_items);
        }
        public JsonResult CreateList(List<Inv_po_items> inv_po_items)
        {
            db.Inv_po_items.AddRange(inv_po_items);
            db.SaveChanges();
            return Json(1);
        }
        // GET: Inventory/Inv_po_items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po_items inv_po_items = db.Inv_po_items.Find(id);
            if (inv_po_items == null)
            {
                return HttpNotFound();
            }
            ViewBag.Deduct_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Deduct_id);
            ViewBag.Po_id = new SelectList(db.Inv_po, "Id", "Currency_id", inv_po_items.Po_id);
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_po_items.Item_id);
            ViewBag.Table_vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Table_vat_id);
            ViewBag.UOM_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_po_items.UOM_id);
            ViewBag.Vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Vat_id);
            return View(inv_po_items);
        }

        // POST: Inventory/Inv_po_items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Item_id,UOM_id,Quantity,Unit_price,Total,Amount_system_currency,Discount,Net_amount,Table_vat_id,Table_vat_amount,Vat_id,Vat_amount,Deduct_id,Deduct_amount,Po_id")] Inv_po_items inv_po_items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_po_items).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Deduct_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Deduct_id);
            ViewBag.Po_id = new SelectList(db.Inv_po, "Id", "Currency_id", inv_po_items.Po_id);
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_po_items.Item_id);
            ViewBag.Table_vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Table_vat_id);
            ViewBag.UOM_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", inv_po_items.UOM_id);
            ViewBag.Vat_id = new SelectList(db.C_TaxSetting_Tables, "CT_ID", "C_Taxcode", inv_po_items.Vat_id);
            return View(inv_po_items);
        }

        // GET: Inventory/Inv_po_items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po_items inv_po_items = db.Inv_po_items.Find(id);
            if (inv_po_items == null)
            {
                return HttpNotFound();
            }
            return View(inv_po_items);
        }

        // POST: Inventory/Inv_po_items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_po_items inv_po_items = db.Inv_po_items.Find(id);
            db.Inv_po_items.Remove(inv_po_items);
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
    }
}
