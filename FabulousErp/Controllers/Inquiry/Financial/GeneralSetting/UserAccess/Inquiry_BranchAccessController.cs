using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.UserAccess
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_BranchAccessController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public Inquiry_BranchAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_BranchAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IBA")]
        public ActionResult BranchAccess()
        {
            ViewBag.CodeList = repetitionBusiness.RetrieveBranchIDList();
            return View();
        }

        public JsonResult GetData()
        {
            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UABranchPremission_Tables.Where(x => x.UserID == userID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.UABranchPremission_Tables.ToList();
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBranchAccessData(string branchID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var check = DB.UABranchPremission_Tables.Where(x => x.BranchID == branchID && x.UserID == userID).FirstOrDefault();
            if (check != null)
            {
                var list = DB.UABranchPremission_Tables.Where(x => x.BranchID == branchID).ToList();
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
        //            item.IBA = Value;
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
        //        if (item.IBA.ToString().Equals("True"))
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