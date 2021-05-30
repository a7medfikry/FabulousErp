using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousModels.DTOModels.Transaction.Financial.Company;
using FabulousModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Transaction.Financial.Company.Accounting
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_GeneralEntryTransactionController : Controller
    {
        DBContext DB = new DBContext();

        // GET: C_JournalEntryTransaction
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "TCGE")]
        public ActionResult CompanyGETransaction()
        {
            ViewBag.IsBatch = false;
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var detectJEPer =  Business.GetPostingSetup();
            //check Journal entry Per Transaction or Batch
            if (detectJEPer != null)
            {
                ViewBag.FJEPer = detectJEPer.CreateJEPer;
                ViewBag.EPD = detectJEPer.EditPostingDate;

                if (detectJEPer.CreateJEPer == "B2")
                {
                    ViewBag.BatchAction = detectJEPer.ExistingBatch;
                    ViewBag.PostDateType = detectJEPer.PostingDataFrom;
                }
            }
            else
            {
                ViewBag.FJEPer = "NoPS";
            }
            //var getCurrencyID = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            //if (getCurrencyID != null)
            //{
            //    SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
            //    ViewBag.CurrencyIDList = currencyIDList;
            //}

            var getFiscalYearOfComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
                if (checkYearOpen != null)
                {
                    ViewBag.CheckYear = "Exist";
                }
            }

            var checkUserCanEditTRate = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == "Financial" && x.TransactionFormName == "Company Journal Entry Transaction").FirstOrDefault();
            if (checkUserCanEditTRate != null)
            {
                if (checkUserCanEditTRate.AllowUserE == true)
                {
                    ViewBag.AllowUserERate = "True";
                }
            }

            var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            var checkCurrencyFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }
            ViewBag.PostingToOrThrow = Business.PostingToOrThrow();
            //ViewBag.FromTCGE = true;
            return View();
        }
        public PartialViewResult GetBatchHeader(bool TCS_JENum = false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var getCurrencyID = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }
            var getBatchID = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.C_BatchLocation == "TCGE" && x.C_Module == "Financial" && x.Removed == false).ToList();
            if (getBatchID != null)
            {
                SelectList batchIDList = new SelectList(getBatchID, "C_CBID", "C_BatchID");
                ViewBag.BatchIDList = batchIDList;
            }
            ViewBag.FromTCGE = true;
            if (TCS_JENum == false)
            {
                ViewBag.FromTCGE = false;
            }
            return PartialView("~/Views/Shared/_GETransactionHeader.cshtml");
        }
        public JsonResult GetBatchCreationDate(int batchID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            if (companyID != null)
            {
                var getDate = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.C_Module == "Financial" && x.C_CBID == batchID).FirstOrDefault();

                return Json(getDate.C_BatchCreationDate, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        public JsonResult AddNewBatch(string batchID, string batchDescription)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            if (companyID != null)
            {

                var checkDuplicate = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.C_Module == "Financial" && x.C_BatchID == batchID && x.C_BatchLocation == "TCGE" && x.Removed == false).FirstOrDefault();

                if (checkDuplicate != null)
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var postingSetup = Business.GetPostingSetup(); //  Business.GetPostingSetup();

                    bool notApproval = false;
                    bool approval = false;
                    if (postingSetup.Batch == "C2")
                    {
                        notApproval = true;
                        approval = false;
                    }
                    else
                    {
                        approval = true;
                        notApproval = false;
                    }

                    C_CreateBatch_Table c_CreateBatch_Table = new C_CreateBatch_Table()
                    {
                        CompanyID = companyID,

                        C_Module = "Financial",

                        C_BatchID = batchID,

                        C_BatchDescription = batchDescription,

                        C_BatchCreationDate = DateTime.Now.ToShortDateString(),

                        C_BatchLocation = "TCGE",

                        NotApproval = notApproval,

                        Approval = approval,

                        Removed = false,

                        UserID = FabulousErp.Business.GetUserId()
                    };

                    DB.C_CreateBatch_Tables.Add(c_CreateBatch_Table);
                    DB.SaveChanges();


                    TCGE_DTO tCGE_DTO = new TCGE_DTO()
                    {
                        C_CBID = c_CreateBatch_Table.C_CBID,

                        C_BatchCreationDate = c_CreateBatch_Table.C_BatchCreationDate
                    };

                    return Json(tCGE_DTO, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return null;
            }
        }
        /***********************************************************************************
         ***********************************************************************************/
        [HttpGet]
       // [Authorize]
        public ActionResult CompanyShowTransactions()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var detectJEPer = Business.GetPostingSetup();// Business.GetPostingSetup();
            //check Journal entry Per Transaction or Batch
            if (detectJEPer != null)
            {
                ViewBag.FJEPer = detectJEPer.CreateJEPer;
                ViewBag.EPD = detectJEPer.EditPostingDate;

                if (detectJEPer.CreateJEPer == "B2")
                {
                    ViewBag.BatchAction = detectJEPer.ExistingBatch;
                    ViewBag.PostDateType = detectJEPer.PostingDataFrom;
                }
            }
            else
            {
                ViewBag.FJEPer = "NoPS";
            }


            var getFiscalYearOfComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
                if (checkYearOpen != null)
                {
                    ViewBag.CheckYear = "Exist";
                }
            }


            var checkUserCanEditTRate = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == "Financial" && x.TransactionFormName == "Company Journal Entry Transaction").FirstOrDefault();
            if (checkUserCanEditTRate != null)
            {
                if (checkUserCanEditTRate.AllowUserE == true)
                {
                    ViewBag.AllowUserERate = "True";
                }
            }

            var checkCurrencyFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }

            var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            return View();
        }
        public JsonResult GetTransactionsBatches()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            IQueryable<TransactionsBatches> batches = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.C_Module == "Financial" && x.Removed == false).Select(x => new TransactionsBatches
            {
                CBID = x.C_CBID,

                BatchID = x.C_BatchID,

                BatchLocation = x.C_BatchLocation
            });

            return Json(batches, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetJENumByBatch(int CBID)
        {
            IQueryable<JEntryNumbers> jEntryNumbers = DB.C_GeneralJournalEntry_Tables.Where(x => x.C_CBID == CBID && x.Post == false).Select(x => new JEntryNumbers
            {
                PostingNumber = x.C_PostingNumber,
                JournalEntryNum = x.C_JournalEntryNumber,
                PostingKey = x.C_PostingKey
            });
            return Json(jEntryNumbers, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllJENum()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            IQueryable<JEntryNumbers> jEntryNumbers = DB.C_GeneralJournalEntry_Tables.Where(x => x.CompanyID == companyID && x.Post == false).Select(x => new JEntryNumbers
            {

                PostingNumber = x.C_PostingNumber,

                JournalEntryNum = x.C_JournalEntryNumber,

                PostingKey = x.C_PostingKey

            });
            return Json(jEntryNumbers, JsonRequestBehavior.AllowGet);
        }
        /***********************************************************************************
         ***********************************************************************************/
        [HttpGet]
       // [Authorize]
        public ActionResult CompanyVoidTransactions()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var detectJEPer = Business.GetPostingSetup();// Business.GetPostingSetup();
            if (detectJEPer != null)
            {
                ViewBag.PT = detectJEPer.PostingType;
                ViewBag.EPD = detectJEPer.EditPostingDate;
            }
            else
            {
                ViewBag.CheckPostingSetup = "NoPS";
            }

            var getFiscalYearOfComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
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

            var postedJENum = DB.C_GeneralJournalEntry_Tables.Where(x => x.Post == true && x.C_PostingKey == "TCGE" && x.VoidPostingNum == null).Select(x => new { PostingNumber = x.C_PostingNumber, JENumber = x.C_JournalEntryNumber + " - " + x.C_PostingKey });
            SelectList JENumList = new SelectList(postedJENum, "PostingNumber", "JENumber");
            ViewBag.JENumList = JENumList;

            return View();
        }
        /***********************************************************************************
         ***********************************************************************************/
        public ActionResult AdjustmentGETransaction()
        {
            ViewBag.IsBatch = false;
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var detectJEPer = Business.GetPostingSetup();//  Business.GetPostingSetup();
            if (detectJEPer != null)
            {
                ViewBag.FJEPer = detectJEPer.CreateJEPer;
                ViewBag.EPD = detectJEPer.EditPostingDate;

                if (detectJEPer.CreateJEPer == "B2")
                {
                    ViewBag.BatchAction = detectJEPer.ExistingBatch;
                    ViewBag.PostDateType = detectJEPer.PostingDataFrom;
                }
            }
            else
            {
                ViewBag.FJEPer = "NoPS";
            }

            var getFiscalYearOfComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (getFiscalYearOfComp != null)
            {
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
                if (checkYearOpen != null)
                {
                    ViewBag.CheckYear = "Exist";
                }
            }

            var checkUserCanEditTRate = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == "Financial" && x.TransactionFormName == "Company Journal Entry Transaction").FirstOrDefault();
            if (checkUserCanEditTRate != null)
            {
                if (checkUserCanEditTRate.AllowUserE == true)
                {
                    ViewBag.AllowUserERate = "True";
                }
            }

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
        public PartialViewResult GetAdjBatchHeader(bool TCS_JENum = false)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var getCurrencyID = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }
            var getBatchID = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.C_BatchLocation == "TCGE" && x.C_Module == "Financial" && x.Removed == false).ToList();
            if (getBatchID != null)
            {
                SelectList batchIDList = new SelectList(getBatchID, "C_CBID", "C_BatchID");
                ViewBag.BatchIDList = batchIDList;
            }
            ViewBag.FromTCGE = true;
            if (TCS_JENum == false)
            {
                ViewBag.FromTCGE = false;
            }
            return PartialView("~/Views/Shared/_AdjGETransactionheader.cshtml");
        }

        public JsonResult CalculateTransactionRate(string CurrencyId,double Amount,DateTime CompareDate)
        {
            try
            {
                
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                string SystemCurrency = DB.CurrenciesDefinition_Tables.FirstOrDefault(x => x.CurrencyID == companyID).CurrencyID;
                if (companyID == CurrencyId)
                {
                    return Json(new {Amount=Amount, Msg = "" });
                }
                else
                {
                    double Exchangerate = DB.CurrenciesExchange_Tables.Where(x => x.CurrencyID == CurrencyId)
                                    .ToList().Where(x => Convert.ToDateTime(x.StartDate) <= CompareDate && Convert.ToDateTime(x.ExpireDate) >= CompareDate)
                                    .DefaultIfEmpty(new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesExchange_Table { Rate = 0 })
                                    .FirstOrDefault().Rate;
                    if (Exchangerate == 0)
                    {
                        return Json(new { Amount = Amount, Msg = "There are No Exchange Rate In That Date For That Currency" });

                    }
                    else
                    {
                        return Json(new { Amount = Amount * Exchangerate, Msg = "" });
                    }
                }
              
            }
            catch
            {
                return Json(new { Amount = Amount, Msg = "Error Some Thing Went Wrong Please Contact the Administration" });

            }


        }

        //public JsonResult AddFavourites(bool Value)
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.TCGE = Value;
        //            DB.SaveChanges();
        //        }
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult CheckFavourites()
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item.TCGE.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}
    }
}