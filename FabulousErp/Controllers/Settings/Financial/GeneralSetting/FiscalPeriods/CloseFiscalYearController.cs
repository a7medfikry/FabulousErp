using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.FiscalPeriods
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CloseFiscalYearController : Controller
    {

        DBContext DB = new DBContext();


        // GET: CloseFiscalYear
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCY")]
        public ActionResult CloseYear()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var getYearDefinition = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

            if (getYearDefinition != null)
            {
                string yearDef = getYearDefinition.Fiscal_Year_ID;

                var getNewYears = DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == yearDef && (x.Closed == false || x.Closed == null) && x.CheckDate == "True").ToList();

                SelectList yearsList = new SelectList(getNewYears, "YearID", "Year");

                ViewBag.yearsList = yearsList;
            }

            var getBallanceSheetAccount = DB.C_CreateAccount_Tables.Where(x => x.PostingType == "BallanceSheet").Select(x => new { AID = x.C_AID, AccountID = x.AccountID + " ( " + x.AccountName + " )" });
            SelectList accountBS = new SelectList(getBallanceSheetAccount, "AID", "AccountID");
            ViewBag.accountBSList = accountBS;

            return View();
        }

        public JsonResult CheckCloseYear(int YearID)
        {
            var updateYear = DB.NewFiscalYear_Table.Where(x => x.YearID == YearID).FirstOrDefault();

            var checkExistOfNewYear = DB.NewFiscalYear_Table.ToList().FirstOrDefault(x => DateTime.Parse(x.Fiscal_Year_Start) > DateTime.Parse(updateYear.Fiscal_Year_End));

            var checkExistOfOldYearNotClosed = DB.NewFiscalYear_Table.ToList().FirstOrDefault(x => DateTime.Parse(x.Fiscal_Year_End) < DateTime.Parse(updateYear.Fiscal_Year_Start) && x.Closed == false);

            if (checkExistOfOldYearNotClosed != null)
            {
                return Json("There exist Old Year not Closed..!", JsonRequestBehavior.AllowGet);
            }
            else if (checkExistOfNewYear == null)
            {
                return Json("Not Found a new Year Created..!", JsonRequestBehavior.AllowGet);
            }
            else
            {
                updateYear.Closed = true;
                DB.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult CheckNotSavedBatches(int yearID, int AID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            if (!YearCloesd(yearID))
            {
                return Json("ClosedYear", JsonRequestBehavior.AllowGet);
            }
            var periodDates = DB.NewFiscalYear_Table.FirstOrDefault(x => x.YearID == yearID);
            DateTime periodStartDate = DateTime.Parse(periodDates.Fiscal_Year_Start);
            DateTime periodEndDate = DateTime.Parse(periodDates.Fiscal_Year_End);

            var getNextYear = DB.NewFiscalYear_Table.ToList().OrderBy(x => x.Fiscal_Year_Start).FirstOrDefault(x => DateTime.Parse(x.Fiscal_Year_Start) > periodEndDate);

            if (getNextYear != null)
            {
                var getBallanceSheetAccount = DB.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == AID);

                CloseYear_DTO closeYear_DTOs = new CloseYear_DTO
                {
                    PLAccounts = new List<PLAccounts>(),
                    BallanceSheetAccounts = new List<PLAccounts>()
                };

                var check = DB.C_GeneralJournalEntry_Tables.ToList().FirstOrDefault(x => /*DateTime.Parse(x.C_PostingDate) >= periodStartDate &&*/ DateTime.Parse(x.C_PostingDate) <= periodEndDate && x.Post == false);
                if (check != null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var transactionsOfPLAccounts = DB.C_GeneralLedger_Tables.ToList().Where(x => x.C_CreateAccount_Table.PostingType == "PL" /*&& DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) >= periodStartDate*/ && DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) <= periodEndDate).ToList();
                    //if (!transactionsOfPLAccounts.Any(x=>x.C_AID== AID))
                    //{
                    //    try
                    //    {
                    //        transactionsOfPLAccounts.Add(new C_GeneralLedger_Table {
                    //            C_AID= AID,
                    //            Ballance=0,
                    //            C_Credit=0,
                    //            C_Debit=0,
                    //            C_CreateAccount_Table=new FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account.C_CreateAccount_Table
                    //            {
                    //                C_AID=AID,
                    //                AccountID=DB.C_CreateAccount_Tables.FirstOrDefault(x=>x.C_AID== AID).AccountID,
                    //                AccountName= DB.C_CreateAccount_Tables.FirstOrDefault(x => x.C_AID == AID).AccountName,
                    //            }
                    //        });
                    //    }
                    //    catch
                    //    {

                    //    }
                    //}

                    foreach (var item in transactionsOfPLAccounts.GroupBy(x => x.C_AID))
                    {
                        closeYear_DTOs.PLAccounts.Add(new PLAccounts
                        {
                            AID = item.FirstOrDefault().C_CreateAccount_Table.C_AID,

                            AccountID = item.FirstOrDefault().C_CreateAccount_Table.AccountID,

                            AccountName = item.FirstOrDefault().C_CreateAccount_Table.AccountName,

                            Ballance = item.Sum(x => x.Ballance)
                        });
                    }

                    var transactionsOfBallancsSheetAccounts = DB.C_GeneralLedger_Tables.ToList().Where(x => x.C_CreateAccount_Table.PostingType == "BallanceSheet" && /*DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) >= periodStartDate &&*/ DateTime.Parse(x.C_GeneralJournalEntry_Table.C_PostingDate) <= periodEndDate).ToList();

                    foreach (var item in transactionsOfBallancsSheetAccounts.GroupBy(x => x.C_AID))
                    {
                        closeYear_DTOs.BallanceSheetAccounts.Add(new PLAccounts
                        {
                            AID = item.FirstOrDefault().C_CreateAccount_Table.C_AID,

                            AccountID = item.FirstOrDefault().C_CreateAccount_Table.AccountID,

                            AccountName = item.FirstOrDefault().C_CreateAccount_Table.AccountName,

                            Ballance = item.Sum(x => x.Ballance)
                        });
                    }

                    closeYear_DTOs.HeaderData = new HeaderData()
                    {
                        LastDayInYear = periodEndDate.ToShortDateString(),

                        AccountID = getBallanceSheetAccount.AccountID,

                        AccountName = getBallanceSheetAccount.AccountName,

                        NextYearID = getNextYear.YearID,

                        NextYearName = getNextYear.Year,

                        FirstDayInNextYear = getNextYear.Fiscal_Year_Start
                    };

                    return Json(closeYear_DTOs, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("NotNewYear", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CloseYear(C_EndingBeginingYear[] SaveEndingBegining)
        {
            foreach (var items in SaveEndingBegining)
            {
                C_EndingBeginingYear c_EndingBeginingYear = new C_EndingBeginingYear()
                {
                    YearID = items.YearID,

                    Type = items.Type,

                    C_AID = items.C_AID,

                    Debit = items.Debit,

                    Credit = items.Credit,

                    Ballance = items.Ballance
                };
                DB.C_EndingBeginingYears.Add(c_EndingBeginingYear);
            }

            DB.SaveChanges();

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckIfYearClosed(int YearId)
        {
            try
            {
                return Json(YearCloesd(YearId));
            }
            catch
            {
                return Json(0);
            }
        }
        bool YearCloesd(int YearId)
        {
            try
            {
                NewFiscalYear_Table ThisYear = DB.NewFiscalYear_Table.FirstOrDefault(x => x.YearID == YearId);
                int PrevYear = Convert.ToInt32(ThisYear.Year) - 1;
                int FirstYear = DB.NewFiscalYear_Table.ToList().Min(x=>Convert.ToInt32(x.Year));
                if (FirstYear==Convert.ToInt32(ThisYear.Year))
                {
                    return true;
                }
                bool? Closed = DB.NewFiscalYear_Table.FirstOrDefault(x => x.Year == PrevYear.ToString()).Closed;
                if (!Closed.HasValue)
                {
                    return false;
                }
                else
                {
                    return Closed.Value;
                }
            }
            catch
            {
                return false;
            }

        }
        //public JsonResult AddFavourites(bool Value)
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.SCY = Value;
        //            DB.SaveChanges();
        //        }
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult CheckFavourites()
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item != null)
        //        {
        //            if (item.SCY.ToString().Equals("True"))
        //            {
        //                return Json("True", JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                return Json("False", JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            return Json("", JsonRequestBehavior.AllowGet);
        //        }

        //    }
        //}

    }

}