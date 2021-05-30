using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Factory.F_Analytic
{
    [AuthorizationFilter]
    public class Inquiry_FactoryAnalyticDistributionController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_FactoryAnalyticDistributionController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_FactoryAnalyticDistribution
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IFADA")]
        public ActionResult FactoryAnalyticDistribution()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.CodeList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
          
            return View();
        }

        public JsonResult GetFactoryName(string factoryID)
        {
            var list = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factoryID).FirstOrDefault();
            if (list != null)
            {
                string FactoryName = list.FactoryName;
                return Json(FactoryName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetAnalyticAccountID(string factoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == factoryID).FirstOrDefault();
            if (check != null)
            {
                List<Inquiry_DTO> inquiry_DTO = DB.F_AnalyticAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Inquiry_DTO
                {
                    Analytic_AccountID = x.F_AnalyticAccountID
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
            var list = DB.F_AnalyticAccount_Tables.Where(x => x.F_AnalyticAccountID == AnalyticID).FirstOrDefault();
            if (list != null)
            {
                string AnalyticName = list.F_AnalyticAccountName;
                return Json(AnalyticName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string AnalyticID)
        {
            List<Inquiry_DTO> inquiry_DTO = DB.F_AnalyticDistribution_Tables.Where(x => x.F_AnalyticAccountID == AnalyticID).Select(x => new Inquiry_DTO
            {
                AnalyticDistribution_ID = x.F_AccountDistributionID,
                AnalyticDistribution_Name = x.F_AccountDistributionName
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
        //            item.IFADA = Value;
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
        //        if (item.IFADA.ToString().Equals("True"))
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