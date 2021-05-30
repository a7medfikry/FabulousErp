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
    public class Inv_quotation_requestController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_quotation_request
        public ActionResult Index()
        {
            var inv_quotation_request = db.Inv_quotation_request.Include(x=>x.Po).Include(i => i.Vendore);
            return View(inv_quotation_request.ToList());
        }

        // GET: Inventory/Inv_quotation_request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_request inv_quotation_request = db.Inv_quotation_request.Find(id);
            if (inv_quotation_request == null)
            {
                return HttpNotFound();
            }
            return View(inv_quotation_request);
        }

        // GET: Inventory/Inv_quotation_request/Create
        public ActionResult Create(int? Id)
        {
            Inv_quotation_request I = db.Inv_quotation_request
                .Where(x => x.Id == Id).ToList().DefaultIfEmpty(new Inv_quotation_request { })
                .FirstOrDefault();

            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            ViewBag.Po_id = new SelectList(db.Inv_purchase_request, "Id", "Pr_number",I.Po_id);
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", I.Vendore_id);
            return View(I);
        }

        // POST: Inventory/Inv_quotation_request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Request_for_qut_no,Date,Pr_no_id,Within_days,Item_id,Quantity,Vendore_id,Delivery_Date,Request_from")] Inv_quotation_request inv_quotation_request)
        {
            if (ModelState.IsValid)
            {
                db.Inv_quotation_request.Add(inv_quotation_request);
                db.SaveChanges();
                return Json(inv_quotation_request.Id);
            }

            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_quotation_request.Vendore_id);
            return View(inv_quotation_request);
        }

        // GET: Inventory/Inv_quotation_request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_request inv_quotation_request = db.Inv_quotation_request.Find(id);
            if (inv_quotation_request == null)
            {
                return HttpNotFound();
            }
            ViewBag.Recive_po_id = new SelectList(db.Inv_receive_po, "Id", "Gr_num");
            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_quotation_request.Vendore_id);
            return View(inv_quotation_request);
        }

        // POST: Inventory/Inv_quotation_request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Request_for_qut_no,Date,Pr_no_id,Within_days,Item_id,Quantity,Vendore_id,Delivery_Date,Request_from")] Inv_quotation_request inv_quotation_request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_quotation_request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Recive_po_id = new SelectList(db.Inv_receive_po, "Id", "Gr_num");

            ViewBag.Vendore_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", inv_quotation_request.Vendore_id);
            return View(inv_quotation_request);
        }

        // GET: Inventory/Inv_quotation_request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_quotation_request inv_quotation_request = db.Inv_quotation_request.Find(id);
            if (inv_quotation_request == null)
            {
                return HttpNotFound();
            }
            return View(inv_quotation_request);
        }

        // POST: Inventory/Inv_quotation_request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_quotation_request inv_quotation_request = db.Inv_quotation_request.Find(id);
            db.Inv_quotation_request.Remove(inv_quotation_request);
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
