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
    public class Creditor_settingController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        // GET: Payable/Creditor_setting
        public ActionResult Index()
        {
            var Payable_creditor_setting = db.Payable_creditor_setting.Include(c => c.Check_book).Include(c => c.Group_setting).Include(c => c.Payment_terms).Include(c => c.Shippint_method).Include(c => c.Tax);
            return View(Payable_creditor_setting.ToList());
        }
        public JsonResult GetVendoreCurr(int VendoreId)
        {
            try
            {
                return Json(db.Creditro_currencies.Where(x => x.Vendore_id == VendoreId).Select(x => x.Currency_id));
            }
            catch
            {
                return Json(null);
            }
        }
        public JsonResult AddCurrencyToVendore(int VendoreId,string Currency_id)
        {
            try
            {
                db.Creditro_currencies.Add(new Payable_creditor_currencies
                {
                    Currency_id= Currency_id,
                    Vendore_id= VendoreId
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
                    C_CBSID = db.Payable_creditor_setting.Include(x => x.Check_book).FirstOrDefault(x => x.Id == Id).Check_book.C_CBSID;
                }
                catch
                {

                }
                Payable_creditor_setting CS = db.Payable_creditor_setting.Find(Id);
                int? PaymentId = null;
                int? Shiping = null;
                if (CS.Payment_term_id != null)
                {
                    try
                    {
                        if (!db.Payment_terms.Find(CS.Payment_term_id).Inactive)
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
                        if (!db.Shipping_methods.Find(CS.Shipping_method_id).Inactive)
                        {
                            Shiping = CS.Shipping_method_id;
                        }
                    }
                    catch
                    {

                    }
                  
                }
                return Json(new { Name =CS.Vendor_name, CBook = C_CBSID,PaymentPer=Convert.ToString(CS.Payment_per)
                                  ,PTI=PaymentId,SI=Shiping});

            }
            catch
            {
                return Json(new { Name = "", CBook = "" });

            }
        }

        public JsonResult CheckCreditorLimit(int Id)
        {
            Payable_creditor_setting C = db.Payable_creditor_setting.Find(Id);
            decimal PT = db.Payable_transactions.Where(x => x.Vendor_id == Id&&x.Is_void==false).ToList()
                .DefaultIfEmpty(new Payable_transaction { Purchase = 0, Taken_discount = 0, Tax = 0 })
                          .Sum(x => x.Purchase - x.Taken_discount + x.Tax);

            decimal PP = db.Payable_payments.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList().DefaultIfEmpty(new Payable_payment { Orginal_amount = 0, Taken_discount = 0 }).Sum(x => x.Orginal_amount - x.Taken_discount);
            decimal AP = db.Assign_payable_docs.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList().DefaultIfEmpty(new Assign_payable_doc { Taken_discount = 0 }).Sum(x => x.Taken_discount);
            decimal PTR = db.Payable_transactions.Where(x => x.Vendor_id == Id && x.Is_void == false).ToList()
                .DefaultIfEmpty(new Payable_transaction { Purchase = 0, Taken_discount = 0, Tax = 0 })
                          .Sum(x => (x.Purchase - x.Taken_discount + x.Tax)*x.Transaction_rate)-
                          db.Payable_payments.Where(x=>x.Vendor_id==Id && x.Is_void == false).ToList().DefaultIfEmpty(new Payable_payment {Orginal_amount=0,Taken_discount=0 }).Sum(x=>(x.Orginal_amount-x.Taken_discount) * x.Transaction_rate)
                          -db.Assign_payable_docs.Where(x=>x.Vendor_id==Id && x.Is_void == false).ToList().DefaultIfEmpty(new Assign_payable_doc { Taken_discount =0}).Sum(x=>x.Taken_discount*x.Transaction_rate);
            if (C.Credit_limit == Credit_limit_enum.No_credit)
            {
                C.Credit_limit_amount = 0;
            }
            if (!C.Credit_limit_amount.HasValue)
            {
                C.Credit_limit_amount = 0;
            }

            //if (PTR > 0)
            //{
            //    PTR = PTR - C.Credit_limit_amount.Value;
            //}
            //else
            //{
            //    PTR += C.Credit_limit_amount.Value;
            //}

            return Json(new
            {
                status=C.Credit_limit,
                amount= PTR,
                veAmount = C.Credit_limit_amount,
                MinAmount=C.Minimum_order,
                MaxAmount=C.Maximum_order
            });
        }
        public JsonResult HasPassword(int Id)
        {
            try
            {
                return Json(!string.IsNullOrEmpty(db.Payable_creditor_setting.Find(Id).Password));
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
                return Json(db.Payable_creditor_setting.Any(x => x.Id == Id && x.Password == Password));
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
                return Json(db.Payable_creditor_setting.Find(Id).Payment_per);

            }
            catch
            {
                return Json(Payment_per.Invoice);
            }
        }
        // GET: Payable/Creditor_setting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_creditor_setting creditor_setting = db.Payable_creditor_setting.Find(id);
            if (creditor_setting == null)
            {
                return HttpNotFound();
            }
            return View(creditor_setting);
        }

        // GET: Payable/Creditor_setting/Create
        public ActionResult Create()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables.Where(x=>x.CompanyID==companyID), "C_CBSID", "C_CheckbookID");

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");

            ViewBag.Group_setting_id = new SelectList(db.Payable_Group_settings.Where(x=>x.Inactive==false), "Id", "Group_id");
            ViewBag.Payment_term_id = new SelectList(db.Payment_terms.Where(x=>x.Inactive==false), "Id", "Terms_id");
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method");
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID");
          
            int Max = 0;
            foreach (string I in db.Payable_creditor_setting.Select(x => x.Vendor_id))
            {
                if (FabulousErp.Business.GetDigitsIgnoreChars(I) != 0)
                {
                    Max = FabulousErp.Business.GetDigitsIgnoreChars(I);
                }
            }
            return View(new Payable_creditor_setting 
            { 
                Vendor_id= (Max+1).ToString()
            });
        }

        // POST: Payable/Creditor_setting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Payable_creditor_setting creditor_setting)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                if (creditor_setting.Credit_limit == Credit_limit_enum.No_credit)
                {
                    creditor_setting.Credit_limit_amount = 0;
                }
                db.Payable_creditor_setting.Add(creditor_setting);
                db.SaveChanges();

                List<Payable_creditor_currencies> CreditorCur = Enumerable.Empty<Payable_creditor_currencies>().ToList();
                foreach (string Cur in creditor_setting.Currency_id)
                {
                    CreditorCur.Add(new Payable_creditor_currencies
                    {
                        Currency_id=Cur,
                        Vendore_id= creditor_setting.Id
                    });
                }
                db.Creditro_currencies.AddRange(CreditorCur);
                db.SaveChanges();
                return Json(creditor_setting.Id);
            }

            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookID", creditor_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode", db.Creditro_currencies.Where(x => x.Vendore_id == creditor_setting.Id).Select(x => x.Currency_id).ToList());
            ViewBag.Group_setting_id = new SelectList(db.Payable_Group_settings.Where(x => x.Inactive == false), "Id", "Group_id", creditor_setting.Group_setting_id);
            ViewBag.Payment_term_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", creditor_setting.Payment_term_id);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", creditor_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", creditor_setting.Tax_group_id);
            return View(creditor_setting);
        }

        // GET: Payable/Creditor_setting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_creditor_setting creditor_setting = db.Payable_creditor_setting.Find(id);
            if (creditor_setting == null)
            {
                return HttpNotFound();
            }
            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookID", creditor_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode",db.Creditro_currencies.Where(x=>x.Vendore_id==id).Select(x=>x.Currency_id).ToList());
            ViewBag.Currency_idSel =string.Join(",",db.Creditro_currencies.Where(x => x.Vendore_id == id).Select(x => x.Currency_id).ToList());
            ViewBag.Group_setting_id = new SelectList(db.Payable_Group_settings.Where(x => x.Inactive == false), "Id", "Group_id", creditor_setting.Group_setting_id);
            ViewBag.Payment_term_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", creditor_setting.Payment_term_id);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", creditor_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", creditor_setting.Tax_group_id);
            return View(creditor_setting);
        }

        // POST: Payable/Creditor_setting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Payable_creditor_setting creditor_setting)
        {
            ModelState.Clear();

            if (ModelState.IsValid)
            {
                if (creditor_setting.Credit_limit == Credit_limit_enum.No_credit)
                {
                    creditor_setting.Credit_limit_amount = 0;
                }
                db.Entry(creditor_setting).State = EntityState.Modified;
                db.Creditro_currencies.RemoveRange(db.Creditro_currencies.Where(x => x.Vendore_id == creditor_setting.Id).ToList());
                foreach (string Cur in creditor_setting.Currency_id)
                {
                    db.Creditro_currencies.Add(new Payable_creditor_currencies
                    {
                        Currency_id = Cur,
                        Vendore_id = creditor_setting.Id
                    });
                }
                db.SaveChanges();
                return Json(creditor_setting.Id);

            }
            ViewBag.Def_Checkbook = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookID", creditor_setting.Def_Checkbook);
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Group_setting_id = new SelectList(db.Payable_Group_settings.Where(x => x.Inactive == false), "Id", "Group_id", creditor_setting.Group_setting_id);
            ViewBag.Payment_term_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", creditor_setting.Payment_term_id);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", creditor_setting.Shipping_method_id);
            ViewBag.Tax_group_id = new SelectList(dbM.TaxGroup_Tables, "TG_ID", "TaxGroupID", creditor_setting.Tax_group_id);
            return View(creditor_setting);
        }

        // GET: Payable/Creditor_setting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_creditor_setting creditor_setting = db.Payable_creditor_setting.Find(id);
            if (creditor_setting == null)
            {
                return HttpNotFound();
            }
            return View(creditor_setting);
        }

        // POST: Payable/Creditor_setting/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Payable_creditor_setting creditor_setting = db.Payable_creditor_setting.Find(id);
            if (db.Payable_transactions.Any(x => x.Vendor_id == id)
                || db.Payable_payments.Any(x => x.Vendor_id == id)
                || db.Assign_payable_docs.Any(x => x.Vendor_id == id))
            {
                return Json("This Creditor Has Transaction You Can't Delete Him");
            }
            try
            {
                db.Payable_creditor_setting.Remove(creditor_setting);

            }
            catch
            {

            }
            try
            {
                db.Payable_address_infos.Remove(db.Payable_address_infos.FirstOrDefault(x => x.Creditor_id == id));

            }
            catch
            {

            }
            try
            {
                db.Bank_info.Remove(db.Bank_info.FirstOrDefault(x => x.Creditor_id == id));

            }
            catch
            {

            }
            try
            {
                db.Legal_infos.Remove(db.Legal_infos.FirstOrDefault(x => x.Creditor_id == id));

            }
            catch
            {

            }
            try
            {
                db.Payable_gl_accounts.Remove(db.Payable_gl_accounts.FirstOrDefault(x => x.Creditor_setting_id == creditor_setting.Id));

            }
            catch
            {

            }
            try
            {
                db.Creditro_currencies.RemoveRange(db.Creditro_currencies.Where(x => x.Vendore_id == creditor_setting.Id));

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
            return Json(db.Payable_creditor_setting.Any(x => x.Vendor_id == VendoreId));
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
