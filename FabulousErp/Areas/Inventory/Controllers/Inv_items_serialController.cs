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
    public class Inv_items_serialController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_items_serial
        public ActionResult Index()
        {
            var inv_items_serial = db.Inv_items_serial.Include(i => i.Item);
            return View(inv_items_serial.ToList());
        }

        // GET: Inventory/Inv_items_serial/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_items_serial inv_items_serial = db.Inv_items_serial.Find(id);
            if (inv_items_serial == null)
            {
                return HttpNotFound();
            }
            return View(inv_items_serial);
        }

        // GET: Inventory/Inv_items_serial/Create
        public ActionResult Create()
        {
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            return View();
        }

        // POST: Inventory/Inv_items_serial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Item_id,Desc,Lot_Desc,Lot_id,Quantity,Type,Serial_number_from,Serial_number_To")] Inv_items_serial inv_items_serial)
        {
            if (ModelState.IsValid)
            {
                db.Inv_items_serial.Add(inv_items_serial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_items_serial.Item_id);
            return View(inv_items_serial);
        }

        // GET: Inventory/Inv_items_serial/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_items_serial inv_items_serial = db.Inv_items_serial.Find(id);
            if (inv_items_serial == null)
            {
                return HttpNotFound();
            }
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_items_serial.Item_id);
            return View(inv_items_serial);
        }

        // POST: Inventory/Inv_items_serial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Item_id,Desc,Lot_Desc,Lot_id,Quantity,Type,Serial_number_from,Serial_number_To")] Inv_items_serial inv_items_serial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_items_serial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_items_serial.Item_id);
            return View(inv_items_serial);
        }

        // GET: Inventory/Inv_items_serial/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_items_serial inv_items_serial = db.Inv_items_serial.Find(id);
            if (inv_items_serial == null)
            {
                return HttpNotFound();
            }
            return View(inv_items_serial);
        }

        // POST: Inventory/Inv_items_serial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_items_serial inv_items_serial = db.Inv_items_serial.Find(id);
            db.Inv_items_serial.Remove(inv_items_serial);
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
