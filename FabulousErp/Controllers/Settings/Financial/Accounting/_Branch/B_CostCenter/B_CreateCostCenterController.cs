using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CostCenter;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CostCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Branch.B_CostCenter
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_CreateCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_CreateCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_CreateCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBCC")]
        public ActionResult BranchCostCenter()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;


            if (companyID != null)
            {
                ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDListCond(companyID);
            }
            else if (branchID != null)
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

            if(checkAccess != null)
            {
                return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SaveBranchCostCenter(string BranchID, string CostCenterID, string CostCenterName)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.B_CostCenter_Tables.Where(x => x.B_CostCenterID == CostCenterID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_CostCenter_Table b_CostCenter_Table = new B_CostCenter_Table()
                {
                    BranchID = BranchID,

                    B_CostCenterID = CostCenterID,

                    B_CostCenterName = CostCenterName,

                    MoveUserID = userID
                };

                DB.B_CostCenter_Tables.Add(b_CostCenter_Table);
                DB.SaveChanges();

                return null;
            }
        }

        public JsonResult GetCostCenter(string branchID)
        {
            IQueryable<Cost_Center_Accounts_DTO> costData = DB.B_CostCenter_Tables.Where(x => x.BranchID == branchID).Select(x => new Cost_Center_Accounts_DTO
            {
                CostCenterID = x.B_CostCenterID,

                CostCenterName = x.B_CostCenterName
            });

            return Json(costData, JsonRequestBehavior.AllowGet);
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
        //            item.SBCC = Value;
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
        //        if (item.SBCC.ToString().Equals("True"))
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