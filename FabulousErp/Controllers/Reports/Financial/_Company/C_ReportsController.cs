using FabulousDB.DB_Context;
using FabulousErp.Repository.Business;
using FabulousModels.DTOModels.Reports.Financial.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Reports.Financial._Company
{
    public class C_ReportsController : Controller
    {
        DBContext DB = new DBContext();

        // GET: C_Reports
        public ActionResult TrailBalance()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var checkCurrencyFormate = RSelectBusiness.DBC().FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }

            var FiscalyearCheck = RSelectBusiness.DBC().CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (FiscalyearCheck != null)
            {
                var checkYearOpen = RSelectBusiness.DBC().NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == FiscalyearCheck.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
                if (checkYearOpen != null)
                {
                    ViewBag.CheckYear = "Exist";
                }
            }

            var segmentsList = DB.AccountChart_Table
                .Where(x => x.C_CreateAccount_Table.FirstOrDefault().CompanyID == companyID && x.AccountChartID == x.C_CreateAccount_Table.FirstOrDefault().AccountChartID)
                .FirstOrDefault();
            if (segmentsList != null)
            {
                ViewBag.SegmentNumber = segmentsList.NumberOfSegment;
            }

            var getCompanyAccounts = DB.C_CreateAccount_Tables
                    .Where(x => x.CompanyID == companyID)
                    .Select(x => new { AID = x.C_AID, AccountName = x.AccountID + "-" + x.AccountName })
                    .ToList();
            SelectList accountsList = new SelectList(getCompanyAccounts, "AID", "AccountName");
            ViewBag.AccountsList = accountsList;

            var currencyList = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList Currency = new SelectList(currencyList, "CurrencyID", "ISOCode");
            ViewBag.Currency = Currency;


            return View();
        }
        public JsonResult GetYears(string status)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            if (status == "open" || status == "current")
            {
                var date = DateTime.Now;

                List<Reports_DTO> Reports_DTO = DB.NewFiscalYear_Table
                    .Where(x => x.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().CompanyID == companyID && x.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().Fiscal_Year_ID == x.Fiscal_Year_ID && x.Closed == null)
                    .Select(x => new Reports_DTO
                    {
                        YearID = x.YearID,
                        YearName = x.Year,
                        First_Year_Start = x.Fiscal_Year_Start,
                        First_Year_End = x.Fiscal_Year_End
                    }).ToList();
                return Json(Reports_DTO, JsonRequestBehavior.AllowGet);
            }
            else if (status == "history")
            {
                List<Reports_DTO> Reports_DTO = DB.NewFiscalYear_Table
                 .Where(x => x.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().CompanyID == companyID && x.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().Fiscal_Year_ID == x.Fiscal_Year_ID && x.Closed != null)
                 .Select(x => new Reports_DTO
                 {
                     YearID = x.YearID,
                     YearName = x.Year,
                 }).ToList();
                return Json(Reports_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        public JsonResult GetYearsDate(int yearID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            Reports_DTO reports_DTO = new Reports_DTO()
            {
                First_Year_Start = DB.NewFiscalYear_Table.Where(x => x.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().CompanyID == companyID && x.YearID == yearID).Select(x => x.Fiscal_Year_Start).FirstOrDefault(),
                First_Year_End = DB.NewFiscalYear_Table.Where(x => x.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().CompanyID == companyID && x.YearID == yearID).Select(x => x.Fiscal_Year_End).FirstOrDefault(),
            };
            return Json(reports_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccounts(int sortValue)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            if (sortValue == 1)
            {
                List<Reports_DTO> Reports_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountID).Select(x => new Reports_DTO
                {
                    C_AID = x.C_AID,
                    AccountID = x.AccountID,
                    AccountName = x.AccountName
                }).ToList();
                return Json(Reports_DTO, JsonRequestBehavior.AllowGet);
            }
            else if (sortValue == 2)
            {
                List<Reports_DTO> Reports_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.DisActive == false).OrderBy(x => x.AccountID).Select(x => new Reports_DTO
                {
                    C_AID = x.C_AID,
                    AccountID = x.AccountID,
                    AccountName = x.AccountName
                }).ToList();
                return Json(Reports_DTO, JsonRequestBehavior.AllowGet);
            }
            else if (sortValue == 3)
            {
                List<Reports_DTO> Reports_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountName).Select(x => new Reports_DTO
                {
                    C_AID = x.C_AID,
                    AccountID = x.AccountID,
                    AccountName = x.AccountName
                }).ToList();
                return Json(Reports_DTO, JsonRequestBehavior.AllowGet);
            }
            else if (sortValue == 4)
            {
                List<Reports_DTO> Reports_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.DisActive == false).OrderBy(x => x.AccountName).Select(x => new Reports_DTO
                {
                    C_AID = x.C_AID,
                    AccountID = x.AccountID,
                    AccountName = x.AccountName
                }).ToList();
                return Json(Reports_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        public JsonResult GetData(List<AccountsData> accountsArray, string startDate, string endDate, int yearID, int? currencyRate, int? currencyOperation)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            decimal debit = 0;
            decimal credit = 0;
            decimal beginningBalance = 0;
            List<AccountsData> AccountsData = new List<AccountsData>();
            foreach (var data in accountsArray)
            {
                beginningBalance = DB.C_EndingBeginingYears
                    .Where(x => x.C_CreateAccount_Table.CompanyID == companyID && x.C_AID == data.C_AID && x.YearID == yearID && x.Type == 2)
                    .ToList()
                    .Select(x => Convert.ToDecimal(x.Ballance)).Sum();
          
                debit = DB.C_GeneralLedger_Tables.ToList()
                    .Where(x => x.C_GeneralJournalEntry_Table.CompanyID == companyID && x.C_AID == data.C_AID && (Convert.ToDateTime(startDate) <= Convert.ToDateTime(x.C_GeneralJournalEntry_Table.C_PostingDate) && Convert.ToDateTime(endDate) >= Convert.ToDateTime(x.C_GeneralJournalEntry_Table.C_PostingDate)))
                    .ToList()
                    .Select(x => Convert.ToDecimal(x.C_Debit)).Sum();
                credit = DB.C_GeneralLedger_Tables.ToList()
                    .Where(x => x.C_GeneralJournalEntry_Table.CompanyID == companyID && x.C_AID == data.C_AID && (Convert.ToDateTime(startDate) <= Convert.ToDateTime(x.C_GeneralJournalEntry_Table.C_PostingDate) && Convert.ToDateTime(endDate) >= Convert.ToDateTime(x.C_GeneralJournalEntry_Table.C_PostingDate)))
                    .ToList()
                    .Select(x => Convert.ToDecimal(x.C_Credit)).Sum();
                AccountsData.Add(new AccountsData
                {
                    AccountID = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.C_AID == data.C_AID).Select(x => x.AccountID).FirstOrDefault(),
                    AccountName = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.C_AID == data.C_AID).Select(x => x.AccountName).FirstOrDefault(),
                    Sum_Debit = debit,
                    Sum_Credit = credit,
                    Net_Change = debit - credit,
                    Beginning_Balance = beginningBalance,
                    Ending_Balance = beginningBalance + (debit - credit)
                });
            }
            return Json(AccountsData, JsonRequestBehavior.AllowGet);
        }
        //-----------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------
        public ActionResult AccountDetails()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var getCompanyAccounts = DB.C_CreateAccount_Tables
                    .Where(x => x.CompanyID == companyID)
                    .Select(x => new { AID = x.C_AID, AccountName = x.AccountID + "-" + x.AccountName })
                    .ToList();
            SelectList accountsList = new SelectList(getCompanyAccounts, "AID", "AccountName");
            ViewBag.AccountsList = accountsList;

            var yearName = DB.NewFiscalYear_Table.Where(x => x.FiscalDefinition_Table.CompanyFiscalYear_Table.FirstOrDefault().CompanyID == companyID).ToList();
            SelectList YearList = new SelectList(yearName, "YearID", "Year");
            ViewBag.YearList = YearList;

            return View();
        }
        public JsonResult GetAccountName(int accountID)
        {
            var getName = DB.C_CreateAccount_Tables.Where(x => x.C_AID == accountID).FirstOrDefault();
            if (getName != null)
            {
                return Json(getName.AccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Get_All_Account_Details_Data(int accountID, int yearID)
        {
            var getDates = DB.NewFiscalYear_Table.Where(x => x.YearID == yearID).FirstOrDefault();
            return Json(GetSearchData(accountID, DateTime.Parse(getDates.Fiscal_Year_Start), DateTime.Parse(getDates.Fiscal_Year_End), yearID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_All_Account_Details_ByData(int accountID, int yearID, DateTime startDate, DateTime endDate)
        {
            var getDates = DB.NewFiscalYear_Table.Where(x => x.YearID == yearID).FirstOrDefault();
            if (startDate >= DateTime.Parse(getDates.Fiscal_Year_Start) && endDate <= DateTime.Parse(getDates.Fiscal_Year_End))
            {
                return Json(GetSearchData(accountID, startDate, endDate, yearID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("inValid", JsonRequestBehavior.AllowGet);
            }
        }
        private IEnumerable<AccountsData> GetSearchData(int AID, DateTime dateFrom, DateTime dateTo, int year)
        {
            return DB.C_GeneralLedger_Tables.ToList().Where(x => x.C_AID == AID && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) >= dateFrom && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) <= dateTo).OrderBy(x => DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate)).Select(x => new AccountsData
            {
                VoidJENum = x.C_GeneralJournalEntry_Table.VoidPostingNum,
                Date = x.C_GeneralJournalEntry_Table.C_PostingDate,
                JournalNumber = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber,
                PostingNumber = x.C_GeneralJournalEntry_Table.C_PostingNumber,
                Postingkey = x.C_GeneralJournalEntry_Table.C_PostingKey,
                JournalType = x.C_GeneralJournalEntry_Table.C_TransactionType,
                Description = x.C_Describtion,
                OriginalAmount = x.C_OriginalAmount,
                CurrencyID = x.C_GeneralJournalEntry_Table.CurrencyID,
                TransactionRate = x.C_GeneralJournalEntry_Table.C_TransactionRate,
                Sum_Debit = Convert.ToDecimal(x.C_Debit),
                Sum_Credit = Convert.ToDecimal(x.C_Credit),
                Document = x.C_Document
            }).ToList();
        }
        //-----------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------
        public ActionResult AvailableCash()
        {
            return View();
        }
        public PartialViewResult GetAvailableCashData(DateTime Date)
        {
            string companyID = Business.GetCompanyId(); //(string)FabulousErp.Business.GetCompanyId();

            List<DisplayAccountData> AccountsData1 = DB.C_CheckbookTransactions_Tables
                .OrderBy(x=> x.C_TransactionDate)
                .Where(x => x.CompanyID == companyID).ToList().Where(z => Convert.ToDateTime(z.C_TransactionDate) <= Date)
                .Select(x => new DisplayAccountData
                {
                    Description = x.C_CheckBookSetting_Table.C_CheckbookName,
                    Currency = x.CurrenciesDefinition_Table.ISOCode,
                    Orginal_amount = x.C_Balance,
                    Rate = x.C_TransactionRate,
                    Balance = x.C_Balance * Convert.ToDecimal(x.C_TransactionRate),
                    CheckBookId=x.C_CBSID
                }
              ).ToList();
            List<DisplayAccountDataWithSum> AccountsData2 = new List<DisplayAccountDataWithSum>();
            foreach (var item in AccountsData1.GroupBy(x => x.Description))
            {
                AccountsData2.Add(new DisplayAccountDataWithSum
                {
                    Data = item.ToList(),
                    Sum = item.Sum(x => x.Balance)
                });
            }
            return PartialView(AccountsData2);
        }
        public JsonResult Get_Available_Cash_Data(string date)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            decimal totalBalance = 0;
            if (DB.C_CheckbookTransactions_Tables.Where(x => x.CompanyID == companyID && x.C_CheckBookSetting_Table.C_CBSID == 4).Any())
            {
                //    totalBalance = DB.C_CheckbookTransactions_Tables
                //        .ToList()
                //        .Select(x => x.C_Balance);
            }

            List<AccountsData> AccountsData1 = DB.C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Cash").Select(x => new AccountsData
                {
                    Description = "Cash",
                    Checkbook_Name = x.C_CheckbookName,
                    IsoCode = x.CurrenciesDefinition_Table.ISOCode
                }).ToList();

            List<AccountsData> AccountsData2 = DB.C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Bank").Select(x => new AccountsData
                {
                    Description = "Bank",
                    Checkbook_Name = x.C_CheckbookName,
                    IsoCode = x.CurrenciesDefinition_Table.ISOCode
                }).ToList();

            List<AccountsData> AccountsData3 = DB.C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Check").Select(x => new AccountsData
                {
                    Description = "Check",
                    Checkbook_Name = x.C_CheckbookName,
                    IsoCode = x.CurrenciesDefinition_Table.ISOCode
                }).ToList();

            List<AccountsData> AccountsData = AccountsData1.Union(AccountsData2).Union(AccountsData3).ToList();
            return Json(AccountsData, JsonRequestBehavior.AllowGet);
        }
        //-----------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------
        //public PartialViewResult ReportsPrintHeader()
        //{
        //    string companyID = (string)FabulousErp.Business.GetCompanyId();
        //    string userID = FabulousErp.Business.GetUserId();
        //    var data = RSelectBusiness.DBC().CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
        //    if (data != null)
        //    {
        //        var logoPath = "";
        //        if (data.LogoByte != null)
        //        {
        //            var base64 = Convert.ToBase64String(data.LogoByte);
        //            logoPath = string.Format("data:image/gif;base64,{0}", base64);
        //        }
        //        ViewBag.CompanyName = data.CompanyName;
        //        ViewBag.UserID = userID;
        //        ViewBag.DateTime = DateTime.Now;
        //        ViewBag.Logo = logoPath;
        //    }
        //    var data2 = RSelectBusiness.DBC().CreateAccount_Tables.Where(x => x.UserID == userID).FirstOrDefault();
        //    if (data2 != null)
        //    {
        //        ViewBag.UserName = data2.UserName;
        //    }
        //    return PartialView();
        //}
        //public PartialViewResult ReportsPrintFooter()
        //{
        //    string companyID = (string)FabulousErp.Business.GetCompanyId();
        //    var data = RSelectBusiness.DBC().CompanyAddressInfo_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
        //    if (data != null)
        //    {
        //        ViewBag.BuildingNo = data.BuldingNo;
        //        ViewBag.StreetName = data.StreetName;
        //        ViewBag.City = data.City;
        //        ViewBag.Governorate = data.Governorate;
        //    }
        //    return PartialView();
        //}
        //public ActionResult PrintReport(string transactionDate)
        //{
        //    ViewBag.transactionDate = transactionDate;
        //    return PartialView("GeneralEntry_Print");
        //}
        //-----------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------




    }




    public class DisplayAccountData
    {
        public int? CheckBookId { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public double? Rate { get; set; }
        public decimal Orginal_amount { get; set; }
    }
    public class DisplayAccountDataWithSum
    {
        public List<DisplayAccountData> Data { get; set; }
        public decimal Sum { get; set; }
    }
    public class AccountsData
    {
        public int C_AID { get; set; }
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public decimal Sum_Debit { get; set; }
        public decimal Sum_Credit { get; set; }
        public decimal Net_Change { get; set; }
        public decimal Beginning_Balance { get; set; }
        public decimal Ending_Balance { get; set; }
        public int? VoidJENum { get; set; }
        public string Date { get; set; }
        public int JournalNumber { get; set; }
        public int PostingNumber { get; set; }
        public string Postingkey { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }
        public double OriginalAmount { get; set; }
        public string CurrencyID { get; set; }
        public string IsoCode { get; set; }
        public double TransactionRate { get; set; }
        public string Document { get; set; }
        public string JournalType { get; set; }
        public decimal Balance { get; set; }
        public string Checkbook_Name { get; set; }
    }
    public class PrintData
    {
        public string CompanyID { get; set; }
        public int UserID { get; set; }
        public DateTime dateTime { get; set; }
    }
}