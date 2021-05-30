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
    public class Shipping_methodController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Receivable/Shipping_method
        public ActionResult Index()
        {
            return View(db.Receivable_shipping_methods.ToList());
        }

        // GET: Receivable/Shipping_method/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_shipping_method Shipping_method = db.Receivable_shipping_methods.Find(id);
            if (Shipping_method == null)
            {
                return HttpNotFound();
            }
            return View(Shipping_method);
        }

        // GET: Receivable/Shipping_method/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Receivable/Shipping_method/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Receivable_shipping_method Shipping_method)
        {
            if (ModelState.IsValid)
            {
                db.Receivable_shipping_methods.Add(Shipping_method);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Shipping_method);
        }

        // GET: Receivable/Shipping_method/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_shipping_method Shipping_method = db.Receivable_shipping_methods.Find(id);
            if (Shipping_method == null)
            {
                return HttpNotFound();
            }
            return View(Shipping_method);
        }

        // POST: Receivable/Shipping_method/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Receivable_shipping_method Shipping_method)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Shipping_method).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Shipping_method);
        }

        // GET: Receivable/Shipping_method/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_shipping_method Shipping_method = db.Receivable_shipping_methods.Find(id);
            if (Shipping_method == null)
            {
                return HttpNotFound();
            }
            return View(Shipping_method);
        }

        // POST: Receivable/Shipping_method/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receivable_shipping_method Shipping_method = db.Receivable_shipping_methods.Find(id);
            db.Receivable_shipping_methods.Remove(Shipping_method);
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
