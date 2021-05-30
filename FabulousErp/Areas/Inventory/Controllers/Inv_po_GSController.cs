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
    public class Inv_po_GSController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_po_GS
        public ActionResult Index()
        {
            return View(db.Inv_po_GS.ToList());
        }

        // GET: Inventory/Inv_po_GS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po_GS inv_po_GS = db.Inv_po_GS.Find(id);
            if (inv_po_GS == null)
            {
                return HttpNotFound();
            }
            return View(inv_po_GS);
        }

        // GET: Inventory/Inv_po_GS/Create
        public ActionResult Create()
        {
            if (!db.Inv_po_GS.Any())
            {
                db.Inv_po_GS.Add(new Inv_po_GS {Next_po_no=1,Next_pr_no=1});
                db.SaveChanges();
            }
            return RedirectToAction("Edit");
            return View();
        }

        // POST: Inventory/Inv_po_GS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Next_po_no,Allow_receiv_without_inv,Allow_receiv_part_of_po,Allow_View_jv,Show_items_cost_in_receiving,passwords_for_unhold_check,passwords_for_unhold,Allow_generate_automatic_po")] Inv_po_GS inv_po_GS)
        {
            if (ModelState.IsValid)
            {
                db.Inv_po_GS.Add(inv_po_GS);
                db.SaveChanges();
                return RedirectToAction("Edit");
            }

            return View(inv_po_GS);
        }

        // GET: Inventory/Inv_po_GS/Edit/5
        public ActionResult Edit()
        {
            ViewBag.StoreC = db.Inv_store_site.Include(x => x.Store)
                .Where(x => x.Inactive == false && x.Store.Inactive == false).Count();
            Inv_po_GS inv_po_GS = db.Inv_po_GS.FirstOrDefault();
            if (inv_po_GS == null)
            {
                inv_po_GS = new Inv_po_GS
                {
                    Next_po_no = 1,
                    Next_pr_no = 1
                };
                db.Inv_po_GS.Add(inv_po_GS);
                db.SaveChanges();
            }
            return View(inv_po_GS);
        }

        // POST: Inventory/Inv_po_GS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inv_po_GS inv_po_GS)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_po_GS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            return View(inv_po_GS);
        }

        // GET: Inventory/Inv_po_GS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_po_GS inv_po_GS = db.Inv_po_GS.Find(id);
            if (inv_po_GS == null)
            {
                return HttpNotFound();
            }
            return View(inv_po_GS);
        }

        // POST: Inventory/Inv_po_GS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_po_GS inv_po_GS = db.Inv_po_GS.Find(id);
            db.Inv_po_GS.Remove(inv_po_GS);
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
