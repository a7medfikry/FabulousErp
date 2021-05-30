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
    public class Inv_sales_GSController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Inventory/Inv_sales_GS
        public ActionResult Index()
        {
            return View(db.Inv_sales_GS.ToList());
        }

        // GET: Inventory/Inv_sales_GS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales_GS inv_sales_GS = db.Inv_sales_GS.Find(id);
            if (inv_sales_GS == null)
            {
                return HttpNotFound();
            }
            return View(inv_sales_GS);
        }

        // GET: Inventory/Inv_sales_GS/Create
        public ActionResult Create()
        {
            if (!db.Inv_sales_GS.Any())
            {
                db.Inv_sales_GS.Add(new Inv_sales_GS { });
                db.SaveChanges();
            }
            return RedirectToAction("Edit");
            return View();
        }

        // POST: Inventory/Inv_sales_GS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Allow_insert_item_no_enough_store,Allow_proforma_inv,Allow_View_jv,Override_price_in_price_list,Allow_price_lower_cost,Allow_edit_sales_price,passwords_for_unhold_check,passwords_for_unhold,Allow_generate_automatic_po")] Inv_sales_GS inv_sales_GS)
        {
            if (ModelState.IsValid)
            {
                db.Inv_sales_GS.Add(inv_sales_GS);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inv_sales_GS);
        }

        // GET: Inventory/Inv_sales_GS/Edit/5
        public ActionResult Edit()
        {
            ViewBag.StoreC = db.Inv_store_site.Include(x => x.Store)
              .Where(x => x.Inactive == false && x.Store.Inactive == false).Count();
            Inv_sales_GS inv_sales_GS = db.Inv_sales_GS.FirstOrDefault();
           
            return View(inv_sales_GS);
        }

        // POST: Inventory/Inv_sales_GS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inv_sales_GS inv_sales_GS)
        {
            if (inv_sales_GS.Id == 0)
            {
                db.Inv_sales_GS.Add(inv_sales_GS);
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(inv_sales_GS).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Edit");
                }
            }
            
            return View(inv_sales_GS);
        }

        // GET: Inventory/Inv_sales_GS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inv_sales_GS inv_sales_GS = db.Inv_sales_GS.Find(id);
            if (inv_sales_GS == null)
            {
                return HttpNotFound();
            }
            return View(inv_sales_GS);
        }

        // POST: Inventory/Inv_sales_GS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inv_sales_GS inv_sales_GS = db.Inv_sales_GS.Find(id);
            db.Inv_sales_GS.Remove(inv_sales_GS);
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
