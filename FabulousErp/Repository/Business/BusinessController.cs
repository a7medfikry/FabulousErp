using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Repository.Business
{
    public class BusinessController : Controller
    {
        // GET: Business
        public PartialViewResult GetMainTransaction(bool FixedAssets = false)
        {
            ViewBag.FixedAssets = FixedAssets;
            SetCommenViewBag(FixedAssets);
            return PartialView("~/Views/Shared/_MainTransaction.cshtml");

        }
        public PartialViewResult SetCommenViewBagView(bool FixedAssets = false)
        {
            SetCommenViewBag(FixedAssets);
            return PartialView("~/Views/Shared/SetCommonViewBag.cshtml");
        }
        private void SetCommenViewBag(bool FixedAssets =false)
        {
            try
            {
                using (DBContext DB = new DBContext())
                {
                    string companyID = FabulousErp.Business.GetCompanyId();

                    var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
                    SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
                    ViewBag.AccountList = accountList;

                    if (!FabulousErp.Business.HasPostringSetup())
                    {
                        ViewBag.FJEPer = "NoPS";
                        return;
                    }
                    string AreaName = FabulousErp.Business.GetCurrentAreaName();
                    var SPSCheck = FabulousErp.Business.GetPostingSetup(); 
                    if (SPSCheck!=null)
                    {
                        ViewBag.EPD = SPSCheck.EditPostingDate;
                        ViewBag.PT = SPSCheck.PostingType;
                    }
                    else
                    {
                        ViewBag.FJEPer = "NoPS";
                    }
                    //check Journal entry Per Transaction or Batch
                    if (SPSCheck != null)
                    {
                        ViewBag.FJEPer = SPSCheck.CreateJEPer;
                        ViewBag.EPD = SPSCheck.EditPostingDate;

                        if (SPSCheck.CreateJEPer == "B2")
                        {
                            if (!FixedAssets)
                            {
                                ViewBag.BatchAction = SPSCheck.ExistingBatch;
                                ViewBag.PostDateType = SPSCheck.PostingDataFrom;
                            }
                            else
                            {
                                ViewBag.FJEPer = "B1";
                            }

                        }
                        ViewBag.PostingToOrThrow = FabulousErp.Business.PostingToOrThrow();
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
                    var checkCurrencyFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
                    if (checkCurrencyFormate != null)
                    {
                        ViewBag.FormateSetting = "True";
                    }
                  
                    ViewBag.PostingToOrThrow = FabulousErp.Business.PostingToOrThrow();

                }
            }
            catch
            {
            }
        }
        
    }
}