using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;

namespace Payable.Controllers
{
    public class Legal_infoController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Payable/Legal_info
        public ActionResult Index()
        {
            var legal_infos = db.Legal_infos.Include(l => l.Creditor);
            return View(legal_infos.ToList());
        }

        // GET: Payable/Legal_info/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_legal_info legal_info = db.Legal_infos.Find(id);
            if (legal_info == null)
            {
                return HttpNotFound();
            }
            return View(legal_info);
        }

        // GET: Payable/Legal_info/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payable/Legal_info/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Payable_legal_info legal_info)
        {
            if (ModelState.IsValid)
            {
                db.Legal_infos.Add(legal_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Creditor_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", legal_info.Creditor_id);
            return View(legal_info);
        }

        // GET: Payable/Legal_info/Edit/5
        public ActionResult Edit(int? CreditorId)
        {
            Payable_legal_info legal_info = db.Legal_infos.FirstOrDefault(x=>x.Creditor_id== CreditorId);
            if (legal_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.Creditor_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", legal_info.Creditor_id);
            return View(legal_info);
        }

        // POST: Payable/Legal_info/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Payable_legal_info legal_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(legal_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Creditor_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", legal_info.Creditor_id);
            return View(legal_info);
        }

        // GET: Payable/Legal_info/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_legal_info legal_info = db.Legal_infos.Find(id);
            if (legal_info == null)
            {
                return HttpNotFound();
            }
            return View(legal_info);
        }

        // POST: Payable/Legal_info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_legal_info legal_info = db.Legal_infos.Find(id);
            db.Legal_infos.Remove(legal_info);
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
