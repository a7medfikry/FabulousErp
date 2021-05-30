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
    public class Inv_purchase_request_itemsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_purchase_request_items
        public ActionResult Index()
        {
            var inv_purchase_request_items = db.Inv_purchase_request_items.Include(i => i.item).Include(i => i.Purchase_request);
            return View(inv_purchase_request_items.ToList());
        }

        // GET: Inventory/Inv_purchase_request_items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase_request_items inv_purchase_request_items = db.Inv_purchase_request_items.Find(id);
            if (inv_purchase_request_items == null)
            {
                return HttpNotFound();
            }
            return View(inv_purchase_request_items);
        }

        // GET: Inventory/Inv_purchase_request_items/Create
        public ActionResult Create(int? Id)
        {
            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            ViewBag.Purchase_request_id = new SelectList(db.Inv_purchase_request, "Id", "Id");
            return View();
        }

        // POST: Inventory/Inv_purchase_request_items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,item_id,Quntaty,Purchase_request_id")] Inv_purchase_request_items inv_purchase_request_items)
        {
            if (ModelState.IsValid)
            {
                db.Inv_purchase_request_items.Add(inv_purchase_request_items);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_purchase_request_items.item_id);
            ViewBag.Purchase_request_id = new SelectList(db.Inv_purchase_request, "Id", "Id", inv_purchase_request_items.Purchase_request_id);
            return View(inv_purchase_request_items);
        }

        // GET: Inventory/Inv_purchase_request_items/Edit/5
        public ActionResult Edit(int? id)
        {
            List<Inv_purchase_request_items> inv_purchase_request_items = db.Inv_purchase_request_items
                .Where(x=>x.Purchase_request_id==id).ToList();
            
            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            ViewBag.Purchase_request_id = new SelectList(db.Inv_purchase_request, "Id", "Id");
            return View(inv_purchase_request_items);
        }

        // POST: Inventory/Inv_purchase_request_items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,item_id,Quntaty,Purchase_request_id")] Inv_purchase_request_items inv_purchase_request_items)
        {
            if (ModelState.IsValid)
            {
                Inv_purchase_request_items I = db.Inv_purchase_request_items.Find(inv_purchase_request_items.Id);
                I.Quntaty = inv_purchase_request_items.Quntaty;
                I.item_id = inv_purchase_request_items.item_id;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_purchase_request_items.item_id);
            ViewBag.Purchase_request_id = new SelectList(db.Inv_purchase_request, "Id", "Id", inv_purchase_request_items.Purchase_request_id);
            return View(inv_purchase_request_items);
        }

        // GET: Inventory/Inv_purchase_request_items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase_request_items inv_purchase_request_items = db.Inv_purchase_request_items.Find(id);
            if (inv_purchase_request_items == null)
            {
                return HttpNotFound();
            }
            return View(inv_purchase_request_items);
        }

        // POST: Inventory/Inv_purchase_request_items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_purchase_request_items inv_purchase_request_items = db.Inv_purchase_request_items.Find(id);
            db.Inv_purchase_request_items.Remove(inv_purchase_request_items);
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
