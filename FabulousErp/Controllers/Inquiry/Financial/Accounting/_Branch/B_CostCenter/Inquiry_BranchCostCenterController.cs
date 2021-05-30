using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Branch.B_CostCenter
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_BranchCostCenterController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_BranchCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_BranchCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IBCC")]
        public ActionResult BranchCostCenter()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.BranchID = repetitionBusiness.RetrieveBranchIDListCond(companyID);
            }
         
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
                var list = DB.B_CostCenter_Tables.Where(x => x.BranchID == branchID).ToList();
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
        //            item.IBCC = Value;
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
        //        if (item.IBCC.ToString().Equals("True"))
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