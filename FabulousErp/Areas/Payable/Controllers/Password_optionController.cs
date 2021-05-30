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
    public class Password_optionController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Payable/Password_option
        //public ActionResult Index()
        //{
        //    return View(db.Password_Options.ToList());
        //}

        // GET: Payable/Password_option/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Password_option password_option = db.Password_Options.Find(id);
        //    if (password_option == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(password_option);
        //}

        // GET: Payable/Password_option/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Payable/Password_option/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,HasPassword,Option")] Password_option password_option)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Password_Options.Add(password_option);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(password_option);
        //}

        // GET: Payable/Password_option/Edit/5
        public ActionResult Edit(int? id)
        {


            List<Payable_password_option> password_option = db.Password_Options.ToList();
            if (!password_option.Any())
            {
                password_option.Add(new Payable_password_option
                {
                    HasPassword=false,
                    Option=Password_optionEnum.Exceed_credit_limit,
                });
                password_option.Add(new Payable_password_option
                {
                    HasPassword = false,
                    Option = Password_optionEnum.Exceed_Min_max,
                });
                password_option.Add(new Payable_password_option
                {
                    HasPassword = false,
                    Option = Password_optionEnum.Remove_inaactive_creditor,
                });
            }
            return View(password_option);
        }

        // POST: Payable/Password_option/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Payable_password_option> password_option)
        {
            if (ModelState.IsValid)
            {
                foreach (Payable_password_option i in password_option)
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
            return View(password_option);
        }

        // GET: Payable/Password_option/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_password_option password_option = db.Password_Options.Find(id);
            if (password_option == null)
            {
                return HttpNotFound();
            }
            return View(password_option);
        }

        // POST: Payable/Password_option/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_password_option password_option = db.Password_Options.Find(id);
            db.Password_Options.Remove(password_option);
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
