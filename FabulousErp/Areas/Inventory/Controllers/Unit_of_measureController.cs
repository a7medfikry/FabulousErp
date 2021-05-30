using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Inventory.Controllers
{
    public class Unit_of_measureController : Controller
    {
        private DBContext db = new DBContext();

        public ActionResult Index()
        {
            var unit_of_measures = db.Unit_of_measures.Include(u => u.Equivalante);
            return View(unit_of_measures.ToList());
        }

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Unit_of_measure unit_of_measure = db.Unit_of_measures.Find(id);
        //    if (unit_of_measure == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(unit_of_measure);
        //}

        public JsonResult CheckDuplicate(string UnitId,int Id)
        {
            return Json(db.Unit_of_measures.Any(x => x.Unit_id.Trim() == UnitId.Trim() && x.Id != Id));
        }

        public ActionResult Create()
        {
            ViewBag.Equivalante_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id");
            return View();
        }
         public JsonResult GetEquivalent()
        {
          return Json(db.Unit_of_measures.Select(x => new { Id = x.Id, x.Unit_id }));
        }
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Unit_id,Quantity_dicimal,Equivalante_id,Equivalante_quantity,Action")] Unit_of_measure unit_of_measure)
        {
            if (ModelState.IsValid)
            {
                db.Unit_of_measures.Add(unit_of_measure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Equivalante_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", unit_of_measure.Equivalante_id);
            return View(unit_of_measure);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_of_measure unit_of_measure = db.Unit_of_measures.Find(id);
            if (unit_of_measure == null)
            {
                return HttpNotFound();
            }
            ViewBag.Equivalante_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", unit_of_measure.Equivalante_id);
            return View(unit_of_measure);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Unit_id,Quantity_dicimal,Equivalante_id,Quantity")] Unit_of_measure unit_of_measure)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit_of_measure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Equivalante_id = new SelectList(db.Unit_of_measures, "Id", "Unit_id", unit_of_measure.Equivalante_id);
            return View(unit_of_measure);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit_of_measure unit_of_measure = db.Unit_of_measures.Find(id);
            if (unit_of_measure == null)
            {
                return HttpNotFound();
            }
            return View(unit_of_measure);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit_of_measure unit_of_measure = db.Unit_of_measures.Find(id);
            db.Unit_of_measures.Remove(unit_of_measure);
            db.SaveChanges();
            return RedirectToAction("Create");
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
