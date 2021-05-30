using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Business;
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
    public class CheckbookTransferController : Controller
    {
        DBContext DB = new DBContext();

        // GET: CheckbookTransfer
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TCBT")]
        public ActionResult Index()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var accountsList = RSelectBusiness.DBC().C_CreateAccount_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { AccountID = x.AccountID + " - " + x.AccountName, x.C_AID })
                .ToList();
            SelectList dataaa = new SelectList(accountsList, "C_AID", "AccountID");
            ViewBag.AccountsList = dataaa;

            return View();
        }

        public PartialViewResult CheckbookData()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var Checkbooklist = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
                .ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            var SPSCheck = Business.GetPostingSetup();
            if (SPSCheck != null)
            {
                ViewBag.EPD = SPSCheck.EditPostingDate;
                ViewBag.PT = SPSCheck.PostingType;
            }
            else
            {
                ViewBag.FJEPer = "NoPS";
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

            var checkEditTransactionRate = RSelectBusiness.DBC().C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == "Financial" && x.TransactionFormName == "Company Cash Reciept").FirstOrDefault();
            if (checkEditTransactionRate != null)
            {
                if (checkEditTransactionRate.AllowUserE == true)
                {
                    ViewBag.AllowUserERate = "True";
                }
            }

            var checkCurrencyFormate = RSelectBusiness.DBC().FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }

            var transferNumber = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCBT")
                .OrderByDescending(x => x.C_DocumentNumber)
                .FirstOrDefault();
            if (transferNumber != null)
            {
                ViewBag.TransferNumber = transferNumber.C_DocumentNumber + 1;
            }
            else
            {
                ViewBag.TransferNumber = 1;
            }

            return PartialView();
        }

        public JsonResult Transfer(List<C_CheckbookTransactions_table> transferArray)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            string UserID = FabulousErp.Business.GetUserId();

            int transferNumber = 1;
            var check = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCBT")
                .OrderByDescending(x => x.C_DocumentNumber)
                .FirstOrDefault();
            if (check != null)
            {
                transferNumber = check.C_DocumentNumber + 1;
            }
            foreach (var data in transferArray)
            {
                C_CheckbookTransactions_table c_CheckbookTransactions_Table = new C_CheckbookTransactions_table()
                {
                    CompanyID = companyID,
                    C_PostingNumber = data.C_PostingNumber,
                    C_CBSID = data.C_CBSID,
                    CurrencyID = data.CurrencyID,
                    C_TransactionDate = data.C_TransactionDate,
                    C_PostingDate = data.C_PostingDate,
                    C_DueDate = data.C_PostingDate,
                    C_SystemRate = data.C_SystemRate,
                    C_TransactionRate = data.C_TransactionRate,
                    C_Difference = data.C_Difference,
                    C_Reference = data.C_Reference,
                    C_DocumentType = data.C_DocumentType,
                    C_Reciept = data.C_Reciept,
                    C_Payment = data.C_Payment,
                    C_Balance = data.C_Balance,
                    C_PostingKey = data.C_PostingKey,
                    C_DocumentNumber = transferNumber,
                    C_CheckNumber = data.C_CheckNumber,
                    C_Reconcile = false,

                    UserID = UserID,
                    C_DateTime = DateTime.Now
                };
                DB.C_CheckbookTransactions_Tables.Add(c_CheckbookTransactions_Table);
            }
            DB.SaveChanges();
            return Json("True", JsonRequestBehavior.AllowGet);
        }






    }
}