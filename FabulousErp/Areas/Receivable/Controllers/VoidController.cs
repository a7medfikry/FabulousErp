using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using MBus = FabulousErp.Business;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.DB_Context;
using FabulousDB.Models;

namespace Receivable.Controllers
{
    public class VoidController : Controller
    {
        DBContext db = new DBContext();
        DBContext dbM = new DBContext();
        // GET: Receivable/Void
        public ActionResult Index(string Type,int Id)
        {
            ViewBag.Type = Type;
            ViewBag.PostingToOrThrow = MBus.PostingToOrThrow();
            ViewBag.Error = "";
            if (Type == TrxPay.Trx.ToString())
            {
                Receivable_transaction PT= db.Receivable_transactions.FirstOrDefault(x => x.Id == Id);
                ViewBag.PostingNum = MBus.GetPotingNumber(PT.Journal_number);
                ViewBag.Currency = PT.Currency_id;
                ViewBag.TransactionDate = PT.Transaction_date;
                ViewBag.RedirectLink = "/Receivable/Receivable_transaction?section=void";
            }
            else if (Type == TrxPay.Pay.ToString())
            {
                Receivable_payment PP = db.Receivable_payments.FirstOrDefault(x => x.Id == Id);
                int PPPO = FabulousErp.Business.GetPotingNumber(PP.Journal_number);
                C_CheckbookTransactions_table CBT = dbM.C_CheckbookTransactions_Tables
                  .FirstOrDefault(x => x.C_PostingNumber == PPPO);
                if (CBT != null && CBT.C_Reconcile.HasValue && CBT.C_Reconcile.Value)
                {
                    ViewBag.Error = "This Payment Is Reconcile In Checkbook You Can't Void It";
                }
                ViewBag.PostingNum = MBus.GetPotingNumber(PP.Journal_number);
                ViewBag.Currency = PP.Currency_id;
                ViewBag.TransactionDate = PP.Transaction_date;
                ViewBag.RedirectLink = "/Receivable/Receivable_payment?section=void";

            }
            else if (Type == "Assign")
            {
                Assign_Receivable_doc AP = db.Assign_Receivable_docs.FirstOrDefault(x => x.Id == Id);
                ViewBag.PostingNum = MBus.GetPotingNumber(AP.JournalEntry);
                ViewBag.Currency = AP.Currency_id;
                ViewBag.TransactionDate = AP.Applay_date;
                ViewBag.RedirectLink = "/Receivable/Assign_Receivable_doc?section=void";

            }

            return View();
        }
        public JsonResult CreateVoid(string Type, int Id, DateTime Transaction_date, DateTime Posting_date, string Reference)
        {
            int PN = 0;

            if (Type == TrxPay.Trx.ToString())
            {
                Receivable_transaction PTX = db.Receivable_transactions.Find(Id);
                PTX.Is_void = true;
                List<int> OtherPayment = db.Related_pay_trans.Where(x => x.Transaction_id == PTX.Trans_doc_type_id).Select(x => x.Payment_id).ToList();

                if (db.Assign_Receivable_docs.Any(x => x.Trans_doc_type_id_to == PTX.Trans_doc_type_id)
                    || db.Assign_Receivable_docs.Any(x => OtherPayment.Any(z => z == x.Trans_doc_type_id)))
                {
                    return Json("This Transaction Has Assign You Can't Void It");
                }

                db.Receivable_void.Add(new Receivable_void
                {
                    Transaction_date = Transaction_date,
                    Posting_date = Posting_date,
                    Trx_id = PTX.Trans_doc_type_id
                });
                db.SaveChanges();

                PN = MBus.GetPotingNumber(PTX.Journal_number);
                if (db.Receivable_payments.Any(x => x.Journal_number == PTX.Journal_number))
                {
                    int PId = db.Receivable_payments.FirstOrDefault(x => x.Journal_number == PTX.Journal_number).Id;
                    VoidPayment(PId, Transaction_date, Posting_date, Reference);
                }
                db.SaveChanges();
                try
                {
                    db.Related_pay_trans.RemoveRange(db.Related_pay_trans.Where(x => x.Transaction_id == PTX.Trans_doc_type_id));
                    db.SaveChanges();
                }
                catch
                {

                }
            }
            else if (Type == TrxPay.Pay.ToString())
            {

                PN = VoidPayment(Id, Transaction_date, Posting_date, Reference);
                if (PN == -1)
                {
                    return Json("This Payament Has Assign You Can't Void It");
                }

            }
            else if (Type == TrxPay.Assign.ToString())
            {
                Assign_Receivable_doc A = db.Assign_Receivable_docs.Find(Id);

                db.Receivable_assign_void.Add(new Receivable_Assign_void
                {
                    Applay_assign = A.Applay_assign,
                    Applay_date = A.Applay_date,
                    Assign_no = A.Assign_no,
                    Creation_date = A.Creation_date,
                    Currency = A.Currency,
                    Currency_id = A.Currency_id,
                    Doc_Num = A.Doc_Num,
                    Doc_type = A.Doc_type,
                    Earn_or_lose = A.Earn_or_lose,
                    JournalEntry = A.JournalEntry,
                    Orginal_amount = A.Orginal_amount,
                    Taken_discount = A.Taken_discount,
                    Trans_doc_type = A.Trans_doc_type,
                    Unassign_amount = A.Unassign_amount,
                    Trans_doc_type_id = A.Trans_doc_type_id,
                    Trans_doc_type_id_to = A.Trans_doc_type_id_to,
                    Trans_doc_type_to = A.Trans_doc_type_to,
                    Vendor = A.Vendor,
                    Vendor_id = A.Vendor_id
                });
                db.Assign_Receivable_docs.Remove(A);
                PN = MBus.GetPotingNumber(A.JournalEntry);

            }
            db.SaveChanges();
            return Json(PN);
        }

