using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;

namespace Receivable.Controllers
{
    public class Aging_periodController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Receivable/Aging_period
        //public ActionResult Index()
        //{
        //    return View(db.Aging_periods.ToList());
        //}
        // GET: Receivable/Aging_period/Details/5
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
        // GET: Receivable/Aging_period/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Receivable/Aging_period/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Receivable_aging_period aging_period)
        {
            if (ModelState.IsValid)
            {
                db.Receivable_aging_periods.Add(aging_period);
                db.SaveChanges();
                return Json(1);
            }

            return View(aging_period);
        }

        // GET: Receivable/Aging_period/Edit/5
        public ActionResult Edit()
        {
            
            List<Receivable_aging_period> aging_period = db.Receivable_aging_periods.OrderBy(x=>x.From).ToList();
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
        // POST: Receivable/Aging_period/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Receivable_aging_period> aging_period)
        {
            if (ModelState.IsValid)
            {
                if (aging_period != null)
                {
                    foreach (Receivable_aging_period i in aging_period)
                    {
                        if (db.Receivable_aging_periods.Any(x => x.Id == i.Id))
                        {
                            Receivable_aging_period A = db.Receivable_aging_periods.FirstOrDefault(x => x.Id == i.Id);
                            A.Name = i.Name;
                            A.From = i.From;
                            A.To = i.To;
                        }
                        else
                        {
                            db.Receivable_aging_periods.Add(i);
                        }
                    }
                    db.SaveChanges();
                    return Json(1);
                }
                return Json(0);
              
            }
            return View(aging_period);
        }
       
        // GET: Receivable/Aging_period/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_aging_period aging_period = db.Receivable_aging_periods.Find(id);
            if (aging_period == null)
            {
                return HttpNotFound();
            }
            return View(aging_period);
        }

        // POST: Receivable/Aging_period/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Receivable_aging_period aging_period = db.Receivable_aging_periods.Find(id);
            db.Receivable_aging_periods.Remove(aging_period);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteJson(int id)
        {
            try
            {
                Receivable_aging_period aging_period = db.Receivable_aging_periods.Find(id);
                db.Receivable_aging_periods.Remove(aging_period);
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
