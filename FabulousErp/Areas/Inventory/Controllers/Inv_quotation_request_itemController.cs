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
    public class Inv_quotation_request_itemController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_quotation_request_item
        public ActionResult Index()
        {
            var inv_quotation_request_item = db.Inv_quotation_request_item.Include(i => i.item).Include(i => i.Quotation_request);
            return View(inv_quotation_request_item.ToList());
        }

        // GET: Inventory/Inv_quotation_request_item/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_request_item inv_quotation_request_item = db.Inv_quotation_request_item.Find(id);
            if (inv_quotation_request_item == null)
            {
                return HttpNotFound();
            }
            return View(inv_quotation_request_item);
        }

        // GET: Inventory/Inv_quotation_request_item/Create
        public ActionResult Create()
        {
            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            ViewBag.Quotation_request_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no");
            return View();
        }

        // POST: Inventory/Inv_quotation_request_item/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,item_id,Quntaty,Quotation_request_id")] Inv_quotation_request_item inv_quotation_request_item)
        {
            if (ModelState.IsValid)
            {
                db.Inv_quotation_request_item.Add(inv_quotation_request_item);
                db.SaveChanges();
                return Json(1);
            }

            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_quotation_request_item.item_id);
            ViewBag.Quotation_request_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no", inv_quotation_request_item.Quotation_request_id);
            return View(inv_quotation_request_item);
        }

        // GET: Inventory/Inv_quotation_request_item/Edit/5
        public ActionResult Edit(int? Id)
        {
            List<Inv_quotation_request_item> Inv_quotation_request_item = db.Inv_quotation_request_item
                .Where(x => x.Quotation_request_id == Id).ToList();

            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            ViewBag.Quotation_request_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no");
            return View(Inv_quotation_request_item);
        }

        // POST: Inventory/Inv_quotation_request_item/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,item_id,Quntaty,Quotation_request_id")] Inv_quotation_request_item inv_quotation_request_item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_quotation_request_item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_quotation_request_item.item_id);
            ViewBag.Quotation_request_id = new SelectList(db.Inv_quotation_request, "Id", "Request_for_qut_no", inv_quotation_request_item.Quotation_request_id);
            return View(inv_quotation_request_item);
        }

        // GET: Inventory/Inv_quotation_request_item/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_request_item inv_quotation_request_item = db.Inv_quotation_request_item.Find(id);
            if (inv_quotation_request_item == null)
            {
                return HttpNotFound();
            }
            return View(inv_quotation_request_item);
        }

        // POST: Inventory/Inv_quotation_request_item/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_quotation_request_item inv_quotation_request_item = db.Inv_quotation_request_item.Find(id);
            db.Inv_quotation_request_item.Remove(inv_quotation_request_item);
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
