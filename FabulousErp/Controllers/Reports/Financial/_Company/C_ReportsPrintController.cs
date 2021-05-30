using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.Repository.Business;
using FabulousModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Reports.Financial._Company
{
    public class C_ReportsPrintController : Controller
    {
        DBContext DB = new DBContext();

        [ValidateInput(false)]
        // GET: C_ReportsPrint
        public ActionResult Done(string ExtraModel)
        {
            ViewBag.ExtraModel = ExtraModel;
            return View();
        }
        public PartialViewResult ReportsPrintHeader()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            string userID = FabulousErp.Business.GetUserId();
            var data = RSelectBusiness.DBC().CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (data != null)
            {
                var logoPath = "";
                if (data.LogoByte != null)
                {
                    var base64 = Convert.ToBase64String(data.LogoByte);
                    logoPath = string.Format("data:image/gif;base64,{0}", base64);
                }
                ViewBag.CompanyName = data.CompanyName;
                ViewBag.UserID = userID;
                ViewBag.DateTime = DateTime.Now;
                ViewBag.Logo = logoPath;
            }
            var data2 = RSelectBusiness.DBC().CreateAccount_Tables.Where(x => x.UserID == userID).FirstOrDefault();
            if (data2 != null)
            {
                ViewBag.UserName = data2.UserName;
            }
            return PartialView();
        }
        public PartialViewResult ReportsPrintFooter()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var data = RSelectBusiness.DBC().CompanyAddressInfo_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (data != null)
            {
                ViewBag.BuildingNo = data.BuldingNo;
                ViewBag.StreetName = data.StreetName;
                ViewBag.City = data.City;
                ViewBag.Governorate = data.Governorate;
            }
            return PartialView();
        }
        [ValidateInput(false)]
        public ActionResult PrintReport(string searchNumber,string ExtraModel)
        {
            List<int> searchList = searchNumber.Split(',').ToList().ConvertAll(x => Convert.ToInt32(x));
            List<PrintTransaction> retrieveTransactionData = new List<PrintTransaction>();
            List<C_GeneralJournalEntry_Table> Res = DB.C_GeneralJournalEntry_Tables.ToList().Where(x => searchList.Contains(x.C_JournalEntryNumber)).ToList();
            if (!Res.Any())
            {
                Res = DB.C_GeneralJournalEntry_Tables.ToList().Where(x => searchList.Contains(x.C_PostingNumber)).ToList();
            }
            if (!Res.Any())
            {
                Res = DB.C_GeneralJournalEntry_Tables.ToList().Where(x => searchList.Contains(String.IsNullOrEmpty(x.C_CBID.ToString()) ? 0 : Convert.ToInt32(x.C_CBID.Value))).ToList();
            }
            int Po = Res.FirstOrDefault().C_PostingNumber;
            ViewBag.PO = Po;
            if (DB.C_SaveCostCenter_Tables.Any(x => x.C_PostingNumber == Po))
            {
                ViewBag.HasCostCenter = true;
            }
            else
            {
                ViewBag.HasCostCenter = false;
            }
            foreach (C_GeneralJournalEntry_Table i in Res)
            { 
                PrintTransaction P = new PrintTransaction()
                {
                    ShowGeneralLedger = new List<ShowTransaction>
                    {},
                    ShowAnalytics=new List<AnaylticRpt>
                    {}
                };
                P.ShowHeader2 = new ShowHeader
                {
                    JENumber = i.C_JournalEntryNumber,
                    PostingDate = Convert.ToDateTime(i.C_PostingDate).ToString("yyyy-MM-dd"),
                    TransactionDate =Convert.ToDateTime(i.C_TransactionDate).ToString("yyyy-MM-dd"),
                    CurrencyID = i.CurrencyID,
                    SystemRate = i.C_SystemRate,
                    TransactionRate = i.C_TransactionRate,
                    Reference = i.C_Refrence,
                    ISO = i.CurrenciesDefinition_Table.ISOCode,
                    PostingKey = i.C_PostingKey,
                    TransactionType = i.C_TransactionType
                };
                foreach (C_GeneralLedger_Table L in i.C_GeneralLedger_Tables)
                {
                    string Description = L.C_Describtion;
                    try
                    {
                        string Desctest = L.C_Describtion.Replace("-", "");
                        if (decimal.TryParse(Desctest,out decimal TRes))
                        {
                            Description = i.C_Refrence;
                        }
                    }
                    catch
                    {
                        Description = L.C_Describtion;
                    }
                    P.ShowGeneralLedger.Add(new ShowTransaction
                    {
                        PostingNumber = L.C_PostingNumber,
                        AID = L.C_AID,
                        AccountID = L.C_CreateAccount_Table.AccountID,
                        OriginalAmount = L.C_OriginalAmount,
                        Credit = L.C_Credit,
                        Debit = L.C_Debit,
                        Describtion = Description,
                        Document = L.C_Document,
                        AccountName = L.C_CreateAccount_Table.AccountName
                    });
                }
                foreach (C_SaveAnalytic_Table A in i.C_SaveAnalytic_Tables.AsQueryable().Include(x=>x.C_GeneralJournalEntry_Table).Include(x=>x.C_GeneralJournalEntry_Table.CurrenciesDefinition_Table).Include(x=>x.C_CreateAccount_Table))
                {
                    try
                    {
                        P.ShowAnalytics.Add(new AnaylticRpt
                        {
                            Balance = A.Ballance,
                            Credit = A.C_Credit,
                            Debit = A.C_Debit,
                            Date = A.C_GeneralJournalEntry_Table.C_PostingDate,
                            Currency = A.C_GeneralJournalEntry_Table.CurrenciesDefinition_Table.ISOCode,
                            Description = A.Describtion,
                            JE_num = A.C_GeneralJournalEntry_Table.C_JournalEntryNumber,
                            Posting_key = A.C_GeneralJournalEntry_Table.C_PostingKey,
                            Transaction_rate = A.C_GeneralJournalEntry_Table.C_TransactionRate,
                            Posting_Num = A.C_PostingNumber,
                            V = A.C_GeneralJournalEntry_Table.VoidPostingNum.HasValue,
                            Account_id= A.C_CreateAccount_Table.AccountID,
                            Prcentage=A.C_Percentage,
                            Anayaltic_Distribution=A.C_AnalyticDistribution_Table.C_AccountDistributionID,
                            Original_amount=A.Ballance,
                            Action ="Print"
                        });
                    }
                    catch (Exception ex)
                    {}
                  
                }
                ViewBag.ExtraModel = ExtraModel;
                retrieveTransactionData.Add(P);
            }
            try
            {
                ViewBag.HasTax = false;
                int JN = Business.GetJournalEntry(retrieveTransactionData.FirstOrDefault().ShowGeneralLedger.FirstOrDefault().PostingNumber);
                if (DB.Taxs.Any(x => x.Journal_number == JN))
                {
                    ViewBag.HasTax = true;
                }
            }
            catch
            {
                ViewBag.HasTax = false;
            }
            return PartialView("GeneralEntry_Print", retrieveTransactionData);
        }
    }


}