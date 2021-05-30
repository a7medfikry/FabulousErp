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
    public class Additional_informationController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Additional_information
        public ActionResult Index()
        {
            var additional_information = db.Additional_information.Include(a => a.Asset);
            return View(additional_information.ToList());
        }

        // GET: Additional_information/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Additional_information additional_information = db.Additional_information.Find(id);
            if (additional_information == null)
            {
                return HttpNotFound();
            }
            return View(additional_information);
        }

        // GET: Additional_information/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        // POST: Additional_information/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Assets_id,Field_no,Field_name")] Additional_information additional_information)
        {
            if (ModelState.IsValid)
            {
                db.Additional_information.Add(additional_information);
                db.SaveChanges();
                return Json(1);
            }
            return Json(0);

        }

        // GET: Additional_information/Edit/5
        public PartialViewResult Edit(int? id)
        {
            Additional_information additional_information = db.Additional_information.Find(id);
            ViewBag.Assets_id = new SelectList(db.Assets, "Id", "Description", additional_information.Assets_id);
            return PartialView(additional_information);
        }

        // POST: Additional_information/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit([Bind(Include = "Id,Assets_id,Field_no,Field_name")] Additional_information additional_information)
        {
            if (ModelState.IsValid)
            {
                db.Entry(additional_information).State = EntityState.Modified;
                db.SaveChanges();
                return Json(1);
            }
            return Json(0);
        }

        // GET: Additional_information/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Additional_information additional_information = db.Additional_information.Find(id);
            if (additional_information == null)
            {
                return HttpNotFound();
            }
            return View(additional_information);
        }

        // POST: Additional_information/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Additional_information additional_information = db.Additional_information.Find(id);
            db.Additional_information.Remove(additional_information);
            db.SaveChanges();
            return Json(0);
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
