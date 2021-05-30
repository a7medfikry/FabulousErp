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
    public class Inv_salesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_sales
        public ActionResult Index()
        {
            var inv_sales = db.Inv_sales.Include(i => i.Items);
            return View(inv_sales.ToList());
        }

        // GET: Inventory/Inv_sales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales inv_sales = db.Inv_sales.Find(id);
            if (inv_sales == null)
            {
                return HttpNotFound();
            }
            return View(inv_sales);
        }

        // GET: Inventory/Inv_sales/Create
        public ActionResult Create()
        {
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            return View();
        }

        // POST: Inventory/Inv_sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Request,Date,Sales_person,Within_days,Item_id,Quantity,Delivery_Date,Request_from")] Inv_sales inv_sales)
        {
            if (ModelState.IsValid)
            {
                db.Inv_sales.Add(inv_sales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_sales.Item_id);
            return View(inv_sales);
        }

        // GET: Inventory/Inv_sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales inv_sales = db.Inv_sales.Find(id);
            if (inv_sales == null)
            {
                return HttpNotFound();
            }
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_sales.Item_id);
            return View(inv_sales);
        }

        // POST: Inventory/Inv_sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Request,Date,Sales_person,Within_days,Item_id,Quantity,Delivery_Date,Request_from")] Inv_sales inv_sales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_sales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_sales.Item_id);
            return View(inv_sales);
        }

        // GET: Inventory/Inv_sales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales inv_sales = db.Inv_sales.Find(id);
            if (inv_sales == null)
            {
                return HttpNotFound();
            }
            return View(inv_sales);
        }

        // POST: Inventory/Inv_sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_sales inv_sales = db.Inv_sales.Find(id);
            db.Inv_sales.Remove(inv_sales);
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
