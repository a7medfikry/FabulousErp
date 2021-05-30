using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Receivable.Controllers
{
    public class Receivable_transactionController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        public JsonResult GetTransactionAmount(int Id, int CheckBookId)
        {
            string CBCurrencyId = dbM.C_CheckBookSetting_Tables.Find(CheckBookId).CurrencyID;
            Receivable_transaction PT = db.Receivable_transactions_types.Include(x => x.Receivable_transaction).Where(x => x.Id == Id).SelectMany(x => x.Receivable_transaction).FirstOrDefault();
            string TrxCurrencyId = PT.Currency_id;
            decimal Rate = 1;
            if (!db.Receivable_transactions.Any(x => x.Currency_id == CBCurrencyId))
            {
                return Json(new { msg = "There are no Transaction With This CheckBook Currency" });
            }
            if (CBCurrencyId != TrxCurrencyId)
            {
                return Json(new { msg = "The Transaction Currency is not Equal To CheckBook Rate" });
            }
            decimal Amount = PT.Purchase - PT.Taken_discount + PT.Tax * Rate;
            return Json(new { msg = "", Amount, TransId = PT.Id, TrxDate = PT.Posting_date.ToString("yyyy-MM-dd") });
        }
        // GET: Receivable/Receivable_transaction
        public ActionResult Index()
        {
            ViewBag.section = Request["section"];

            return View();
        }
        public ActionResult IndexPartial(string section, int SortBy, DateTime? From = null, DateTime? To = null)
        {
            section = Request["section"];
            List<Receivable_transaction> Res = db.Receivable_transactions
              .Include(a => a.Trans_doc_type).Include(x => x.Vendor).Include(x => x.Currency).ToList();
            if (section == "void")
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
        // GET: Receivable/Receivable_transaction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;

            Receivable_transaction Receivable_transaction = db.Receivable_transactions.Find(id);
            if (Receivable_transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.Void = null;

            if (Receivable_transaction.Is_void)
            {
                try
                {
                    C_GeneralJournalEntry_Table GJ = dbM.C_GeneralJournalEntry_Tables.Where(z => z.C_PostingNumber == dbM.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == Receivable_transaction.Journal_number).
                              FirstOrDefault().VoidPostingNum).FirstOrDefault();

                    ViewBag.Void = $"<div class='clearfix text-danger' style='display:block;font-weight:bold;'>This Transaction Has Been Voided In Date :<span style='color:#000;'> {GJ.C_PostingDate}</span> And Journal Entry Number (Voided) Is : <span style='color:#000;'>{GJ.C_JournalEntryNumber} ( {GJ.C_PostingKey} )</span></div>";
                }
                catch
                {
                    ViewBag.Void = null;
                }

            }
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.PostingNum = FabulousErp.Business.GetPotingNumber(Receivable_transaction.Journal_number);
            ViewBag.JE = Receivable_transaction.Journal_number;
            ViewBag.PostingToOrThrow = FabulousErp.Business.PostingToOrThrow();
            ViewBag.DocType = Receivable_transaction.Doc_type;
            ViewBag.PostingDate = Receivable_transaction.Posting_date.ToString("yyyy-MM-dd");
            ViewBag.TransactionDate = Receivable_transaction.Transaction_date.ToString("yyyy-MM-dd");
            ViewBag.TrxNum = db.Receivable_transactions_types.Find(Receivable_transaction.Trans_doc_type_id).Trx_num;

            return View(Receivable_transaction);
        }

        // GET: Receivable/Receivable_transaction/Create
        public ActionResult Create(int? id = 0, bool Partial = false)
        {
            ViewBag.AllowPayment = db.Receivable_other_options.Where(x => x.Option == Other_option_enum.Active_payment).ToList().DefaultIfEmpty(new Receivable_other_option { Checked = false }).FirstOrDefault().Checked;
            ViewBag.Partial = Partial;
            ViewBag.PageKey = "RecTrans";
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.LocalCurr = dbM.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == companyID).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "" }).FirstOrDefault().ISOCode;
            if (string.IsNullOrEmpty(companyID))
            {
                return Redirect("/");
            }
            ViewBag.companyID = companyID;
            Receivable_transaction Receivable_transaction = db.Receivable_transactions.Find(id);
            if (Partial)
            {
                ViewBag.PostingNum = FabulousErp.Business.GetPotingNumber(Receivable_transaction.Journal_number);
            }
            if (Receivable_transaction == null)
            {
                ViewBag.Vendor_id = Business.GetCustomerReceivableSelect();
                ViewBag.Payment_terms_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id");
                ViewBag.Trans_doc_type_id = new SelectList(db.Receivable_transactions_types, "Id", "Id");
                ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x=>x.Inactive==false), "Id", "Ship_method");
                ViewBag.Doc_type = new SelectList(db.Receivable_general_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Doc_type", "Doc_type");
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode",db.CurrenciesDefinition_Tables.FirstOrDefault(x=>x.CurrencyID== companyID).CurrencyID);
                return View(new Receivable_transaction
                {
                    Transaction_date = DateTime.Now,
                    Transaction_rate=1,
                    System_rate=1
                });
            }
            else
            {
                ViewBag.Vendor_id = Business.GetCustomerReceivableSelect(Receivable_transaction.Vendor_id);
                ViewBag.Payment_terms_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", Receivable_transaction.Payment_terms_id);
                ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", Receivable_transaction.Shipping_method_id);
                ViewBag.Trans_doc_type_id = new SelectList(db.Receivable_transactions_types, "Id", "Id", Receivable_transaction.Trans_doc_type_id);
                ViewBag.Doc_type = new SelectList(db.Receivable_general_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Doc_type", "Doc_type", Receivable_transaction.Doc_type);
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode", Receivable_transaction.Currency_id);
                if (Receivable_transaction.Doc_type == Doc_type.Credit_Memo
                 || Receivable_transaction.Doc_type == Doc_type.Return)
                {
                    Receivable_transaction.Purchase = -Receivable_transaction.Purchase;
                    Receivable_transaction.Taken_discount = -Receivable_transaction.Taken_discount;
                    Receivable_transaction.Tax = -Receivable_transaction.Tax;

                }

                return View(Receivable_transaction);
            }


        }
        public JsonResult ValidateTransaction(Receivable_transaction Receivable_transaction)
        {
            ModelState["Trans_doc_type_id"].Errors.Clear();
            ModelState["Journal_number"].Errors.Clear();
            //ModelState["Creation_date"].Errors.Clear();

            if (!ModelState.IsValid)
            {
                return Json(new { status = false, msg = ModelState.Where(x => x.Value.Errors.Any()).SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage) });

            }
            return Json(new { status = true, msg = "" });
        }
        // POST: Receivable/Receivable_transaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Receivable_transaction Receivable_transaction)
        {
            try
            {
                ModelState["Trans_doc_type_id"].Errors.Clear();
                ModelState["Journal_number"].Errors.Clear();
                //ModelState["Creation_date"].Errors.Clear();
                if (Receivable_transaction.Transaction_rate == 0)
                {
                    Receivable_transaction.Transaction_rate = 1;
                }
                if (Receivable_transaction.System_rate == 0)
                {
                    Receivable_transaction.System_rate = 1;
                }
                Receivable_transaction.Creation_date = DateTime.Now;
                if (ModelState.IsValid)
                {
                    if (Receivable_transaction.Id == 0)
                    {
                        return Json(AddReceivableTransaction(Receivable_transaction));
                    }
                    else
                    {
                        //db.Entry(Receivable_transaction).State = EntityState.Modified;
                        //db.SaveChanges();
                        //Receivable_transaction.Trans_doc_type = db.Receivable_transactions_types.Where(x => x.Id == Receivable_transaction.Trans_doc_type_id).DefaultIfEmpty(new Receivable_transactions_types { }).FirstOrDefault();
                        //return Json(new { PayId = Receivable_transaction.Id, TrxId = Receivable_transaction.Trans_doc_type_id, Trx_num = Receivable_transaction.Trans_doc_type.Trx_num, Counter = Receivable_transaction.Trans_doc_type.Counter });
                    }
                }
                List<string> Errors = ModelState.Where(x => x.Value.Errors.Any()).Select(x => x.Value.Errors.FirstOrDefault().ErrorMessage).ToList();
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings.Where(x => x.Inactive == false), "Id", "Vendor_id");
                ViewBag.Payment_terms_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id");
                ViewBag.Trans_doc_type_id = new SelectList(db.Receivable_transactions_types, "Id", "Id");
                ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method");
                ViewBag.Doc_type = new SelectList(db.Receivable_general_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Doc_type", "Doc_type");
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
                return Json(new { Error = string.Join(",", Errors) });
            }
            catch (Exception ex)
            {
                return Json(new { Error = ex });


            }

        }

        public dynamic AddReceivableTransaction(Receivable_transaction Receivable_transaction
            ,bool SetCounterAsDocNumber=false)
        {
            Receivable_transactions_types PTY = new Receivable_transactions_types
            {
                Counter = Business.GetNextDocNumber(Receivable_transaction.Doc_type),
                Doc_type = Receivable_transaction.Doc_type,
                Trx_num = Business.TrxNum(),
                Origin = TrxPay.Trx
            };
            if (Receivable_transaction.Currency_id == null)
            {
                Receivable_transaction.Currency_id = FabulousErp.Business.GetCompanyId();
            }
            db.Receivable_transactions_types.Add(PTY);
            db.SaveChanges();

            Receivable_transaction.Trans_doc_type_id = PTY.Id;

            if (Receivable_transaction.Doc_type == Doc_type.Credit_Memo
                || Receivable_transaction.Doc_type == Doc_type.Return)
            {
                Receivable_transaction.Purchase = -(Receivable_transaction.Purchase);
                Receivable_transaction.Taken_discount = -(Receivable_transaction.Taken_discount);
                Receivable_transaction.Tax = -(Receivable_transaction.Tax);
            }
            if (SetCounterAsDocNumber)
            {
                Receivable_transaction.VDocument_number = PTY.Counter.ToString();
            }
            db.Receivable_transactions.Add(Receivable_transaction);
            db.SaveChanges();
            return new { PayId = Receivable_transaction.Id,RecId = Receivable_transaction.Id, TrxId = PTY.Id, Trx_num = PTY.Trx_num, Counter = PTY.Counter };
        }

        // GET: Receivable/Receivable_transaction/Edit/5
        public ActionResult Edit(int? id, bool Partial = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_transaction Receivable_transaction = db.Receivable_transactions.Find(id);
            if (Receivable_transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.Partial = Partial;

            ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings.Where(x => x.Inactive == false), "Id", "Vendor_id", Receivable_transaction.Vendor_id);
            ViewBag.Payment_terms_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", Receivable_transaction.Payment_terms_id);
            ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", Receivable_transaction.Shipping_method_id);
            ViewBag.Trans_doc_type_id = new SelectList(db.Receivable_transactions_types, "Id", "Id", Receivable_transaction.Trans_doc_type_id);
            ViewBag.Doc_type = new SelectList(db.Receivable_general_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Id", "Doc_type");

            return View(Receivable_transaction);
        }

        // POST: Receivable/Receivable_transaction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Trans_doc_type_id,Doc_type,Desc,Transation_date,Posting_date,System_rate,Transaction_rate,Vendor_id,PONumber,VDocument_number,Doc_date,Payment_terms_id,Shipping_method_id,Purchase,Taken_discount,Tax")] Receivable_transaction Receivable_transaction)
        {
            ModelState["Creation_date"].Errors.Clear();

            if (ModelState.IsValid)
            {
                db.Entry(Receivable_transaction).State = EntityState.Modified;
                db.Entry(Receivable_transaction).Property(x => x.Creation_date).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings.Where(x => x.Inactive == false), "Id", "Vendor_id", Receivable_transaction.Vendor_id);
            ViewBag.Payment_terms_id = new SelectList(db.Receivable_payment_terms.Where(x => x.Inactive == false), "Id", "Terms_id", Receivable_transaction.Payment_terms_id);
            ViewBag.Shipping_method_id = new SelectList(db.Receivable_shipping_methods.Where(x => x.Inactive == false), "Id", "Ship_method", Receivable_transaction.Shipping_method_id);
            ViewBag.Trans_doc_type_id = new SelectList(db.Receivable_transactions_types, "Id", "Id", Receivable_transaction.Trans_doc_type_id);
            ViewBag.Doc_type = new SelectList(db.Receivable_general_settings.Where(x => x.Checked && x.Doc_type != Doc_type.Payment), "Id", "Doc_type");

            return View(Receivable_transaction);
        }

      
        public JsonResult CheckDocNumWithVendore(int VendoreId, string DocNum)
        {
            try
            {
                return Json(db.Receivable_transactions.Any(x => x.Vendor_id == VendoreId && x.VDocument_number == DocNum));
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
