using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;

namespace Receivable.Controllers
{
    public class Payment_termController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Receivable/Payment_term
        public ActionResult Index()
        {
            return View(db.Receivable_payment_terms.ToList());
        }

        // GET: Receivable/Payment_term/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_payment_term payment_term = db.Receivable_payment_terms.Find(id);
            if (payment_term == null)
            {
                return HttpNotFound();
            }
            return View(payment_term);
        }

        // GET: Receivable/Payment_term/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Receivable/Payment_term/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Terms_id,Inactive,Amount_type,Amount,Net_Days,Total_Days,Date_option")] Receivable_payment_term payment_term)
        {
            if (ModelState.IsValid)
            {
                db.Receivable_payment_terms.Add(payment_term);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payment_term);
        }

        // GET: Receivable/Payment_term/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_payment_term payment_term = db.Receivable_payment_terms.Find(id);
            if (payment_term == null)
            {
                return HttpNotFound();
            }
            return View(payment_term);
        }

        // POST: Receivable/Payment_term/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Terms_id,Inactive,Amount_type,Amount,Net_Days,Total_Days,Date_option")] Receivable_payment_term payment_term)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment_term).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment_term);
        }

        // GET: Receivable/Payment_term/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_payment_term payment_term = db.Receivable_payment_terms.Find(id);
            if (payment_term == null)
            {
                return HttpNotFound();
            }
            return View(payment_term);
        }

        // POST: Receivable/Payment_term/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receivable_payment_term payment_term = db.Receivable_payment_terms.Find(id);
            db.Receivable_payment_terms.Remove(payment_term);
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
