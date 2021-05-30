using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.FiscalPeriods;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.FiscalPeriods
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CompanyFiscalYearController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public CompanyFiscalYearController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();
        // GET: CompanyFiscalYear
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCFY")]
        public ActionResult AddCompanyFiscalYear()
        {
            ViewBag.CompanyID = repetitionBusiness.RetrieveCompIDList().Where(x=>x.Value==Business.GetCompanyId());

            ViewBag.FiscalYearID = repetitionBusiness.RetrieveFiscalYearIDList();

            return View();
        }


        public ActionResult AssignCompanyFiscalYear(string CompanyID, string FiscalYearID)
        {
            var check = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {

                CompanyFiscalYear_Table companyFiscalYear_Table = new CompanyFiscalYear_Table()
                {
                    CompanyID = CompanyID,

                    Fiscal_Year_ID = FiscalYearID
                };

                DB.CompanyFiscalYear_Tables.Add(companyFiscalYear_Table);
                DB.SaveChanges();

                return Json("True", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetCompanyID(string CompanyID)
        {

            var check = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();
            if (check != null)
            {
                Company_FY_DTO company_FY_DTO = new Company_FY_DTO()
                {
                    CompanyName = check.CompanyMainInfo_Table.CompanyName,

                    FiscalYearID = check.Fiscal_Year_ID,

                    FiscalYearName = check.FiscalDefinition_Table.Fiscal_Year_Name
                };

                return Json(company_FY_DTO, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
            }
            //if (check != null)
            //{
            //    return Json("True", JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
            //}

        }

        public JsonResult GetFiscalYearID(string FiscalyearID)
        {
            return Json(repetitionBusiness.GetFiscalYearName(FiscalyearID), JsonRequestBehavior.AllowGet);
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
        //            item.SCFY = Value;
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
        //            if (item.SCFY.ToString().Equals("True"))
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