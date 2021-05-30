using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Inquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousModels.ViewModels;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;

namespace FabulousErp.Controllers.Inquiry.Financial
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CompanyJETransactionsDetailsController : Controller
    {
        DBContext DB = new DBContext();

        // GET: Inquiry_CompanyAccountDetails
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IADI")]
        public ActionResult AccountDetails()
        {
            return View();
        }

        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IHADI")]
        public ActionResult HistoricalAccountDetails()
        {
            return View();
        }

        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IHADI")]
        public ActionResult AnalyticsRpt()
        {
            return View();
        } 
        
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IHADI")]
        public ActionResult CostRpt()
        {
            return View();
        }
        public PartialViewResult AnalyticsRptSearchHeader()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var getFiscalYearID = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

            var yearName = DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == getFiscalYearID.Fiscal_Year_ID).ToList();
            SelectList YearList = new SelectList(yearName, "YearID", "Year");

            ViewBag.YearList = YearList;
            var AnalyticsGroup = DB.C_AnalyticAccount_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList accountList = new SelectList(AnalyticsGroup, "C_AnalyticAccountID", "C_AnalyticAccountName");
            ViewBag.AnalyticsGroup = accountList;

            return PartialView();
        }
         public PartialViewResult CostRptSearchHeader()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var getFiscalYearID = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID ==
            companyID).FirstOrDefault();

            var yearName = DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == 
            getFiscalYearID.Fiscal_Year_ID).ToList();

            SelectList YearList = new SelectList(yearName, "YearID", "Year");

            ViewBag.YearList = YearList;
            var AnalyticsGroup = DB.C_CostCenterAccounts_Tables.ToList();
            SelectList accountList = new SelectList(AnalyticsGroup, "C_CostAccountID", "C_CostAccountName");
            ViewBag.CostGroup = accountList;

            return PartialView();
        }

        public PartialViewResult AnalyticsRptSearchBody(int? DistId, bool IsAll, int Year,string MainAccId, DateTime? Start=null, DateTime? End=null)
        {
            List<AnaylticRpt> Rpt = new List<AnaylticRpt>();
            if (DistId == 0)
            {
                DistId = null;
            }
            string CompanyId = Business.GetCompanyId();

            NewFiscalYear_Table FiscalYear = DB.NewFiscalYear_Table.FirstOrDefault(x => x.Fiscal_Year_ID == CompanyId && x.Year == Year.ToString());
            DateTime StartOfYear = Convert.ToDateTime(FiscalYear.Fiscal_Year_Start);


            
            //Rpt.AddRange(Beging);
            Rpt.AddRange(DB.C_SaveAnalytic_Tables.Include(x => x.C_GeneralJournalEntry_Table)
                .Include(x => x.C_GeneralJournalEntry_Table.CurrenciesDefinition_Table)
                .Include(x => x.C_GeneralJournalEntry_Table.C_GeneralLedger_Tables)
                .Include(x=>x.C_AnalyticDistribution_Table).ToList()
                .Where(x =>x.C_AnalyticAccount_Table.C_AnalyticAccountID== MainAccId && Convert.ToDateTime(x.C_GeneralJournalEntry_Table.C_PostingDate).Year == Year)
                .Select(x => new AnaylticRpt
                {
                    Credit = x.C_Credit,
                    Debit = x.C_Debit,
                    Currency = x.C_GeneralJournalEntry_Table.CurrenciesDefinition_Table.ISOCode,
                    Date = x.C_GeneralJournalEntry_Table.C_PostingDate,
                    Description = x.Describtion,
                    V = x.C_GeneralJournalEntry_Table.VoidPostingNum.HasValue,
                    Original_amount = x.Ballance,
                    Transaction_rate = x.C_GeneralJournalEntry_Table.C_TransactionRate,
                    Posting_key = x.C_GeneralJournalEntry_Table.C_PostingKey,
                    Posting_Num = x.C_GeneralJournalEntry_Table.C_PostingNumber,
                    JE_num = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber,
                    Balance = Rpt.Where(z => z.IsBeging == true).Sum(z => z.Balance) + x.C_Debit - x.C_Credit,
                    DistId=x.C_DistID,
                    Dist_name=x.C_AnalyticDistribution_Table.C_AccountDistributionName,
                    Beging = DB.C_SaveAnalytic_Tables.Include(z => z.C_GeneralJournalEntry_Table)
                            .Include(z => z.C_GeneralJournalEntry_Table.CurrenciesDefinition_Table)
                            .Include(z => z.C_GeneralJournalEntry_Table.C_GeneralLedger_Tables)
                             .Where(z=>z.C_DistID==x.C_DistID)
                            .ToList().Where(z => Convert.ToDateTime(z.C_GeneralJournalEntry_Table.C_PostingDate) < Convert.ToDateTime(FiscalYear.Fiscal_Year_Start))
                            .Select(z => new AnaylticRpt
                            {
                                Credit = 0,
                                Debit = 0,
                                Currency = "",
                                Date = "",
                                Description = "Beginning",
                                V = false,
                                Original_amount = 0,
                                Balance = z.Ballance,
                                Transaction_rate = 0,
                                Posting_key = "",
                                JE_num = 0,
                                Posting_Num = 0,
                                IsBeging = true

                            }).DefaultIfEmpty(new AnaylticRpt
                            {
                                Credit = 0,
                                Debit = 0,
                                Currency = "",
                                Date = "",
                                Description = "Beginning",
                                V = false,
                                Original_amount = 0,
                                Balance = 0,
                                Transaction_rate = 0,
                                Posting_key = "",
                                JE_num = 0,
                                Posting_Num = 0,
                                IsBeging = true,
                                DistId = DistId
                            }).ToList()
                }));
            if (DistId.HasValue)
            {
                Rpt = Rpt.Where(x => x.DistId == DistId.Value).ToList();
            }
            if (!IsAll)
            {
                Rpt = Rpt.Where(x=>!string.IsNullOrEmpty(x.Date)).Where(x => Convert.ToDateTime(x.Date) >= Start && Convert.ToDateTime(x.Date) <= End).ToList();
                //Rpt.InsertRange(0, Beging);
            }
            
            return PartialView(Rpt);
        }
        public PartialViewResult JvCostCenterRpt(int PostingNumber)
        {
            return PartialView(DB.C_SaveCostCenter_Tables.Where(x=>x.C_PostingNumber==PostingNumber)
                .Include(x=>x.C_CostCenter_Table)
                .Include(x=>x.C_CostCenterAccounts_Table)
                .Include(x=>x.C_CreateAccount_Table)
                .Include(x=>x.C_GeneralJournalEntry_Table)
                .Include(x=>x.C_MainCostCenter_Table)
                )
                ;
        }
        public JsonResult GetAnaylticsDis(string AnaylticId)
        {
            try
            {
                return Json(DB.C_AnalyticDistribution_Tables
                    .Where(x => x.C_AnalyticAccountID == AnaylticId)
                    .Select(x => new { id = x.C_DistID, name = x.C_AccountDistributionID }).ToList());
            }
            catch
            {
                return Json("");
            }
        }  
        public JsonResult GetCostDis(string CostId)
        {
            try
            {
                return Json(DB.C_AnalyticDistribution_Tables
                    .Where(x => x.C_AnalyticAccountID == CostId)
                    .Select(x => new { id = x.C_DistID, name = x.C_AccountDistributionID }).ToList());
            }
            catch
            {
                return Json("");
            }
        }
        public JsonResult GetAnalyticsData()
        {
            return Json(null);
        }
        public JsonResult GetAccountName(int AID)
        {
            var getName = DB.C_CreateAccount_Tables.Where(x => x.C_AID == AID).FirstOrDefault();
            if (getName != null)
            {
                return Json(getName.AccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAllSearchData(int AID, int year)
        {
            //2
            var getDates = DB.NewFiscalYear_Table.Where(x => x.YearID == year).FirstOrDefault();

            return Json(GetSearchData(AID, DateTime.Parse(getDates.Fiscal_Year_Start), DateTime.Parse(getDates.Fiscal_Year_End), year), JsonRequestBehavior.AllowGet);
            //return null;
        }
        public JsonResult GetSearchDataByDates(int AID, int year, DateTime dateFrom, DateTime dateTo)
        {
            //3
            var getDates = DB.NewFiscalYear_Table.Where(x => x.YearID == year).FirstOrDefault();

            if (dateFrom >= DateTime.Parse(getDates.Fiscal_Year_Start) && dateTo <= DateTime.Parse(getDates.Fiscal_Year_End))
            {
                return Json(GetSearchData(AID, dateFrom, dateTo, year), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("inValid", JsonRequestBehavior.AllowGet);
            }
            //return null;
        }
        private IEnumerable<Inquiry_JETransaction_DTO> GetSearchData(int AID, DateTime dateFrom, DateTime dateTo, int year)
        {
            return DB.C_GeneralLedger_Tables.ToList().Where(x => x.C_AID == AID && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) >= dateFrom && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) <= dateTo).OrderBy(x => DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate)).Select(x => new Inquiry_JETransaction_DTO
            {
                JournalEntryNumber = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber,

                PostingNumber = x.C_GeneralJournalEntry_Table.C_PostingNumber,

                Date = x.C_GeneralJournalEntry_Table.C_PostingDate,

                OriginalAmount = x.C_OriginalAmount,

                Currency = x.C_GeneralJournalEntry_Table.CurrenciesDefinition_Table.ISOCode,

                TransactionRate = x.C_GeneralJournalEntry_Table.C_TransactionRate,

                Describtion = x.C_Describtion,

                Debit = x.C_Debit,

                Credit = x.C_Credit,

                CurrencyID = x.C_GeneralJournalEntry_Table.CurrencyID,

                PostingKey = x.C_GeneralJournalEntry_Table.C_PostingKey,

                VoidJENum = x.C_GeneralJournalEntry_Table.VoidPostingNum

            }).ToList();
        }



        public PartialViewResult SearchHeader()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var getFiscalYearID = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

            var yearName = DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == getFiscalYearID.Fiscal_Year_ID && x.CheckDate == "True" && (x.Closed == null || x.Closed == false)).ToList();
            SelectList YearList = new SelectList(yearName, "YearID", "Year");

            ViewBag.YearList = YearList;

            var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            return PartialView("SearchHeader");
        }
        public PartialViewResult HistoricalSearchHeader()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var getFiscalYearID = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

            var yearName = DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == getFiscalYearID.Fiscal_Year_ID && x.CheckDate == "True" && x.Closed == true).ToList();
            SelectList YearList = new SelectList(yearName, "YearID", "Year");

            ViewBag.YearList = YearList;

            var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            return PartialView();
        }



        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IASI")]
        public ActionResult AccountSummary()
        {
            return View();
        }
        public JsonResult PeriodsDate(int yearID, int periodNo)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var data = DB.FiscalYear_Tables.Where(x => x.NewFiscalYear_Table.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().CompanyID == companyID && x.YearID == yearID && x.Period_No == periodNo).FirstOrDefault();
            PeriodsData periodsData = new PeriodsData()
            {
                StartDate = data.Period_Start_Date,
                EndDate = data.Period_End_Date
            };
            return Json(periodsData, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IHASI")]
        public ActionResult HistoricalAccountSummary()
        {
            return View();
        }
        public JsonResult GetAccountSummary(int AID, int year)
        {
            var periods = DB.FiscalYear_Tables.Where(x => x.YearID == year).OrderBy(x => x.Period_No).ToList();

            double? beDebit = 0;
            double? beCredit = 0;
            var beBallance = DB.C_EndingBeginingYears.Where(x => x.YearID == year && x.C_AID == AID && x.Type == 2).FirstOrDefault();
            if (beBallance != null)
            {
                beDebit = beBallance.Debit;
                beCredit = beBallance.Credit;
            }
           
            List<Inquiry_JETransaction_DTO> finalData = new List<Inquiry_JETransaction_DTO>
            {
                new Inquiry_JETransaction_DTO
                {
                    Periods = BusController.Translate("Beginning Ballance"),

                    Debit = beDebit,

                    Credit = beCredit,

                    NetChange = beDebit - beCredit,

                    EndingBalance = beDebit - beCredit
                }
            };

            for (int i = 0; i < periods.Count; i++)
            {
                double? debit = DB.C_GeneralLedger_Tables.ToList().Where(x => x.C_AID == AID && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) >= DateTime.Parse(periods[i].Period_Start_Date) && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) <= DateTime.Parse(periods[i].Period_End_Date)).Select(x => x.C_Debit).Sum();
                double? credit = DB.C_GeneralLedger_Tables.ToList().Where(x => x.C_AID == AID && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) >= DateTime.Parse(periods[i].Period_Start_Date) && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) <= DateTime.Parse(periods[i].Period_End_Date)).Select(x => x.C_Credit).Sum();

                double? oldBalance = finalData[i].EndingBalance;
                double? newBalance = oldBalance + debit - credit;

                finalData.Add(new Inquiry_JETransaction_DTO
                {
                    Periods = BusController.Translate("Period ",true) + periods[i].Period_No.ToString(),

                    Debit = debit,

                    Credit = credit,

                    NetChange = debit - credit,

                    EndingBalance = newBalance
                });
            }
            return Json(finalData, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ITDI")]
        public ActionResult TransactionDetails()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var getCurrencyID = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }

            var getJENum = DB.C_GeneralJournalEntry_Tables.Where(x => x.CompanyID == companyID && x.Post == true).Select(x => new { PostingNumber = x.C_PostingNumber, JournalEntryNum = x.C_JournalEntryNumber + "-" + x.C_PostingKey }).ToList();
            if (getJENum != null)
            {
                SelectList JEList = new SelectList(getJENum, "PostingNumber", "JournalEntryNum");
                ViewBag.JEList = JEList;
            }

            return View();
        }



        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICBSI")]
        public ActionResult BatchSecurity()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            List<Inquiry_JETransaction_DTO> data = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID).Select(x => new Inquiry_JETransaction_DTO
            {
                BatchID = x.C_BatchID,

                BatchDescribtion = x.C_BatchDescription,

                BatchFrom = x.C_BatchLocation,

                BatchCreationDate = x.C_BatchCreationDate,

                UserCreatedBatch = x.UserID,

                UserCreatedBatchName = x.CreateAccount_Table.UserName,

                UserApprovedBatch = x.C_UserBatchApproval_Table.UserID,

                UserApprovedBatchName = x.C_UserBatchApproval_Table.CreateAccount_Table.UserName,

                ApprovedDate = x.C_UserBatchApproval_Table.ApprovedDate,

                NoOfTrx = x.C_GeneralJournalEntry_Tables.Count,

                BatchTotal = x.C_GeneralJournalEntry_Tables.Sum(y => y.C_TotalDebit) + x.C_GeneralJournalEntry_Tables.Sum(y => y.C_TotalCredit)
            }).ToList();

            var batches = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID).Select(x => new { batchID = x.C_BatchID + " ( " + x.C_BatchLocation + " )", CBID = x.C_CBID }).ToList();
            SelectList batchList = new SelectList(batches, "CBID", "batchID");
            ViewBag.batchList = batchList;

            var users = DB.UACompPremission_Tables.Where(x => x.CompanyID == companyID).Select(x => new { userID = x.UserID, userIDName = x.UserID + " - " + x.UserName }).ToList();
            SelectList userList = new SelectList(users, "userID", "userIDName");
            ViewBag.userList = userList;

            return View(data);
        }
    }


    public class PeriodsData
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
 
}