using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Branch.B_Analytic
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_BranchAnalyticDistributionController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_BranchAnalyticDistributionController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: Inquiry_BranchAnalyticDistribution
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IBADA")]
        public ActionResult BranchAnalyticDistribution()
        {
            ViewBag.BranchName = "";
            return View();
        }

        public JsonResult GetBranchName(string branchID)
        {
            var list = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == branchID).FirstOrDefault();
            if (list != null)
            {
                string BranchName = list.BranchName;
                return Json(BranchName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetAnalyticAccountID(string branchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UABranchPremission_Tables.Where(x => x.UserID == userID && x.BranchID == branchID).FirstOrDefault();
            if (check != null)
            {
                List<Inquiry_DTO> inquiry_DTO = DB.B_AnalyticAccount_Tables.Where(x => x.BranchID == branchID).Select(x => new Inquiry_DTO
                {
                    Analytic_AccountID = x.B_AnalyticAccountID
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
            var list = DB.B_AnalyticAccount_Tables.Where(x => x.B_AnalyticAccountID == AnalyticID).FirstOrDefault();
            if (list != null)
            {
                string AnalyticName = list.B_AnalyticAccountName;
                return Json(AnalyticName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string AnalyticID)
        {
            List<Inquiry_DTO> inquiry_DTO = DB.B_AnalyticDistribution_Tables.Where(x => x.B_AnalyticAccountID == AnalyticID).Select(x => new Inquiry_DTO
            {
                AnalyticDistribution_ID = x.B_AccountDistributionID,
                AnalyticDistribution_Name = x.B_AccountDistributionName
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
        //            item.IBADA = Value;
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
        //        if (item.IBADA.ToString().Equals("True"))
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