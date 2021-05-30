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
    public class BankReconcileController : Controller
    {
        DBContext DB = new DBContext();

        // GET: BankReconcile
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TBR")]
        public ActionResult Index()
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

            var AccountsList = RSelectBusiness.DBC().C_CreateAccount_Tables
                .Where(x => x.CompanyID == companyID && x.ReconcileAccount == false && x.ConsolidationAccount == false && (x.C_AnalyticAccountID == null || x.C_AnalyticAccountID == "") && (x.C_CostCenterID == null || x.C_CostCenterID == ""))
                .Select(x => new { AccountID = x.AccountID + " - " + x.AccountName, x.C_AID })
                .ToList();

            SelectList AccountsID = new SelectList(AccountsList, "C_AID", "AccountID");
            ViewBag.AccountsID = AccountsID;

            var Checkbooklist = RSelectBusiness.DBC().C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID&&x.C_CheckbookType== "Bank")
                .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
                .ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            var ReconcileNumber = RSelectBusiness.DBC().C_BankReconcile_Tables
                .Where(x => x.CompanyID == companyID)
                .OrderByDescending(x => x.BankReconcile_Number)
                .FirstOrDefault();
            if (ReconcileNumber != null)
            {
                ViewBag.ReconcileNumber = ReconcileNumber.BankReconcile_Number + 1;
            }
            else
            {
                ViewBag.ReconcileNumber = 1;
            }

            var ReconcileList = RSelectBusiness.DBC().C_BankReconcile_Tables
                .Where(x => x.CompanyID == companyID && x.Reconciled == null)
                .ToList();

            SelectList BankReconcileNumbers = new SelectList(ReconcileList, "BankReconcile_Number", "BankReconcile_Number");
            ViewBag.BankReconcileNumbers = BankReconcileNumbers;

            return View();
        }

        public JsonResult SaveBankReconcile(int bankReconcileNumber, int checkbookID, decimal bankEndingBalance, string bankEndingDate, string bookEndingDate)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            string UserID = FabulousErp.Business.GetUserId();
            C_BankReconcile_table c_BankReconcile_Table = new C_BankReconcile_table()
            {
                CompanyID = companyID,
                BankReconcile_Number = bankReconcileNumber,
                C_CBSID = checkbookID,
                Bank_Statment_Ending_Balance = bankEndingBalance,
                Bank_Statment_Ending_Date = bankEndingDate,
                Book_Statment_Ending_Date = bookEndingDate,

                UserID = UserID,
                BankReconcile_DateTime = DateTime.Now,
            };
            DB.C_BankReconcile_Tables.Add(c_BankReconcile_Table);
            DB.SaveChanges();
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransactionData(int checkbookID, string bookEndingDate, int sortType)
        {
            return Json(RecnocileTransaction(checkbookID, bookEndingDate, sortType), JsonRequestBehavior.AllowGet);
        }

        public List<CheckbookTransactions_DTO> RecnocileTransaction(int checkbookID, string bookEndingDate, int sortType)
        {
            string companyID = Business.GetCompanyId();
            if (sortType == 1)
            {
                List<CheckbookTransactions_DTO> checkbookTransactions_DTOs = DB.C_CheckbookTransactions_Tables.ToList()
                    .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && x.C_Reconcile == false && Convert.ToDateTime(x.C_PostingDate) <= Convert.ToDateTime(bookEndingDate))
                    .OrderBy(x => x.C_DueDate)
                    .Select(x => new CheckbookTransactions_DTO
                    {
                        C_CBT = x.C_CBT,
                        DocumentType = x.C_PostingKey,
                        DocumentNumber = x.C_DocumentNumber,
                        CheckNumber = x.C_CheckNumber,
                        Date = x.C_DueDate,
                        Payment = x.C_Payment,
                        Deposit = x.C_Reciept,
                        PostingKey=x.C_PostingKey
                    }).ToList();
                return checkbookTransactions_DTOs;
            }
            else if (sortType == 2)
            {
                List<CheckbookTransactions_DTO> checkbookTransactions_DTOs = DB.C_CheckbookTransactions_Tables.ToList()
                    .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && x.C_Reconcile == false && Convert.ToDateTime(x.C_PostingDate) <= Convert.ToDateTime(bookEndingDate))
                    .OrderByDescending(x => x.C_Reciept)
                    .Select(x => new CheckbookTransactions_DTO
                    {
                        C_CBT = x.C_CBT,
                        DocumentType = x.C_DocumentType,
                        DocumentNumber = x.C_DocumentNumber,
                        CheckNumber = x.C_CheckNumber,
                        Date = x.C_DueDate,
                        Payment = x.C_Payment,
                        Deposit = x.C_Reciept,
                        PostingKey=x.C_PostingKey
                    }).ToList();
                return checkbookTransactions_DTOs;
            }
            else if (sortType == 3)
            {
                List<CheckbookTransactions_DTO> checkbookTransactions_DTOs = DB.C_CheckbookTransactions_Tables.ToList()
                  .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && x.C_Reconcile != true && Convert.ToDateTime(x.C_PostingDate) <= Convert.ToDateTime(bookEndingDate))
                  .OrderByDescending(x => x.C_DueDate)
                  .Select(x => new CheckbookTransactions_DTO
                  {
                      C_CBT = x.C_CBT,
                      DocumentType = x.C_DocumentType,
                      DocumentNumber = x.C_DocumentNumber,
                      CheckNumber = x.C_CheckNumber,
                      Date = x.C_DueDate,
                      Payment = x.C_Payment,
                      Deposit = x.C_Reciept,
                      PostingKey=x.C_PostingKey
                  }).ToList();
                return checkbookTransactions_DTOs;
            }
            else
            {
                return new List<CheckbookTransactions_DTO>
                {
                };
            }
        }

        public JsonResult UpdateReconcile(int checkbookID, int rowID, bool? reconcileStatus, int? reconcileNumber)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            if (reconcileStatus == false)
            {
                reconcileNumber = null;
            }
            var check = DB.C_CheckbookTransactions_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && x.C_CBT == rowID).FirstOrDefault();
            if (check != null)
            {
                check.C_ReconcileNumber = reconcileNumber;
                check.C_Reconcile = reconcileStatus;
                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SearchReconcile(int bankReconcileNumber)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var data = DB.C_BankReconcile_Tables.Where(x => x.CompanyID == companyID && x.BankReconcile_Number == bankReconcileNumber).FirstOrDefault();
            if (data != null)
            {
                CheckbookTransactions_DTO checkbookTransactions_DTO = new CheckbookTransactions_DTO()
                {
                    C_CBSID = data.C_CBSID,
                    CheckbookName = data.C_CheckBookSetting_Table.C_CheckbookName,
                    CurrencyID = data.C_CheckBookSetting_Table.CurrencyID,
                    CurrencyName = data.C_CheckBookSetting_Table.CurrenciesDefinition_Table.CurrencyName,
                    BankReconcile_Number = data.BankReconcile_Number,
                    Bank_Statment_Ending_Balance = data.Bank_Statment_Ending_Balance,
                    Bank_Statment_Ending_Date = data.Bank_Statment_Ending_Date,
                    Book_Statment_Ending_Date = data.Book_Statment_Ending_Date,
                    C_AID = data.C_CheckBookSetting_Table.C_AID,
                    Company_AccountsID = data.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountID,
                    Company_AccountsName = data.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountName,
                    Date = data.BankReconcile_DateTime.ToShortDateString()
                };
                return Json(checkbookTransactions_DTO, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return null;
            }
        }

        public JsonResult GetReconcileTable(int checkbookID, int bankReconcileNumber, string bookEndingDate)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<CheckbookTransactions_DTO> checkbookTransactions_DTOs = DB.C_CheckbookTransactions_Tables.ToList()
                .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && Convert.ToDateTime(x.C_PostingDate) <= Convert.ToDateTime(bookEndingDate) && (x.C_ReconcileNumber == bankReconcileNumber || x.C_ReconcileNumber == null))
                .OrderBy(x => x.C_PostingDate)
                .Select(x => new CheckbookTransactions_DTO
                {
                    C_CBT = x.C_CBT,
                    DocumentType = x.C_PostingKey,
                    DocumentNumber = x.C_DocumentNumber,
                    CheckNumber = x.C_CheckNumber,
                    Date = x.C_DueDate,
                    Payment = x.C_Payment,
                    Deposit = x.C_Reciept,
                    ReconcileStatus = x.C_Reconcile
                }).ToList();
            return Json(checkbookTransactions_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBookEndingDateBalance(int checkbookID, string bookStatementEndingDate)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            try
            {
                decimal totalBalance = 0;
                if (DB.C_CheckbookTransactions_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID).Any())
                {
                    totalBalance = DB.C_CheckbookTransactions_Tables.ToList()
                    .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && Convert.ToDateTime(x.C_PostingDate) <= Convert.ToDateTime(bookStatementEndingDate))
                    .OrderBy(x => x.C_PostingDate)
                    .Select(x => x.C_Balance).Sum();
                }
                return Json(totalBalance, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult ReconcileChecked(int bankReconcileNumber)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var check = DB.C_BankReconcile_Tables.Where(x => x.CompanyID == companyID && x.BankReconcile_Number == bankReconcileNumber).FirstOrDefault();
            if (check != null)
            {
                check.Reconciled = true;
                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckbookAdjustment(List<C_CheckbookTransactions_table> reconcileArr)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            string UserID = FabulousErp.Business.GetUserId();

            foreach (var data in reconcileArr)
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
                    C_Reciept = data.C_Reciept,
                    C_Payment = data.C_Payment,
                    C_Balance = data.C_Balance,
                    C_PostingKey = data.C_PostingKey,
                    C_DocumentNumber = data.C_DocumentNumber,
                    C_DocumentType = data.C_DocumentType,
                    C_Payment_To_Recieved_From = data.C_Payment_To_Recieved_From,
                    C_Reconcile = true,
                    C_ReconcileNumber = data.C_ReconcileNumber,

                    UserID = UserID,
                    C_DateTime = DateTime.Now
                };
                DB.C_CheckbookTransactions_Tables.Add(c_CheckbookTransactions_Table);
            }
            DB.SaveChanges();
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteBankReconcile(int bankReconcileNumber)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var check = DB.C_CheckbookTransactions_Tables.FirstOrDefault(x => x.C_ReconcileNumber == bankReconcileNumber);
            if (check == null)
            {
                var item = DB.C_BankReconcile_Tables.Where(x => x.CompanyID == companyID && x.BankReconcile_Number == bankReconcileNumber).FirstOrDefault();
                DB.C_BankReconcile_Tables.Remove(item);
                DB.SaveChanges();
                return Json("NotFound", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Found", JsonRequestBehavior.AllowGet);
            }
        }






    }
}


