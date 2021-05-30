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

namespace FixedAssets.Controllers
{
    public class Depreication_assets_id_connectionController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Depreication_assets_id_connection
        public ActionResult Index()
        {
            var depreication_assets_id_connection = db.Depreication_assets_id_connection.Include(d => d.Asset).Include(d => d.Deprecation);
            return View(depreication_assets_id_connection.ToList());
        }

        // GET: Depreication_assets_id_connection/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depreication_assets_id_connection depreication_assets_id_connection = db.Depreication_assets_id_connection.Find(id);
            if (depreication_assets_id_connection == null)
            {
                return HttpNotFound();
            }
            return View(depreication_assets_id_connection);
        }

        // GET: Depreication_assets_id_connection/Create
        public ActionResult Create()
        {
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number");
            return View();
        }

        // POST: Depreication_assets_id_connection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Deprecation_id,Assets_id")] Depreication_assets_id_connection depreication_assets_id_connection)
        {
            if (ModelState.IsValid)
            {
                db.Depreication_assets_id_connection.Add(depreication_assets_id_connection);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number", depreication_assets_id_connection.Assets_id);
            ViewBag.Deprecation_id = new SelectList(db.Deprecations, "Id", "Id", depreication_assets_id_connection.Deprecation_id);
            return View(depreication_assets_id_connection);
        }

        // GET: Depreication_assets_id_connection/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depreication_assets_id_connection depreication_assets_id_connection = db.Depreication_assets_id_connection.Find(id);
            if (depreication_assets_id_connection == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number", depreication_assets_id_connection.Assets_id);
            ViewBag.Deprecation_id = new SelectList(db.Deprecations, "Id", "Id", depreication_assets_id_connection.Deprecation_id);
            return View(depreication_assets_id_connection);
        }

        // POST: Depreication_assets_id_connection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Deprecation_id,Assets_id")] Depreication_assets_id_connection depreication_assets_id_connection)
        {
            if (ModelState.IsValid)
            {
                db.Entry(depreication_assets_id_connection).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Assets_number", depreication_assets_id_connection.Assets_id);
            ViewBag.Deprecation_id = new SelectList(db.Deprecations, "Id", "Id", depreication_assets_id_connection.Deprecation_id);
            return View(depreication_assets_id_connection);
        }

        // GET: Depreication_assets_id_connection/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Depreication_assets_id_connection depreication_assets_id_connection = db.Depreication_assets_id_connection.Find(id);
            if (depreication_assets_id_connection == null)
            {
                return HttpNotFound();
            }
            return View(depreication_assets_id_connection);
        }

        // POST: Depreication_assets_id_connection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Depreication_assets_id_connection depreication_assets_id_connection = db.Depreication_assets_id_connection.Find(id);
            db.Depreication_assets_id_connection.Remove(depreication_assets_id_connection);
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
