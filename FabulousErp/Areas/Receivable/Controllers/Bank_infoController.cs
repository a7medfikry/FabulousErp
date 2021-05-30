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
    public class Bank_infoController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Receivable/Bank_info
        public ActionResult Index()
        {
            var bank_info = db.Receivable_bank_info.Include(b => b.Creditor);
            return View(bank_info.ToList());
        }

        // GET: Receivable/Bank_info/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_bank_info bank_info = db.Receivable_bank_info.Find(id);
            if (bank_info == null)
            {
                return HttpNotFound();
            }
            return View(bank_info);
        }

        // GET: Receivable/Bank_info/Create
        public ActionResult Create()
        {
            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id");
            return View();
        }

        // POST: Receivable/Bank_info/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Receivable_bank_info bank_info)
        {
            if (ModelState.IsValid)
            {
                db.Receivable_bank_info.Add(bank_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", bank_info.Creditor_id);
            return View(bank_info);
        }

        // GET: Receivable/Bank_info/Edit/5
        public ActionResult Edit(int? CreditorId)
        {
            Receivable_bank_info bank_info = db.Receivable_bank_info.FirstOrDefault(x=>x.Creditor_id== CreditorId);
            if (bank_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", bank_info.Creditor_id);
            return View(bank_info);
        }

        // POST: Receivable/Bank_info/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cheque_name,Bank_name,Branch,Account_name,Account_number,Swift_code,Bank_address,Iban,Creditor_id")] Receivable_bank_info bank_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bank_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", bank_info.Creditor_id);
            return View(bank_info);
        }

        // GET: Receivable/Bank_info/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_bank_info bank_info = db.Receivable_bank_info.Find(id);
            if (bank_info == null)
            {
                return HttpNotFound();
            }
            return View(bank_info);
        }

        // POST: Receivable/Bank_info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receivable_bank_info bank_info = db.Receivable_bank_info.Find(id);
            db.Receivable_bank_info.Remove(bank_info);
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
