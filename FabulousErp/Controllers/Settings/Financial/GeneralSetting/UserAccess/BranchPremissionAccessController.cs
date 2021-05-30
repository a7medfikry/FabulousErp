using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.UserAccess
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class BranchPremissionAccessController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public BranchPremissionAccessController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: BranchPremissionAccess
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUABP")]
        public ActionResult UABranchPremission()
        {

            ViewBag.UserList = repetitionBusiness.RetrieveUserIDList();

            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDList();

            return View();
        }

        public JsonResult GetUserName(string UserID)
        {
            return Json(repetitionBusiness.GetUserName(UserID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyName(string CompanyID)
        {
            return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterBranchID(string CompanyID)
        {
            return Json(repetitionBusiness.FilterBranchID(CompanyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchName(string BranchID)
        {
            return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddUserToBranch(string UserID, string UserName, string CompanyID, string CompanyName, string BranchID, string BarnchName)
        {

            var Check = DB.UABranchPremission_Tables.Where(x => x.UserID == UserID && x.CompanyID == CompanyID && x.BranchID == BranchID).FirstOrDefault();

            if (Check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                UABranchPremission_Table uABranchPremission_Table = new UABranchPremission_Table()
                {
                    UserID = UserID,

                    UserName = UserName,

                    CompanyID = CompanyID,

                    CompanyName = CompanyName,

                    BranchID = BranchID,

                    BranchName = BarnchName
                };

                DB.UABranchPremission_Tables.Add(uABranchPremission_Table);
                DB.SaveChanges();

                return Json(JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUserBranchAccess(string UserID)
        {

            List<UABranchPremission_Table> uABranchPremissions = new List<UABranchPremission_Table>();

            uABranchPremissions = DB.UABranchPremission_Tables.Where(x => x.UserID == UserID).ToList();


            return Json(uABranchPremissions, JsonRequestBehavior.AllowGet);
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
        //            item.SUABP = Value;
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
        //        if (item.SUABP.ToString().Equals("True"))
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