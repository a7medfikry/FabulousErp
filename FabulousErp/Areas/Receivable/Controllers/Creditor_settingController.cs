using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Receivable.Controllers
{
    public class Creditor_settingController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        // GET: Receivable/Creditor_setting
        public ActionResult Index()
        {
            var creditor_settings = db.Receivable_vendore_settings.Include(c => c.Check_book).Include(c => c.Group_setting).Include(c => c.Payment_terms).Include(c => c.Shippint_method).Include(c => c.Tax);
            return View(creditor_settings.ToList());
        }
        public JsonResult GetVendoreCurr(int VendoreId)
        {
            try
            {
                return Json(db.Receivable_vendore_currencies.Where(x => x.Vendore_id == VendoreId).Select(x => x.Currency_id));
            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult AddCurrencyToVendore(int VendoreId, string Currency_id)
        {
            try
            {
                db.Receivable_vendore_currencies.Add(new Receivable_vendore_currencies
                {
                    Currency_id = Currency_id,
                    Vendore_id = VendoreId
                });
                db.SaveChanges();
                return Json(1);
            }
            catch
            {
                return Json(0);
            }
        }
        public JsonResult GetNameAndCBookById(int Id)
        {
            int C_CBSID = 0;
            try
            {
                try
                {
                    C_CBSID = db.Receivable_vendore_settings.Include(x => x.Check_book).FirstOrDefault(x => x.Id == Id).Check_book.C_CBSID;
                }
                catch
                {

                }
                Receivable_vendore_setting CS = db.Receivable_vendore_settings.Find(Id);
                int? PaymentId = null;
                int? Shiping = null;
                if (CS.Payment_term_id != null)
                {
                    try
                    {
                        if (!db.Receivable_payment_terms.Find(CS.Payment_term_id).Inactive)
                        {
                            PaymentId = CS.Payment_term_id;
                        }
                    }
                    catch
                    {

                    }

                }
                if (CS.Shipping_method_id != null)
                {
                    try
                    {
                        if (!db.Receivable_shipping_methods.Find(CS.Shipping_method_id).Inactive)
                        {
                            Shiping = CS.Shipping_method_id;
                        }
                    }
                    catch
                    {

                    }

                }
                return Json(new
                {
                    Name = CS.Vendor_name,
                    CBook = C_CBSID,
                    PaymentPer = Convert.ToString(CS.Payment_per)
                                  ,
                    PTI = PaymentId,
                    SI = Shiping
                });

            }
            catch
            {
                return Json(new { Name = "", CBook = "" });

            }
        }

        public JsonResult CheckCreditorLimit(int Id)
        {
            Receivable_vendore_setting C = db.Receivable_vendore_settings.Find(Id);
            decimal PT = db.Receivable_transactions.Where(x => x.Vendor_id == Id&&x.Is_void==false).ToList()
                .DefaultIfEmpty(new Receivable_transaction { Purchase = 0, Taken_discount = 0, Tax = 0 })
                          .Sum(x => x.Purchase - x.Taken_discount + x.Tax);

            decimal PP = db.Receivable_payments.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList().DefaultIfEmpty(new Receivable_payment { Orginal_amount = 0, Taken_discount = 0 }).Sum(x => x.Orginal_amount - x.Taken_discount);
           
            decimal AP = db.Assign_Receivable_docs.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Taken_discount = 0 }).Sum(x => x.Taken_discount);
           
            decimal PTR = db.Receivable_transactions.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList()
                .DefaultIfEmpty(new Receivable_transaction { Purchase = 0, Taken_discount = 0, Tax = 0 })
                          .Sum(x => (x.Purchase - x.Taken_discount + x.Tax) * x.Transaction_rate) -
                          db.Receivable_payments.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList().DefaultIfEmpty(new Receivable_payment { Orginal_amount = 0, Taken_discount = 0 }).Sum(x => (x.Orginal_amount - x.Taken_discount) * x.Transaction_rate)
                          - db.Assign_Receivable_docs.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Taken_discount = 0 }).Sum(x => x.Taken_discount * x.Transaction_rate);
            if (C.Credit_limit == Credit_limit_enum.No_credit)
            {
                C.Credit_limit_amount = 0;
            }
            if (!C.Credit_limit_amount.HasValue)
            {
                C.Credit_limit_amount = 0;
            }
          
            return Json(new
            {
                status = C.Credit_limit,
                amount = PTR,
                veAmount = C.Credit_limit_amount,
                MinAmount = C.Minimum_order,
                MaxAmount = C.Maximum_order
            });
        }
        public JsonResult HasPassword(int Id)
        {
            try
            {
                return Json(!string.IsNullOrEmpty(db.Receivable_vendore_settings.Find(Id).Password));
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult CheckPass(int Id, string Password)
        {
            try
            {
                return Json(db.Receivable_vendore_settings.Any(x => x.Id == Id && x.Password == Password));
            }
            catch
            {
                return Json(false);
            }
        }
        public JsonResult VendorePaymentPer(int Id)
        {
            try
            {
                return Json(db.Receivable_vendore_settings.Find(Id).Payment_per);

            }
            catch
            {
                return Json(Payment_per.Invoice);
            }
        }
        // GET: Receivable/Creditor_setting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_vendore_setting creditor_setting = db.Receivable_vendore_settings.Find(id);
            if (creditor_setting == null)
            {
                return HttpNotFound();
            }
            return View(creditor_setting);
        }

        // GET: Receivable/Creditor_setting/Create
        public ActionResult Create()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables.Where(x=>x.CompanyID==companyID), "C_CBSID", "C_CheckbookID");

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");

            ViewBag.Group_setting_id = new SelectList(db.Receivable_Group_settings.Where(x => x.Inactive == false), "Id", "Group_id");
            ViewBag.Payment_term_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id");
            ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method");
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID");
           
            int Max = 0;
            foreach (string I in db.Receivable_vendore_settings.Select(x => x.Vendor_id))
            {
                if (FabulousErp.Business.GetDigitsIgnoreChars(I) != 0)
                {
                    Max = FabulousErp.Business.GetDigitsIgnoreChars(I);
                }
            }
            return View(new Receivable_vendore_setting
            {
                Vendor_id = (Max + 1).ToString()
            });
        }

        // POST: Receivable/Creditor_setting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Receivable_vendore_setting creditor_setting)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (creditor_setting.Credit_limit == Credit_limit_enum.No_credit)
                {
                    creditor_setting.Credit_limit_amount = 0;
                }
                db.Receivable_vendore_settings.Add(creditor_setting);
                db.SaveChanges();

                List<Receivable_vendore_currencies> CreditorCur = Enumerable.Empty<Receivable_vendore_currencies>().ToList();
                foreach (string Cur in creditor_setting.Currency_id)
                {
                    CreditorCur.Add(new Receivable_vendore_currencies
                    {
                        Currency_id=Cur,
                        Vendore_id= creditor_setting.Id
                    });
                }
                db.Receivable_vendore_currencies.AddRange(CreditorCur);
                db.SaveChanges();
                return Json(creditor_setting.Id);
            }

            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookID", creditor_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Group_setting_id = new SelectList(db.Receivable_Group_settings.Where(x => x.Inactive == false), "Id", "Group_id", creditor_setting.Group_setting_id);
            ViewBag.Payment_term_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", creditor_setting.Payment_term_id);
            ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", creditor_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", creditor_setting.Tax_group_id);
            return View(creditor_setting);
        }

        // GET: Receivable/Creditor_setting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_vendore_setting creditor_setting = db.Receivable_vendore_settings.Find(id);
            if (creditor_setting == null)
            {
                return HttpNotFound();
            }
            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookID", creditor_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Group_setting_id = new SelectList(db.Receivable_Group_settings.Where(x => x.Inactive == false), "Id", "Group_id", creditor_setting.Group_setting_id);
            ViewBag.Payment_term_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", creditor_setting.Payment_term_id);
            ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", creditor_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", creditor_setting.Tax_group_id);
            return View(creditor_setting);
        }

        // POST: Receivable/Creditor_setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Receivable_vendore_setting creditor_setting)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (creditor_setting.Credit_limit == Credit_limit_enum.No_credit)
                {
                    creditor_setting.Credit_limit_amount = 0;
                }
                db.Entry(creditor_setting).State = EntityState.Modified;
                db.SaveChanges();
                return Json(creditor_setting.Id);

            }
            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookID", creditor_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Group_setting_id = new SelectList(db.Receivable_Group_settings.Where(x => x.Inactive == false), "Id", "Group_id", creditor_setting.Group_setting_id);
            ViewBag.Payment_term_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", creditor_setting.Payment_term_id);
            ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", creditor_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", creditor_setting.Tax_group_id);
            return View(creditor_setting);
        }

        // GET: Receivable/Creditor_setting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Receivable_vendore_setting creditor_setting = db.Receivable_vendore_settings.Find(id);
            if (creditor_setting == null)
            {
                return HttpNotFound();
            }
            return View(creditor_setting);
        }

        // POST: Receivable/Creditor_setting/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            Receivable_vendore_setting creditor_setting = db.Receivable_vendore_settings.Find(id);

            if (db.Receivable_transactions.Any(x => x.Vendor_id == id)
                || db.Receivable_payments.Any(x => x.Vendor_id == id)
                || db.Assign_Receivable_docs.Any(x => x.Vendor_id == id))
            {
                return Json("This Customer Has Transaction You Can't Delete Him");
            }
            db.Receivable_vendore_settings.Remove(creditor_setting);
            try
            {

            db.Receivable_address_infos.Remove(db.Receivable_address_infos.FirstOrDefault(x=>x.Creditor_id==id));
            }
            catch
            {

            }
            try
            {
                db.Receivable_bank_info.Remove(db.Receivable_bank_info.FirstOrDefault(x => x.Creditor_id == id));
            }
            catch
            {

            }
            try
            {
                db.Receivable_legal_infos.Remove(db.Receivable_legal_infos.FirstOrDefault(x => x.Vendore_id == id));

            }
            catch
            {

            }
            try
            {
                db.Receivable_gl_accounts.Remove(db.Receivable_gl_accounts.FirstOrDefault(x => x.Creditor_setting_id == creditor_setting.Id));
            }
            catch
            {

            }
            try
            {
                db.Receivable_vendore_currencies.RemoveRange(db.Receivable_vendore_currencies.Where(x => x.Vendore_id == creditor_setting.Id));
            }
            catch
            {

            }
            try
            {
                db.SaveChanges();

                return Json(1);
            }
            catch (Exception ex)
            {
                return Json("This Creditor Has Payment You Can't Delete Him");
            }
        }
        public JsonResult CheckVendoreId(string VendoreId)
        {
            return Json(db.Receivable_vendore_settings.Any(x => x.Vendor_id == VendoreId));
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
