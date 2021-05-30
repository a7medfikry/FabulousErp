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
    public class Receivable_address_infoController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Receivable/Receivable_address_info
        public ActionResult Index()
        {
            var Receivable_address_info = db.Receivable_address_infos.Include(a => a.Creditor);
            return View(Receivable_address_info.ToList());
        }

        // GET: Receivable/Receivable_address_info/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_address_info Receivable_address_info = db.Receivable_address_infos.Find(id);
            if (Receivable_address_info == null)
            {
                return HttpNotFound();
            }
            return View(Receivable_address_info);
        }

        // GET: Receivable/Receivable_address_info/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Receivable/Receivable_address_info/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Receivable_address_info Receivable_address_info)
        {
            if (ModelState.IsValid)
            {
                db.Receivable_address_infos.Add(Receivable_address_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", Receivable_address_info.Creditor_id);
            return View(Receivable_address_info);
        }

        // GET: Receivable/Receivable_address_info/Edit/5
        public ActionResult Edit(int? CreditorId)
        {
            
            Receivable_address_info Receivable_address_info = db.Receivable_address_infos.FirstOrDefault(x=>x.Creditor_id==CreditorId);
            if (Receivable_address_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", Receivable_address_info.Creditor_id);
            return View(Receivable_address_info);
        }

        // POST: Receivable/Receivable_address_info/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,Country,Post_code,Phone_number,Fax,Contact_person,Mobile_number,Email,Creditor_id")] Receivable_address_info Receivable_address_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Receivable_address_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Creditor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", Receivable_address_info.Creditor_id);
            return View(Receivable_address_info);
        }

        // GET: Receivable/Receivable_address_info/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_address_info Receivable_address_info = db.Receivable_address_infos.Find(id);
            if (Receivable_address_info == null)
            {
                return HttpNotFound();
            }
            return View(Receivable_address_info);
        }

        // POST: Receivable/Receivable_address_info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receivable_address_info Receivable_address_info = db.Receivable_address_infos.Find(id);
            db.Receivable_address_infos.Remove(Receivable_address_info);
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
