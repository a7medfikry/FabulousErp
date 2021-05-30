using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.Models;
using FabulousErp.Payable.Models; using FabulousDB.DB_Context;
using FabulousModels.ViewModels;
using FBus = FabulousErp.Business;

namespace Payable.Controllers
{
    public class Payable_paymentController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        // GET: Payable/Payable_payment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexPartial(string section, int SortBy, DateTime? From = null, DateTime? To = null)
        {
            List<Payable_payment> Res = Enumerable.Empty<Payable_payment>().ToList();

            if (section == "void")
            {
                Res = db.Payable_payments
             .Include(a => a.Trans_doc_type).Include(x => x.Vendor).Include(x => x.CheckBook_setting).Include(x => x.CheckBook_transaction).Where(x => x.Is_void == false).ToList();
            }
            else
            {
                Res = db.Payable_payments
             .Include(a => a.Trans_doc_type).Include(x => x.Vendor).Include(x => x.CheckBook_setting).Include(x => x.CheckBook_transaction).Include(x => x.Currency).ToList();
            }

            ViewBag.section = section;
            if (section != "void")
            {
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

        // GET: Payable/Payable_payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_payment payable_payment = db.Payable_payments.Find(id);

            ViewBag.id = payable_payment.Transaction_id;

            if (payable_payment == null)
            {
                return HttpNotFound();
            }

            return View(payable_payment);
        }

        // GET: Payable/Payable_payment/Create
        public ActionResult Create(int? id = null)
        {
            string companyID = FabulousErp.Business.GetCompanyId();
            ViewBag.D = Request["D"];
            if (ViewBag.D != null)
            {
                if (id.HasValue)
                {
                    if (db.Payable_payments.Find(id).Is_void)
                    {
                        try
                        {
                            int JN = db.Payable_payments.Find(id).Journal_number;
                            C_GeneralJournalEntry_Table GJ = dbM.C_GeneralJournalEntry_Tables.Where(z => z.C_PostingNumber == dbM.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == JN).
                                      FirstOrDefault().VoidPostingNum).FirstOrDefault();

                            ViewBag.Void = $"<div class='clearfix text-danger' style='display:block;font-weight:bold;'>This Transaction Has Been Voided In Date :<span style='color:#000;'> {GJ.C_PostingDate}</span> And Journal Entry Number (Voided) Is : <span style='color:#000;'>{GJ.C_JournalEntryNumber} ( {GJ.C_PostingKey} )</span></div>";
                        }
                        catch
                        {
                            ViewBag.Void = null;
                        }

                    }
                }

            }
            try
            {
                ViewBag.JE = db.Payable_payments.Find(id).Journal_number;

            }
            catch
            {

            }

