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

namespace Installment.Controllers
{
    public class Custom_installmentController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Installment/Custom_installment
        public ActionResult Index()
        {
            var custom_installments = db.Custom_installments.Include(c => c.Installment_setting);
            return View(custom_installments.ToList());
        }

        // GET: Installment/Custom_installment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custom_installment custom_installment = db.Custom_installments.Find(id);
            if (custom_installment == null)
            {
                return HttpNotFound();
            }
            return View(custom_installment);
        }

        // GET: Installment/Custom_installment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Installment/Custom_installment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Custom_no,Percetage,Installment_setting_id")] Custom_installment custom_installment)
        {
            if (ModelState.IsValid)
            {
                if (custom_installment.Id == 0)
                {
                    db.Custom_installments.Add(custom_installment);
                }
                else
                {
                    db.Entry(custom_installment).State = EntityState.Modified;
                }
                db.SaveChanges();
                return Json(custom_installment.Id);
            }

            ViewBag.Installment_setting_id = new SelectList(db.Installment_settings, "Id", "Plan_id", custom_installment.Installment_setting_id);
            return View(custom_installment);
        }

        // GET: Installment/Custom_installment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<Custom_installment> custom_installment = db.Custom_installments.Where(x=>x.Installment_setting_id== id).ToList();
           
            return View(custom_installment);
        }

        // POST: Installment/Custom_installment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Custom_no,Percetage,Installment_setting_id")] Custom_installment custom_installment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custom_installment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Installment_setting_id = new SelectList(db.Installment_settings, "Id", "Plan_id", custom_installment.Installment_setting_id);
            return View(custom_installment);
        }

        // GET: Installment/Custom_installment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Custom_installment custom_installment = db.Custom_installments.Find(id);
            if (custom_installment == null)
            {
                return HttpNotFound();
            }
            return View(custom_installment);
        }

        // POST: Installment/Custom_installment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Custom_installment custom_installment = db.Custom_installments.Find(id);
            db.Custom_installments.Remove(custom_installment);
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
