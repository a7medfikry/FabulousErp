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

namespace FabulousErp.Controllers
{
    public class TranslatesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Translates
        public ActionResult Index()
        {
            return View(db.Translates.ToList());
        }

        // GET: Translates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translate translate = db.Translates.Find(id);
            if (translate == null)
            {
                return HttpNotFound();
            }
            return View(translate);
        }

        // GET: Translates/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Translates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Key,English,Arabic")] Translate translate)
        {
            if (ModelState.IsValid)
            {
                db.Translates.Add(translate);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(translate);
        }

        // GET: Translates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translate translate = db.Translates.Find(id);
            if (translate == null)
            {
                return HttpNotFound();
            }
            return View(translate);
        }

        // POST: Translates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Key,English,Arabic")] Translate translate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(translate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(translate);
        }

        // GET: Translates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Translate translate = db.Translates.Find(id);
            if (translate == null)
            {
                return HttpNotFound();
            }
            return View(translate);
        }

        // POST: Translates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Translate translate = db.Translates.Find(id);
            db.Translates.Remove(translate);
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
