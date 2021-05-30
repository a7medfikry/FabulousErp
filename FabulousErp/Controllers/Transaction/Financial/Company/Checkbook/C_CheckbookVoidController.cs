using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.Repository.Business;
using FabulousModels.DTOModels.Transaction.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Transaction.Financial.Company.Checkbook
{
    public class C_CheckbookVoidController : Controller
    {
        DBContext DB = new DBContext();

        // GET: C_CheckbookVoid
        public ActionResult Cash()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var detectJEPer = Business.GetPostingSetup();
            if (detectJEPer != null)
            {
                ViewBag.PT = detectJEPer.PostingType;
                ViewBag.EPD = detectJEPer.EditPostingDate;
            }
            else
            {
                ViewBag.CheckPostingSetup = "NoPS";
            }

            var getFiscalYearOfComp = RSelectBusiness.DBC().CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = RSelectBusiness.DBC().NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
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

            var postedJENum = RSelectBusiness.DBC().C_GeneralJournalEntry_Tables.Where(x => x.Post == true && x.C_PostingKey == "TCGE" && x.VoidPostingNum == null).Select(x => new { PostingNumber = x.C_PostingNumber, JENumber = x.C_JournalEntryNumber + " - " + x.C_PostingKey });
            SelectList JENumList = new SelectList(postedJENum, "PostingNumber", "JENumber");
            ViewBag.JENumList = JENumList;

            var Checkbooklist = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_Reconcile == false && x.C_CBTVoid == null && x.C_GeneralJournalEntry_Table.Post == true && (x.C_PostingKey == "TCCR" || x.C_PostingKey == "TCCW"))
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber + " - " + x.C_PostingKey, x.C_PostingNumber }).ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
                .ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist2, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            var getCurrencyID = RSelectBusiness.DBC().CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }

            return View();
        }


        public ActionResult Bank()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            
            var detectJEPer = Business.GetPostingSetup();
            if (detectJEPer != null)
            {
                ViewBag.PT = detectJEPer.PostingType;
                ViewBag.EPD = detectJEPer.EditPostingDate;
            }
            else
            {
                ViewBag.CheckPostingSetup = "NoPS";
            }

            var getFiscalYearOfComp = RSelectBusiness.DBC().CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = RSelectBusiness.DBC().NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
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

            var postedJENum = RSelectBusiness.DBC().C_GeneralJournalEntry_Tables.Where(x => x.Post == true && x.C_PostingKey == "TCGE" && x.VoidPostingNum == null).Select(x => new { PostingNumber = x.C_PostingNumber, JENumber = x.C_JournalEntryNumber + " - " + x.C_PostingKey });
            SelectList JENumList = new SelectList(postedJENum, "PostingNumber", "JENumber");
            ViewBag.JENumList = JENumList;

            var Checkbooklist = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_Reconcile == false && x.C_CBTVoid == null && x.C_GeneralJournalEntry_Table.Post == true && (x.C_PostingKey == "TCBR" || x.C_PostingKey == "TCBC"))
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber + " - " + x.C_PostingKey, x.C_PostingNumber }).ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
                .ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist2, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            var getCurrencyID = RSelectBusiness.DBC().CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }

            return View();
        }


        public ActionResult Transfer()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var detectJEPer = Business.GetPostingSetup();
            if (detectJEPer != null)
            {
                ViewBag.PT = detectJEPer.PostingType;
                ViewBag.EPD = detectJEPer.EditPostingDate;
            }
            else
            {
                ViewBag.CheckPostingSetup = "NoPS";
            }

            var getFiscalYearOfComp = RSelectBusiness.DBC().CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = RSelectBusiness.DBC().NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
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

            var postedJENum = RSelectBusiness.DBC().C_GeneralJournalEntry_Tables.Where(x => x.Post == true && x.C_PostingKey == "TCGE" && x.VoidPostingNum == null).Select(x => new { PostingNumber = x.C_PostingNumber, JENumber = x.C_JournalEntryNumber + " - " + x.C_PostingKey });
            SelectList JENumList = new SelectList(postedJENum, "PostingNumber", "JENumber");
            ViewBag.JENumList = JENumList;

            var Checkbooklist = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_Reconcile == false && x.C_CBTVoid == null && x.C_GeneralJournalEntry_Table.Post == true && x.C_PostingKey == "TCBT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber + " - " + x.C_PostingKey, x.C_PostingNumber }).Distinct().ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;

            return View();
        }


        public ActionResult Check()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var detectJEPer = Business.GetPostingSetup();
            if (detectJEPer != null)
            {
                ViewBag.PT = detectJEPer.PostingType;
                ViewBag.EPD = detectJEPer.EditPostingDate;
            }
            else
            {
                ViewBag.CheckPostingSetup = "NoPS";
            }

            var getFiscalYearOfComp = RSelectBusiness.DBC().CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = RSelectBusiness.DBC().NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
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


            var Checkbooklist = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_Reconcile == false && x.C_CBTVoid == null && x.C_GeneralJournalEntry_Table.Post == true && x.C_PostingKey == "TCT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_DocumentNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
                .ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist2, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            var getCurrencyID = RSelectBusiness.DBC().CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }

            return View();
        }
        public JsonResult GetCheckData(int jvNumber)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<CheckbookTransactions_DTO> CheckbookTransactions_DTO = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingNumber == jvNumber && x.C_PostingKey == "TCT" && x.C_Reciept > 0)
                .Select(x => new CheckbookTransactions_DTO
                {
                    CheckNumber = x.C_CheckNumber,
                    Date = x.C_DueDate,
                    Balance = x.C_Reciept,
                    RecievedFrom = x.C_Payment_To_Recieved_From,
                    CurrencyID = x.C_CheckBookSetting_Table.CurrencyID,
                    C_AID = x.C_CheckBookSetting_Table.C_AID,
                    AccountID = x.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountID,
                    AccountName = x.C_CheckBookSetting_Table.C_CreateAccount_Table.AccountName
                }).ToList();
            return Json(CheckbookTransactions_DTO, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult UpdateVoidReconcile(List<VoidReconcileData> reconcileArray)
        //{
        //    string companyID = (string)FabulousErp.Business.GetCompanyId();
        //    foreach (var data in reconcileArray)
        //    {
        //        var item = DB.C_CheckbookTransactions_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == data.C_CBSID && x.C_CheckNumber == data.C_CheckNumber).FirstOrDefault();
        //        item.C_Reconcile = false;
        //        DB.SaveChanges();
        //    }
        //    return Json("True", JsonRequestBehavior.AllowGet);
        //}
        public JsonResult CheckVoidTransfer(List<C_CheckbookTransactions_table> transferArray)
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
                    C_CBTVoid = data.C_CBTVoid,

                    UserID = UserID,
                    C_DateTime = DateTime.Now
                };
                DB.C_CheckbookTransactions_Tables.Add(c_CheckbookTransactions_Table);

                var dataa = DB.C_CheckbookTransactions_Tables.Where(x => x.C_CBT == data.C_CBTVoid).FirstOrDefault();
                dataa.C_CBTVoid = c_CheckbookTransactions_Table.C_CBT;
                DB.SaveChanges();
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }



        public PartialViewResult TransferData()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
                .ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist2, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            var getCurrencyID = RSelectBusiness.DBC().CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }

            return PartialView();
        }
        public JsonResult TransferVoid(List<C_CheckbookTransactions_table> transferArray)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            string UserID = FabulousErp.Business.GetUserId();
            int documentNumber = 0;

            var newDocumentNumber = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                    .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCVC")
                    .OrderByDescending(x => x.C_DocumentNumber)
                    .FirstOrDefault();
            if (newDocumentNumber != null)
            {
                documentNumber = newDocumentNumber.C_DocumentNumber + 1;
            }
            else
            {
                documentNumber = 1;
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
                    C_DocumentNumber = documentNumber,
                    C_CheckNumber = data.C_CheckNumber,
                    C_Reconcile = false,
                    C_CBTVoid = data.C_CBTVoid,

                    UserID = UserID,
                    C_DateTime = DateTime.Now
                };
                DB.C_CheckbookTransactions_Tables.Add(c_CheckbookTransactions_Table);

                var dataa = DB.C_CheckbookTransactions_Tables.Where(x => x.C_CBT == data.C_CBTVoid).FirstOrDefault();
                dataa.C_CBTVoid = c_CheckbookTransactions_Table.C_CBT;
                DB.SaveChanges();

            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }

    }
    public class VoidReconcileData
    {
        public int C_CBSID { get; set; }
        public int C_CheckNumber { get; set; }
    }



}