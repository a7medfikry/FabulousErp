using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.Repository.Business;
using FabulousModels.DTOModels.Transaction.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.Models;

namespace FabulousErp.Controllers.Transaction.Financial.Company.Checkbook
{
    public class C_CheckTransferController : Controller
    {
        DBContext DB = new DBContext();

        // GET: C_CheckTransfer
        public ActionResult CompanyCheckTransfer()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var Checkbooklist = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Check")
                .ToList();
            SelectList CheckCheckbookID = new SelectList(Checkbooklist, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckCheckbookID = CheckCheckbookID;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Bank")
                .ToList();
            SelectList BankCheckbookID = new SelectList(Checkbooklist2, "C_CBSID", "C_CheckbookID");
            ViewBag.BankCheckbookID = BankCheckbookID;

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

            var checkCurrencyFormate = RSelectBusiness.DBC().FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }

            var accountsData = RSelectBusiness.DBC().C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            var transferNumber = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCT")
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

            return View();
        }

        public JsonResult GetCheckData(int checkCheckBookID, string cutDate,bool IsRpt=false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            
            List<CheckbookTransactions_DTO> CheckbookTransactions_DTO = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .OrderBy(x => x.C_DueDate)
                .ToList()
                .Where(x => x.CompanyID == companyID && x.C_CBSID == checkCheckBookID && x.C_Reconcile == false && x.C_Reciept > 0 && Convert.ToDateTime(x.C_DueDate) <= Convert.ToDateTime(cutDate))
                .Select(x => new CheckbookTransactions_DTO
                {
                    CheckbookName = x.C_CheckBookSetting_Table.C_CheckbookName,
                    CheckNumber = x.C_CheckNumber,
                    Date = x.C_DueDate,
                    Balance = x.C_Reciept,
                    RecievedFrom = x.C_Payment_To_Recieved_From,
                    CurrencyID = x.C_CheckBookSetting_Table.CurrencyID,
                    C_AID = x.C_CheckBookSetting_Table.C_AID,
                    AccountID = x.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountID,
                    AccountName = x.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountName,
                    Refrence = x.C_Reference,
                    
                }).ToList();
            if (IsRpt)
            {
                CheckbookTransactions_DTO = CheckbookTransactions_DTO.Where(x => x.Refrence == "Transfer Check To Bank").ToList();
            }
            return Json(CheckbookTransactions_DTO, JsonRequestBehavior.AllowGet);
        }   
        public JsonResult GetCheckDataByPo(int Po)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            
            List<CheckbookTransactions_DTO> CheckbookTransactions_DTO = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .OrderBy(x => x.C_DueDate)
                .ToList()
                .Where(x => x.CompanyID == companyID &&x.C_PostingNumber==Po)
                .Select(x => new CheckbookTransactions_DTO
                {
                    CheckbookName = x.C_CheckBookSetting_Table.C_CheckbookName,
                    CheckNumber = x.C_CheckNumber,
                    Date = x.C_DueDate,
                    Balance = x.C_Reciept,
                    RecievedFrom = x.C_Payment_To_Recieved_From,
                    CurrencyID = x.C_CheckBookSetting_Table.CurrencyID,
                    C_AID = x.C_CheckBookSetting_Table.C_AID,
                    AccountID = x.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountID,
                    AccountName = x.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountName,
                    Refrence = x.C_Reference,
                }).ToList();
           
            return Json(CheckbookTransactions_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBankData(int bankCheckBookID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var data = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CBSID == bankCheckBookID).FirstOrDefault();
            if (data != null)
            {
                CheckbookTransactions_DTO CheckbookTransactions_DTO = new CheckbookTransactions_DTO
                {
                    CheckbookName = data.C_CheckbookName,
                    CurrencyID = data.CurrencyID,
                    C_AID = data.C_CreateAccount_Table.C_AID,
                    AccountID = data.C_CreateAccount_Table.AccountID,
                    AccountName = data.C_CreateAccount_Table.AccountName
                };
                return Json(CheckbookTransactions_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult CheckTransfer(List<C_CheckbookTransactions_table> transferArray)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            string UserID = FabulousErp.Business.GetUserId();

            int transferNumber = 1;
            var check = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCT")
                .OrderByDescending(x => x.C_DocumentNumber)
                .FirstOrDefault();
            if (check != null)
            {
                transferNumber = check.C_DocumentNumber + 1;
            }
            foreach (var data in transferArray)
            {
                //string Ref = DB.C_CheckbookTransactions_Tables
                //    .Where(x =>!string.IsNullOrEmpty(x.C_CheckNumber)&& x.C_CheckNumber == data.C_CheckNumber && x.C_Payment_To_Recieved_From == data.C_Payment_To_Recieved_From)
                //    .ToList().DefaultIfEmpty(new C_CheckbookTransactions_table { C_Reference = data.C_Reference })
                //    .FirstOrDefault().C_Reference;
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
                    C_Payment_To_Recieved_From = data.C_Payment_To_Recieved_From,
                    C_Reconcile = false,

                    UserID = UserID,
                    C_DateTime = DateTime.Now
                };
                DB.C_CheckbookTransactions_Tables.Add(c_CheckbookTransactions_Table);
            }
            DB.SaveChanges();
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateReconcile(List<ReconcileData> reconcileArray)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            foreach (var data in reconcileArray)
            {
                var item = DB.C_CheckbookTransactions_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == data.C_CBSID && x.C_CheckNumber == data.C_CheckNumber).FirstOrDefault();

                item.C_Reconcile = true;
                DB.SaveChanges();
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }

    }





    public class ReconcileData
    {
        public int C_CBSID { get; set; }
        public string C_CheckNumber { get; set; }
        public int ToC_CBSID { get; set; }
    }

}