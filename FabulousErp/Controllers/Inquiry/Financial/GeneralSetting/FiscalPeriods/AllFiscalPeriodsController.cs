using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousErp.MyRoleProvider;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.FiscalPeriods
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class AllFiscalPeriodsController : Controller
    {
        DBContext DB = new DBContext();

        // GET: AllFiscalPeriods
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ISFP")]
        public ActionResult ShowFiscalPeriods()
        {
            FiscalDefandPeriods fiscalDefandPeriods = new FiscalDefandPeriods();

            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var getFiscalYearOfCompany = DB.CompanyFiscalYear_Tables.FirstOrDefault(x => x.CompanyID == companyID);

            var List = DB.NewFiscalYear_Table.Where(x=>x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfCompany.Fiscal_Year_ID).ToList();
            SelectList FiscalYear = new SelectList(List, "Year", "Year");
            ViewBag.CodeList = FiscalYear;

            var list2 = DB.FiscalYear_Tables.Where(x=> x.CheckDate=="True" && x.NewFiscalYear_Table.Fiscal_Year_ID == getFiscalYearOfCompany.Fiscal_Year_ID).ToList();
            List<Fiscal_Year> Results = new List<Fiscal_Year>();
            foreach(var item in list2)
            {
                Fiscal_Year fiscal_Year = new Fiscal_Year()
                {
                    //Fiscal_Year_ID = item.Fiscal_Year_ID,
                    Fiscal_Year_Name = item.NewFiscalYear_Table.Year,
                    Period_No = item.Period_No,
                    Period_Start_Date = item.Period_Start_Date,
                    Period_End_Date = item.Period_End_Date
                };
                Results.Add(fiscal_Year);
            }
            fiscalDefandPeriods.Fiscal_Year_List = Results;

            var list3 = DB.FiscalAdjustment_Tables.Where(x => x.CheckDate == "True" && x.NewFiscalYear_Table.Fiscal_Year_ID == getFiscalYearOfCompany.Fiscal_Year_ID).ToList();
            List<Fiscal_Adjustment> Results2 = new List<Fiscal_Adjustment>();
            foreach(var item in list3)
            {
                Fiscal_Adjustment fiscal_Adjustment = new Fiscal_Adjustment()
                {
                    //Fiscal_Year_ID = item.Fiscal_Year_ID,
                    Fiscal_Year_Name = item.NewFiscalYear_Table.Year,
                    Period_No = item.Period_No,
                    Period_Start_Date = item.Period_Start_Date
                    //Period_End_Date = item.Period_End_Date
                };
                Results2.Add(fiscal_Adjustment);
            }
            fiscalDefandPeriods.Fiscal_Adjustment_List = Results2;
            return View(fiscalDefandPeriods);
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
        //            item.ISFP = Value;
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
        //        if (item.ISFP.ToString().Equals("True"))
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