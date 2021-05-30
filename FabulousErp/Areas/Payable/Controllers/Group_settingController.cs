using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;

namespace Payable.Controllers
{
    public class Group_settingController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        // GET: Payable/Payable_Group_setting
        public ActionResult Index()
        {
            var Payable_Group_settings = db.Payable_Group_settings.Include(g => g.Check_book).Include(g => g.Currency).Include(g => g.Payment_term).Include(g => g.Shipping_method).Include(g => g.Tax);
            return View(Payable_Group_settings.ToList());
        }

        // GET: Payable/Payable_Group_setting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_Group_setting Payable_Group_setting = db.Payable_Group_settings.Find(id);
            if (Payable_Group_setting == null)
            {
                return HttpNotFound();
            }
            return View(Payable_Group_setting);
        }
        public JsonResult GetGroupSeeting(int Id)
        {
            try
            {
                return Json(new GetData
                {
                    GS= db.Payable_Group_settings.Find(Id),
                    PGA= db.Payable_gl_accounts.Where(x=>x.Payable_Group_setting_id==Id).ToList().DefaultIfEmpty(new Payable_gl_account { }).FirstOrDefault(),
                    Min= db.Payable_Group_settings.Find(Id).Minimum_transaction,
                    Max= db.Payable_Group_settings.Find(Id).Maximum_transaction,
                });

            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult HasPassword(int Id)
        {
            try
            {
                return Json(!string.IsNullOrEmpty(db.Payable_Group_settings.Find(Id).Password));
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult CheckPass(int Id,string Password)
        {
            try
            {
                return Json(db.Payable_Group_settings.Any(x => x.Id == Id && x.Password == Password));
            }
            catch
            {
                return Json(false);
            }
        }
        // GET: Payable/Payable_Group_setting/Create
        public ActionResult Create()
        {
            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName");
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Payment_terms = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id");
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method");
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID");
            return View();
        }

        // POST: Payable/Payable_Group_setting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payable_Group_setting Payable_Group_setting)
        {
            if (ModelState.IsValid)
            {
                db.Payable_Group_settings.Add(Payable_Group_setting);
                db.SaveChanges();
                return Json(Payable_Group_setting.Id);
            }

            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", Payable_Group_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Payment_terms = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", Payable_Group_setting.Payment_terms);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", Payable_Group_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", Payable_Group_setting.Tax_group_id);
            return View(Payable_Group_setting);
        }

        // GET: Payable/Payable_Group_setting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_Group_setting Payable_Group_setting = db.Payable_Group_settings.Find(id);
            if (Payable_Group_setting == null)
            {
                return HttpNotFound();
            }
            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", Payable_Group_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode",Payable_Group_setting.Currency_id);
            ViewBag.Payment_terms = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", Payable_Group_setting.Payment_terms);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", Payable_Group_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", Payable_Group_setting.Tax_group_id);
            return View(Payable_Group_setting);
        }

        // POST: Payable/Payable_Group_setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payable_Group_setting Payable_Group_setting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Payable_Group_setting).State = EntityState.Modified;
                db.Entry(Payable_Group_setting).Property(x => x.Group_id).IsModified = false;
                db.SaveChanges();
                return Json(Payable_Group_setting.Id);
            }
            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", Payable_Group_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Payment_terms = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", Payable_Group_setting.Payment_terms);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", Payable_Group_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", Payable_Group_setting.Tax_group_id);
            return View(Payable_Group_setting);
        }

        // GET: Payable/Payable_Group_setting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_Group_setting Payable_Group_setting = db.Payable_Group_settings.Find(id);
            if (Payable_Group_setting == null)
            {
                return HttpNotFound();
            }
            return View(Payable_Group_setting);
        }

        // POST: Payable/Payable_Group_setting/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_Group_setting Payable_Group_setting = db.Payable_Group_settings.Find(id);
          
            try
            {
                db.Payable_Group_settings.Remove(Payable_Group_setting);
                db.Payable_gl_accounts.RemoveRange(db.Payable_gl_accounts.Where(x => x.Payable_Group_setting_id == id));
                db.SaveChanges();
                return Json(1);
            }
            catch
            {
                return Json("This Group Has Creditor, You Can't Delete It");
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
    public class GetData
    {
       public Payable_Group_setting GS { get; set; }
        public Payable_gl_account PGA { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
    }
}
