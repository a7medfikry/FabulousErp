using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FabulousDB.Models;

namespace Installment.Controllers
{
    public class RptController : Controller
    {
        // GET: Installment/Rpt
        public ActionResult InstallmentRpt()
        {
            ViewBag.Sec = Request["Sec"];
            if (ViewBag.Sec == "Rec")
            {
                using (DBContext db = new DBContext())
                {
                    ViewBag.Vendores = new SelectList(db.Receivable_vendore_settings.ToList(), "Id", "Vendor_id");
                    ViewBag.DocType = new SelectList(Enum.GetValues(typeof(Doc_type))
                        .Cast<Doc_type>().Where(x => x == Doc_type.Invoice
                        || x == Doc_type.Debit_Memo)
                        .Select(x => new { Id = (int)x, Name = x.ToString() })
                                            , "Id", "Name");
                    using (DBContext dbM = new DBContext())
                    {

                        string CompId = FabulousErp.Business.GetCompanyId();
                        ViewBag.CheckBook = new SelectList(dbM.C_CheckBookSetting_Tables.Where(x => x.CompanyID == CompId).ToList(), "C_CBSID", "C_CheckbookID");
                    }
                    return View();

                }
            }
            else
            {
                using (DBContext db = new DBContext())
                {
                    ViewBag.Vendores = new SelectList(db.Payable_creditor_setting.ToList(), "Id", "Vendor_id");
                    ViewBag.DocType = new SelectList(Enum.GetValues(typeof(Doc_type))
                        .Cast<Doc_type>().Where(x => x == Doc_type.Invoice
                        || x == Doc_type.Debit_Memo)
                        .Select(x => new { Id = (int)x, Name = x.ToString() })
                                            , "Id", "Name");
                    using (DBContext dbM = new DBContext())
                    {

                        string CompId = FabulousErp.Business.GetCompanyId();
                        ViewBag.CheckBook = new SelectList(dbM.C_CheckBookSetting_Tables.Where(x => x.CompanyID == CompId).ToList(), "C_CBSID", "C_CheckbookID");
                    }
                    return View();

                }
            }
        }
        public JsonResult GetContract(int VendoreId, string Sec, int CheckBookId = 0)
        {
            if (Sec == "Rec")
            {
                using (DBContext db = new DBContext())
                {
                    using (DBContext dbM = new DBContext())
                    {
                        List<Installment_contract> Contracts =
                        dbM.Installment_contracts.Where(x => x.Customer_id == VendoreId).ToList();

                        return Json(Contracts.Select(x => new { Id = x.Id, Ref = x.Desc }).ToList());//, "C_CBT", "C_Reference");
                    }
                }
            }
            else
            {
                using (DBContext db = new DBContext())
                {
                    using (DBContext dbM = new DBContext())
                    {
                        List<Installment_contract> Contracts =
                                             dbM.Installment_contracts.Where(x => x.Vendore_id == VendoreId).ToList();

                        return Json(Contracts.Select(x => new { Id = x.Id, Ref = x.Desc }).ToList());//, "C_CBT", "C_Reference");
                    }
                }
            }

        }
        public PartialViewResult GetInstallments(int Contract, string Sec)
        {
            using (DBContext db = new DBContext())
            {
                List<Installments> Contracts =
                db.Installments.Include(x => x.Contract).Include(x => x.Contract.Currency)
                .Include(x => x.Check_book_trx).Where(x => x.Contract_id == Contract)
               .ToList();
                ViewBag.OrginalAmount = db.Installment_contracts.Find(Contract).Amount;
                ViewBag.InstallmentPlan = db.Installment_contracts.Include(x => x.Installment_plan).FirstOrDefault(x => x.Id == Contract).Installment_plan.Plan_id;
                List<Installment_view> ContView = new List<Installment_view>();
                if (Sec == "Rec")
                {
                    ContView.AddRange(Contracts.Select(x => new FabulousModels.ViewModels.Installment_view
                    {
                        Amount = (decimal)x.Amount,
                        Cheque_number = x.Cheque_number,
                        Date = x.Cheque_Date,
                        Refrence = x.Refrence,
                        Currency = (x.Contract.Currency==null)? db.CurrenciesDefinition_Tables.FirstOrDefault().ISOCode : x.Contract.Currency.ISOCode,
                        State =(x.Paid)? Installment_due_state.Achieved : (x.Check_book_trx == null) ?
                                                  (x.Cheque_Date > DateTime.Now) ? Installment_due_state.Not_due :
                                                  Installment_due_state.Due : (x.Check_book_trx.C_Reconcile == true) ? Installment_due_state.Achieved :
                                                  ((x.Check_book_trx.C_CheckBookSetting_Table.C_CheckbookType == "Bank"
                                                  || x.Check_book_trx.C_CheckBookSetting_Table.C_CheckbookType == "Check")
                                                  &&
                                                  x.Check_book_trx.C_Reconcile == false) ? Installment_due_state.Collecting :
                                                  (Convert.ToDateTime(x.Cheque_Date) > DateTime.Now.Date && x.Check_book_trx.C_Reconcile == false)
                                                  ? Installment_due_state.Not_due :
                                                  (Convert.ToDateTime(x.Cheque_Date) <= DateTime.Now.Date && x.Check_book_trx.C_Reconcile == false) ?
                                                  Installment_due_state.Due : Installment_due_state.Due,
                        Historical = x.Historical,
                        Transaction_date=(x.Check_book_trx!=null)?x.Check_book_trx.C_TransactionDate:""
                    }).ToList());
                    ContView.Where(x => x.Historical == true).ToList().ForEach(x => x.State = Installment_due_state.Achieved);

                }
                else
                {
                    ContView.AddRange(Contracts.Select(x => new FabulousModels.ViewModels.Installment_view
                    {
                        Amount = (decimal)x.Amount,
                        Cheque_number = x.Cheque_number,
                        Date = x.Cheque_Date,
                        Refrence = x.Refrence,
                        Currency = (x.Contract.Currency == null) ? db.CurrenciesDefinition_Tables.FirstOrDefault().ISOCode : x.Contract.Currency.ISOCode,
                        State = (x.Paid) ? Installment_due_state.Achieved : (x.Check_book_trx == null) ?
                              (x.Cheque_Date > DateTime.Now) ? Installment_due_state.Not_due :
                              Installment_due_state.Due : (x.Check_book_trx.C_Reconcile == true) ? Installment_due_state.Paid :
                              ((x.Check_book_trx.C_CheckBookSetting_Table.C_CheckbookType == "Bank"
                              || x.Check_book_trx.C_CheckBookSetting_Table.C_CheckbookType == "Check")
                              &&
                              x.Check_book_trx.C_Reconcile == false) ? Installment_due_state.Payment_in_progress :
                              (Convert.ToDateTime(x.Cheque_Date) > DateTime.Now.Date && x.Check_book_trx.C_Reconcile == false)
                              ? Installment_due_state.Not_due :
                              (Convert.ToDateTime(x.Cheque_Date) <= DateTime.Now.Date && x.Check_book_trx.C_Reconcile == false) ?
                              Installment_due_state.Due : Installment_due_state.Due,
                        Historical = x.Historical,
                        Transaction_date = (x.Check_book_trx != null) ? x.Check_book_trx.C_TransactionDate : ""

                    }).ToList());
                    ContView.Where(x => x.Historical == true).ToList().ForEach(x => x.State = Installment_due_state.Paid);

                }
                return PartialView(ContView);
            }
        }
        public ActionResult DelayRpt(string Sec="Rec")
        {
            ViewBag.IsPay = (Sec == "Rec") ? false : true;
            return View();
        }
        public ActionResult DelayRptRes(DateTime? From,DateTime? To,bool IsPay)
        {
            ViewBag.IsPay = IsPay;
            DBContext db = new DBContext();
            List<IDR> Res = new List<IDR>();
            List<Installments> Contracts =
                db.Installments
                .Include(x => x.Contract)
                .Include(x => x.Contract.Customer)
                .Include(x => x.Contract.Vendore)
                .Include(x => x.Contract.Currency)
                .Include(x => x.Check_book_trx)
                .Include(x => x.Contract.Customer.Receivable_address_info)
                .Include(x => x.Contract.Vendore.Payable_address_info)
                .Where(x => x.Contract.IsPay == IsPay)
               .ToList();

            int Collected, Late_installment, UnDue_installment;
            decimal Late_installment_amount, UnDue_installment_amount;

            if (From.HasValue && To.HasValue)
            {
             
                Contracts = Contracts.Where(x => x.Cheque_Date >= From && x.Cheque_Date <= To).ToList();
                Contracts = CalcInstallment(Contracts, out Collected, out Late_installment, out Late_installment_amount, out UnDue_installment, out UnDue_installment_amount);

                Res.AddRange(Contracts.Select(x => new FabulousModels.ViewModels.IDR
                {
                    Customer_name = (IsPay) ? x.Contract.Vendore.Vendor_name : x.Contract.Customer.Vendor_name,
                    Collected_installment_amount = Late_installment_amount,
                    Collected_installment = Collected,
                    Late_installment = Late_installment,
                    Late_installment_amount = Late_installment_amount,
                    UnDue_installment = UnDue_installment,
                    UnDue_installment_amount = UnDue_installment_amount,
                    Customer_phone= (IsPay) ? x.Contract.Vendore.Payable_address_info.FirstOrDefault()
                    .Phone_number: x.Contract.Customer.Receivable_address_info.FirstOrDefault()
                    .Phone_number
                }));
            }
            else
            {
                Contracts = CalcInstallment(Contracts, out Collected, out Late_installment, out Late_installment_amount, out UnDue_installment, out UnDue_installment_amount);
                string Name = "";
              
                Res.AddRange(Contracts.Select(x => new FabulousModels.ViewModels.IDR
                {
                    Customer_name =(IsPay)? x.Contract.Vendore.Vendor_name:x.Contract.Customer.Vendor_name,
                    Collected_installment_amount = Late_installment_amount,
                    Collected_installment = Collected,
                    Late_installment = Late_installment,
                    Late_installment_amount = Late_installment_amount,
                    UnDue_installment = UnDue_installment,
                    UnDue_installment_amount = UnDue_installment_amount,
                    Customer_phone = (IsPay) ? x.Contract.Vendore.Payable_address_info.FirstOrDefault()
                    .Phone_number : x.Contract.Customer.Receivable_address_info.FirstOrDefault()
                    .Phone_number
                }));
            }
           
            Res = Res.Distinct().ToList();
            return View(Res);
        }

        private static List<Installments> CalcInstallment(List<Installments> Contracts, out int Collected, out int Late_installment, out decimal Late_installment_amount, out int UnDue_installment, out decimal UnDue_installment_amount)
        {

            Collected = Contracts.Count(x => x.Paid);
            decimal CollectedAmount = Contracts.Where(x => x.Paid == true).Sum(x => x.Amount);
            Late_installment = Contracts.Where(x => x.Paid == false && x.Cheque_Date <= DateTime.Now).Count();
            Late_installment_amount = Contracts.Where(x => x.Paid == false && x.Cheque_Date <= DateTime.Now)
.Sum(x => x.Amount);
            UnDue_installment = Contracts.Where(x => x.Paid == false && x.Cheque_Date > DateTime.Now).Count();
            ;
            UnDue_installment_amount = Contracts.Where(x => x.Paid == false && x.Cheque_Date > DateTime.Now)
                .Sum(x => x.Amount);
            return Contracts;
        }

        public class PostingAndInv
        {
            public int Posting { get; set; }
            public int Inv { get; set; }
        }
        
    }
}