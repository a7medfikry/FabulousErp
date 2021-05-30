using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;

namespace Payable.Controllers
{
    public class Aging_periodController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Payable/Aging_period
        //public ActionResult Index()
        //{
        //    return View(db.Aging_periods.ToList());
        //}
        // GET: Payable/Aging_period/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Aging_period aging_period = db.Aging_periods.Find(id);
        //    if (aging_period == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(aging_period);
        //}
        // GET: Payable/Aging_period/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Payable/Aging_period/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payable_aging_period aging_period)
        {
            if (ModelState.IsValid)
            {
                db.Aging_periods.Add(aging_period);
                db.SaveChanges();
                return Json(1);
            }

            return View(aging_period);
        }

        // GET: Payable/Aging_period/Edit/5
        public ActionResult Edit()
        {
            
            List<Payable_aging_period> aging_period = db.Aging_periods.ToList();
            if (aging_period == null)
            {
                return HttpNotFound();
            }
            return View(aging_period);
        }
        public ActionResult AddNewAging()
        {
            return View();
        }
        // POST: Payable/Aging_period/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Payable_aging_period> aging_period)
        {
            if (ModelState.IsValid)
            {
                if (aging_period != null)
                {
                    foreach (Payable_aging_period i in aging_period)
                    {
                        if (db.Aging_periods.Any(x => x.Id == i.Id))
                        {
                            Payable_aging_period A = db.Aging_periods.FirstOrDefault(x => x.Id == i.Id);
                            A.Name = i.Name;
                            A.From = i.From;
                            A.To = i.To;
                        }
                        else
                        {
                            db.Aging_periods.Add(i);
                        }
                    }
                    db.SaveChanges();
                    return Json(1);
                }
                return Json(0);
              
            }
            return View(aging_period);
        }
       
        // GET: Payable/Aging_period/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_aging_period aging_period = db.Aging_periods.Find(id);
            if (aging_period == null)
            {
                return HttpNotFound();
            }
            return View(aging_period);
        }

        // POST: Payable/Aging_period/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_aging_period aging_period = db.Aging_periods.Find(id);
            db.Aging_periods.Remove(aging_period);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteJson(int id)
        {
            try
            {
                Payable_aging_period aging_period = db.Aging_periods.Find(id);
                db.Aging_periods.Remove(aging_period);
                db.SaveChanges();
                return Json(1);
            }
            catch
            {
                return Json(-1);
            }
         
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
