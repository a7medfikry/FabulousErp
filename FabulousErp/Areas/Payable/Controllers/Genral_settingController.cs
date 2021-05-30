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
    public class Genral_settingController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Payable/Payable_genral_setting
        public ActionResult Index()
        {
            return RedirectToAction("Edit");
            return View(db.General_settings.ToList());
        }
        public JsonResult CheckNextNumber(List<CheckNextNumber> NextNumbers)
        {
            bool Valid = true;
            string DocTypeString = "";
            foreach (CheckNextNumber i in NextNumbers)
            {
                string ExistingNumber = db.General_settings.Where(x => x.Doc_type == i.Doc_type).ToList().DefaultIfEmpty(new Payable_genral_setting {Next_number="1" }).FirstOrDefault().Next_number;
                int ExisitingNum = Business.GetDigits(ExistingNumber);
                int NextNumber = Business.GetDigits(i.NextNumber);
                if (NextNumber < ExisitingNum)
                {
                    Valid = false;
                    DocTypeString += $"  {i.Doc_type.ToString()} Valid Number are {ExisitingNum}";
                }
            }
            return Json(new { status = Valid, msg = DocTypeString });
        }
    
        // GET: Payable/Payable_genral_setting/Edit/5
        public ActionResult Edit()
        {

            List<Payable_genral_setting> Payable_genral_setting = db.General_settings.ToList();

            if (!Payable_genral_setting.Any())
            {
                Payable_genral_setting = new List<Payable_genral_setting>
                {
                    new Payable_genral_setting
                    {
                        Checked=false,
                        Doc_type=Doc_type.Invoice,
                        Id=0,
                        Next_number="1"
                    }, new Payable_genral_setting
                    {
                         Checked=false,
                        Doc_type=Doc_type.Credit_Memo,
                        Id=0,
                        Next_number="1"
                    }, new Payable_genral_setting
                    {
                         Checked=false,
                        Doc_type=Doc_type.Debit_Memo,
                        Id=0,
                        Next_number="1"
                    }, new Payable_genral_setting
                    {
                          Checked=false,
                        Doc_type=Doc_type.Payment,
                        Id=0,
                        Next_number="1"
                    }, new Payable_genral_setting
                    {
                         Checked=false,
                        Doc_type=Doc_type.Return,
                        Id=0,
                        Next_number="1"
                    },
                };
            }
            ViewBag.Aging_date_option = db.Aging_date_option.ToList()
                .DefaultIfEmpty(new Payable_aging_date_option
                {
                }).FirstOrDefault().Date_option;
            return View(Payable_genral_setting);
        }
        public JsonResult AddAgingDate(Date_option Agd)
        {
            if (db.Aging_date_option.Any())
            {
                db.Aging_date_option.FirstOrDefault().Date_option = Agd;
            }
            else
            {
                db.Aging_date_option.Add(new Payable_aging_date_option
                {
                    Date_option=Agd
                });
            }
            db.SaveChanges();
            return Json(1);
        }
        // POST: Payable/Payable_genral_setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Payable_genral_setting> genral_setting)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(Payable_genral_setting).State = EntityState.Modified;
                foreach (Payable_genral_setting i in genral_setting)
                {
                    if (db.General_settings.Any(x => x.Doc_type == i.Doc_type))
                    {
                        db.Entry(i).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Entry(i).State = EntityState.Added;
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(genral_setting);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public JsonResult GetInactivePasswordOption()
        {
            try
            {
                return Json(db.Password_Options.FirstOrDefault(x => x.Option == Password_optionEnum.Remove_inaactive_creditor).HasPassword);
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult CheckInactivePasswordOption(string Password)
        {
            try
            {
                return Json(db.Password_Options.FirstOrDefault(x => x.Option == Password_optionEnum.Remove_inaactive_creditor).Password == Password);
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult GetMinMaxPasswordOption()
        {
            try
            {
                return Json(db.Password_Options.FirstOrDefault(x => x.Option == Password_optionEnum.Exceed_Min_max).HasPassword);
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult CheckMinMaxPasswordOption(string Password)
        {
            try
            {
                return Json(db.Password_Options.FirstOrDefault(x => x.Option == Password_optionEnum.Exceed_Min_max).Password == Password);
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult GetCreditLimitPasswordOption()
        {
            try
            {
                return Json(db.Password_Options.FirstOrDefault(x => x.Option == Password_optionEnum.Exceed_credit_limit).HasPassword);
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult CheckCreditLimitPasswordOption(string Password)
        {
            try
            {
                return Json(db.Password_Options.FirstOrDefault(x => x.Option == Password_optionEnum.Exceed_credit_limit).Password == Password);
            }
            catch
            {
                return Json(false);
            }
        }
    }
    public class CheckNextNumber
    {
        public string NextNumber { get; set; }
        public Doc_type Doc_type { get; set; }
    }
}
