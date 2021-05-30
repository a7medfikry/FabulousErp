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
using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;
using FabulousModels.ViewModels;
using FBus = FabulousErp.Business;
using FabulousErp;

namespace Receivable.Controllers
{
    public class Receivable_paymentController : Controller
    {
        private DBContext db = new DBContext();
        private FabulousDB.DB_Context.DBContext dbM = new FabulousDB.DB_Context.DBContext();

        // GET: Receivable/Receivable_payment
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IndexPartial(string section, int SortBy, DateTime? From = null, DateTime? To = null)
        {
            List<Receivable_payment> Res = Enumerable.Empty<Receivable_payment>().ToList();

            if (section == "void")
            {
                Res = db.Receivable_payments
             .Include(a => a.Trans_doc_type).Include(x => x.Vendor).Include(x => x.CheckBook_setting).Include(x => x.CheckBook_transaction).Where(x => x.Is_void == false).ToList();
            }
            else
            {
                Res = db.Receivable_payments
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

        // GET: Receivable/Receivable_payment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_payment Receivable_payment = db.Receivable_payments.Find(id);

            ViewBag.id = Receivable_payment.Transaction_id;

            if (Receivable_payment == null)
            {
                return HttpNotFound();
            }

            return View(Receivable_payment);
        }

        // GET: Receivable/Receivable_payment/Create
        public ActionResult Create(int? id = null)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.D = Request["D"];
            if (ViewBag.D != null)
            {
                if (id.HasValue)
                {
                    if (db.Receivable_payments.Find(id).Is_void)
                    {
                        try
                        {
                            int JN = db.Receivable_payments.Find(id).Journal_number;
                            C_GeneralJournalEntry_Table GJ = dbM.C_GeneralJournalEntry_Tables.Where(z => z.C_PostingNumber == dbM.C_GeneralJournalEntry_Tables.Where(x => x.C_JournalEntryNumber == JN).
                                      FirstOrDefault().VoidPostingNum).FirstOrDefault();

                            ViewBag.Void = $"<div class='clearfix text-danger' style='display:block;font-weight:bold;'>This Transaction Has Been Voided In Date :<span style='color:#000;'> {GJ.C_PostingDate}</span> And Journal Entry Number (Voided) Is : <span style='color:#000;'>{GJ.C_JournalEntryNumber} ( {GJ.C_PostingKey} )</span></div>";
                        }
                        catch (Exception ex)
                        {
                            ViewBag.Void = null;
                        }

                    }
                }

            }
            try
            {
                ViewBag.JE = db.Receivable_payments.Find(id).Journal_number;
            }
            catch
            { }
            
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
                   Type= x.C_CheckbookType
               }).ToList());
            }
            return Json(null);
        }
        public ActionResult PartialCreate(int? id = null, bool Partial = false
            , bool IsTransaction = false,bool IsInstallment=false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Partial = Partial;

            ViewBag.companyID = companyID;
            Receivable_payment Receivable_payment = new Receivable_payment();
            Receivable_payment.System_rate = 1;
            Receivable_payment.Transaction_rate = 1;
            Receivable_payment.IsInstallment = IsInstallment;
            ViewBag.IsTransaction = IsTransaction;
            if (IsTransaction)
            {
                ViewBag.InstallmentSelect = new SelectList(dbM.Installment_settings.ToList(), "Id", "Plan_id");
            }
            if (id != null)
            {
                if (IsTransaction)
                {
                    Receivable_payment = db.Receivable_payments.Include(x => x.Vendor).FirstOrDefault(x => x.Id == id);
                }
                else
                {
                    Receivable_payment = db.Receivable_payments.Include(x => x.Vendor).FirstOrDefault(x => x.Transaction_id == id);
                }
            }
            else
            {
                Receivable_payment = null;
            }

        
            if (Receivable_payment == null)
            {
                ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName");
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
                ViewBag.Transaction_id = new SelectList(Enumerable.Empty<Receivable_transactions_types>().ToList(), "Id", "Trx_num");//db.Receivable_transactions_types.Where(x=>x.Origin==TrxPay.Trx)
                ViewBag.Vendor_id = Business.GetCustomerReceivableSelect();
                return View(new Receivable_payment { Transaction_date = DateTime.Now, Posting_date = DateTime.Now , System_rate =1,Transaction_rate=1});
            }
            else
            {
                ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", Receivable_payment.Check_book_id);
                ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode", Receivable_payment.Currency_id);
                ViewBag.Transaction_id = new SelectList(Enumerable.Empty<Receivable_transactions_types>().ToList(), "Id", "Id", Receivable_payment.Transaction_id);//db.Receivable_transactions_types.Where(x => x.Origin == TrxPay.Trx)
                ViewBag.Vendor_id = Business.GetCustomerReceivableSelect(Receivable_payment.Vendor_id);
                return View(Receivable_payment);
            }
        }
        public JsonResult GetTranstion(Doc_type DT, int VendoreId)
        {
            //var Res = db.Receivable_transactions.Where(x => x.Doc_type == DT&&x.Vendor_id== VendoreId).Include(x => x.Trans_doc_type)
            //    .ToList().Where(x =>
            //   (x.Purchase - x.Taken_discount + x.Tax -
            //     db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Taken_discount = 0 })
            //   .Sum(y => y.Taken_discount)) >
            //   db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id_to == x.Trans_doc_type_id)
            //   .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 })
            //   .Sum(y => y.Applay_assign)).Select(x => new { x.Trans_doc_type.Id, x.Trans_doc_type.Counter }).ToList();

            List<Receivable_transaction> UnPaidTrnx = Receivable.Controllers.Business.GetUnpaidTransaction(VendoreId).Where(x => x.Doc_type == DT).ToList();


            UnPaidTrnx.RemoveAll(x => x.Purchase - x.Taken_discount + x.Tax == 0);

            var Res = UnPaidTrnx
               .Select(x => new { x.Trans_doc_type.Id, x.Trans_doc_type.Counter }).ToList();

            return Json(Res);
        }
        public JsonResult GetInstContract(int VendoreId)
        {
            List<Installment_contract> Contract = dbM.Installment_contracts.Where(x => x.Customer_id
            == VendoreId).ToList();

            var Res = Contract
               .Select(x => new { Id = x.Id, Name = x.Desc }).ToList();

            return Json(Res);
        }
        public JsonResult GetInstallment(int ContractId)
        {
            List<Installments> UnPaidInst = dbM.Installments.Where(x => x.Contract_id == ContractId
            && x.Contract.IsPay == false && x.Paid == false&&x.Historical==false).ToList();

            var Res = UnPaidInst
               .Select(x => new { x.Id, Name = x.Refrence }).ToList();

            return Json(Res);
            return Json(null);
        }
        public JsonResult GetInstallmentAmount(int Id)
        {
            Installments Inst = dbM.Installments.Find(Id);
            return Json(new { amount = Inst.Amount, duedate = (Inst.Cheque_Date.HasValue)? Inst.Cheque_Date.Value.ToString("yyyy-MM-dd") :null, chequenumber = Inst.Cheque_number });
        }
        // POST: Receivable/Receivable_payment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Receivable_payment Receivable_payment, string InstallmentStr = null)
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
            //ModelState["Creation_date"].Errors.Clear();
            ////List<Installment_view> Installment = null;
            ////if (InstallmentStr != null)
            ////{
            ////    Installment = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Installment_view>>(InstallmentStr);
            ////    Installment.ForEach(x => x.Date = DateTime.Now);
            ////}
            string companyID = FabulousErp.Business.GetCompanyId();
            Receivable_payment.Creation_date = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (Receivable_payment.Id == 0)
                {
                    int Max = 1;
                    if (db.Receivable_payments.Any())
                    {
                        Max = (int)db.Receivable_payments.Max(x => x.Payment_no) + 1;
                    }
                    Receivable_transactions_types PTY = new Receivable_transactions_types
                    {
                        Counter = Business.GetNextDocNumber(Doc_type.Payment),
                        Doc_type = Doc_type.Payment,
                        Trx_num = Business.TrxNum(),
                        Origin = TrxPay.Pay
                    };
                    db.Receivable_transactions_types.Add(PTY);
                    db.SaveChanges();
                    Receivable_payment.Trans_doc_type_id = PTY.Id;


                    Receivable_payment.Payment_no = Max;
                    Receivable_transaction PT = Enumerable.Empty<Receivable_transaction>().FirstOrDefault();
                    int? Transaction_id = null;
                    try
                    {
                        PT = db.Receivable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == Receivable_payment.Trx_trans_doc_type_id);

                        //if (!Receivable_payment.Transaction_id.HasValue)
                        //{
                        //    PT = db.Receivable_transactions.FirstOrDefault(x => x.Trans_doc_type_id == Receivable_payment.Transaction_id);

                        //    Transaction_id = PT.Trans_doc_type_id;
                        //}
                        //else
                        //{
                        //    Transaction_id = Receivable_payment.Transaction_id;
                        //    // PT = db.Receivable_transactions.Find(Receivable_payment.Transaction_id);
                        //    if (PT == null)
                        //    {
                        //    }
                        //}
                    }
                    catch
                    {

                    }
                    Receivable_payment.Transaction_p_id = null;
                    Receivable_payment.Transaction_id = null;

                    if (Receivable_payment.Currency_id == null)
                    {
                        Receivable_payment.Currency_id = FabulousErp.Business.GetCompanyId();
                    }

                    db.Receivable_payments.Add(Receivable_payment);

                    db.SaveChanges();
                    if (PT != null)
                    {
                        try
                        {
                            db.Related_rec_trans.Add(new Related_rec_trans
                            {
                                Payment_id = Receivable_payment.Trans_doc_type_id,
                                Transaction_id = PT.Trans_doc_type_id
                            });
                            db.SaveChanges();
                        }
                        catch
                        {

                        }
                    }


                    if (Receivable_payment.Currency_id == companyID
                        && PT != null)
                    {
                        decimal UnAssign = 0;
                        decimal Assign = 0;
                        if (db.Assign_Receivable_docs.Any(x => x.Trans_doc_type_id_to == PT.Trans_doc_type_id))
                        {
                            try
                            {
                                PT.Purchase = PT.Purchase - db.Assign_Receivable_docs.Where(x => x.Trans_doc_type_id_to == PT.Trans_doc_type_id)
                                                    .Sum(x => x.Taken_discount)
                                                     - db.Assign_Receivable_docs.Where(x => x.Trans_doc_type_id_to == PT.Trans_doc_type_id)
                                                     .DefaultIfEmpty(new Assign_Receivable_doc { Trans_doc_type = new Receivable_transactions_types { Receivable_payment = new List<Receivable_payment> { new Receivable_payment { Taken_discount = 0 } } } })
                                                     .SelectMany(x => x.Trans_doc_type.Receivable_payment).Sum(x => x.Taken_discount);
                            }
                            catch
                            {

                            }

                        }
                        if (Receivable_payment.Orginal_amount >= (PT.Purchase - PT.Taken_discount + PT.Tax))
                        {
                            Assign = (PT.Purchase - PT.Taken_discount + PT.Tax);
                            UnAssign = Receivable_payment.Orginal_amount - (PT.Purchase - PT.Taken_discount + PT.Tax);
                        }
                        else
                        {
                            Assign = Receivable_payment.Orginal_amount;
                            UnAssign = 0;
                        }
                        db.Assign_Receivable_docs.Add(new Assign_Receivable_doc
                        {
                            Applay_assign = Assign,
                            Applay_date = Receivable_payment.Posting_date,
                            Creation_date = DateTime.Now,
                            Is_void = false,
                            Assign_no = db.Assign_Receivable_docs.ToList().DefaultIfEmpty(new Assign_Receivable_doc { Assign_no = 0 }).Max(x => x.Assign_no) + 1,
                            Currency_id = Receivable_payment.Currency_id,
                            Doc_Num = "",
                            JournalEntry = 0,
                            Trans_doc_type_id = Receivable_payment.Trans_doc_type_id,
                            Trans_doc_type_id_to = PT.Trans_doc_type_id,
                            Vendor_id = PT.Vendor_id.Value,
                            Taken_discount = Receivable_payment.Taken_discount,
                            Unassign_amount = (UnAssign < 0) ? 0 : UnAssign,
                            Orginal_amount = Receivable_payment.Orginal_amount,
                            Doc_type = Doc_type.Payment,
                            Earn_or_lose = 0,
                            Transaction_rate = PT.Transaction_rate
                        });
                        db.SaveChanges();
                    }


                    decimal ReciptAmount = 0;
                    //if (PT != null)
                    //{
                    //    ReciptAmount = PT.Purchase;
                    //}
                    //else
                    //{
                    //}
                    try
                    {
                        ReciptAmount = Receivable_payment.Orginal_amount;
                    }
                    catch
                    {

                    }
                    string UserID = "";
                    try
                    {
                        UserID = FabulousErp.Business.GetUserId();
                    }
                    catch
                    {
                        UserID = Receivable_payment.Profitable_user;
                    }
                    int PostingNumber = FabulousErp.Business.GetPotingNumber(Receivable_payment.Journal_number);
                    string DueDate = null;
                    if (Receivable_payment.Due_date.HasValue)
                    {
                        DueDate = Receivable_payment.Due_date.Value.ToShortDateString();
                    }
                    int C_CBSID = dbM.C_CheckBookSetting_Tables.Find(Receivable_payment.Check_book_id).C_CBSID;
                    string CheckNumber = "";
                    try
                    {
                        CheckNumber = Receivable_payment.Cheque_number;
                    }
                    catch
                    {

                    }

                    //Add To Check Book
                    Receivable_payment.UserId = UserID;
                    Receivable_payment.PostingNumber = PostingNumber;
                    Receivable_payment.Check_book_id = C_CBSID;
                    Receivable_payment.Orginal_amount = ReciptAmount;
                    C_CheckbookTransactions_table CheckBook =
                        AddToCheckBook(Receivable_payment , CheckNumber);

                    List<C_CheckbookTransactions_table> CBT = new List<C_CheckbookTransactions_table>();

                    //Add To Check Book

                    if (Receivable_payment.IsInstallment)
                    {
                        try
                        {
                            dbM.Installments.Find(Receivable_payment.Installment_id).Paid = true;
                            dbM.Installments.Find(Receivable_payment.Installment_id).Check_book_trx_id = CheckBook.C_CBT;
                            dbM.SaveChanges();
                        }
                        catch
                        {}
                    }
                    string BookName = "";
                    try
                    {
                        BookName= db.C_CheckBookSetting_Tables.Find(C_CBSID).C_CheckbookName;
                    }
                    catch
                    {

                    }
                    Receivable_payment.Check_book_transaction_id = CheckBook.C_CBT;
                    db.SaveChanges();
                    return Json(new { PaymentId = Receivable_payment.Id,
                        BookId = CheckBook.C_DocumentNumber,
                        BookName = BookName,
                        PostingNumber = PostingNumber, 
                        InstallmentList = CBT.Select(x => x.C_DocumentNumber) });
                }
                else
                {
                    //db.Entry(Receivable_payment).State = EntityState.Modified;
                    //db.Entry(Receivable_payment).Property(x => x.Payment_no).IsModified = false;
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                }

            }

            ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", Receivable_payment.Check_book_id);
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Transaction_id = new SelectList(db.Receivable_transactions_types.Where(x => x.Origin == TrxPay.Trx), "Trx_num", "Id", Receivable_payment.Transaction_id);
            ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings.Where(x => x.Inactive == false), "Id", "Vendor_id", Receivable_payment.Vendor_id);
            return View(Receivable_payment);
        }
        public C_CheckbookTransactions_table AddToCheckBook(Receivable_payment Receivable_payment,string CheckNumber)
        {
            C_CheckbookTransactions_table CheckBook = new C_CheckbookTransactions_table();

            string companyID = FabulousErp.Business.GetCompanyId();

            CheckBook = new C_CheckbookTransactions_table
            {
                CompanyID = companyID,
                C_PostingNumber = Receivable_payment.PostingNumber,
                C_TransactionDate = Receivable_payment.Transaction_date.ToShortDateString(),
                C_PostingDate = Receivable_payment.Posting_date.ToShortDateString(),
                C_SystemRate = (double?)Receivable_payment.System_rate,
                C_TransactionRate = (double?)Receivable_payment.Transaction_rate,
                C_Reference = Receivable_payment.Reference,
                C_DocumentType = "SID",
                C_Payment_To_Recieved_From = (Receivable_payment.Vendor_id==0)?Receivable_payment.Payment_To_Recieved_From: db.Receivable_vendore_settings.Find(Receivable_payment.Vendor_id).Vendor_name,
                C_Reciept = Receivable_payment.Orginal_amount,
                C_Payment = 0,
                UserID =string.IsNullOrEmpty(Receivable_payment.UserId)?FabulousErp.Business.GetUserId(): Receivable_payment.UserId,
                CurrencyID = Receivable_payment.Currency_id,
                C_DateTime = DateTime.Now,
                C_CBSID = Receivable_payment.Check_book_id,
                C_Difference = (double)(Receivable_payment.System_rate - Receivable_payment.Transaction_rate),
                C_Reconcile = false,
                C_Balance = Receivable_payment.Orginal_amount,
            };

            if (Receivable_payment.Cash_type == Cash_type.Cash)
            {
                CheckBook.C_CheckNumber = null;

                CheckBook.C_PostingKey = FBus.CheckBookKey.TCCR.ToString();
                CheckBook.C_DueDate = Receivable_payment.Posting_date.ToShortDateString();
                //CheckBook.C_CBSID = 1;
                int CashMax = 1;
                try
                {
                    CheckBook.C_DocumentNumber = FBus.GetCheckBookNumber(FBus.CheckBookKey.TCCR, companyID, Receivable_payment.Check_book_id);
                }
                catch
                {
                    CheckBook.C_DocumentNumber = 1;
                }
            }
            else
            {
                CheckBook.C_CheckNumber = CheckNumber;

                CheckBook.C_PostingKey = FBus.CheckBookKey.TCBR.ToString();
                if (Receivable_payment.Due_date.HasValue)
                {
                    CheckBook.C_DueDate = Receivable_payment.Due_date.Value.ToShortDateString();
                }


                //CheckBook.C_CBSID = 2;
                int CashMax = 1;
                try
                {
                    CheckBook.C_DocumentNumber = FBus.GetCheckBookNumber(FBus.CheckBookKey.TCBR, companyID, Receivable_payment.Check_book_id);
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
        private void InsertInstallment(Receivable_payment Receivable_payment, List<Installment_view> Installment, string companyID, decimal ReciptAmount, string UserID, int PostingNumber, int C_CBSID, ref List<C_CheckbookTransactions_table> CBT)
        {

            foreach (Installment_view i in Installment)
            {
                int InstMax = 1;
                try
                {
                    InstMax = FBus.GetCheckBookNumber(FBus.CheckBookKey.TCBR, companyID, Receivable_payment.Check_book_id);
                }
                catch
                {
                    InstMax = 1;
                }
                if (Receivable_payment.Currency_id == null)
                {
                    Receivable_payment.Currency_id = FabulousErp.Business.GetCompanyId();
                }
                CBT.Add(new C_CheckbookTransactions_table
                {
                    CompanyID = companyID,
                    C_PostingNumber = PostingNumber,
                    C_TransactionDate = Receivable_payment.Transaction_date.ToShortDateString(),
                    C_PostingDate = Receivable_payment.Posting_date.ToShortDateString(),
                    C_SystemRate = (double?)Receivable_payment.System_rate,
                    C_TransactionRate = (double?)Receivable_payment.Transaction_rate,
                    C_Reference = (string.IsNullOrEmpty(i.Refrence)) ? "--" : i.Refrence,
                    C_DocumentType = "SID",
                    C_Payment_To_Recieved_From = db.Receivable_vendore_settings.Find(Receivable_payment.Vendor_id).Vendor_name,
                    C_Reciept = i.Amount,
                    C_Payment = 0,
                    UserID = UserID,
                    CurrencyID = Receivable_payment.Currency_id,
                    C_DateTime = DateTime.Now,
                    C_CBSID = C_CBSID,
                    C_Difference = (double)(Receivable_payment.System_rate - Receivable_payment.Transaction_rate),
                    C_Reconcile = false,
                    C_Balance = ReciptAmount - i.Amount,
                    C_CheckNumber = i.Cheque_number,
                    C_DueDate = (i.Date.HasValue)? i.Date.Value.ToShortDateString() :null,
                    C_DocumentNumber = InstMax,
                    C_PostingKey = FBus.CheckBookKey.TCBR.ToString()
                });
            }
            dbM.C_CheckbookTransactions_Tables.AddRange(CBT);
            dbM.SaveChanges();
        }

        // GET: Receivable/Receivable_payment/Edit/5
        public ActionResult Edit(int? id, bool Partial = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receivable_payment Receivable_payment = db.Receivable_payments.Find(id);
            if (Receivable_payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Partial = Partial;

            ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", Receivable_payment.Check_book_id);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Transaction_id = new SelectList(db.Receivable_transactions_types.Where(x => x.Origin == TrxPay.Trx), "Trx_num", "Id", Receivable_payment.Transaction_id);
            ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", Receivable_payment.Vendor_id);
            return View(Receivable_payment);
        }

        // POST: Receivable/Receivable_payment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Receivable_payment Receivable_payment)
        {
            //ModelState["Creation_date"].Errors.Clear();

            if (ModelState.IsValid)
            {
                db.Entry(Receivable_payment).State = EntityState.Modified;
                db.Entry(Receivable_payment).Property(x => x.Creation_date).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Check_book_id = new SelectList(dbM.C_CheckBookSetting_Tables, "C_CBSID", "C_CheckbookName", Receivable_payment.Check_book_id);
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.Currency_id = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");
            ViewBag.Transaction_id = new SelectList(db.Receivable_transactions_types.Where(x => x.Origin == TrxPay.Trx), "Trx_num", "Id", Receivable_payment.Transaction_id);
            ViewBag.Vendor_id = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id", Receivable_payment.Vendor_id);
            return View(Receivable_payment);
        }

        // GET: Receivable/Receivable_payment/Delete/5


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