            ViewBag.IsInstallment = Request["Installment"];
            ViewBag.Id = id;
            ViewBag.companyID = companyID;
            return View();
        }
        public JsonResult GetCheckBType(Cash_type Type)
        {
            if (Type == Cash_type.Cheque)
            {
                return Json(dbM.C_CheckBookSetting_Tables.Where(x => x.C_CheckbookType == "Bank" || x.C_CheckbookType == "Check")
                      .Select(x => new
                      {
                          Id = x.C_CBSID,
                          Name = x.C_CheckbookName
                      }).ToList());
            }
            else if (Type == Cash_type.Cash)
            {
                return Json(dbM.C_CheckBookSetting_Tables.Where(x => x.C_CheckbookType == "Cash")
                 .Select(x => new
                 {
                     Id = x.C_CBSID,
                     Name = x.C_CheckbookName
                 }).ToList());
            }
            else
            {
                return Json(dbM.C_CheckBookSetting_Tables
               .Select(x => new
               {
                   Id = x.C_CBSID,
                   Name = x.C_CheckbookName,
                   Type = x.C_CheckbookType

               }).ToList());
            }
            return Json(null);
        }
        public ActionResult PartialCreate(int? id = null, bool Partial = false,
            bool IsTransaction = false, bool IsInstallment= false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Partial = Partial;

            ViewBag.companyID = companyID;
            Payable_payment payable_payment = new Payable_payment();
            ViewBag.IsTransaction = IsTransaction;
            List<Installment_setting> a = dbM.Installment_settings.ToList();
            payable_payment.System_rate = 1;
            payable_payment.Transaction_rate = 1;
            payable_payment.IsInstallment = IsInstallment;

            if (IsTransaction)
            {
                ViewBag.InstallmentSelect = new SelectList(dbM.Installment_settings.ToList(), "Id", "Plan_id");
            }
            if (id != null)
            {
                if (IsTransaction)
                {

                    payable_payment = db.Payable_payments.Include(x => x.Vendor).FirstOrDefault(x => x.Id == id);
                }
                else
                {
                    payable_payment = db.Payable_payments.Include(x => x.Vendor).FirstOrDefault(x => x.Transaction_id == id);
                }
            }
            else
            {
                payable_payment = null;
            }
           
            if (payable_payment == null)
            {
                ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName");
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
                ViewBag.Transaction_id = new SelectList(Enumerable.Empty<Payable_transactions_types>().ToList(), "Id", "Trx_num");//db.Payable_transactions_types.Where(x => x.Origin == TrxPay.Trx)
                ViewBag.Vendor_id = Business.GetPayableVendoreSelect(); 
                return View(new Payable_payment { Transaction_date = DateTime.Now, Posting_date = DateTime.Now , System_rate =1,Transaction_rate=1});
            }
            else
            {
                ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", payable_payment.Check_book_id);
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode", payable_payment.Currency_id);
                ViewBag.Transaction_id = new SelectList(Enumerable.Empty<Payable_transactions_types>().ToList(), "Id", "Id", payable_payment.Transaction_id);
                ViewBag.Vendor_id = Business.GetPayableVendoreSelect(payable_payment.Vendor_id);

                return View(payable_payment);
            }

        }
        public JsonResult GetTranstion(Doc_type DT, int VendoreId)
        {
            List<Payable_transaction> UnPaidTrnx = Payable.Controllers.Business.GetUnpaidTransaction(VendoreId).Where(x => x.Doc_type == DT).ToList();

            UnPaidTrnx.RemoveAll(x => x.Purchase - x.Taken_discount + x.Tax == 0);

            var Res = UnPaidTrnx
               .Select(x => new { x.Trans_doc_type.Id, x.Trans_doc_type.Counter }).ToList();
            return Json(Res);
        }
        public JsonResult GetInstContract(int VendoreId)
        {
            List<Installment_contract> Contract = dbM.Installment_contracts.Where(x => x.Vendore_id
            == VendoreId).ToList();

            var Res = Contract
               .Select(x => new {Id= x.Id,Name= x.Desc }).ToList();

            return Json(Res);
            return Json(null);
        }
        public JsonResult GetInstallment(int ContractId)
        {
            List<Installments> UnPaidInst = dbM.Installments.Where(x => x.Contract_id == ContractId
            && x.Contract.IsPay == true&&x.Paid==false&&x.Historical==false).ToList();
            
            var Res = UnPaidInst
               .Select(x => new { x.Id,Name= x.Refrence}).ToList();

            return Json(Res);
            return Json(null);
        }
        public JsonResult GetInstallmentAmount (int Id)
        {
            Installments Inst = dbM.Installments.Find(Id);
            return Json(new { amount = Inst.Amount,duedate= Inst.Cheque_Date.HasValue? Inst.Cheque_Date.Value.ToString("yyyy-MM-dd") :null,chequenumber= Inst.Cheque_number });
        }
        // POST: Payable/Payable_payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Payable_payment payable_payment)
        {
            try
            {
                ModelState["Payment_no"].Errors.Clear();
                ModelState["Id"].Errors.Clear();
                ModelState["System_rate"].Errors.Clear();
                ModelState["Transaction_rate"].Errors.Clear();
            }
            catch
            {

            }
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            payable_payment.Creation_date = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (payable_payment.Id == 0)
                {
                    int Max = 1;
                    if (db.Payable_payments.Any())
                    {
                        Max = (int)db.Payable_payments.Max(x => x.Payment_no) + 1;
                    }
                    Payable_transactions_types PTY = new Payable_transactions_types
                    {
                        Counter = Business.GetNextDocNumber(Doc_type.Payment),
                        Doc_type = Doc_type.Payment,
                        Trx_num = Business.TrxNum(),
                        Origin = TrxPay.Pay
                    };
                    db.Payable_transactions_types.Add(PTY);
                    db.SaveChanges();
                    payable_payment.Trans_doc_type_id = PTY.Id;


                    payable_payment.Payment_no = Max;
                    Payable_transaction PT = Enumerable.Empty<Payable_transaction>().FirstOrDefault();
                    int? Transaction_id = null;
                    try
                    {
                        PT = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == payable_payment.Trx_trans_doc_type_id);

                        //if (!payable_payment.Transaction_id.HasValue)
                        //{
                        //    PT = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == payable_payment.Transaction_id);

                        //    // Transaction_id = PT.Trans_doc_type_id;
                        //}
                        //else
                        //{
                        //    // Transaction_id = payable_payment.Transaction_id;
                        //    PT = db.Payable_transactions.Find(payable_payment.Transaction_id);
                        //    if (PT == null)
                        //    {
                        //       PT = db.Payable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == payable_payment.Transaction_id);
                        //    }
                        //}
                    }
                    catch
                    {

                    }

                    payable_payment.Transaction_p_id = Transaction_id;
                    payable_payment.Transaction_id = null;

                    if (payable_payment.Currency_id == null)
                    {
                        payable_payment.Currency_id = FabulousErp.Business.GetCompanyId();
                    }

                    db.Payable_payments.Add(payable_payment);

                    db.SaveChanges();
                    if (PT != null)
                    {
                        try
                        {
                            db.Related_pay_trans.Add(new Related_pay_trans
                            {
                                Payment_id = payable_payment.Trans_doc_type_id,
                                Transaction_id = PT.Trans_doc_type_id
                            });
                            db.SaveChanges();
                        }
                        catch
                        {

                        }
                    }


                    if (payable_payment.Currency_id == companyID
                        && PT != null)
                    {
                        decimal UnAssign = 0;
                        decimal Assign = 0;
                        if (db.Assign_payable_docs.Any(x => x.Trans_doc_type_id_to == PT.Trans_doc_type_id))
                        {
                            try
                            {
                                PT.Purchase = PT.Purchase - db.Assign_payable_docs.Where(x => x.Trans_doc_type_id_to == PT.Trans_doc_type_id)
                                                    .Sum(x => x.Taken_discount)
                                                     - db.Assign_payable_docs.Where(x => x.Trans_doc_type_id_to == PT.Trans_doc_type_id)
                                                     .DefaultIfEmpty(new Assign_payable_doc { Trans_doc_type = new Payable_transactions_types { Payable_payment = new List<Payable_payment> { new Payable_payment { Taken_discount = 0 } } } })
                                                     .SelectMany(x => x.Trans_doc_type.Payable_payment).Sum(x => x.Taken_discount);
                            }
                            catch
                            {
                                PT.Purchase = payable_payment.Orginal_amount;
                            }

                        }
                        if (payable_payment.Orginal_amount >= (PT.Purchase - PT.Taken_discount + PT.Tax))
                        {
                            Assign = (PT.Purchase - PT.Taken_discount + PT.Tax);
                            UnAssign = payable_payment.Orginal_amount - (PT.Purchase - PT.Taken_discount + PT.Tax);
                        }
                        else
                        {
                            Assign = payable_payment.Orginal_amount;
                            UnAssign = 0;
                        }
                        db.Assign_payable_docs.Add(new Assign_payable_doc
                        {
                            Applay_assign = Assign,
                            Applay_date = payable_payment.Posting_date,
                            Creation_date = DateTime.Now,
                            Is_void = false,
                            Assign_no = db.Assign_payable_docs.ToList().DefaultIfEmpty(new Assign_payable_doc { Assign_no = 0 }).Max(x => x.Assign_no) + 1,
                            Currency_id = payable_payment.Currency_id,
                            Doc_Num = "",
                            JournalEntry = 0,
                            Trans_doc_type_id = payable_payment.Trans_doc_type_id,
                            Trans_doc_type_id_to = PT.Trans_doc_type_id,
                            Vendor_id = PT.Vendor_id.Value,
                            Taken_discount = payable_payment.Taken_discount,
                            Unassign_amount = (UnAssign < 0) ? 0 : UnAssign,
                            Orginal_amount = payable_payment.Orginal_amount,
                            Doc_type = Doc_type.Payment,
                            Earn_or_lose = 0,
                            Transaction_rate = PT.Transaction_rate
                        });
                        db.SaveChanges();
                    }



                    decimal ReciptAmount = 0; //db.Payable_transactions.Where(x => x.Trans_doc_type_id == payable_payment.Transaction_id).ToList().DefaultIfEmpty(new Payable_transaction { }).FirstOrDefault().Purchase;
                    //if (PT == null)
                    //{
                    //}
                    try
                    {
                        ReciptAmount = payable_payment.Orginal_amount;
                    }
                    catch 
                    {

                    }

                    string UserID = FabulousErp.Business.GetUserId();
                    int PostingNumber = FabulousErp.Business.GetPotingNumber(payable_payment.Journal_number);
                    string DueDate = null;
                    if (payable_payment.Due_date.HasValue)
                    {
                        DueDate = payable_payment.Due_date.Value.ToShortDateString();
                    }
                    int C_CBSID = dbM.C_CheckBookSetting_Tables.Find(payable_payment.Check_book_id).C_CBSID;
                    string CheckNumber = null;
                    try
                    {
                        CheckNumber = payable_payment.Cheque_number;
                    }
                    catch
                    {

                    }


                    //Add To Check Book
                    List<C_CheckbookTransactions_table> CBT = new List<C_CheckbookTransactions_table>();
                    payable_payment.UserId = UserID;
                    payable_payment.PostingNumber = PostingNumber;
                    payable_payment.Check_book_id = C_CBSID;
                    payable_payment.Payment = ReciptAmount;
                    
                    C_CheckbookTransactions_table CheckBook =
                        AddToCheckBook(payable_payment, CheckNumber);

                   


                    if (payable_payment.IsInstallment)
                    {
                        try
                        {
                            dbM.Installments.Find(payable_payment.Installment_id).Paid = true;
                            dbM.Installments.Find(payable_payment.Installment_id).Check_book_trx_id = CheckBook.C_CBT;
                            dbM.SaveChanges();
                        }
                        catch
                        {

                        }
                    }
                   

                    payable_payment.Check_book_transaction_id = CheckBook.C_CBT;
                    db.SaveChanges();
                    if (CheckBook.C_DocumentNumber == 0)
                    {
                        try
                        {
                            CheckBook.C_DocumentNumber = CBT.FirstOrDefault().C_DocumentNumber;
                        }
                        catch
                        {

                        }
                    }
                    if (CheckBook.C_DocumentNumber == 0)
                    {
                        try
                        {
                            CheckBook.C_DocumentNumber = CBT.FirstOrDefault().C_DocumentNumber;
                        }
                        catch
                        {

                        }
                    }
                    return Json(new { PaymentId = payable_payment.Id,
                        BookId = CheckBook.C_DocumentNumber,
                        PostingNumber = PostingNumber,
                        InstallmentList = CBT.Select(x => x.C_DocumentNumber) });
                }
                else
                {
                    db.Entry(payable_payment).State = EntityState.Modified;
                    db.Entry(payable_payment).Property(x => x.Payment_no).IsModified = false;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", payable_payment.Check_book_id);
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Transaction_id = new SelectList(db.Payable_transactions_types.Where(x => x.Origin == TrxPay.Trx), "Trx_num", "Id", payable_payment.Transaction_id);
            ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id", payable_payment.Vendor_id);
            return View(payable_payment);
        }
        public C_CheckbookTransactions_table AddToCheckBook(Payable_payment payable_payment, string CheckNumber)
        {
            C_CheckbookTransactions_table CheckBook = new C_CheckbookTransactions_table();

            string companyID = FabulousErp.Business.GetCompanyId();
            CheckBook = new C_CheckbookTransactions_table
            {
                CompanyID = companyID,
                C_PostingNumber = payable_payment.PostingNumber,
                C_TransactionDate = payable_payment.Transaction_date.ToShortDateString(),
                C_PostingDate = payable_payment.Posting_date.ToShortDateString(),
                C_SystemRate = (double?)payable_payment.System_rate,
                C_TransactionRate = (double?)payable_payment.Transaction_rate,
                C_Reference = payable_payment.Reference,
                C_DocumentType = "SID",
                C_Payment_To_Recieved_From =(payable_payment.Vendor_id!=0)?db.Payable_creditor_setting.Find(payable_payment.Vendor_id).Vendor_name: payable_payment.Payment_To_Recieved_From,
                C_Reciept = payable_payment.ReciptAmount,
                C_Payment = payable_payment.Orginal_amount,
                UserID = string.IsNullOrEmpty(payable_payment.UserId) ? FabulousErp.Business.GetUserId() : payable_payment.UserId,
                CurrencyID = payable_payment.Currency_id,
                C_DateTime = DateTime.Now,
                C_CBSID = payable_payment.Check_book_id,
                C_Difference = (double)(payable_payment.System_rate - payable_payment.Transaction_rate),
                C_Reconcile = false,
                C_Balance =-payable_payment.Orginal_amount
            };

            if (payable_payment.Cash_type == Cash_type.Cash)
            {
                CheckBook.C_CheckNumber = null;

                CheckBook.C_PostingKey = FBus.CheckBookKey.TCCW.ToString();
                CheckBook.C_DueDate = payable_payment.Posting_date.ToShortDateString();
                //CheckBook.C_CBSID = 1;
                int CashMax = 1;
                try
                {
                    CheckBook.C_DocumentNumber = FBus.GetCheckBookNumber(FBus.CheckBookKey.TCCW, companyID, payable_payment.Check_book_id);
                }
                catch
                {
                    CheckBook.C_DocumentNumber = 1;
                }
            }
            else
            {
                CheckBook.C_CheckNumber = CheckNumber;

                CheckBook.C_PostingKey = FBus.CheckBookKey.TCBC.ToString();
                if (payable_payment.Due_date.HasValue)
                {
                    CheckBook.C_DueDate = payable_payment.Due_date.Value.ToShortDateString();
                }


                //CheckBook.C_CBSID = 2;
                int CashMax = 1;
                try
                {
                    CheckBook.C_DocumentNumber = FBus.GetCheckBookNumber(FBus.CheckBookKey.TCBC, companyID, payable_payment.Check_book_id);
                }
                catch
                {
                    CheckBook.C_DocumentNumber = 1;
                }
            }
            dbM.C_CheckbookTransactions_Tables.Add(CheckBook);
            dbM.SaveChanges();
            return CheckBook;
        }
        // GET: Payable/Payable_payment/Edit/5
        public ActionResult Edit(int? id, bool Partial = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payable_payment payable_payment = db.Payable_payments.Find(id);
            if (payable_payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Partial = Partial;

            ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", payable_payment.Check_book_id);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Transaction_id = new SelectList(db.Payable_transactions_types.Where(x => x.Origin == TrxPay.Trx), "Trx_num", "Id", payable_payment.Transaction_id);
            ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id", payable_payment.Vendor_id);
            return View(payable_payment);
        }

        // POST: Payable/Payable_payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Payable_payment payable_payment)
        {
            //ModelState["Creation_date"].Errors.Clear();

            if (ModelState.IsValid)
            {
                db.Entry(payable_payment).State = EntityState.Modified;
                db.Entry(payable_payment).Property(x => x.Creation_date).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", payable_payment.Check_book_id);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Transaction_id = new SelectList(db.Payable_transactions_types.Where(x => x.Origin == TrxPay.Trx), "Trx_num", "Id", payable_payment.Transaction_id);
            ViewBag.Vendor_id = new SelectList(db.Payable_creditor_setting.Where(x => x.Inactive == false), "Id", "Vendor_id", payable_payment.Vendor_id);
            return View(payable_payment);
        }


        public JsonResult GetCheckBookCurrency(int Id)
        {
            try
            {
                return Json(dbM.C_CheckBookSetting_Tables.Find(Id).CurrencyID);
            }
            catch
            {
                return Json("");
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
