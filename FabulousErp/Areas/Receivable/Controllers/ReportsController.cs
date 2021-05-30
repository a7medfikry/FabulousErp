using FabulousDB.DB_Context;
using FabulousErp.Receivable.Models; using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MBus = FabulousErp.Business;
using FabulousDB.Models;
using FabulousErp.Bussiness;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Receivable.Controllers
{
    public class ReportsController : Controller
    {
        DBContext db = new DBContext();
        DBContext dbM = new DBContext();

        // GET: Receivable/Reports
        public ActionResult VendoreBalance()
        {
            ViewBag.VendoreId = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id");
            ViewBag.ClassId = new SelectList(db.Receivable_Group_settings, "Id", "Group_id");

            return View();
        }
        public ActionResult VendoreBalanceRes(int? VendoreId = null, int? GroupId = null)
        {
            List<Receivable_payment> PP = new List<Receivable_payment>();
            List<Receivable_transaction> PT = new List<Receivable_transaction>();
            List<Assign_Receivable_doc> APD = new List<Assign_Receivable_doc>();
            Date_option AgingOption = db.Receivable_aging_date_option.FirstOrDefault().Date_option;
            List<Receivable_aging_period> AP = db.Receivable_aging_periods.OrderBy(x => x.From).ToList();
            List<TrxPayNumber> TPA = new List<TrxPayNumber>();

            TPA.AddRange(db.Receivable_transactions.Where(x=>x.Vendor_id==VendoreId&&x.Is_void==false).Select(i => new TrxPayNumber
            {
                Amount_of_transaction = (i.Purchase - i.Taken_discount + i.Tax) * i.Transaction_rate,
                Vendor_id = i.Vendor_id,
                Vendore_name = i.Vendor.Vendor_name,
                IsTrx=true
            }));


            TPA.AddRange(db.Receivable_payments.Where(x => x.Vendor_id == VendoreId && x.Is_void == false).Select(i => new TrxPayNumber
            {
                Amount_of_payment = (i.Orginal_amount) * i.Transaction_rate,
                Number_of_payment = 1,
                Vendor_id = i.Vendor_id,
                Vendore_name = i.Vendor.Vendor_name,
                IsTrx = false
            }));

            if (VendoreId != null)
            {
                PP = db.Receivable_payments.Where(x => x.Vendor_id == VendoreId && x.Is_void == false && x.Transaction_id == null).Include(x => x.Vendor).ToList();
                PT = db.Receivable_transactions.Where(x => x.Vendor_id == VendoreId && x.Is_void == false).Include(x => x.Vendor).ToList().Where(x => !PP.Any(z => z.Transaction_id == x.Id)).ToList();
                APD = db.Assign_Receivable_docs.Where(x => x.Vendor_id == VendoreId).ToList();
            }
            else if (GroupId != null)
            {
                List<Receivable_Group_setting> GS = db.Receivable_Group_settings.Where(x => x.Id == GroupId).ToList();
                PP = db.Receivable_payments.Where(x => x.Transaction_id == null).Include(x => x.Vendor).ToList().Where(x => GS.Any(z => z.Id == x.Vendor.Group_setting_id) && x.Is_void == false).ToList();
                PT = db.Receivable_transactions.Include(z => z.Vendor).Where(x => x.Vendor_id != null && x.Is_void == false).ToList().Where(x => GS.Any(z => z.Id == x.Vendor.Group_setting_id)).ToList().Where(x => !PP.Any(z => z.Transaction_id == x.Id)).ToList();
                APD = db.Assign_Receivable_docs.Include(z => z.Vendor).Where(x => x.Vendor.Group_setting_id == GroupId).ToList();
            }
            else
            {
                return View(new VendoreBalance
                {
                    Aging = AP
                });

            }

            decimal ORate = 1;
            List<AgingRpt> Res0 = CalcAgingRpt("Details", null, null, null, null, null, null, null, "", "", ref ORate).ToList();
            if (VendoreId.HasValue)
            {
                Res0 = Res0.Where(x => x.VendoreId == VendoreId).ToList();
            }
            List<Receivable_aging_period> AgingRes = Res0.SelectMany(x => x.AgingAmount).ToList();
            List<Receivable_aging_period> AgingORes = db.Receivable_aging_periods.OrderBy(x => x.From).ToList();
            foreach (Receivable_aging_period i in AgingRes)
            {
                AgingORes.Where(x => x.Id == i.Id).FirstOrDefault().Amount += i.Amount;
            }
            VendoreBalance Res = new VendoreBalance
            {
                TrxPayNumber = TPA,
                Aging = AgingORes
            };

            return View(Res);
        }

        public ActionResult VendoreCurrentActivtiy(bool LastYear = false)
        {
            ViewBag.VendoreId = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_id");

            ViewBag.ClassId = new SelectList(db.Receivable_Group_settings, "Id", "Group_id");
            ViewBag.Aging = new SelectList(db.Receivable_aging_periods, "Id", "Name");
            ViewBag.GetLastYear = LastYear;
            if (LastYear)
            {
                ViewBag.Title = "Customer Last Activtiy";
            }
            else
            {
                ViewBag.Title = "Customer Current Activtiy";

            }
            return View();
        }
        public ActionResult VendoreCurrentActivtiyRes(int? VendoreId = null, int? GroupId = null
            , int? VendoreIdFrom = null, int? VendoreIdTo = null, int? ClassIdFrom = null, int? ClassIdTo = null,
            int? PeriodType = null, DateTime? PeriodFrom = null, DateTime? PeriodTo = null, bool GetlastYear = false)
        {
            List<VendoreCurrentActivtiy> FRes = CalcVendoreActive(VendoreId, GroupId, VendoreIdFrom, VendoreIdTo, ClassIdFrom, ClassIdTo, PeriodType, PeriodFrom, PeriodTo, DateTime.Now);

            if (GetlastYear)
            {
                List<VendoreCurrentActivtiy> LRes = CalcVendoreActive(VendoreId, GroupId, VendoreIdFrom, VendoreIdTo, ClassIdFrom, ClassIdTo, PeriodType, PeriodFrom, PeriodTo, DateTime.Now.AddYears(-1));
                LRes.ForEach(x => x.LastYear = true);
                FRes.AddRange(LRes);
            }
            ViewBag.GetlastYear = GetlastYear;

            return View(FRes);
        }

        private List<VendoreCurrentActivtiy> CalcVendoreActive(int? VendoreId, int? GroupId,
            int? VendoreIdFrom, int? VendoreIdTo, int? ClassIdFrom, int? ClassIdTo, int? PeriodType, DateTime? PeriodFrom, DateTime? PeriodTo
            , DateTime Time)
        {
            List<Receivable_payment> PP = new List<Receivable_payment>();
            List<Receivable_transaction> PT = new List<Receivable_transaction>();
            Date_option AgingOption = db.Receivable_aging_date_option.FirstOrDefault().Date_option;
            //List<Aging_period> AP = new List<Aging_period>();
            List<VendoreCurrentActivtiy> Res = new List<VendoreCurrentActivtiy>();
            List<VendoreCurrentActivtiy> FRes = new List<VendoreCurrentActivtiy>();

            if (VendoreId != null)
            {
                PP = db.Receivable_payments.Include(x => x.Trans_doc_type).Where(x => x.Vendor_id == VendoreId && x.Is_void == false).ToList();
                PT = db.Receivable_transactions.Where(x => x.Vendor_id == VendoreId && x.Is_void == false).ToList().Where(x => !PP.Any(z => z.Transaction_id == x.Id)).ToList();
            }
            else if (GroupId != null)
            {
                List<Receivable_Group_setting> GS = db.Receivable_Group_settings.Where(x => x.Id == GroupId).ToList();
                PP = db.Receivable_payments.Include(x => x.Trans_doc_type).Include(x => x.Vendor).ToList().Where(x => GS.Any(z => z.Id == x.Vendor.Group_setting_id) && x.Is_void == false).ToList();
                PT = db.Receivable_transactions.Include(z => z.Vendor).Where(x => x.Vendor_id != null && x.Is_void == false).ToList().Where(x => GS.Any(z => z.Id == x.Vendor.Group_setting_id)).ToList().Where(x => !PP.Any(z => z.Transaction_id == x.Id)).ToList();
            }
            if (VendoreIdFrom != null && VendoreIdTo != null)
            {
                PP = db.Receivable_payments.Where(x => x.Vendor_id >= VendoreIdFrom && x.Vendor_id <= VendoreIdTo && x.Is_void == false).ToList();
                PT = db.Receivable_transactions.Where(x => x.Vendor_id >= VendoreIdFrom && x.Vendor_id <= VendoreIdTo && x.Is_void == false).ToList().Where(x => !PP.Any(z => z.Transaction_id == x.Id)).ToList();
            }
            else if (ClassIdFrom != null && ClassIdTo != null)
            {
                List<Receivable_Group_setting> GS = db.Receivable_Group_settings.Where(x => x.Id <= ClassIdFrom && x.Id >= ClassIdTo).ToList();
                PP = db.Receivable_payments.Include(x => x.Trans_doc_type).Include(x => x.Vendor).ToList().Where(x => GS.Any(z => z.Id == x.Vendor.Group_setting_id) && x.Is_void == false).ToList();
                PT = db.Receivable_transactions.Include(z => z.Vendor).Where(x => x.Vendor_id != null && x.Is_void == false).ToList().Where(x => GS.Any(z => z.Id == x.Vendor.Group_setting_id)).ToList().Where(x => !PP.Any(z => z.Transaction_id == x.Id)).ToList();

            }

            if (PeriodFrom != null && PeriodTo != null)
            {
                if (db.Receivable_aging_date_option.FirstOrDefault().Date_option == Date_option.From_document_date)
                {
                    PP = PP.Where(x => x.Posting_date >= PeriodFrom && x.Posting_date <= PeriodTo).ToList();
                    PT = PT.Where(x => x.Posting_date >= PeriodFrom && x.Posting_date <= PeriodTo).ToList();
                }
                else
                {
                    PP = PP.Where(x => x.Due_date >= PeriodFrom && x.Due_date <= PeriodTo).ToList();
                    PT = PT.Where(x => x.Due_date >= PeriodFrom && x.Due_date <= PeriodTo).ToList();
                }

            }
            PP = PP.Where(x => x.Creation_date.Year == Time.Year).ToList();
            PT = PT.Where(x => x.Creation_date.Year == Time.Year).ToList();


            foreach (Receivable_transaction i in PT)
            {
                double DueDateNumbers = 1;
                if (AgingOption == Date_option.From_document_date)
                {
                    if (i.Posting_date > Time)
                    {
                        DueDateNumbers = i.Posting_date.Subtract(Time).TotalDays;
                    }
                    else
                    {
                        DueDateNumbers = Time.Subtract(i.Posting_date).TotalDays;

                    }
                }
                else
                {
                    if (!i.Due_date.HasValue)
                    {
                        i.Due_date = i.Doc_date.AddDays(30);
                    }
                    if (i.Due_date > Time)
                    {
                        DueDateNumbers = i.Due_date.Value.Subtract(Time).TotalDays;
                    }
                    else
                    {
                        DueDateNumbers = Time.Subtract(i.Due_date.Value).TotalDays;

                    }
                }

                int AgingId = 0;
                //try
                //{
                //    if (PeriodType == 2)
                //    {
                //        AgingId = AP.Where(x => DueDateNumbers <= x.From && DueDateNumbers >= x.To).ToList()
                //        .FirstOrDefault().Id;
                //    }
                //    else
                //    {
                //        AgingId = AP.ToList().FirstOrDefault().Id;
                //    }
                //}
                //catch
                //{

                //}

                try
                {
                    //try
                    //{
                    //    if (PeriodType == 2)
                    //    {
                    //        AgingId = AP.Where(x => DueDateNumbers <= x.From && DueDateNumbers >= x.To).ToList()
                    //        .FirstOrDefault().Id;
                    //    }
                    //    else
                    //    {
                    //        AgingId = AP.ToList().FirstOrDefault().Id;
                    //    }
                    //}
                    //catch
                    //{

                    //}

                    //AP.FirstOrDefault(x => x.Id == AgingId).Amount += i.Purchase - i.Taken_discount + i.Tax;
                    //AP.FirstOrDefault(x => x.Id == AgingId).NumberOfTransactions++;
                    //AP.FirstOrDefault(x => x.Id == AgingId).ThisTransactionAmount += i.Purchase - i.Taken_discount + i.Tax;
                    Res.Add(new VendoreCurrentActivtiy
                    {
                        DocType = i.Doc_type,
                        Value = i.Purchase - i.Taken_discount + i.Tax,
                        UnassignedAmount = i.Purchase - i.Taken_discount + i.Tax- db.Assign_Receivable_docs.Where(x => x.Trans_doc_type_id_to == i.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc{ Applay_assign=0}).Sum(x => x.Applay_assign)

                    });
                }
                catch
                { }
            }


            foreach (Receivable_payment i in PP)
            {
                try
                {
                    double DueDateNumbers = 1;
                    if (AgingOption == Date_option.From_document_date)
                    {
                        DueDateNumbers = i.Posting_date.Subtract(Time).TotalDays;
                    }
                    else
                    {
                        try
                        {
                            DueDateNumbers = i.Due_date.Value.Subtract(Time).TotalDays;
                        }
                        catch
                        {

                        }
                    }
                    // int AgingId = AP.Where(x => DueDateNumbers <= x.From && DueDateNumbers >= x.To).ToList().DefaultIfEmpty(AP.LastOrDefault()).FirstOrDefault().Id;
                    try
                    {

                        //AP.FirstOrDefault(x => x.Id == AgingId).Amount -= i.Orginal_amount ;
                        //AP.FirstOrDefault(x => x.Id == AgingId).NumberOfPayments++;
                        //AP.FirstOrDefault(x => x.Id == AgingId).ThisPaymentAmount += i.Orginal_amount;
                        Res.Add(new VendoreCurrentActivtiy
                        {
                            DocType = db.Receivable_transactions_types.Where(x => x.Id == i.Trans_doc_type_id).FirstOrDefault().Doc_type,
                            Value = i.Orginal_amount,
                            UnassignedAmount = i.Orginal_amount-db.Assign_Receivable_docs.Where(x => x.Trans_doc_type_id == i.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 }).Sum(x => x.Applay_assign)
                        });
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
            }

            foreach (List<VendoreCurrentActivtiy> i in Res.GroupBy(x => x.DocType).Select(x => x.ToList()))
            {
                FRes.Add(new Controllers.VendoreCurrentActivtiy
                {
                    DocType = i.FirstOrDefault().DocType,
                    Number = i.Count(),
                    Value = i.Sum(x => x.Value),
                    UnassignedAmount=i.Sum(x=>x.UnassignedAmount)
                });
            }

            return FRes;
        }

        public ActionResult AgingRpt()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Aging = new SelectList(db.Receivable_aging_periods.OrderBy(x => x.From).ToList(), "Id", "Name");
            ViewBag.Creditor = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_name");
            ViewBag.Class = new SelectList(db.Receivable_Group_settings, "Id", "Group_id");
            ViewBag.Currency = new SelectList(dbM.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID), "CurrencyID", "ISOCode");

            return View();
        }
        public ActionResult AgingRptTbl(string DetailsSumary = "Details", string SortBy = "Vendore", string Aging = "All",
         DateTime? From = null, DateTime? To = null, List<IncludeEnum> Include = null, int? VendoreFrom = null, int? VendoreTo = null,
         int? ClassFrom = null, int? ClassTo = null, string MultiDev = "", string OCurrencyId = "", decimal ORate = 1)
        {
            List<AgingRpt> Res = CalcAgingRpt(DetailsSumary, From, To, Include, VendoreFrom, VendoreTo, ClassFrom, ClassTo, MultiDev, OCurrencyId, ref ORate);
            TempData["Error"]= TempData["Error"];
            return View(Res);
        }

        private List<AgingRpt> CalcAgingRpt(string DetailsSumary, DateTime? From, DateTime? To, List<IncludeEnum> Include, int? VendoreFrom, int? VendoreTo, int? ClassFrom, int? ClassTo, string MultiDev, string OCurrencyId, ref decimal ORate)
        {
            List<AgingRpt> Res = new List<AgingRpt>();
            List<Receivable_transaction> PT = new List<Receivable_transaction>();
            List<Receivable_payment> PP = new List<Receivable_payment>();
            List<Assign_Receivable_doc> AP = new List<Assign_Receivable_doc>();
            List<Receivable_aging_period> AgingPeriod = db.Receivable_aging_periods.OrderBy(x => x.From).ToList();
            if (db.Receivable_aging_date_option.FirstOrDefault()==null)
            {
                TempData["Error"] = $"{FabulousErp.Business.Translate("There are no Aging Date Option")}";
                return new List<AgingRpt> ();
            }
            Date_option AO = db.Receivable_aging_date_option.FirstOrDefault().Date_option;
            ViewBag.AgingNames = AgingPeriod;
            ViewBag.DetailsSumary = DetailsSumary;
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            PT = db.Receivable_transactions.Where(x => x.Is_void == false).Include(x => x.Vendor).Include(x => x.Trans_doc_type).Include(x => x.Currency).ToList();
            PP = db.Receivable_payments.Where(x => x.Is_void == false).Include(x => x.Vendor).Include(x => x.Trans_doc_type).Include(x => x.Currency).ToList();
            List<Receivable_aging_period> MyAging = new List<Receivable_aging_period>();

            AP = db.Assign_Receivable_docs.Where(x => x.Is_void == false).Include(x => x.Trans_doc_type).Include(x => x.Trans_doc_type_to).Include(x => x.Currency).Include(x => x.Vendor).ToList();


            if (Include != null)
            {
                if (Include.Any(x => x == IncludeEnum.Fully))
                {

                }
                else
                {
                    PT = Business.GetUnpaidTransaction().AsQueryable().Include(x => x.Vendor).Include(x => x.Trans_doc_type).Include(x => x.Currency).ToList();
                    AP = db.Assign_Receivable_docs.Where(x => x.Is_void == false).Include(x => x.Trans_doc_type).Include(x => x.Trans_doc_type_to).Include(x => x.Currency).Include(x => x.Vendor).ToList()
                        .Where(x => PT.Any(z => z.Trans_doc_type_id == x.Trans_doc_type_id_to)).ToList();
                    PP.ForEach(x => x.Orginal_amount -= db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id && z.Is_void == false)
                                                         .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 }).Sum(z => z.Applay_assign));
                }
            }
            else
            {
                PT = Business.GetUnpaidTransaction().AsQueryable().Include(x => x.Vendor).Include(x => x.Trans_doc_type).Include(x => x.Currency).ToList();
                AP = db.Assign_Receivable_docs.Where(x => x.Is_void == false).Include(x => x.Trans_doc_type).Include(x => x.Trans_doc_type_to).Include(x => x.Currency).Include(x => x.Vendor).ToList()
                    .Where(x => PT.Any(z => z.Trans_doc_type_id == x.Trans_doc_type_id_to)).ToList();
                PP.ForEach(x => x.Orginal_amount -= db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id && z.Is_void == false)
                                                      .ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 }).Sum(z => z.Applay_assign));
            }

            #region Includes Upper
            if (VendoreFrom != null && VendoreTo != null)
            {
                PT = PT.AsQueryable().Include(x => x.Vendor).Include(x => x.Trans_doc_type).Include(x => x.Currency).Where(x => x.Vendor_id >= VendoreFrom && x.Vendor_id <= VendoreTo).ToList();
                PP = PP.AsQueryable().Include(x => x.Vendor).Include(x => x.Trans_doc_type).Include(x => x.Currency).Where(x => x.Vendor_id >= VendoreFrom && x.Vendor_id <= VendoreTo).ToList();
                AP = AP.AsQueryable().Include(x => x.Vendor).Include(x => x.Trans_doc_type).Include(x => x.Trans_doc_type_to).Include(x => x.Currency).Where(x => x.Vendor_id >= VendoreFrom && x.Vendor_id <= VendoreTo).ToList();
            }
            if (To != null && From != null)
            {
                PT = PT.Where(x => x.Posting_date >= From && x.Posting_date <= To).ToList();
                PP = PP.Where(x => x.Posting_date >= From && x.Posting_date <= To).ToList();
            }
            if (ClassFrom != null && ClassTo != null)
            {
                PT = PT.Where(x => x.Vendor != null).Where(x => x.Vendor.Group_setting_id >= ClassFrom && x.Vendor.Group_setting_id <= ClassTo).ToList();
                PP = PP.Where(x => x.Vendor != null).Where(x => x.Vendor.Group_setting_id >= ClassFrom && x.Vendor.Group_setting_id <= ClassTo).ToList();
            }
            if (Include == null
                || !Include.Any(x => x == IncludeEnum.MultiCurrency))
            {
                ORate = 1;
            }

            #endregion

            Res.AddRange(PT.Select(x => new Controllers.AgingRpt
            {
                Id = x.Id,
                TransactionNo = x.Trans_doc_type.Trx_num.ToString(),
                Currency = x.Currency.ISOCode,
                DocDate = x.Doc_date,
                DocNum = x.VDocument_number ,
                DueDate = x.Due_date,


                OrginalAmount = (x.Doc_type != Doc_type.Credit_Memo
                && x.Doc_type != Doc_type.Return) ? (x.Purchase - x.Taken_discount + x.Tax
                 - db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_to.Receivable_transaction.Any(y => y.Trans_doc_type_id == x.Trans_doc_type_id)).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Taken_discount = 0 }).Sum(z => z.Taken_discount))
                   :
                   (x.Purchase - x.Taken_discount + x.Tax)
                   + db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 }).Sum(z => z.Applay_assign),


                Amount = (x.Doc_type != Doc_type.Credit_Memo && x.Doc_type != Doc_type.Return) ?
                    (((x.Purchase - x.Taken_discount + x.Tax) * x.Transaction_rate)
                   - db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_to.Receivable_transaction.Any(y => y.Trans_doc_type_id == x.Trans_doc_type_id)).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0, Taken_discount = 0, Transaction_rate = 1 }).Sum(z => z.Taken_discount * z.Transaction_rate))
                   :
                   (((x.Purchase - x.Taken_discount + x.Tax) * x.Transaction_rate)
                   + db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0, Taken_discount = 0, Transaction_rate = 1 }).Sum(z => z.Taken_discount + z.Applay_assign * z.Transaction_rate)),

                Rate = x.Transaction_rate,
                Type = x.Trans_doc_type.Doc_type.ToString(),

                DueDays = (AO == Date_option.From_due_date) ?
              (x.Due_date.HasValue ? MBus.PostiveSubtract(x.Due_date.Value, DateTime.Now) : MBus.PostiveSubtract(x.Transaction_date, DateTime.Now)) :
              (MBus.PostiveSubtract(x.Posting_date, DateTime.Now)),
                AgingAmount = new List<Receivable_aging_period> { },
                VendoreId = x.Vendor_id,
                JournalEntryNumber = x.Journal_number,
                Currency_id = x.Currency_id,
                Transaction_id = x.Id,
                Doc_type_id_to = x.Trans_doc_type_id,
                Doc_type = x.Doc_type
            }));

            Res.AddRange(AP.Select(x => new Controllers.AgingRpt
            {
                Id = x.Id,
                TransactionNo = x.Trans_doc_type.Trx_num.ToString(),
                Currency = x.Currency.ISOCode,
                OrginalAmount = -((x.Applay_assign)),
                Rate = x.Transaction_rate,
                Type = x.Trans_doc_type.Doc_type.ToString(),
                Amount = -((x.Applay_assign) * x.Transaction_rate),
                DueDays = MBus.PostiveSubtract(x.Applay_date, DateTime.Now),
                AgingAmount = new List<Receivable_aging_period> { },
                VendoreId = x.Vendor_id,
                VendoreName = x.Vendor.Vendor_name,
                JournalEntryNumber = x.JournalEntry,
                Currency_id = x.Currency_id,
                DocDate = x.Applay_date,
                Transaction_id = x.Trans_doc_type_to.Receivable_transaction.FirstOrDefault().Id,
                DocNum = x.Trans_doc_type.Counter.ToString(),
                Doc_type_id = x.Trans_doc_type_id,
                Doc_type_id_to = x.Trans_doc_type_id_to,
                Doc_type = x.Doc_type

            }));

            if (Include != null && Include.Any(x => x == IncludeEnum.Fully))
            {
                Res.AddRange(PP.Select(x => new Controllers.AgingRpt
                {
                    Id = x.Id,
                    TransactionNo = x.Trans_doc_type.Trx_num.ToString(),
                    Currency = x.Currency.ISOCode,
                    OrginalAmount = -((x.Orginal_amount) - db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 }).Sum(z => z.Applay_assign)),
                    Rate = x.Transaction_rate,
                    Type = x.Trans_doc_type.Doc_type.ToString(),
                    Amount = -((x.Orginal_amount - db.Assign_Receivable_docs.Where(z => z.Trans_doc_type_id == x.Trans_doc_type_id).ToList().DefaultIfEmpty(new Assign_Receivable_doc { Applay_assign = 0 }).Sum(z => z.Applay_assign)) * x.Transaction_rate),
                    DueDays = (AO == Date_option.From_due_date) ?
              (x.Due_date.HasValue ? MBus.PostiveSubtract(x.Due_date.Value, DateTime.Now) : MBus.PostiveSubtract(x.Transaction_date, DateTime.Now)) :
              (MBus.PostiveSubtract(x.Posting_date, DateTime.Now)),
                    AgingAmount = new List<Receivable_aging_period> { },
                    VendoreId = x.Vendor_id,
                    VendoreName = x.Vendor.Vendor_name,
                    JournalEntryNumber = x.Journal_number,
                    Currency_id = x.Currency_id,
                    DocDate = x.Posting_date,
                    Transaction_id = x.Transaction_id,
                    DocNum = x.Trans_doc_type.Counter.ToString(),
                    Doc_type_id = x.Trans_doc_type_id,
                    Doc_type = x.Trans_doc_type.Doc_type

                }));
            }
            else
            {
                Res.AddRange(PP.Select(x => new Controllers.AgingRpt
                {
                    Id = x.Id,
                    TransactionNo = x.Trans_doc_type.Trx_num.ToString(),
                    Currency = x.Currency==null?db.CurrenciesDefinition_Tables.FirstOrDefault().ISOCode: x.Currency.ISOCode,
                    OrginalAmount = -((x.Orginal_amount)),
                    Rate = x.Transaction_rate,
                    Type = x.Trans_doc_type.Doc_type.ToString(),
                    Amount = -((x.Orginal_amount * x.Transaction_rate)),
                    DueDays = (AO == Date_option.From_due_date) ?
                 (x.Due_date.HasValue ? MBus.PostiveSubtract(x.Due_date.Value, DateTime.Now) : MBus.PostiveSubtract(x.Transaction_date, DateTime.Now)) :
                 (MBus.PostiveSubtract(x.Posting_date, DateTime.Now)),
                    AgingAmount = new List<Receivable_aging_period> { },
                    VendoreId = x.Vendor_id,
                    VendoreName = x.Vendor.Vendor_name,
                    JournalEntryNumber = x.Journal_number,
                    Currency_id = x.Currency_id,
                    DocDate = x.Posting_date,
                    Transaction_id = x.Transaction_id,
                    DocNum = x.Trans_doc_type.Counter.ToString(),
                    Doc_type_id = x.Trans_doc_type_id,
                    Doc_type = x.Trans_doc_type.Doc_type
                }));

            }

            Res.Where(x => x.DueDays != -1 && x.Amount != 0)
                .ToList()
                .ForEach(x => x.AgingAmount.Add(new Receivable_aging_period
                {
                    Amount = x.Amount,
                    Id = AgingPeriod.Where(z => z.From <= x.DueDays && z.To >= x.DueDays).ToList().DefaultIfEmpty(new Receivable_aging_period { Id = AgingPeriod.ToList().LastOrDefault().Id }).FirstOrDefault().Id,
                    Name = AgingPeriod.Where(z => z.From <= x.DueDays && z.To >= x.DueDays).ToList().DefaultIfEmpty(new Receivable_aging_period { Name = AgingPeriod.ToList().LastOrDefault().Name }).FirstOrDefault().Name,
                    From = AgingPeriod.Where(z => z.From <= x.DueDays && z.To >= x.DueDays).ToList().DefaultIfEmpty(new Receivable_aging_period { From = AgingPeriod.ToList().LastOrDefault().From }).FirstOrDefault().From,
                    To = AgingPeriod.Where(z => z.From <= x.DueDays && z.To >= x.DueDays).ToList().DefaultIfEmpty(new Receivable_aging_period { To = AgingPeriod.ToList().LastOrDefault().To }).FirstOrDefault().To,
                }));


            #region Includes Down
            if (Include != null)
            {
                if (Include.Any(x => x == IncludeEnum.Zero))
                {
                    Res.AddRange(db.Receivable_vendore_settings.ToList().Where(x => !Res.Any(z => z.VendoreId == x.Id))
                                 .ToList().Select(x => new Controllers.AgingRpt
                                 {
                                     VendoreId = x.Id,
                                     VendoreName = x.Vendor_name
                                 }));
                }
                else
                {
                    RemoveZero(ref Res);
                }
            }
            if (Include != null)
            {
                if (!Include.Any(x => x == IncludeEnum.Debit))
                {
                    CalcDebit(ref Res);
                }
                if (!Include.Any(x => x == IncludeEnum.UnPosted))
                {
                    CalcUnPosted(ref Res);
                }

                if (Include.Any(x => x == IncludeEnum.MultiCurrency))
                {

                    foreach (AgingRpt i in Res)
                    {
                        if (MultiDev == "multiple")
                        {
                            i.Amount = i.Amount * ORate;

                        }
                        else
                        {
                            i.Amount = i.Amount / ORate;
                        }
                        i.Currency = OCurrencyId;
                        i.Rate = ORate;
                    }
                }
            }
            else
            {
                CalcDebit(ref Res);
                CalcUnPosted(ref Res);
                RemoveZero(ref Res);

            }
            #endregion
            return Res;
        }

        public ActionResult ClasicAging()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.Vendore = new SelectList(db.Receivable_vendore_settings, "Id", "Vendor_name");
            ViewBag.Years = new SelectList(dbM.NewFiscalYear_Table.Where(x=>x.Fiscal_Year_ID==companyID).ToList().OrderBy(x=>Convert.ToInt32(x.Year)), "YearID", "Year");
            return View();
        }

        public ActionResult ClasicAgingRes(int VendoreId,int Year)
        {
            List<AgingClasic> AC = new List<AgingClasic>();
           List<Receivable_transaction> PT = db.Receivable_transactions.Where(x=>x.Is_void==false)
                .Include(x=>x.Currency).Include(x=>x.Trans_doc_type).ToList();
            List<Receivable_payment> PP = db.Receivable_payments.Where(x=>x.Is_void==false)
                 .Include(x => x.Currency).Include(x => x.Trans_doc_type).ToList();

            decimal FirstRawBalance = PP.Where(x=>x.Transaction_date.Year < Year&&x.Vendor_id== VendoreId).Sum(x=> x.Orginal_amount)
                                 +PT.Where(x => x.Transaction_date.Year < Year && x.Vendor_id == VendoreId).Where(x => x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo)
                                 .Sum(x=> x.Purchase - x.Taken_discount + x.Tax)
                                 - PT.Where(x => x.Transaction_date.Year< Year && x.Vendor_id == VendoreId).Where(x => x.Doc_type == Doc_type.Credit_Memo || x.Doc_type == Doc_type.Return || x.Doc_type == Doc_type.Payment)
                                 .Sum(x => x.Purchase - x.Taken_discount + x.Tax);

            PT = PT.Where(x => x.Vendor_id == VendoreId && x.Transaction_date.Year == Year).ToList();
            PT.Where(x => x.Doc_type == Doc_type.Return || x.Doc_type == Doc_type.Credit_Memo).ToList()
               .ForEach(x => { x.Purchase = -x.Purchase; x.Taken_discount = -x.Taken_discount; x.Tax = -x.Tax; });

            PP = PP.Where(x => x.Vendor_id == VendoreId && x.Transaction_date.Year == Year).ToList();
            AC.AddRange(PT.Select(x => new AgingClasic
            {
                JVNo = x.Journal_number,
                Posting_date = x.Posting_date,
                Desc = x.Desc,
                Debit = (x.Doc_type == Doc_type.Invoice || x.Doc_type == Doc_type.Debit_Memo)
                ? (x.Purchase - x.Taken_discount + x.Tax) : 0,
                Credit= (x.Doc_type == Doc_type.Credit_Memo || x.Doc_type == Doc_type.Return 
                         || x.Doc_type == Doc_type.Payment)
                      ? (x.Purchase - x.Taken_discount + x.Tax) : 0,
                Currency=x.Currency.ISOCode,
                Rate=x.Transaction_rate,
                Doc_type = x.Trans_doc_type.Doc_type,
                Doc_num = x.VDocument_number
            }));
            AC.AddRange(PP.Select(x => new AgingClasic
            {
                JVNo = x.Journal_number,
                Posting_date = x.Posting_date,
                Desc = x.Reference,
                Debit = 0,
                Credit= x.Orginal_amount,
                Currency=x.Currency.ISOCode,
                Rate=x.Transaction_rate,
                Doc_type=x.Trans_doc_type.Doc_type,
                Doc_num=x.Trans_doc_type.Trx_num.ToString()
            }));

            AC.AddRange(PP.Where(x=>x.Taken_discount!=0).Select(x => new AgingClasic
            {
                JVNo = x.Journal_number,
                Posting_date = x.Posting_date,
                Desc = x.Reference,
                Debit = 0,
                Credit= x.Taken_discount,
                Currency=x.Currency.ISOCode,
                Rate=x.Transaction_rate,
                Doc_type=x.Trans_doc_type.Doc_type,
                Doc_num=x.Trans_doc_type.Trx_num.ToString()
            }));

            AC.AddRange(db.Assign_Receivable_docs.Where(x => x.Taken_discount != 0)
                .ToList().Select(x => new AgingClasic
            {
                JVNo = x.JournalEntry,
                Posting_date = x.Applay_date,
                Desc = FabulousErp.BusController.Translate("Assign Payalbe document"),
                Debit = 0,
                Credit = x.Taken_discount,
                Currency = x.Currency.ISOCode,
                Rate =x.Transaction_rate,
                Doc_type = x.Trans_doc_type.Doc_type,
                Doc_num = x.Trans_doc_type.Trx_num.ToString()
            }));

            AC.Where(x => x.Credit != 0).ToList().ForEach(x => x.Orginal_amount = x.Credit);
            AC.Where(x => x.Debit != 0).ToList().ForEach(x => x.Orginal_amount = x.Debit);

            AC.Where(x => x.Credit != 0).ToList().ForEach(x => x.Credit = x.Credit * x.Rate);
            AC.Where(x => x.Debit != 0).ToList().ForEach(x => x.Debit = x.Debit * x.Rate);

            AC.ForEach(x => x.Balance = x.Debit - x.Credit);
            AC.Add(new AgingClasic
            {
                Posting_date=new DateTime(Year,1,1),
                Balance= FirstRawBalance,
                Credit=(FirstRawBalance<0)? FirstRawBalance :0,
                Debit= (FirstRawBalance > 0) ? FirstRawBalance :0 ,
                Currency="",
                Desc= FabulousErp.BusController.Translate("Begingn Balance"),
                IsFirst=true
            });
            return View(AC);
        }

        private static void CalcFully(ref List<AgingRpt> Res)
        {
            List<AgingRpt> RmRange = new List<AgingRpt>();
            bool ChangeAmountSign = false;
            bool ChangeAssignAmountSign = false;
            foreach (AgingRpt i in Res)
            {
                if ( i.Amount != 0)
                {
                    if (i.Amount < 0)
                    {
                        i.Amount = -i.Amount;
                        ChangeAmountSign = true;
                    }
                    if (i.AssignAmount < 0)
                    {
                        i.AssignAmount = -i.AssignAmount;
                        ChangeAssignAmountSign = true;
                    }
                    if (i.Amount <= i.AssignAmount)
                    {
                        RmRange.Add(i);
                    }
                    if (ChangeAmountSign)
                    {
                        i.Amount = -i.Amount;
                    }
                    if (ChangeAssignAmountSign)
                    {
                        i.AssignAmount = -i.AssignAmount;

                    }
                    ChangeAssignAmountSign = false;

                    ChangeAmountSign = false;
                }
               
            }
            foreach (AgingRpt i in RmRange)
            {
                Res.Remove(i);
            }
        }
        static void RemoveZero(ref List<AgingRpt> Res)
        {
            List<AgingRpt> Temp = new List<AgingRpt>();
            Temp = Res.Where(x => x.OrginalAmount == 0).ToList();
            foreach (AgingRpt i in Temp)
            {
                Res.Remove(i);
            }
        }
       
        private void CalcUnPosted(ref List<AgingRpt> Res)
        {
            List<AgingRpt> RmRange = new List<AgingRpt>();

            foreach (AgingRpt i in Res)
            {
                try
                {
                    if (string.IsNullOrEmpty(Convert.ToString(dbM.C_GeneralJournalEntry_Tables.FirstOrDefault(x => x.C_JournalEntryNumber == i.JournalEntryNumber).C_Posting)))
                    {
                        RmRange.Add(i);
                    }
                }
                catch
                {

                }
              
            }
            foreach (AgingRpt i in RmRange)
            {
                Res.Remove(i);
            }
        }

        private static void CalcDebit(ref List<AgingRpt> Res)
        {
            List<AgingRpt> RmRange = new List<AgingRpt>();
            foreach (List<AgingRpt> i in Res.GroupBy(x => x.VendoreId).Select(x => x.ToList()))
            {
                if (i.Sum(x => x.Amount) < 0)
                {
                    RmRange.AddRange(RmRange);
                }
            }
            foreach (AgingRpt i in RmRange)
            {
                Res.Remove(i);
            }
        }
        //public JsonResult InsertTrx()
        //{
        //    List<RecTemp> Rec = db.Database.SqlQuery<RecTemp>("select R.*,S.Id CusId from RecTemp R inner join Receivable_vendore_setting S on R.Name=S.Vendor_name where R.Tr_date is not null").ToList();
        //    if (Rec.Any(x => x.CusId == null))
        //    {
        //        return Json(Rec.FirstOrDefault(x => x.CusId == null).Name,JsonRequestBehavior.AllowGet);
        //    }
        //    Receivable_transactionController R = new Receivable_transactionController();
        //    string Cur = db.CurrenciesDefinition_Tables.FirstOrDefault().CurrencyID;
        //    foreach (RecTemp inv_receive_po in Rec)
        //    {
        //        R.AddReceivableTransaction(new Receivable_transaction
        //        {
        //            Currency_id = Cur,
        //            Vendor_id = inv_receive_po.CusId,
        //            Doc_date = inv_receive_po.Tr_date,
        //            Transaction_date = inv_receive_po.Tr_date,
        //            Posting_date = inv_receive_po.Tr_date,
        //            Doc_type = Doc_type.Invoice,
        //            Desc = "ترحيل اول",
        //            Creation_date = DateTime.Now,
        //            Journal_number = 0,
        //            Purchase = inv_receive_po.Balance,
        //            Taken_discount = 0,
        //            Tax = 0,
        //            System_rate = 1,
        //            Transaction_rate = 1,
        //        });
        //    }
        //    return Json(1, JsonRequestBehavior.AllowGet);
        //}
    }
    public class RecTemp
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Tr_date { get; set; }
        public decimal Balance { get; set; }
        public int? TrxNum { get; set; }
        [NotMapped]
        public int? CusId { get; set; }
    }
    public class VendoreBalance
    {
       public List<TrxPayNumber> TrxPayNumber { get; set; }
       public List<Receivable_aging_period> Aging { get; set; }
    }
    public class VendoreCurrentActivtiy
    {
       public Doc_type DocType { get; set; }
       public int Number { get; set; }
       public decimal Value { get; set; }
       public decimal UnassignedAmount { get; set; }
        public bool LastYear { get; set; } = false;
    }
    public class AgingRpt
    {
        public int Id { get; set; }

        public string TransactionNo { get; set; }
        public string DocNum { get; set; }
        public string Type { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal OrginalAmount { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public double? DueDays { get; set; }
        public int? VendoreId { get; set; }
        public string VendoreName { get; set; }
        public List<Receivable_aging_period> AgingAmount { get; set; }
        public int JournalEntryNumber { get; set; }
        public decimal AssignAmount { get; set; }
        public string Currency_id { get; set; }
        public int? Transaction_id { get; set; }
        public int? Doc_type_id { get; set; }
        public int? Doc_type_id_to { get; set; }
        public Doc_type Doc_type { get; set; }
    }
    public class TrxPayNumber
    {
        public int? Vendor_id { get; set; }
        public string Vendore_name { get; set; }
        public int Number_of_transaction { get; set; }
        public decimal Amount_of_transaction { get; set; }
        public int Number_of_payment { get; set; }
        public decimal Amount_of_payment { get; set; }
        public bool IsTrx { get; set; }
    }
    
    public class AgingClasic
    {
        public decimal JVNo { get; set; }
        public DateTime Posting_date { get; set; }
        public string Desc { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal Balance { get; set; }
        public decimal Orginal_amount { get; set; }
        public bool IsFirst { get; set; } = false;
        public Doc_type Doc_type { get; set; }
        public string Doc_num { get; set; }
    }
    public class AgingSum
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }
    public enum IncludeEnum
    {
        Zero=1,
        Debit=2,
        UnPosted=3,
        Fully=4,
        MultiCurrency=5
    }
}