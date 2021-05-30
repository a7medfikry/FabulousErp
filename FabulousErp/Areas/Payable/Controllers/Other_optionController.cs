using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Payable.Controllers
{
    public class Other_optionController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Payable/Other_option
        //public ActionResult Index()
        //{
        //    return View(db.Other_options.ToList());
        //}

        //// GET: Payable/Other_option/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Other_option other_option = db.Other_options.Find(id);
        //    if (other_option == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(other_option);
        //}

        // GET: Payable/Other_option/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Payable/Other_option/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Option,Checked")] Other_option other_option)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Other_options.Add(other_option);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(other_option);
        //}

        // GET: Payable/Other_option/Edit/5
        public ActionResult Edit()
        {
            List<Payable_other_option> other_option = db.Other_options.ToList();
            if (!other_option.Any())
            {
                other_option.Add(new Payable_other_option
                {
                    Checked =false,
                    Id=0,
                    Option=Other_option_enum.Active_payment,
                });
                other_option.Add(new Payable_other_option
                {
                    Checked =false,
                    Id=0,
                    Option=Other_option_enum.Ovride,
                });
                other_option.Add(new Payable_other_option
                {
                    Checked =false,
                    Id=0,
                    Option=Other_option_enum.Print_tax,
                });
            }
            return View(other_option);
        }

        // POST: Payable/Other_option/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Payable_other_option> other_option)
        {
            if (ModelState.IsValid)
            {
                foreach (Payable_other_option i in other_option)
                {
                    if (i.Id == 0)
                    {
                        db.Entry(i).State = EntityState.Added;
                    }
                    else
                    {
                        db.Entry(i).State = EntityState.Modified;
                    }
                }
               
                db.SaveChanges();
                return Json(1);
            }
            return View(other_option);
        }

        // GET: Payable/Other_option/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_other_option other_option = db.Other_options.Find(id);
            if (other_option == null)
            {
                return HttpNotFound();
            }
            return View(other_option);
        }

        // POST: Payable/Other_option/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_other_option other_option = db.Other_options.Find(id);
            db.Other_options.Remove(other_option);
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
