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
    public class Inv_purchaseController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_purchase
        public ActionResult Index()
        {
            var inv_purchase = db.Inv_purchase.Include(i => i.Items);
            return View(inv_purchase.ToList());
        }

        // GET: Inventory/Inv_purchase/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase inv_purchase = db.Inv_purchase.Find(id);
            if (inv_purchase == null)
            {
                return HttpNotFound();
            }
            return View(inv_purchase);
        }

        // GET: Inventory/Inv_purchase/Create
        public ActionResult Create()
        {
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            return View();
        }

        // POST: Inventory/Inv_purchase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Request,Date,Within_days,Delivery_date,Send_to,Item_id,Quantity")] Inv_purchase inv_purchase)
        {
            if (ModelState.IsValid)
            {
                db.Inv_purchase.Add(inv_purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_purchase.Item_id);
            return View(inv_purchase);
        }

        // GET: Inventory/Inv_purchase/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase inv_purchase = db.Inv_purchase.Find(id);
            if (inv_purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_purchase.Item_id);
            return View(inv_purchase);
        }

        // POST: Inventory/Inv_purchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Request,Date,Within_days,Delivery_date,Send_to,Item_id,Quantity")] Inv_purchase inv_purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_purchase.Item_id);
            return View(inv_purchase);
        }

        // GET: Inventory/Inv_purchase/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase inv_purchase = db.Inv_purchase.Find(id);
            if (inv_purchase == null)
            {
                return HttpNotFound();
            }
            return View(inv_purchase);
        }

        // POST: Inventory/Inv_purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_purchase inv_purchase = db.Inv_purchase.Find(id);
            db.Inv_purchase.Remove(inv_purchase);
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
