using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FabulousErp.MyRoleProvider;
using System.Web.Mvc;
using FabulousErp.Repository.Interface;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.FiscalPeriods
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CreateNewYearController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public CreateNewYearController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: CreateNewYear
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCNY")]
        public ActionResult NewYear()
        {

            ViewBag.CNFiscalYearID = repetitionBusiness.RetrieveFiscalYearIDList();

            return View();
        }

        public JsonResult CreateNewYear(string CNFiscalYearID, string CNYear, string CNStartDate, string CNEndDate)
        {
            var removeInvalidDatesP = DB.FiscalYear_Tables.Where(x => x.CheckDate != "True").ToList();
            DB.FiscalYear_Tables.RemoveRange(removeInvalidDatesP);

            var removeInvalidDatesA = DB.FiscalAdjustment_Tables.Where(x => x.CheckDate != "True").ToList();
            DB.FiscalAdjustment_Tables.RemoveRange(removeInvalidDatesA);

            var removeInvalidPeriod = DB.NewFiscalYear_Table.Where(x => x.CheckDate != "True").ToList();
            DB.NewFiscalYear_Table.RemoveRange(removeInvalidPeriod);

            //5
            var CheckYearDuplicate = DB.NewFiscalYear_Table.Where(x => x.Year == CNYear && x.Fiscal_Year_ID == CNFiscalYearID && x.CheckDate == "True").FirstOrDefault();

            if (CheckYearDuplicate != null)
            {
                return Json("Duplicate", JsonRequestBehavior.AllowGet);
            }
            else
            {

                NewFiscalYear_Table newFiscalYear_Table = new NewFiscalYear_Table()
                {
                    Fiscal_Year_ID = CNFiscalYearID,

                    Year = CNYear,

                    Fiscal_Year_Start = CNStartDate,

                    Fiscal_Year_End = CNEndDate
                };
                DB.NewFiscalYear_Table.Add(newFiscalYear_Table);
                DB.SaveChanges();

                return Json(newFiscalYear_Table.YearID, JsonRequestBehavior.AllowGet);
            }
            //return null;
        }



        public ActionResult CreatePeriodsAndAdjusments(string CNFiscalYearID, int CNYear, string CNStartDate, string CNEndDate)
        {

            var getNumberOfPA = DB.FiscalDefinition_Tables.Where(x => x.Fiscal_Year_ID == CNFiscalYearID).FirstOrDefault();

            int periodsCount = getNumberOfPA.Number_Of_Periods;

            int AdjustmentCount = getNumberOfPA.Number_Of_Adjustment_Periods;

            DateTime EnterStartDate = Convert.ToDateTime(CNStartDate);

            DateTime EnterEndDate = Convert.ToDateTime(CNEndDate);


            string[] startdatearr = new string[12];
            startdatearr[0] = EnterStartDate.Year.ToString() + "-01-01";
            startdatearr[1] = EnterStartDate.Year.ToString() + "-02-01";
            startdatearr[2] = EnterStartDate.Year.ToString() + "-03-01";
            startdatearr[3] = EnterStartDate.Year.ToString() + "-04-01";
            startdatearr[4] = EnterStartDate.Year.ToString() + "-05-01";
            startdatearr[5] = EnterStartDate.Year.ToString() + "-06-01";
            startdatearr[6] = EnterStartDate.Year.ToString() + "-07-01";
            startdatearr[7] = EnterStartDate.Year.ToString() + "-08-01";
            startdatearr[8] = EnterStartDate.Year.ToString() + "-09-01";
            startdatearr[9] = EnterStartDate.Year.ToString() + "-10-01";
            startdatearr[10] = EnterStartDate.Year.ToString() + "-11-01";
            startdatearr[11] = EnterStartDate.Year.ToString() + "-12-01";

            string[] enddatearr = new string[12];
            enddatearr[0] = EnterStartDate.Year.ToString() + "-01-31";
            enddatearr[1] = EnterStartDate.Year.ToString() + "-02-28";
            enddatearr[2] = EnterStartDate.Year.ToString() + "-03-31";
            enddatearr[3] = EnterStartDate.Year.ToString() + "-04-30";
            enddatearr[4] = EnterStartDate.Year.ToString() + "-05-31";
            enddatearr[5] = EnterStartDate.Year.ToString() + "-06-30";
            enddatearr[6] = EnterStartDate.Year.ToString() + "-07-31";
            enddatearr[7] = EnterStartDate.Year.ToString() + "-08-31";
            enddatearr[8] = EnterStartDate.Year.ToString() + "-09-30";
            enddatearr[9] = EnterStartDate.Year.ToString() + "-10-31";
            enddatearr[10] = EnterStartDate.Year.ToString() + "-11-30";
            enddatearr[11] = EnterStartDate.Year.ToString() + "-12-31";


            if (periodsCount == 12 && EnterStartDate.Month.ToString().Equals("1") && EnterEndDate.Month.ToString().Equals("12") && EnterStartDate.Year == EnterEndDate.Year)
            {
                for (int i = 0; i < periodsCount; i++)
                {
                    string StartDate = startdatearr[i];

                    string EndDate = enddatearr[i];

                    FiscalYear_Table fiscalYear_Table = new FiscalYear_Table()
                    {
                        //Fiscal_Year_ID = fiscalDefinition.Fiscal_Year_ID,
                        YearID = CNYear,
                        Period_No = i + 1,
                        Period_Start_Date = StartDate,
                        Period_End_Date = EndDate
                    };
                    DB.FiscalYear_Tables.Add(fiscalYear_Table);
                    DB.SaveChanges();
                }
                for (int i = 0; i < AdjustmentCount; i++)
                {
                    FiscalAdjustment_Table fiscalAdjustment_Table = new FiscalAdjustment_Table()
                    {
                        //Fiscal_Year_ID = fiscalDefinition.Fiscal_Year_ID,
                        YearID = CNYear,
                        Period_No = i + 1,
                        Period_Start_Date = "",
                        //Period_End_Date = ""
                    };
                    DB.FiscalAdjustment_Tables.Add(fiscalAdjustment_Table);
                    DB.SaveChanges();
                }
            }
            else
            {
                for (int i = 0; i < periodsCount; i++)
                {
                    string StartDate = startdatearr[i];
                    string EndDate = enddatearr[i];
                    FiscalYear_Table fiscalYear_Table = new FiscalYear_Table()
                    {
                        //Fiscal_Year_ID = fiscalDefinition.Fiscal_Year_ID,
                        YearID = CNYear,
                        Period_No = i + 1,
                        Period_Start_Date = "",
                        Period_End_Date = ""
                    };
                    DB.FiscalYear_Tables.Add(fiscalYear_Table);
                    DB.SaveChanges();
                }
                for (int i = 0; i < AdjustmentCount; i++)
                {
                    FiscalAdjustment_Table fiscalAdjustment_Table = new FiscalAdjustment_Table()
                    {
                        //Fiscal_Year_ID = fiscalDefinition.Fiscal_Year_ID,
                        YearID = CNYear,
                        Period_No = i + 1,
                        Period_Start_Date = "",
                        //Period_End_Date = ""
                    };
                    DB.FiscalAdjustment_Tables.Add(fiscalAdjustment_Table);
                    DB.SaveChanges();
                }
            }
            TempData["YearID"] = CNYear;
            return RedirectToAction("FiscalYearPeriods", "FiscalPeriods");
        }


        public JsonResult GetFiscalYearID(string YearID)
        {
            var ID = DB.FiscalDefinition_Tables.Where(x => x.Fiscal_Year_ID.Equals(YearID)).FirstOrDefault();
            if (ID != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckStartDateOverlap(string StartDate, string CNFiscalYearID)
        {

            DateTime Start = Convert.ToDateTime(StartDate);

            var GetAllPeriods = DB.NewFiscalYear_Table.ToList().Where(x => DateTime.Parse(x.Fiscal_Year_Start) <= Start && DateTime.Parse(x.Fiscal_Year_End) >= Start && x.Fiscal_Year_ID == CNFiscalYearID && x.CheckDate == "True").FirstOrDefault();


            if (GetAllPeriods != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("True", JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult CheckEndDateOverlap(string EndDate, string CNFiscalYearID)
        {
            DateTime End = Convert.ToDateTime(EndDate);

            var GetAllPeriods = DB.NewFiscalYear_Table.ToList().Where(x => DateTime.Parse(x.Fiscal_Year_Start) <= End && DateTime.Parse(x.Fiscal_Year_End) >= End && x.Fiscal_Year_ID == CNFiscalYearID && x.CheckDate == "True").FirstOrDefault();


            if (GetAllPeriods != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("True", JsonRequestBehavior.AllowGet);

            }
        }

        public JsonResult GetFiscalYearName(string FiscalYearID)
        {
            return Json(repetitionBusiness.GetFiscalYearName(FiscalYearID), JsonRequestBehavior.AllowGet);
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
        //            item.SCNY = Value;
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
        //        if (item.SCNY.ToString().Equals("True"))
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