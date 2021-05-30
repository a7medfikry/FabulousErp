using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;

namespace Payable.Controllers
{
    public class Address_infoController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Payable/Address_info
        public ActionResult Index()
        {
            var address_infos = db.Payable_address_infos.Include(a => a.Creditor);
            return View(address_infos.ToList());
        }

        // GET: Payable/Address_info/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_address_info address_info = db.Payable_address_infos.Find(id);
            if (address_info == null)
            {
                return HttpNotFound();
            }
            return View(address_info);
        }

        // GET: Payable/Address_info/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payable/Address_info/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,Country,Post_code,Phone_number,Fax,Contact_person,Mobile_number,Email,Creditor_id")] Payable_address_info address_info)
        {
            if (ModelState.IsValid)
            {
                db.Payable_address_infos.Add(address_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Creditor_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", address_info.Creditor_id);
            return View(address_info);
        }

        // GET: Payable/Address_info/Edit/5
        public ActionResult Edit(int? CreditorId)
        {
            
            Payable_address_info address_info = db.Payable_address_infos.FirstOrDefault(x=>x.Creditor_id==CreditorId);
            if (address_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.Creditor_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", address_info.Creditor_id);
            return View(address_info);
        }

        // POST: Payable/Address_info/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,Country,Post_code,Phone_number,Fax,Contact_person,Mobile_number,Email,Creditor_id")] Payable_address_info address_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Creditor_id = new SelectList(db.Payable_creditor_setting, "Id", "Vendor_id", address_info.Creditor_id);
            return View(address_info);
        }

        // GET: Payable/Address_info/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_address_info address_info = db.Payable_address_infos.Find(id);
            if (address_info == null)
            {
                return HttpNotFound();
            }
            return View(address_info);
        }

        // POST: Payable/Address_info/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_address_info address_info = db.Payable_address_infos.Find(id);
            db.Payable_address_infos.Remove(address_info);
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
