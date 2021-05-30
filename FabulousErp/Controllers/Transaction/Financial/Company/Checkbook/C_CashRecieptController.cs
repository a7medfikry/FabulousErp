using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Business;
using FabulousModels.DTOModels.Transaction.Financial;
using FabulousModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace FabulousErp.Controllers.Transaction.Financial.Company.Checkbook
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_CashRecieptController : Controller
    {
        DBContext DB = new DBContext();

        // GET: C_CashReciept
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TCCR")]
        public ActionResult CompanyCashReciept()
        {
            #region Obselete_check_next_function

            //string companyID = (string)FabulousErp.Business.GetCompanyId();
            //var SPSCheck =  Business.GetPostingSetup();
            //if (SPSCheck != null)
            //{
            //    ViewBag.EPD = SPSCheck.EditPostingDate;
            //    ViewBag.PT = SPSCheck.PostingType;
            //}
            //else
            //{
            //    ViewBag.FJEPer = "NoPS";
            //}

            //var FiscalyearCheck = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            //if (FiscalyearCheck != null)
            //{
            //    var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == FiscalyearCheck.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
            //    if (checkYearOpen != null)
            //    {
            //        ViewBag.CheckYear = "Exist";
            //    }
            //}

            //var checkEditTransactionRate = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == "Financial" && x.TransactionFormName == "Company Cash Reciept").FirstOrDefault();
            //if (checkEditTransactionRate != null)
            //{
            //    if (checkEditTransactionRate.AllowUserE == true)
            //    {
            //        ViewBag.AllowUserERate = "True";
            //    }
            //}

            //var Checkbooklist = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Cash").ToList();
            //SelectList CheckbookID = new SelectList(Checkbooklist, "C_CBSID", "C_CheckbookID");
            //ViewBag.CheckbookID = CheckbookID;

            //var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            //SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            //ViewBag.AccountList = accountList;

            //var checkCurrencyFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            //if (checkCurrencyFormate != null)
            //{
            //    ViewBag.FormateSetting = "True";
            //}
            #endregion
            return View();
        }
        public PartialViewResult CurrencyAndCheckBook()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var SPSCheck = Business.GetPostingSetup();// Business.GetPostingSetup();
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

            var Checkbooklist = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CheckbookType == "Cash").ToList();
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
            ViewBag.PostingToOrThrow = Business.PostingToOrThrow();

            return PartialView();
        }
        public JsonResult GetCurrencyAccounts(string currencyID, string profit)
        {
            List<CheckbookTransactions_DTO> checkbookTransactions_DTOs = DB.AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID && x.Type == profit).Select(x => new CheckbookTransactions_DTO
            {
                C_AID = x.C_AID,
                Currency_AccountsID = x.C_CreateAccount_Table.AccountID,
                Company_AccountsName = x.C_CreateAccount_Table.AccountName
            }).ToList();
            return Json(checkbookTransactions_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveTransactions(int journalEntryNumber, string PostingKey, int CheckbookID, string Currency, string TransactionDate, string PostingDate, double? SystemRate, double? TransactionRate, double? Difference, string Reference, string DocumentType, string ReceivedFrom, decimal Receipt_PaymentAmount, string Bank_CheckNumber, string Bank_DueDate, int? identityVoid, string postingKeyVoid)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            string UserID = FabulousErp.Business.GetUserId();

            int documentNumber = 0;
            decimal reciptAmount = 0;
            decimal paymentAmount = 0;
            if (PostingKey == "TCCR")
            {
                var nextDepositNumber = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == CheckbookID).FirstOrDefault();
                documentNumber = nextDepositNumber.C_NextDepositNumber;
                nextDepositNumber.C_NextDepositNumber = nextDepositNumber.C_NextDepositNumber + 1;
                DB.SaveChanges();

                reciptAmount = Receipt_PaymentAmount;

                Bank_DueDate = PostingDate;
            }
            else if (PostingKey == "TCBR")
            {
                var nextDepositNumber = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == CheckbookID).FirstOrDefault();
                documentNumber = nextDepositNumber.C_NextDepositNumber;
                nextDepositNumber.C_NextDepositNumber = nextDepositNumber.C_NextDepositNumber + 1;
                DB.SaveChanges();

                reciptAmount = Receipt_PaymentAmount;
            }
            else if (PostingKey == "TCCW")
            {
                var nextWithdrawNumber = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == CheckbookID).FirstOrDefault();
                documentNumber = nextWithdrawNumber.C_NextWithdrawNumber;
                nextWithdrawNumber.C_NextWithdrawNumber = nextWithdrawNumber.C_NextWithdrawNumber + 1;
                DB.SaveChanges();

                paymentAmount = Receipt_PaymentAmount;

                Bank_DueDate = PostingDate;
            }
            else if (PostingKey == "TCBC")
            {
                var nextWithdrawNumber = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == CheckbookID).FirstOrDefault();
                documentNumber = nextWithdrawNumber.C_NextWithdrawNumber;
                nextWithdrawNumber.C_NextWithdrawNumber = nextWithdrawNumber.C_NextWithdrawNumber + 1;
                DB.SaveChanges();

                paymentAmount = Receipt_PaymentAmount;
            }
            else if (postingKeyVoid == "TCCR")
            {
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

                reciptAmount = Receipt_PaymentAmount;
                Bank_DueDate = PostingDate;
            }
            else if (postingKeyVoid == "TCCW")
            {
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

                paymentAmount = Receipt_PaymentAmount * -1;
                Bank_DueDate = PostingDate;
            }
            else if (postingKeyVoid == "TCBC")
            {
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

                paymentAmount = Receipt_PaymentAmount * -1;
            }
            else if (postingKeyVoid == "TCBR")
            {
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

                reciptAmount = Receipt_PaymentAmount;
            }
            C_CheckbookTransactions_table c_CheckbookTransactions_Table = new C_CheckbookTransactions_table()
            {
                CompanyID = companyID,
                C_PostingNumber = journalEntryNumber,
                C_CBSID = CheckbookID,
                CurrencyID = Currency,
                C_TransactionDate = TransactionDate,
                C_PostingDate = PostingDate,
                C_SystemRate = SystemRate,
                C_TransactionRate = TransactionRate,
                C_Difference = Difference,
                C_Reference = Reference,
                C_DocumentType = DocumentType,
                C_Payment_To_Recieved_From = ReceivedFrom,
                C_Reciept = reciptAmount,
                C_Payment = paymentAmount,
                C_Balance = reciptAmount - paymentAmount,
                C_PostingKey = PostingKey,
                C_DocumentNumber = documentNumber,
                C_CheckNumber = Bank_CheckNumber,
                C_DueDate = Bank_DueDate,
                C_Reconcile = false,
                C_CBTVoid = identityVoid,
                
                UserID = UserID,
                C_DateTime = DateTime.Now,
            };
            DB.C_CheckbookTransactions_Tables.Add(c_CheckbookTransactions_Table);

            if (identityVoid.HasValue)
            {
                var data = DB.C_CheckbookTransactions_Tables.Where(x => x.C_CBT == identityVoid).FirstOrDefault();
                data.C_CBTVoid = c_CheckbookTransactions_Table.C_CBT;
            }

            DB.SaveChanges();

            return Json("True", JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintRecipt(int JN,bool IsDoc=false,string PostingKey="")
        {
            using (ReciptsController Re = new ReciptsController())
            {
                List<ReciptsValues> Word = new List<ReciptsValues>();
                string CompanyId = Business.GetCompanyId();
                CompanyMainInfo_Table CompanyMainInfo_Tables = DB.CompanyMainInfo_Tables.FirstOrDefault(x => x.CompanyID == CompanyId);

                C_CheckbookTransactions_table CB = Enumerable.Empty<C_CheckbookTransactions_table>().FirstOrDefault();
                if (IsDoc)
                {
                    CB = DB.C_CheckbookTransactions_Tables.Include(x => x.C_CheckBookSetting_Table).FirstOrDefault(x => x.C_DocumentNumber == JN&&x.C_PostingKey== PostingKey);
                }
                else
                {
                    CB = DB.C_CheckbookTransactions_Tables.Include(x => x.C_CheckBookSetting_Table).FirstOrDefault(x => x.C_PostingNumber == JN);
                }
                string Title;
                bool IsCash = true;
                bool IsFrom = true;
                bool ShowBank = false;
                
                if (CB.C_PostingKey== "TCBR")
                {
                    Title = "ايصال استلام شيكات";
                    IsCash = false;
                }
                else if (CB.C_PostingKey == "TCCW")
                {
                    Title = "إذن صرف نقدية";
                    IsCash = true;
                    IsFrom = false;
                }
                else if (CB.C_PostingKey== "TCBC")
                {
                    Title = "إذن صرف شيكات";
                    IsCash = false;
                    IsFrom = false;
                    ShowBank = true;
                }
                else
                {
                    Title = "ايصال استلام نقدية";
                    IsCash = true;
                }
                string Company_name = CompanyMainInfo_Tables.CompanyName;
                decimal Balance = CB.C_Balance;

                if (Balance < 0)
                {
                    Balance = -Balance;
                }
                string BlanceStr = Balance.ToString(FabulousErp.Business.GetDecimalNumber());
                string Moneytext =  Bussiness.ConvertNumberToText.ConvertNumberToAlpha(BlanceStr, (CB.CurrenciesDefinition_Table!=null)?CB.CurrenciesDefinition_Table.Currency_unit_name:"", (CB.CurrenciesDefinition_Table!=null)? CB.CurrenciesDefinition_Table.Currency_small_unit_name :"" );

                string BankName = "";
                string CheqNumber = "";
                if (CB.C_CheckBookSetting_Table != null)
                {
                    BankName= CB.C_CheckBookSetting_Table.C_CheckbookName;
                }
                CheqNumber = (CB.C_CheckNumber==null)?"": CB.C_CheckNumber;


                string Logo = Business.GetCompLogo();
                DateTime? DueDate = null;
                if (DateTime.TryParse(CB.C_DueDate,out DateTime T))
                {
                    DueDate = Convert.ToDateTime(CB.C_DueDate);
                }
                List<ReciptsValues> ReciptView = new List<ReciptsValues>();
                ReciptView = Re.FillModel(Company_name, Logo, Title,
                    CB.C_DocumentNumber.ToString(), BlanceStr, Convert.ToDateTime(CB.C_TransactionDate)
                    , CB.C_Payment_To_Recieved_From, Moneytext, CB.C_Reference
                    , IsCash, CheqNumber, DueDate, BankName, IsFrom,ShowBank
                    ,CB.C_CheckBookSetting_Table.C_CheckbookName, CB.C_CheckBookSetting_Table.C_CheckbookID);
              
                return Re.Recipt(Recipts.CheckBook2.ToString(), Recipts.CheckBook2.ToString()
                    , ReciptView, ReciptView);
            }
        }


    }
}