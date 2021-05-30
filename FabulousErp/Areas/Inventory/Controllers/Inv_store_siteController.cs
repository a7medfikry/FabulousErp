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

namespace Inventory.Controllers
{
    public class Inv_store_siteController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_store_site
        public ActionResult Index()
        {
            var inv_store_site = db.Inv_store_site.Include(i => i.Store);
            return View(inv_store_site.ToList());
        }

        // GET: Inventory/Inv_store_site/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_store_site inv_store_site = db.Inv_store_site.Find(id);
            if (inv_store_site == null)
            {
                return HttpNotFound();
            }
            return View(inv_store_site);
        }

        // GET: Inventory/Inv_store_site/Create
        public ActionResult Create()
        {
            ViewBag.Store_id = new SelectList(db.Inv_store.Where(x=>x.Inactive==false), "Id", "Store_id");
            return View();
        }

        // POST: Inventory/Inv_store_site/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_store_site inv_store_site)
        {
            if (ModelState.IsValid)
            {
                db.Inv_store_site.Add(inv_store_site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_store_site.Store_id);
            return View(inv_store_site);
        }
        public JsonResult GetSitesByStoreId(int StoreId)
        {
            return Json(db.Inv_store_site.Where(x => x.Store_id == StoreId&&x.Inactive==false)
                        .Select(x => new { Id = x.Id, Name = x.Site_name }));
        }
        // GET: Inventory/Inv_store_site/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_store_site inv_store_site = db.Inv_store_site.Find(id);
            if (inv_store_site == null)
            {
                return HttpNotFound();
            }
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_store_site.Store_id);
            return View(inv_store_site);
        }

        // POST: Inventory/Inv_store_site/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inv_store_site inv_store_site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_store_site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_store_site.Store_id);
            return View(inv_store_site);
        }

        // GET: Inventory/Inv_store_site/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_store_site inv_store_site = db.Inv_store_site.Find(id);
            if (inv_store_site == null)
            {
                return HttpNotFound();
            }
            return View(inv_store_site);
        }

        // POST: Inventory/Inv_store_site/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_store_site inv_store_site = db.Inv_store_site.Find(id);
            db.Inv_store_site.Remove(inv_store_site);
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