        private int VoidPayment(int Id, DateTime Transaction_date, DateTime Posting_date, string Reference)
        {
            Receivable_payment PP = db.Receivable_payments.Include(x => x.CheckBook_transaction).Include(x => x.Trans_doc_type).FirstOrDefault(x => x.Id == Id);


            if (db.Assign_Receivable_docs.Any(x => x.Trans_doc_type_id == PP.Trans_doc_type_id))
            {
                return -1;
            }

            PP.Is_void = true;

            db.Receivable_void.Add(new Receivable_void
            {
                Transaction_date = Transaction_date,
                Posting_date = Posting_date,
                Trx_id = PP.Trans_doc_type_id
            });


            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string UserID = FabulousErp.Business.GetUserId();
            int? Trx = PP.Trans_doc_type.Trx_num;
            C_CheckbookTransactions_table c_CheckbookTransactions_Table = new C_CheckbookTransactions_table()
            {
                CompanyID = companyID,
                C_PostingNumber = MBus.GetPotingNumber(PP.Journal_number),
                C_CBSID = PP.Check_book_id,
                CurrencyID = PP.Currency_id,
                C_TransactionDate = Transaction_date.ToShortDateString(),
                C_PostingDate = Posting_date.ToShortDateString(),
                C_SystemRate = (double)PP.System_rate,
                C_TransactionRate = (double)PP.Transaction_rate,
                C_Difference = (double)PP.System_rate - (double)PP.Transaction_rate,
                C_Reference = $"Void Transaction {Trx}",
                C_DocumentType = "SID",
                C_Payment_To_Recieved_From = PP.CheckBook_transaction.C_Payment_To_Recieved_From,
                C_Reciept = PP.Orginal_amount - PP.Taken_discount,
                C_Payment = 0,
                C_Balance = PP.Orginal_amount - PP.Taken_discount,
                C_PostingKey = PP.CheckBook_transaction.C_PostingKey,
                C_DocumentNumber = PP.CheckBook_transaction.C_DocumentNumber,
                C_CheckNumber = PP.CheckBook_transaction.C_CheckNumber,
                C_DueDate = PP.CheckBook_transaction.C_DueDate,
                C_Reconcile = false,
                C_CBTVoid = PP.CheckBook_transaction.C_CBT,
                UserID = UserID,
                C_DateTime = DateTime.Now,

            };
            dbM.C_CheckbookTransactions_Tables.Add(c_CheckbookTransactions_Table);

            var data = dbM.C_CheckbookTransactions_Tables.Where(x => x.C_CBT == PP.CheckBook_transaction.C_CBT).FirstOrDefault();
            data.C_CBTVoid = c_CheckbookTransactions_Table.C_CBT;

            db.SaveChanges();
            dbM.SaveChanges();
            return MBus.GetPotingNumber(PP.Journal_number);
        }
    }
}