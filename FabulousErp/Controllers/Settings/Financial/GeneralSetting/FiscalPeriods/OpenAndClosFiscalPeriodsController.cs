using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.Models;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.FiscalPeriods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.FiscalPeriods
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class OpenAndClosFiscalPeriodsController : Controller
    {

        DBContext DB = new DBContext();
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SOCP")]
        // GET: OpenAndClosFiscalPeriods
        public ActionResult OpenClosePeriods()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var getFiscalYearID = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).ToList().DefaultIfEmpty(new CompanyFiscalYear_Table { Fiscal_Year_ID="0"}).FirstOrDefault();
            
            var yearName = DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == getFiscalYearID.Fiscal_Year_ID && x.CheckDate == "True" && x.Closed!=true).ToList();
            SelectList YearList = new SelectList(yearName, "YearID", "Year");
            ViewBag.YearList = YearList;
            return View();
        }
        public JsonResult GetOpenClosePeriods(int Year)
        {
            //ReturnHERE
            List<Open_Close_Periods_DTO> open_Close_Periods = DB.FiscalYear_Tables
                .Include(x=>x.Fiscal_year_area)
                .Where(x => x.YearID == Year).Select(x => new Open_Close_Periods_DTO
            {
                ID = x.ID,
                Period_No = x.Period_No,
                Period_Start_Date = x.Period_Start_Date,
                Period_End_Date = x.Period_End_Date,
                AreaNames= x.Fiscal_year_area
                //Financial = x.Financial,
                //Inventory = x.Inventory,
                //Purchasing = x.Purchasing,
                //Sales = x.Sales
            }).ToList();
            open_Close_Periods.ForEach(x => x.AreaNames.ToList().ForEach(z => z.FiscalAdjustment_Table = null));
            foreach (Open_Close_Periods_DTO i in open_Close_Periods)
            {
                foreach (var ii in Business.GetAreasNames())
                {
                    if (!i.AreaNames.Any(x => x.Area_name == ii))
                    {
                        i.AreaNames.Add(new Fiscal_year_area
                        {
                            Area_name = ii,
                            Allowed = false,
                            Allow_adjust = false
                        });
                    }
                }
            }
            return Json(open_Close_Periods, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePeriodsOAndC(FiscalYear_Table[] FiscalYear)
        {
            //ReturnHERE
            foreach (var item in FiscalYear)
            {
                var getData = DB.FiscalYear_Tables.Where(x => x.ID == item.ID).FirstOrDefault();
                getData.Period_No = item.Period_No;
                getData.Period_Start_Date = item.Period_Start_Date;
                getData.Period_End_Date = item.Period_End_Date;

                foreach (Fiscal_year_area Area in item.Fiscal_year_area)
                {
                    if (DB.Fiscal_year_area.Any(x => x.Area_name == Area.Area_name
                                && (x.FiscalYear_Table_id== getData.ID || x.FiscalYear_Table_id == null)))
                    {
                        DB.Fiscal_year_area.
                            FirstOrDefault(x => x.Area_name == Area.Area_name
                             && (x.FiscalYear_Table_id == getData.ID || x.FiscalYear_Table_id == null))
                            .FiscalYear_Table_id = getData.ID;
                        
                        DB.Fiscal_year_area.
                            FirstOrDefault(x => x.Area_name == Area.Area_name
                             && (x.FiscalYear_Table_id == getData.ID || x.FiscalYear_Table_id == null))
                            .Allowed = Area.Allowed;
                    }
                    else
                    {
                        Area.FiscalYear_Table_id = getData.ID;

                        DB.Fiscal_year_area.Add(Area);
                    }
                    DB.SaveChanges();
                }
                //getData.Financial = item.Financial;
                //getData.Purchasing = item.Purchasing;
                //getData.Sales = item.Sales;
                //getData.Inventory = item.Inventory;
            }
            DB.SaveChanges();
            return Json("Successfully Saved..", JsonRequestBehavior.AllowGet);
        }



        public ActionResult AdjOpenClosePeriods()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var getFiscalYearID = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            var yearName = DB.NewFiscalYear_Table.Where(x => x.Fiscal_Year_ID == getFiscalYearID.Fiscal_Year_ID).ToList();
            SelectList YearList = new SelectList(yearName, "YearID", "Year");
            ViewBag.YearList = YearList;
            return View();
        }
        public JsonResult GetOpenCloseAdjusmentPeriods(int Year)
        {
            //ReturnHERE
            List<Open_Close_Periods_DTO> open_Close_Periods = DB.FiscalAdjustment_Tables
                .Include(x=>x.Fiscal_year_area)
                .Where(x => x.YearID == Year).ToList().Select(x => new Open_Close_Periods_DTO
            {
                ID = x.ID,
                Period_No = x.Period_No,
                Period_Start_Date = x.Period_Start_Date,
                //Financial = x.Financial,
                //Inventory = x.Inventory,
                //Purchasing = x.Purchasing,
                //Sales = x.Sales
                AreaNames=x.Fiscal_year_area
            }).ToList();
            open_Close_Periods.ForEach(x => x.AreaNames.ToList().ForEach(z => z.FiscalAdjustment_Table = null));
            foreach (Open_Close_Periods_DTO i in open_Close_Periods)
            {
                foreach (var ii in Business.GetAreasNames())
                {
                    if (!i.AreaNames.Any(x => x.Area_name == ii))
                    {
                        i.AreaNames.Add(new Fiscal_year_area
                        {
                            Area_name = ii,
                            Allowed=false,
                            Allow_adjust=false
                        }) ;
                    }
                }
            }
            return Json(open_Close_Periods.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveAdjOAndC(FiscalAdjustment_Table[] AdjusmentFiscalYear)
        {
            //ReturnHERE
            foreach (var adjItem in AdjusmentFiscalYear)
            {
                var getData = DB.FiscalAdjustment_Tables.Where(x => x.ID == adjItem.ID).FirstOrDefault();
                getData.Period_No = adjItem.Period_No;
                getData.Period_Start_Date = adjItem.Period_Start_Date;
                //getData.Financial = adjItem.Financial;
                //getData.Purchasing = adjItem.Purchasing;
                //getData.Sales = adjItem.Sales;
                //getData.Inventory = adjItem.Inventory;
                foreach (Fiscal_year_area Area in adjItem.Fiscal_year_area)
                {
                    if (DB.Fiscal_year_area.Any(x => x.Area_name == Area.Area_name
                          && (x.FiscalAdjustment_Table_id== getData.ID || x.FiscalAdjustment_Table_id ==null)))
                    {

                        DB.Fiscal_year_area.
                            FirstOrDefault(x => x.Area_name == Area.Area_name
                            && (x.FiscalAdjustment_Table_id == getData.ID || x.FiscalAdjustment_Table_id == null))
                            .FiscalAdjustment_Table_id = getData.ID;

                        DB.Fiscal_year_area.
                            FirstOrDefault(x => x.Area_name == Area.Area_name
                             && (x.FiscalAdjustment_Table_id == getData.ID || x.FiscalAdjustment_Table_id == null))
                            .Allow_adjust = Area.Allow_adjust;
                    }
                    else
                    {
                        Area.FiscalAdjustment_Table_id = getData.ID;
                        DB.Fiscal_year_area.Add(Area);
                    }
                    DB.SaveChanges();
                }
            }
            DB.SaveChanges();
            return Json("Successfully Saved..", JsonRequestBehavior.AllowGet);
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
        //            item.SOCP = Value;
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
        //        if (item.SOCP.ToString().Equals("True"))
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