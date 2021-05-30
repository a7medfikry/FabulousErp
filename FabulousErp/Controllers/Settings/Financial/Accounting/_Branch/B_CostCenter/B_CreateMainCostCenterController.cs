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
    public class B_CreateMainCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_CreateMainCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_CreateMainCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBMCC")]
        public ActionResult BranchMainCostCenter()
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

            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.BranchID == BranchID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetBranchName(BranchID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult FilterCostCenterIDForBranch(string BranchID)
        {
            return Json(repetitionBusiness.FilterCostCenterIDForBranch(BranchID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetBranchCostCenter(string CostCenterID)
        {
            return Json(repetitionBusiness.GetBranchCostCenterName(CostCenterID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckDuplicateCostCenterGroupIDBranch(string CostCenterGroupID)
        {

            var check = DB.B_MainCostCenter_Tables.Where(x => x.B_CostCenterGroupID == CostCenterGroupID).FirstOrDefault();

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public ActionResult SaveBranchCostGroup(string CostCenterGroupID, string CostCenterGroupName, string BranchID, B_GroupCostCenter_Table[] order)
        {

            string userID = FabulousErp.Business.GetUserId();

            B_MainCostCenter_Table b_MainCostCenter_Table = new B_MainCostCenter_Table()
            {
                B_CostCenterGroupID = CostCenterGroupID,

                B_CostCenterGroupName = CostCenterGroupName,

                BranchID = BranchID,

                MoveUserID = userID
            };
            DB.B_MainCostCenter_Tables.Add(b_MainCostCenter_Table);
            DB.SaveChanges();

            foreach (var item in order)
            {
                B_GroupCostCenter_Table b_GroupCostCenter_Table = new B_GroupCostCenter_Table()
                {
                    B_CostCenterGroupID = CostCenterGroupID,

                    B_CostCenterID = item.B_CostCenterID,

                    B_Percentage = item.B_Percentage,

                    MoveUserID = userID
                };
                DB.B_GroupCostCenter_Tables.Add(b_GroupCostCenter_Table);
                DB.SaveChanges();
            }

            return Json("True", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSavedMainCC(string branchID)
        {
            IQueryable<Main_Cost_Center_DTO> main_Cost_Center_DTOs = DB.B_MainCostCenter_Tables.Where(x => x.BranchID == branchID).Select(x => new Main_Cost_Center_DTO
            {
                MainCostCenterID = x.B_CostCenterGroupID,

                MainCostCenterName = x.B_CostCenterGroupName
            });

            return Json(main_Cost_Center_DTOs, JsonRequestBehavior.AllowGet);
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
        //            item.SBMCC = Value;
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
        //        if (item.SBMCC.ToString().Equals("True"))
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