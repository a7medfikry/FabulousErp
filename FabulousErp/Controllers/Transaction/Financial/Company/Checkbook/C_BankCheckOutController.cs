using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Transaction.Financial.Company.Checkbook
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_BankCheckOutController : Controller
    {
        DBContext DB = new DBContext();

        // GET: C_BankCheckOut
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TCBC")]
        public ActionResult CompanyBankCheckOut()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var SPSCheck =  Business.GetPostingSetup();
            if (SPSCheck != null)
            {
                ViewBag.EPD = SPSCheck.EditPostingDate;
                ViewBag.PT = SPSCheck.PostingType;
            }
            else
            {
                ViewBag.FJEPer = "NoPS";
            }

            var FiscalyearCheck = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (FiscalyearCheck != null)
            {
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == FiscalyearCheck.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
                if (checkYearOpen != null)
                {
                    ViewBag.CheckYear = "Exist";
                }
            }

            var checkEditTransactionRate = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == "Financial" && x.TransactionFormName == "Company Cash Reciept").FirstOrDefault();
            if (checkEditTransactionRate != null)
            {
                if (checkEditTransactionRate.AllowUserE == true)
                {
                    ViewBag.AllowUserERate = "True";
                }
            }

            var Checkbooklist = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && (x.C_CheckbookType == "Bank" || x.C_CheckbookType == "Check")).ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            var checkCurrencyFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }
            return View();
        }
    }
}