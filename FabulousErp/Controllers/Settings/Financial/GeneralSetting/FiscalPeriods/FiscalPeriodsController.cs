using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.FiscalPeriods;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.FiscalPeriods
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class FiscalPeriodsController : Controller
    {
        DBContext DB = new DBContext();

        [HttpGet]
       // [Authorize]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        // GET: FiscalPeriods
        public ActionResult FiscalYearPeriods()
        {

            int? yearID = (int?)TempData["YearID"];
            FiscalDefandPeriods fiscalDefandPeriods = new FiscalDefandPeriods();
            List<Fiscal_Year> Results = new List<Fiscal_Year>();

            if (yearID != null)
            {

                var list = DB.FiscalYear_Tables.Where(x => x.YearID == yearID).ToList();

                foreach (var item in list)
                {
                    Fiscal_Year fiscal_Year = new Fiscal_Year()
                    {
                        ID = item.ID,
                        Fiscal_Year_ID = item.YearID,
                        Fiscal_Year_Name = item.NewFiscalYear_Table.Year,
                        Period_No = item.Period_No,
                        Period_Start_Date = item.Period_Start_Date,
                        Period_End_Date = item.Period_End_Date
                    };
                    Results.Add(fiscal_Year);
                    fiscalDefandPeriods.Fiscal_Year_List = Results;
                }

                var list2 = DB.FiscalAdjustment_Tables.Where(x => x.YearID == yearID).ToList();
                List<Fiscal_Adjustment> Results2 = new List<Fiscal_Adjustment>();
                foreach (var item in list2)
                {
                    Fiscal_Adjustment fiscal_Adjustment = new Fiscal_Adjustment()
                    {
                        ID = item.ID,
                        Fiscal_Year_ID = item.YearID,
                        Fiscal_Year_Name = item.NewFiscalYear_Table.Year,
                        Period_No = item.Period_No,
                        Period_Start_Date = item.Period_Start_Date,
                    };
                    Results2.Add(fiscal_Adjustment);
                    fiscalDefandPeriods.Fiscal_Adjustment_List = Results2;
                }
                return View(fiscalDefandPeriods);
            }
            else
            {
                return RedirectToAction("NewYear", "CreateNewYear");
            }
        }

        [HttpPost]
        public ActionResult FiscalYearPeriod(FiscalDefandPeriods fa)
        {
            int count = 0;
            string start = "";
            string end = "";
            string startAdj = "";
            int? YearId = fa.Fiscal_Year_List.FirstOrDefault().Fiscal_Year_ID;
            foreach (var checkDatesAdj in fa.Fiscal_Adjustment_List)
            {
                foreach (var checkDatesAdj1 in fa.Fiscal_Adjustment_List.Where(x => x.ID != checkDatesAdj.ID))
                {
                    if (DateTime.Parse(checkDatesAdj1.Period_Start_Date) == DateTime.Parse(checkDatesAdj.Period_Start_Date))
                    {
                        count++;
                        startAdj = checkDatesAdj1.Period_Start_Date;
                        goto Foo;
                    }
                }
            }

            foreach (var checkDates in fa.Fiscal_Year_List)
            {
                foreach (var checkDates1 in fa.Fiscal_Year_List.Where(x => x.ID != checkDates.ID))
                {
                    if (CheckEnterDate(checkDates, checkDates1))
                    {
                        count++;
                        start = checkDates1.Period_Start_Date;
                        end = checkDates1.Period_End_Date;
                        goto Foo;
                    }
                }
            }
            Foo:
            Console.WriteLine("");
            if (count > 0)
            {
                if (ModelState.IsValid)
                {
                    UpdateCheckDatesInNew("False", YearId);

                    foreach (var list in fa.Fiscal_Year_List)
                    {
                        var item = DB.FiscalYear_Tables.Where(x => x.ID == list.ID).FirstOrDefault();
                        if (item != null)
                        {
                            item.CheckDate = "False";
                        }
                        DB.SaveChanges();
                    }
                    foreach (var list in fa.Fiscal_Adjustment_List)
                    {

                        var item = DB.FiscalAdjustment_Tables.Where(x => x.ID == list.ID).FirstOrDefault();
                        if (item != null)
                        {
                            item.CheckDate = "False";
                        }
                        DB.SaveChanges();


                    }
                    OverlappingError overlappingError = new OverlappingError()
                    {
                        Message = "There Exist Date OverLapping in : ",
                        Start = start,
                        End = end,
                        StartAdj = startAdj
                    };
                    return Json(overlappingError, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (ModelState.IsValid)
                {

                    UpdateCheckDatesInNew("True", YearId);

                    foreach (var list in fa.Fiscal_Year_List)
                    {
                        var item = DB.FiscalYear_Tables.Where(x => x.ID == list.ID).FirstOrDefault();
                        if (item != null)
                        {
                            item.Period_Start_Date = list.Period_Start_Date;
                            item.Period_End_Date = list.Period_End_Date;
                            item.CheckDate = "True";
                        }
                        DB.SaveChanges();
                    }
                    foreach (var list in fa.Fiscal_Adjustment_List)
                    {

                        var item = DB.FiscalAdjustment_Tables.Where(x => x.ID == list.ID).FirstOrDefault();
                        if (item != null)
                        {
                            item.Period_Start_Date = list.Period_Start_Date;
                            item.CheckDate = "True";
                        }
                        DB.SaveChanges();

                    }
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckDate(string Date, int YearID)
        {
            //6
            DateTime StartDate = new DateTime();

            DateTime EndDate = new DateTime();

            var list = DB.NewFiscalYear_Table.Where(x => x.YearID == YearID).FirstOrDefault();

            if (list != null)
            {
                StartDate = Convert.ToDateTime(list.Fiscal_Year_Start);

                EndDate = Convert.ToDateTime(list.Fiscal_Year_End);
            }
            if (Date.Length > 0)
            {
                if (Convert.ToDateTime(Date) >= StartDate && Convert.ToDateTime(Date) <= EndDate)
                {
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            //return null;
        }

        public void UpdateCheckDatesInNew(string MSG,int? yearID)
        {
            //7
            var updateCheckDateInNew = DB.NewFiscalYear_Table.Where(x => x.YearID == yearID).FirstOrDefault();
            updateCheckDateInNew.CheckDate = MSG;
            DB.SaveChanges();
        }


        private bool CheckEnterDate(Fiscal_Year checkDates, Fiscal_Year checkDates1)
        {
            DateTime startDate = DateTime.Parse(checkDates.Period_Start_Date);
            DateTime endDate = DateTime.Parse(checkDates.Period_End_Date);

            DateTime startDate1 = DateTime.Parse(checkDates1.Period_Start_Date);
            DateTime endDate1 = DateTime.Parse(checkDates1.Period_End_Date);


            return (startDate1 <= endDate && startDate1 >= startDate) || (endDate1 <= endDate && endDate1 >= startDate);
        }


    }
}