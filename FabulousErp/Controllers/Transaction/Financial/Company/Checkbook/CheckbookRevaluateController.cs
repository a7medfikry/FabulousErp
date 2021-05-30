using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Transaction.Financial;
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
    public class CheckbookRevaluateController : Controller
    {
        DBContext DB = new DBContext();

        // GET: CheckbookRevaluate
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TCBR")]
        public ActionResult Revaluate()
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

            var checkCurrencyFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }

            var accountsList = DB.C_CreateAccount_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { AccountID = x.AccountID + " - " + x.AccountName, x.C_AID })
                .ToList();
            SelectList dataaa = new SelectList(accountsList, "C_AID", "AccountID");
            ViewBag.AccountsList = dataaa;
            return View();
        }

        public JsonResult GetRevaluateData(string date)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var checkbookTransactions_DTOs = DB.C_CheckbookTransactions_Tables.ToList()
                .Where(x => x.CompanyID == companyID && Convert.ToDateTime(x.C_PostingDate) <= Convert.ToDateTime(date) && x.CurrencyID != companyID)
                .OrderBy(x => x.C_PostingDate).ToList();
            List<CheckbookTransactions_DTO> checkbookTransactions_DTO = new List<CheckbookTransactions_DTO>();
            foreach (var data in checkbookTransactions_DTOs.GroupBy(x => x.C_CBSID))
            {
                double result = 0;
                foreach (var ballanceData in data)
                {
                    result += ballanceData.C_GeneralJournalEntry_Table.C_GeneralLedger_Tables.Where(x => x.C_AID == ballanceData.C_CheckBookSetting_Table.C_AID).Sum(x => x.Ballance);
                }
                checkbookTransactions_DTO.Add(new CheckbookTransactions_DTO
                {
                    Checkbook_ID = data.FirstOrDefault().C_CheckBookSetting_Table.C_CheckbookID,
                    C_CBSID = data.FirstOrDefault().C_CBSID,
                    C_AID = data.FirstOrDefault().C_CheckBookSetting_Table.C_AID,
                    CurrencyID = data.FirstOrDefault().CurrencyID,
                    CurrencyName = data.FirstOrDefault().C_CheckBookSetting_Table.CurrenciesDefinition_Table.CurrencyName,
                    Balance = data.Sum(x => x.C_Balance),
                    CashAccountBalance2 = result
                });
            }
            return Json(checkbookTransactions_DTO, JsonRequestBehavior.AllowGet);
        }








    }
}