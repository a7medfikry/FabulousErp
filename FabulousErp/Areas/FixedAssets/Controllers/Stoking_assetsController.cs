using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.Models;
using System.Data.Entity.Migrations;
using System.Data.Entity.SqlServer;
using FabulousDB.DB_Context;

namespace FixedAssets.Controllers
{
    public class Stoking_assetsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Stoking_assets
        public ActionResult Index()
        {
            var stoking_assets = db.Stoking_assets.Include(s => s.Asset).Include(s => s.Assets_class);
            return View(stoking_assets.ToList());
        }
        public ActionResult Test()
        {
            return View();
        }
        // GET: Stoking_assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stoking_assets stoking_assets = db.Stoking_assets.Find(id);
            if (stoking_assets == null)
            {
                return HttpNotFound();
            }
            return View(stoking_assets);
        }

        // GET: Stoking_assets/Create
        public ActionResult Create()
        {
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description");
            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description");
            //ViewBag.Assets_class_list = db.Assets_class.ToList();
            //ViewBag.Assets_list = db.Assets.ToList();
            List<Stoking_assets> MyStockingAssets = db.Stoking_assets.ToList();
            MyStockingAssets.Where(x => x.Asset == null).ToList().ForEach(x => x.Asset = new Asset { Description = "" });
            MyStockingAssets.Where(x => x.Assets_class == null).ToList().ForEach(x => x.Assets_class = new Assets_class { Description = "" });


            return View(MyStockingAssets);
        }

        // POST: Stoking_assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(List<Stocking_assets_transaction> stoking_assets)
        {
            if (ModelState.IsValid)
            {
                int StockingNumberStart = db.Stocking_assets_transaction.Select(x => x.Stocking_no).Where(x=>SqlFunctions.IsNumeric(x)!=0).ToList().DefaultIfEmpty("0").Max(x => Convert.ToInt32(x));
                int Count = 1;
                foreach (Stocking_assets_transaction i in stoking_assets)
                {
                    i.Stocking_no = (StockingNumberStart + Count).ToString();
                    db.Stocking_assets_transaction.Add(new Stocking_assets_transaction
                    {
                        Reconcile=i.Reconcile,
                        Status=i.Status,
                        Stocking_no=i.Stocking_no,
                        Transaction_date=i.Transaction_date,
                        Reconcile_date=DateTime.Now,
                        Stocking_assets_id= i.Stocking_assets_id
                    });
                    Count++;
                }
                db.SaveChanges();
                return Json(1);
            }
            return Json(-1);
        }
        public ActionResult StockingReport()
        {
            return View(db.Stocking_assets_transaction.ToList());
        }
        // GET: Stoking_assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stoking_assets stoking_assets = db.Stoking_assets.Find(id);
            if (stoking_assets == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", stoking_assets.Assets_id);
            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description", stoking_assets.Assets_class_id);
            return View(stoking_assets);
        }

        // POST: Stoking_assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Assets_id,Assets_class_id,Status,Reconcile")] Stoking_assets stoking_assets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stoking_assets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", stoking_assets.Assets_id);
            ViewBag.Assets_class_id = new SelectList(db.Assets_class, "Id", "Description", stoking_assets.Assets_class_id);
            return View(stoking_assets);
        }

        // GET: Stoking_assets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stoking_assets stoking_assets = db.Stoking_assets.Find(id);
            if (stoking_assets == null)
            {
                return HttpNotFound();
            }
            return View(stoking_assets);
        }

        // POST: Stoking_assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stoking_assets stoking_assets = db.Stoking_assets.Find(id);
            db.Stoking_assets.Remove(stoking_assets);
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
