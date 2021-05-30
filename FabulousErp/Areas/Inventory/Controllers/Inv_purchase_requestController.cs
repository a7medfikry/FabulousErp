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
    public class Inv_purchase_requestController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_purchase_request
        public ActionResult Index()
        {
            var inv_purchase_request = db.Inv_purchase_request;
            return View(inv_purchase_request.ToList());
        }

        // GET: Inventory/Inv_purchase_request/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase_request inv_purchase_request = db.Inv_purchase_request.Find(id);
            if (inv_purchase_request == null)
            {
                return HttpNotFound();
            }
            return View(inv_purchase_request);
        }
        // GET: Inventory/Inv_purchase_request/Create
        public ActionResult Create(int? Id)
        {
            Inv_purchase_request I = db.Inv_purchase_request
                .Where(x=>x.Id==Id).ToList().DefaultIfEmpty(new Inv_purchase_request { })
                .FirstOrDefault();

            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id",I.Store_id);
            if (I.Within_days == 0)
            {
                I.Within_days = 14;
            }
            return View(I);
        }

        // POST: Inventory/Inv_purchase_request/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_purchase_request inv_purchase_request)
        {
            if (ModelState.IsValid)
            {
                Inv_po_GS InvGs = db.Inv_po_GS.FirstOrDefault();
                inv_purchase_request.Pr_number = InvGs.Next_pr_no;
                InvGs.Next_pr_no = InvGs.Next_pr_no+1;
                if (inv_purchase_request.Id == 0)
                {
                    db.Inv_purchase_request.Add(inv_purchase_request);
                }
                else
                {
                    Inv_purchase_request IP= db.Inv_purchase_request.Find(inv_purchase_request.Id);
                    IP.Site_id = inv_purchase_request.Site_id;
                    IP.Store_id = inv_purchase_request.Store_id;
                    IP.Within_days = inv_purchase_request.Within_days;
                    IP.Within_days_date = inv_purchase_request.Within_days_date;
                }
                db.SaveChanges();
                return Json(inv_purchase_request.Id);
            }
            return View(inv_purchase_request);
        }

        // GET: Inventory/Inv_purchase_request/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase_request inv_purchase_request = db.Inv_purchase_request.Find(id);
            if (inv_purchase_request == null)
            {
                return HttpNotFound();
            }
            return View(inv_purchase_request);
        }

        // POST: Inventory/Inv_purchase_request/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PR_no,Pr_date,Within_days,item_id,Quntaty,Within_days_date")] Inv_purchase_request inv_purchase_request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_purchase_request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inv_purchase_request);
        }

        // GET: Inventory/Inv_purchase_request/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_purchase_request inv_purchase_request = db.Inv_purchase_request.Find(id);
            if (inv_purchase_request == null)
            {
                return HttpNotFound();
            }
            return View(inv_purchase_request);
        }

        // POST: Inventory/Inv_purchase_request/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_purchase_request inv_purchase_request = db.Inv_purchase_request.Find(id);
            db.Inv_purchase_request.Remove(inv_purchase_request);
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
