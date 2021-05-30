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
    public class Inv_item_store_siteController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_item_store_site
        public ActionResult Index()
        {
            var inv_item_store_sites = db.Inv_item_store_sites.Include(i => i.Item).Include(i => i.Site).Include(i => i.Store);
            return View(inv_item_store_sites.ToList());
        }

        // GET: Inventory/Inv_item_store_site/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_store_site inv_item_store_site = db.Inv_item_store_sites.Find(id);
            if (inv_item_store_site == null)
            {
                return HttpNotFound();
            }
            return View(inv_item_store_site);
        }

        // GET: Inventory/Inv_item_store_site/Create
        public ActionResult Create(int? Id)
        {
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id");
            ViewBag.Site_id = new SelectList(new List<Inv_item_store_site> { }, "Id", "Site_id");
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id");
            return View();
        }

        // POST: Inventory/Inv_item_store_site/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Item_id,Store_id,Site_id")] Inv_item_store_site inv_item_store_site)
        {
            if (ModelState.IsValid)
            {
                db.Inv_item_store_sites.Add(inv_item_store_site);
                db.SaveChanges();
                return Json(1);
            }

            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_item_store_site.Item_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_name", inv_item_store_site.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_item_store_site.Store_id);
            return View(inv_item_store_site);
        }

        // GET: Inventory/Inv_item_store_site/Edit/5
        public ActionResult Edit(int? ItemId)
        {
            List<Inv_item_store_site>
                inv_item_store_site = db.Inv_item_store_sites.Where(x=>x.Item_id== ItemId).ToList();
            int? ThisSite=null;
            int? ThisStore=null;
            try
            {
                ThisSite = inv_item_store_site.FirstOrDefault(x => x.Item_id == ItemId).Site_id;
            }
            catch
            {

            }
            try
            {
                ThisStore = inv_item_store_site.FirstOrDefault(x => x.Item_id == ItemId).Store_id;
            }
            catch
            {

            }
            ViewBag.SiteList = db.Inv_store_site.ToList();
            ViewBag.StoreList = db.Inv_store.ToList();
            //ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_name", ThisSite);
            //ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", ThisStore);
            return View(inv_item_store_site);
        }

        // POST: Inventory/Inv_item_store_site/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Item_id,Store_id,Site_id")] Inv_item_store_site inv_item_store_site)
        {
            if (ModelState.IsValid)
            {
                Inv_item_store_site I = db.Inv_item_store_sites.Find(inv_item_store_site.Id);
                I.Site_id = inv_item_store_site.Site_id;
                I.Store_id = inv_item_store_site.Store_id;
                db.SaveChanges();
                return Json(1);
            }
            ViewBag.Item_id = new SelectList(db.Inv_item, "Id", "Item_id", inv_item_store_site.Item_id);
            ViewBag.Site_id = new SelectList(db.Inv_store_site, "Id", "Site_id", inv_item_store_site.Site_id);
            ViewBag.Store_id = new SelectList(db.Inv_store, "Id", "Store_id", inv_item_store_site.Store_id);
            return View(inv_item_store_site);
        }

        // GET: Inventory/Inv_item_store_site/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_item_store_site inv_item_store_site = db.Inv_item_store_sites.Find(id);
            if (inv_item_store_site == null)
            {
                return HttpNotFound();
            }
            return View(inv_item_store_site);
        }

        // POST: Inventory/Inv_item_store_site/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_item_store_site inv_item_store_site = db.Inv_item_store_sites.Find(id);
            db.Inv_item_store_sites.Remove(inv_item_store_site);
            db.SaveChanges();
            return Json(id);
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
