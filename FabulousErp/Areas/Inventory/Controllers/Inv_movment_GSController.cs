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
    public class Inv_movment_GSController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_movment_GS
        public ActionResult Index()
        {
            return View(db.Inv_movment_GS.ToList());
        }

        // GET: Inventory/Inv_movment_GS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_movment_GS inv_movment_GS = db.Inv_movment_GS.Find(id);
            if (inv_movment_GS == null)
            {
                return HttpNotFound();
            }
            return View(inv_movment_GS);
        }

        // GET: Inventory/Inv_movment_GS/Create
        public ActionResult Create()
        {
            if (!db.Inv_movment_GS.Any())
            {
                db.Inv_movment_GS.Add(new Inv_movment_GS
                {
                    Next_adjustment_no=1,
                    Next_transfer_no=1,
                });
                db.SaveChanges();
            }
            return RedirectToAction("Edit");
        }

        // POST: Inventory/Inv_movment_GS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inv_movment_GS inv_movment_GS)
        {
            if (ModelState.IsValid)
            {
                db.Inv_movment_GS.Add(inv_movment_GS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inv_movment_GS);
        }

        // GET: Inventory/Inv_movment_GS/Edit/5
        public ActionResult Edit()
        {
            ViewBag.StoreC = db.Inv_store_site.Include(x=>x.Store)
                .Where(x=>x.Inactive==false&&x.Store.Inactive==false).Count();
            Inv_movment_GS inv_movment_GS = db.Inv_movment_GS.FirstOrDefault();
            return View(inv_movment_GS);
        }

        // POST: Inventory/Inv_movment_GS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inv_movment_GS inv_movment_GS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_movment_GS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(inv_movment_GS);
        }

        // GET: Inventory/Inv_movment_GS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_movment_GS inv_movment_GS = db.Inv_movment_GS.Find(id);
            if (inv_movment_GS == null)
            {
                return HttpNotFound();
            }
            return View(inv_movment_GS);
        }

        // POST: Inventory/Inv_movment_GS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_movment_GS inv_movment_GS = db.Inv_movment_GS.Find(id);
            db.Inv_movment_GS.Remove(inv_movment_GS);
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
