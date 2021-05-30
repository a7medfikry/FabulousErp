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
    public class Receivable_legal_infoController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Receivable/Legal_info
        public ActionResult Index()
        {
            var legal_infos = db.Receivable_legal_infos.Include(l => l.Vendore);
            return View(legal_infos.ToList());
        }

        // GET: Receivable/Legal_info/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_legal_info legal_info = db.Receivable_legal_infos.Find(id);
            if (legal_info == null)
            {
                return HttpNotFound();
            }
            return View(legal_info);
        }

        // GET: Receivable/Legal_info/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Receivable/Legal_info/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Receivable_legal_info legal_info)
        {
            if (ModelState.IsValid)
            {
                db.Receivable_legal_infos.Add(legal_info);
                db.SaveChanges();
                return Json(legal_info.Id);
            }

            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", legal_info.Vendore_id);
            return View(legal_info);
        }

        // GET: Receivable/Legal_info/Edit/5
        public ActionResult Edit(int? CreditorId)
        {
            Receivable_legal_info legal_info = db.Receivable_legal_infos.FirstOrDefault(x=>x.Vendore_id== CreditorId);
            if (legal_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", legal_info.Vendore_id);
            return View(legal_info);
        }

        // POST: Receivable/Legal_info/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tax_file_no,Tax_Id,Commercial_register,Social_insurance,Creditor_id")] Receivable_legal_info legal_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(legal_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", legal_info.Vendore_id);
            return View(legal_info);
        }

        // GET: Receivable/Legal_info/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_legal_info legal_info = db.Receivable_legal_infos.Find(id);
            if (legal_info == null)
            {
                return HttpNotFound();
            }
            return View(legal_info);
        }

        // POST: Receivable/Legal_info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receivable_legal_info legal_info = db.Receivable_legal_infos.Find(id);
            db.Receivable_legal_infos.Remove(legal_info);
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
