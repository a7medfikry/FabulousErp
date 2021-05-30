using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Business;
using FabulousModels.DTOModels.Transaction.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CheckbookTransactionsController : Controller
    {
        DBContext DB = new DBContext();

        // GET: Inquiry_CheckbookTransactions
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICBT")]
        public ActionResult Cash()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var checkbookList = DB.C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
                .ToList();
            SelectList CheckbookID = new SelectList(checkbookList, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;
            return View();
        }
        public JsonResult CheckbookData(int checkbookID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            decimal totalBalance = 0;
            decimal cashAccountBalance = 0;
            var data = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID).FirstOrDefault();
            int accountID = data.C_AID;
            cashAccountBalance = DB.C_GeneralLedger_Tables
               .Where(x => x.C_AID == accountID).ToList()
               .Select(x => Convert.ToDecimal(x.Ballance)).Sum();

            if (DB.C_CheckbookTransactions_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID).Any())
            {
                totalBalance = DB.C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID)
                .Select(x => x.C_Balance)
                .Sum();
            }
            CheckbookTransactions_DTO checkbookTransactions_DTOs = new CheckbookTransactions_DTO()
            {
                CheckbookName = data.C_CheckbookName,
                CurrencyName = data.CurrenciesDefinition_Table.ISOCode,
                Balance = totalBalance,
                CashAccountBalance = cashAccountBalance
            };
            return Json(checkbookTransactions_DTOs, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllDate(int checkbookID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<CheckbookTransactions_DTO> checkbookTransactions_DTOs = new List<CheckbookTransactions_DTO>();
            var
                data = DB.C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID)
                .ToList();
            data.RemoveAll(x => x.C_CBTVoid < x.C_CBT);
            try
            {
                data = data.Where(x => !string.IsNullOrEmpty(x.C_DueDate)).OrderBy(x => Convert.ToDateTime(x.C_DueDate)).ToList();
            }
            catch
            {

            }
            if (data != null)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (i == 0)
                    {
                        checkbookTransactions_DTOs.Add(new CheckbookTransactions_DTO
                        {
                            C_CBTVoid = data[i].C_CBTVoid,
                            Date = data[i].C_DueDate,
                            DocumentNumber = data[i].C_DocumentNumber,
                            PostingNumber = data[i].C_PostingNumber,
                            DocumentType = data[i].C_PostingKey,
                            Payment = data[i].C_Payment,
                            Deposit = data[i].C_Reciept,
                            Balance = data[i].C_Balance,
                            RecievedFrom=data[i].C_Payment_To_Recieved_From,
                            Refrence= data[i].C_Reference
                        });
                    }
                    else
                    {
                        decimal? oldBalance = checkbookTransactions_DTOs[i - 1].Balance;
                        if (i == 1)
                        {
                            if (data[i - 1].C_CBTVoid.HasValue)
                                oldBalance = 0;
                        }
                        decimal? newBalance = oldBalance;
                        if (!data[i].C_CBTVoid.HasValue)
                        {
                            newBalance = (oldBalance + data[i].C_Reciept) - data[i].C_Payment;
                        }
                        checkbookTransactions_DTOs.Add(new CheckbookTransactions_DTO
                        {
                            C_CBTVoid = data[i].C_CBTVoid,
                            Date = data[i].C_DueDate,
                            DocumentNumber = data[i].C_DocumentNumber,
                            PostingNumber = data[i].C_PostingNumber,
                            DocumentType = data[i].C_PostingKey,
                            Payment = data[i].C_Payment,
                            Deposit = data[i].C_Reciept,
                            Balance = newBalance,
                            RecievedFrom = data[i].C_Payment_To_Recieved_From,
                            Refrence = data[i].C_Reference
                        });
                    }
                }
                return Json(checkbookTransactions_DTOs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ActionResult CashInquiry()
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
                .Where(x => x.CompanyID == companyID && (x.C_PostingKey == "TCCR" || x.C_PostingKey == "TCCW"))
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber, x.C_PostingNumber }).ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Cash")
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
        public JsonResult GetDocumentNumbers(int checkbookID, string documentType)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<CheckbookTransactions_DTO> CheckbookTransactions_DTO = DB.C_CheckbookTransactions_Tables
             .Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && x.C_PostingKey == documentType)
             .Select(x => new CheckbookTransactions_DTO
             {
                 PostingNumber = x.C_PostingNumber,
                 DocumentNumber = x.C_DocumentNumber,
                 
             }).ToList();
            return Json(CheckbookTransactions_DTO, JsonRequestBehavior.AllowGet);
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ActionResult BankInquiry()
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
                .Where(x => x.CompanyID == companyID && (x.C_PostingKey == "TCBR" || x.C_PostingKey == "TCBC"))
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber /*+ " - " + x.C_PostingKey*/, x.C_PostingNumber }).ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && (x.C_CheckbookType == "Bank" || x.C_CheckbookType == "Check"))
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
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
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
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCBT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber /*+ " - " + x.C_PostingKey*/, x.C_PostingNumber }).Distinct().ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCBT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_DocumentNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist2, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;
            ViewBag.TransNum = 0;
            return View();
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ActionResult CheckInquiry()
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
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_DocumentNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist2, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;

            var Checkbooklist3 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
               .Where(x => x.CompanyID == companyID)
               .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
               .ToList();
            if (Request["PO"] != null)
            {
                int PO = Convert.ToInt32(Request["PO"]);
                ViewBag.CBID = RSelectBusiness.DBC().C_CheckbookTransactions_Tables.Where(x => x.C_PostingNumber == PO).ToList().LastOrDefault().C_CBSID;
            }
            SelectList CheckbookID = new SelectList(Checkbooklist3, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;
            try
            {
                ViewBag.JN = FabulousErp.Business.GetJournalEntry(Checkbooklist.FirstOrDefault().C_PostingNumber);
                ViewBag.DocNum = Checkbooklist.FirstOrDefault().C_DocumentName;
            }
            catch
            {
                ViewBag.JN = "";
                ViewBag.DocNum = "";
            }
            return View();
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ActionResult CashVoidInquiry()
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
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCVC")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber, x.C_PostingNumber }).ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Cash")
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
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ActionResult BankVoidInquiry()
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
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TBVC")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber, x.C_PostingNumber }).ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID && (x.C_CheckbookType == "Bank" || x.C_CheckbookType == "Check"))
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
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ActionResult TransferVoidInquiry()
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
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCVT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCVT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_DocumentNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist2, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;

            return View();
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------
        public ActionResult CheckVoidInquiry()
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
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCVVT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_GeneralJournalEntry_Table.C_JournalEntryNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList JVNumber = new SelectList(Checkbooklist, "C_PostingNumber", "C_DocumentName");
            ViewBag.JVNumber = JVNumber;

            var Checkbooklist2 = RSelectBusiness.DBC().C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_PostingKey == "TCVVT")
                .OrderBy(x => x.C_PostingKey)
                .Select(x => new { C_DocumentName = x.C_DocumentNumber, x.C_PostingNumber }).Distinct().ToList();
            SelectList DocumentNumber = new SelectList(Checkbooklist2, "C_PostingNumber", "C_DocumentName");
            ViewBag.DocumentNumber = DocumentNumber;

            var Checkbooklist3 = RSelectBusiness.DBC().C_CheckBookSetting_Tables
               .Where(x => x.CompanyID == companyID)
               .Select(x => new { C_CheckbookID = x.C_CheckbookID + " - " + x.C_CheckbookType, x.C_CBSID })
               .ToList();
            SelectList CheckbookID = new SelectList(Checkbooklist3, "C_CBSID", "C_CheckbookID");
            ViewBag.CheckbookID = CheckbookID;

            return View();
        }
        //------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------


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





    }
}