using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Branch.B_Analytic
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_BranchAnalyticController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public Inquiry_BranchAnalyticController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_BranchAnalytic
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IBAA")]
        public ActionResult BranchAnalyticAccount()
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

        public JsonResult GetData(string branchID)
        {

            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UABranchPremission_Tables.Where(x => x.UserID == userID && x.BranchID == branchID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.B_AnalyticAccount_Tables.Where(x => x.BranchID == branchID).ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }

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
        //            item.IBAA = Value;
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
        //        if (item.IBAA.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}

        public static object BranchID()
        {
            Repository.Business.RSelectBusiness u = new Repository.Business.RSelectBusiness();
            object branchID = null;
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                branchID = u.RetrieveBranchIDListCond(companyID);
            }
        
            return branchID;
        }



    }
}