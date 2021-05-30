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

namespace Installment.Controllers
{
    public class Installment_settingController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Installment/Installment_setting
        public ActionResult Index()
        {
            return View(db.Installment_settings.ToList());
        }

        // GET: Installment/Installment_setting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment_setting installment_setting = db.Installment_settings.Find(id);
            if (installment_setting == null)
            {
                return HttpNotFound();
            }
            return View(installment_setting);
        }

        // GET: Installment/Installment_setting/Create
        public ActionResult Create(int? Id = null)
        {
            if (Id != null)
            {
                return View(db.Installment_settings.Find(Id));
            }
            else
            {
                return View();
            }
        }

        // POST: Installment/Installment_setting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Installment_setting installment_setting)
        {
            if (ModelState.IsValid)
            {
                if (installment_setting.Id==0)
                {
                    db.Installment_settings.Add(installment_setting);
                }
                else
                {
                    db.Entry(installment_setting).State = EntityState.Modified;
                }
                db.SaveChanges();
                return Json(installment_setting.Id);
            }

            return View(installment_setting);
        }

        // GET: Installment/Installment_setting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment_setting installment_setting = db.Installment_settings.Find(id);
            if (installment_setting == null)
            {
                return HttpNotFound();
            }
            return View(installment_setting);
        }

        // POST: Installment/Installment_setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Installment_setting installment_setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(installment_setting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(installment_setting);
        }

        // GET: Installment/Installment_setting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Installment_setting installment_setting = db.Installment_settings.Find(id);
            if (installment_setting == null)
            {
                return HttpNotFound();
            }
            return View(installment_setting);
        }

        // POST: Installment/Installment_setting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Installment_setting installment_setting = db.Installment_settings.Find(id);
            db.Installment_settings.Remove(installment_setting);
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

        public PartialViewResult InstallmentRelation(int SettingId,float OrginalAmount,DateTime TDate,float CashAdvance)
        {
            ViewBag.OrginalAmount = OrginalAmount;
            ViewBag.TDate = TDate;
            ViewBag.CashAdvance = CashAdvance;
            Installment_setting IS = db.Installment_settings.Include(x => x.Custom_installment).FirstOrDefault(x => x.Id == SettingId);
            if (CashAdvance != -1)
            {
                IS.Cash_advanced_percentage =(float)Math.Round((CashAdvance*100)/ OrginalAmount,FabulousErp.Business.GetDecimalPointNumber());
            }
            float IncreaseAmount = 0;
            if (IS.Increase_by== Increase_by.Percentage)
            {
                IncreaseAmount = 1 + ((float)IS.Increase / 100);
                ViewBag.InstallmentOrginalAmount = (OrginalAmount - ((OrginalAmount * IS.Cash_advanced_percentage) / 100))
                  * IncreaseAmount;

            }
            else
            {
                ViewBag.InstallmentOrginalAmount = (OrginalAmount - ((OrginalAmount * IS.Cash_advanced_percentage) / 100))
                 + IncreaseAmount;
            }
            ViewBag.CashAdvanceAmount = ((ViewBag.OrginalAmount * IS.Cash_advanced_percentage) / 100).ToString(Business.GetDecimalNumber());
            ViewBag.InstallMentPercentage = ((100 - IS.Cash_advanced_percentage - IS.Custom_installment.Sum(x => x.Percetage)) / IS.Number_of_installment);
          
            return PartialView(IS);
        }
    }
}
