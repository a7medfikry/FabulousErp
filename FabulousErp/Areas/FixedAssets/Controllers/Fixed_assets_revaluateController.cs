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
using FabulousErp;

namespace FixedAssets.Controllers
{
    public class Fixed_assets_revaluateController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Fixed_assets_revaluate
        public ActionResult Index()
        {
            var fixed_assets_revaluate = db.Fixed_assets_revaluate.Include(f => f.Asset);
            return View(fixed_assets_revaluate.ToList());
        }

        // GET: Fixed_assets_revaluate/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_revaluate fixed_assets_revaluate = db.Fixed_assets_revaluate.Find(id);
            if (fixed_assets_revaluate == null)
            {
                return HttpNotFound();
            }
            return View(fixed_assets_revaluate);
        }

        // GET: Fixed_assets_revaluate/Create
        public ActionResult Create()
        {
            ViewBag.PostingToOrThrow = Business.Business.PostingToOrThrow();
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description");
            
            return View(new FabulousDB.Models.Fixed_assets_revaluate { 
            Revaluate_no=(FabulousErp.Business.GetDigits
            (db.Fixed_assets_revaluate.Max(x=>x.Revaluate_no))).ToString()
            });
        }

        // POST: Fixed_assets_revaluate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Revaluate_no,Transaction_date,Revaluate_date,Assets_id,Old_cost,Old_use_life,Adjustment_cost,Net_profit")] Fixed_assets_revaluate fixed_assets_revaluate)
        {
            if (ModelState.IsValid)
            {
                db.Fixed_assets_revaluate.Add(fixed_assets_revaluate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", fixed_assets_revaluate.Assets_id);
            return View(fixed_assets_revaluate);
        }

        // GET: Fixed_assets_revaluate/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_revaluate fixed_assets_revaluate = db.Fixed_assets_revaluate.Find(id);
            if (fixed_assets_revaluate == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", fixed_assets_revaluate.Assets_id);
            return View(fixed_assets_revaluate);
        }

        // POST: Fixed_assets_revaluate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Revaluate_no,Transaction_date,Revaluate_date,Assets_id,Old_cost,Old_use_life,Adjustment_cost,Net_profit")] Fixed_assets_revaluate fixed_assets_revaluate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fixed_assets_revaluate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", fixed_assets_revaluate.Assets_id);
            return View(fixed_assets_revaluate);
        }

        // GET: Fixed_assets_revaluate/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fixed_assets_revaluate fixed_assets_revaluate = db.Fixed_assets_revaluate.Find(id);
            if (fixed_assets_revaluate == null)
            {
                return HttpNotFound();
            }
            ViewBag.TransactionDate = DateTime.Now.ToShortDateString().ToString();
            return View(fixed_assets_revaluate);
        }

        // POST: Fixed_assets_revaluate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fixed_assets_revaluate fixed_assets_revaluate = db.Fixed_assets_revaluate.Find(id);
            db.Fixed_assets_revaluate.Remove(fixed_assets_revaluate);
            db.Delete_fixed_assets_revaluate.Add(Business.Business.GetDeleteRevluate(fixed_assets_revaluate));
            db.SaveChanges();
            return Json(1);
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
