using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Company.C_Analytic
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CompanyAnalyticDistributionController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_CompanyAnalyticDistributionController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: Inquiry_CompanyAnalyticDistributionController
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICADA")]
        public ActionResult CompanyAnalyticDistribution()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.CompanyID = companyID;
            string companyName = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).Select(x => x.CompanyName).First().ToString();
            ViewBag.CompanyName = companyName;
            return View();
        }

        public JsonResult GetAnalyticAccountID(string companyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UACompPremission_Tables.Where(x => x.UserID == userID && x.CompanyID == companyID).FirstOrDefault();
            if (check != null)
            {
                List<Inquiry_DTO> inquiry_DTO = DB.C_AnalyticAccount_Tables.Where(x => x.CompanyID == companyID).Select(x => new Inquiry_DTO
                {
                    Analytic_AccountID = x.C_AnalyticAccountID
                }).ToList();

                return Json(inquiry_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAnalyticAccountName(string AnalyticID)
        {
            var list = DB.C_AnalyticAccount_Tables.Where(x => x.C_AnalyticAccountID == AnalyticID).FirstOrDefault();
            if (list != null)
            {
                string AnalyticName = list.C_AnalyticAccountName;
                return Json(AnalyticName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string AnalyticID)
        {
            List<Inquiry_DTO> inquiry_DTO = DB.C_AnalyticDistribution_Tables.Where(x => x.C_AnalyticAccountID == AnalyticID).Select(x => new Inquiry_DTO
            {
                AnalyticDistribution_ID = x.C_AccountDistributionID,
                AnalyticDistribution_Name = x.C_AccountDistributionName
            }).ToList();
            return Json(inquiry_DTO, JsonRequestBehavior.AllowGet);
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
        //            item.ICADA = Value;
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
        //        if (item.ICADA.ToString().Equals("True"))
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