using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Payable.Controllers
{
    public class Payable_transactionController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        public JsonResult GetTransactionAmount(int Id, int CheckBookId)
        {
            string CBCurrencyId = dbM.C_CheckBookSetting_Tables.Find(CheckBookId).CurrencyID;
            Payable_transaction PT = db.Payable_transactions_types.Include(x => x.Payable_transaction).Where(x => x.Id == Id).SelectMany(x => x.Payable_transaction).FirstOrDefault();
            string TrxCurrencyId = PT.Currency_id;
            decimal Rate = 1;
            if (!db.Payable_transactions.Any(x => x.Currency_id == CBCurrencyId))
            {
                return Json(new {msg= "There are no Transaction With This CheckBook Currency" });
            }
            if (CBCurrencyId!= TrxCurrencyId)
            {
                return Json(new {msg= "The Transaction Currency is not Equal To CheckBook Rate" });
            }
            decimal Amount = PT.Purchase - PT.Taken_discount + PT.Tax* Rate;
            return Json(new {msg="", Amount,TransId= PT.Id,TrxDate= PT.Posting_date.ToString("yyyy-MM-dd") });
        }
        // GET: Payable/Payable_transaction
        public ActionResult Index()
        {
            ViewBag.section = Request["section"];

            return View();
        }
        public ActionResult IndexPartial(string section, int SortBy, DateTime? From = null, DateTime? To = null)
        {
            section = Request["section"];
            List<Payable_transaction> Res = db.Payable_transactions
              .Include(a => a.Trans_doc_type).Include(x=>x.Vendor).Include(x=>x.Currency).ToList();
            if (section== "void")
            {
                Res = Res.Where(x => x.Is_void == false).ToList();
            }
            

            ViewBag.section = section;
            if (SortBy == 2)
            {
                Res = Res.OrderBy(x => x.Creation_date).ToList();
            }
            else if (SortBy == 3)
            {
                Res = Res.OrderBy(x => x.Posting_date).ToList();

            }
            if ((From == null || To == null) && SortBy != 1)
            {
                SortBy = 4;
            }
            if (section != "void")
            {

                if (SortBy == 1)
                {
                    Res = Res.Where(x => x.Creation_date.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();
                }
                else if (SortBy == 2)
                {
                    Res = Res.Where(x => Convert.ToDateTime(x.Creation_date.ToShortDateString()) >=
                    Convert.ToDateTime(From.Value.ToShortDateString()) &&
                    Convert.ToDateTime(x.Creation_date.ToShortDateString()) <=
                    Convert.ToDateTime(To.Value.ToShortDateString())).ToList();
                }
                else if (SortBy == 3)
                {
                    Res = Res.Where(x => Convert.ToDateTime(x.Posting_date.ToShortDateString()) >=
                   Convert.ToDateTime(From.Value.ToShortDateString()) &&
                   Convert.ToDateTime(x.Posting_date.ToShortDateString()) <=
                   Convert.ToDateTime(To.Value.ToShortDateString())).ToList();
                }
            }
            return View(Res);
        }
        // GET: Payable/Payable_transaction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;
            Payable_transaction payable_transaction = db.Payable_transactions.Find(id);
            if (payable_transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.Void = null;

            if (payable_transaction.Is_void)
            {
                try
                {
                    C_GeneralJournalEntry_Table GJ = dbM.C_GeneralJournalEntry_Tables.Where(z => z.C_PostingNumber == dbM.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == payable_transaction.Journal_number).
                              FirstOrDefault().VoidPostingNum).FirstOrDefault();

                    ViewBag.Void = $"<div class='clearfix text-danger' style='display:block;font-weight:bold;'>This Transaction Has Been Voided In Date :<span style='color:#000;'> {GJ.C_PostingDate}</span> And Journal Entry Number (Voided) Is : <span style='color:#000;'>{GJ.C_JournalEntryNumber} ( {GJ.C_PostingKey} )</span></div>";
                }
                catch
                {
                    ViewBag.Void = null;
                }
               
            }
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.PostingNum = FabulousErp.Business.GetPotingNumber(payable_transaction.Journal_number);
            ViewBag.JE = payable_transaction.Journal_number;
            ViewBag.PostingToOrThrow = FabulousErp.Business.PostingToOrThrow();
            ViewBag.DocType = payable_transaction.Doc_type;
            ViewBag.PostingDate = payable_transaction.Posting_date.ToString("yyyy-MM-dd");
            ViewBag.TransactionDate = payable_transaction.Transaction_date.ToString("yyyy-MM-dd");
            ViewBag.TrxNum = db.Payable_transactions_types.Find(payable_transaction.Trans_doc_type_id).Trx_num;
            return View(payable_transaction);
        }

        // GET: Payable/Payable_transaction/Create
        public ActionResult Create(int? id = 0, bool Partial = false)
        {
            ViewBag.AllowPayment = db.Other_options.Where(x => x.Option == Other_option_enum.Active_payment).ToList().DefaultIfEmpty(new Payable_other_option { Checked = false }).FirstOrDefault().Checked;
            ViewBag.Partial = Partial;
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.LocalCurr = dbM.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "" }).FirstOrDefault().ISOCode;
            if (string.IsNullOrEmpty(companyID))
            {
                return Redirect("/");
            }
            ViewBag.companyID = companyID;
            Payable_transaction payable_transaction = db.Payable_transactions.Find(id);
            if (Partial)
            {
                ViewBag.PostingNum = FabulousErp.Business.GetPotingNumber(payable_transaction.Journal_number);
            }
            if (payable_transaction == null)
            {
                ViewBag.Vendor_id =Business.GetPayableVendoreSelect();
                ViewBag.Payment_terms_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id");
                ViewBag.Trans_doc_type_id = new SelectList(db.Payable_transactions_types, "Id", "Id");
                ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method");
                ViewBag.Doc_type = new SelectList(db.General_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Doc_type", "Doc_type");
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode", db.CurrenciesDefinition_Tables.FirstOrDefault(x => x.CurrencyID == companyID).CurrencyID);
                return View(new Payable_transaction
                {
                    Transaction_date = DateTime.Now,
                    Transaction_rate=1,
                    System_rate=1
                });
            }
            else
            {
                ViewBag.Vendor_id = Business.GetPayableVendoreSelect(payable_transaction.Vendor_id);
                ViewBag.Payment_terms_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", payable_transaction.Payment_terms_id);
                ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", payable_transaction.Shipping_method_id);
                ViewBag.Trans_doc_type_id = new SelectList(db.Payable_transactions_types, "Id", "Id", payable_transaction.Trans_doc_type_id);
                ViewBag.Doc_type = new SelectList(db.General_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Doc_type", "Doc_type");
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode", payable_transaction.Currency_id);
                if (payable_transaction.Doc_type == Doc_type.Credit_Memo
                   || payable_transaction.Doc_type == Doc_type.Return)
                {
                    payable_transaction.Purchase = -payable_transaction.Purchase;
                    payable_transaction.Taken_discount = -payable_transaction.Taken_discount;
                    payable_transaction.Tax = -payable_transaction.Tax;

                }
                return View(payable_transaction);
            }
        }
        public JsonResult ValidateTransaction(Payable_transaction payable_transaction)
        {
            ModelState["Trans_doc_type_id"].Errors.Clear();
            ModelState["Journal_number"].Errors.Clear();
            ModelState["Taken_discount"].Errors.Clear();
            ModelState["Tax"].Errors.Clear();
            //ModelState["Creation_date"].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg =ModelState.Where(x=>x.Value.Errors.Any()).SelectMany(x=>x.Value.Errors).Select(x=>x.ErrorMessage)});

            }
            if (!db.Other_options.FirstOrDefault(x => x.Option == Other_option_enum.Ovride).Checked
                && string.IsNullOrWhiteSpace(payable_transaction.VDocument_number))
            {
                return Json(new { status = false, msg = "You Should Enter  V.Document Number" });
            }
            else
            {
                return Json(new { status = true, msg = "" });
            }
        }
        // POST: Payable/Payable_transaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payable_transaction payable_transaction)
        {
            try
            {
                try
                {
                    ModelState["Trans_doc_type_id"].Errors.Clear();
                    ModelState["Journal_number"].Errors.Clear();
                    ModelState["Taken_discount"].Errors.Clear();
                    ModelState["Tax"].Errors.Clear();
                }
                catch
                {

                }
                if (payable_transaction.Transaction_rate == 0)
                {
                    payable_transaction.Transaction_rate = 1;
                } 
                if (payable_transaction.System_rate == 0)
                {
                    payable_transaction.System_rate = 1;
                }
                //ModelState["Creation_date"].Errors.Clear();
                payable_transaction.Creation_date = DateTime.Now;
                if (ModelState.IsValid)
                {
                    if (payable_transaction.Id == 0)
                    {
                        return Json(AddPayableTransaction(payable_transaction));
                    }
                    else
                    {

                        //db.Entry(payable_transaction).State = EntityState.Modified;
                        //db.SaveChanges();
                        //payable_transaction.Trans_doc_type = db.Payable_transactions_types.Where(x => x.Id == payable_transaction.Trans_doc_type_id).DefaultIfEmpty(new Payable_transactions_types { }).FirstOrDefault();
                        //return Json(new { PayId = payable_transaction.Id, TrxId = payable_transaction.Trans_doc_type_id, Trx_num = payable_transaction.Trans_doc_type.Trx_num, Counter = payable_transaction.Trans_doc_type.Counter });

                    }
                }
                List<string> Errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => x.Value.Errors.FirstOrDefault().ErrorMessage).ToList();
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id");
                ViewBag.Payment_terms_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id");
                ViewBag.Trans_doc_type_id = new SelectList(db.Payable_transactions_types, "Id", "Id");
                ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method");
                ViewBag.Doc_type = new SelectList(db.General_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Doc_type", "Doc_type");
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
                return Json(new { Error = string.Join(",", Errors) });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex });
            }

        }

        public dynamic AddPayableTransaction(Payable_transaction payable_transaction)
        {
            Payable_transactions_types PTY = new Payable_transactions_types
            {
                Counter = Business.GetNextDocNumber(payable_transaction.Doc_type),
                Doc_type = payable_transaction.Doc_type,
                Trx_num = Business.TrxNum(),
                Origin = TrxPay.Trx
            };
            if (payable_transaction.Currency_id == null)
            {
                payable_transaction.Currency_id = FabulousErp.Business.GetCompanyId();
            }

                db.Payable_transactions_types.Add(PTY);
            db.SaveChanges();
            payable_transaction.Trans_doc_type_id = PTY.Id;
            if (payable_transaction.Doc_type == Doc_type.Credit_Memo
                || payable_transaction.Doc_type == Doc_type.Return)
            {
                payable_transaction.Purchase = -(payable_transaction.Purchase);
                payable_transaction.Taken_discount = -(payable_transaction.Taken_discount);
                payable_transaction.Tax = -(payable_transaction.Tax);
            }
            db.Payable_transactions.Add(payable_transaction);
            db.SaveChanges();
            return new { PayId = payable_transaction.Id, TrxId = PTY.Id, Trx_num = PTY.Trx_num, Counter = PTY.Counter };
        }

        // GET: Payable/Payable_transaction/Edit/5
        public ActionResult Edit(int? id, bool Partial = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_transaction payable_transaction = db.Payable_transactions.Find(id);
            if (payable_transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.Partial = Partial;

            ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id", payable_transaction.Vendor_id);
            ViewBag.Payment_terms_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", payable_transaction.Payment_terms_id);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", payable_transaction.Shipping_method_id);
            ViewBag.Trans_doc_type_id = new SelectList(db.Payable_transactions_types, "Id", "Id", payable_transaction.Trans_doc_type_id);
            ViewBag.Doc_type = new SelectList(db.General_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Id", "Doc_type");

            return View(payable_transaction);
        }

        // POST: Payable/Payable_transaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Trans_doc_type_id,Doc_type,Desc,Transation_date,Posting_date,System_rate,Transaction_rate,Vendor_id,PONumber,VDocument_number,Doc_date,Payment_terms_id,Shipping_method_id,Purchase,Taken_discount,Tax")] Payable_transaction payable_transaction)
        {
            ModelState["Creation_date"].Errors.Clear();

            if (ModelState.IsValid)
            {
                db.Entry(payable_transaction).State = EntityState.Modified;
                db.Entry(payable_transaction).Property(x => x.Creation_date).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id", payable_transaction.Vendor_id);
            ViewBag.Payment_terms_id = new SelectList(db.Payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", payable_transaction.Payment_terms_id);
            ViewBag.Shipping_method_id = new SelectList(db.Shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", payable_transaction.Shipping_method_id);
            ViewBag.Trans_doc_type_id = new SelectList(db.Payable_transactions_types, "Id", "Id", payable_transaction.Trans_doc_type_id);
            ViewBag.Doc_type = new SelectList(db.General_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Id", "Doc_type");

            return View(payable_transaction);
        }


        public JsonResult CheckDocNumWithVendore(int VendoreId,string DocNum)
        {
            try
            {
              return  Json(db.Payable_transactions.Any(x => x.Vendor_id == VendoreId && x.VDocument_number==DocNum));
            }
            catch
            {
                return Json(false);
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
