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
    public class B_CreateCostCenterAccountsController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_CreateCostCenterAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_CreateCostCenterAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBCCA")]
        public ActionResult BranchCCAccounts()
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


        public JsonResult GetBranchCCAccountsData(string BranchCostCenterID)
        {

            List<Cost_Center_Accounts_DTO> cost_Center_Accounts = DB.B_CostCenterAccounts_Tables.Where(x => x.B_CostCenterID == BranchCostCenterID).Select(x => new Cost_Center_Accounts_DTO
            {

                CostAccountID = x.B_CostAccountID,

                CostAccountName = x.B_CostAccountName

            }).ToList();

            return Json(cost_Center_Accounts, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveRecordBranchCCAccounts(string CostCenterID, string CostAccountID, string CostAccountName)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.B_CostCenterAccounts_Tables.Where(x => x.B_CostAccountID == CostAccountID && x.B_CostCenterID == CostCenterID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_CostCenterAccounts_Table b_CostCenterAccounts_Table = new B_CostCenterAccounts_Table()
                {
                    B_CostCenterID = CostCenterID,

                    B_CostAccountID = CostAccountID,

                    B_CostAccountName = CostAccountName,

                    MoveUserID = userID
                };

                DB.B_CostCenterAccounts_Tables.Add(b_CostCenterAccounts_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult DeleteCostAccountBranch(string CCAccountID)
        {
            var deleteCostAccount = DB.B_CostCenterAccounts_Tables.Where(x => x.B_CostAccountID == CCAccountID).FirstOrDefault();

            DB.B_CostCenterAccounts_Tables.Remove(deleteCostAccount);
            DB.SaveChanges();

            return null;
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
        //            item.SBCCA = Value;
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
        //        if (item.SBCCA.ToString().Equals("True"))
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