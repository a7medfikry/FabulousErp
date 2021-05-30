using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Branch.B_Analytic
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_CreateAnalyticAccountsController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public B_CreateAnalyticAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_CreateAnalyticAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBAA")]
        public ActionResult BranchAnalyticAccounts()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;

            if (companyID != null)
            {
                ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDListCond(companyID);
            }
            else if(branchID != null)
            {
                ViewBag.BranchList = branchID;

                ViewBag.BranchName = repetitionBusiness.GetBranchName(branchID);
            }

            return View();
        }

        public JsonResult GetBranchName(string BranchID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.BranchID == BranchID && x.UserID == userID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddAnalyticAccounts(string BranchID, string AnalyticID, string AnalyticName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.B_AnalyticAccount_Tables.Where(x => x.B_AnalyticAccountID == AnalyticID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_AnalyticAccount_Table analyticAccount_Table = new B_AnalyticAccount_Table()
                {

                    B_AnalyticAccountID = AnalyticID,

                    B_AnalyticAccountName = AnalyticName,

                    BranchID = BranchID,

                    MoveUserID = userID

                };

                DB.B_AnalyticAccount_Tables.Add(analyticAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult GetAnalyticAccounts(string branchID)
        {
            IQueryable<Analytic_To_Account_DTO> analyticData = DB.B_AnalyticAccount_Tables.Where(x => x.BranchID == branchID).Select(x => new Analytic_To_Account_DTO
            {
                AnalyticAccountID = x.B_AnalyticAccountID,

                AnalyticAccountName = x.B_AnalyticAccountName
            });

            return Json(analyticData, JsonRequestBehavior.AllowGet);
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
        //            item.SBAA = Value;
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
        //        if (item.SBAA.ToString().Equals("True"))
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