using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CostCenter;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
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
    public class B_UpdateMainCostCenterController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public B_UpdateMainCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: B_UpdateMainCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUBMCC")]
        public ActionResult BranchUpdateGroupCC()
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


        public JsonResult FilterCostCenterGroupIDFrorBranch(string BranchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.B_MainCostCenter_Tables.Where(x => x.BranchID == BranchID).Select(x => new Get_Small_Data_DTO
            {

                CostCenterGroupID = x.B_CostCenterGroupID

            }).ToList();

            return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCCGroupIDForBranch(string CCGroupID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var checkUsed = DB.B_CreateAccount_Tables.Where(x => x.B_CostCenterGroupID == CCGroupID).FirstOrDefault();

            var getCCGrouprName = DB.B_MainCostCenter_Tables.Where(x => x.B_CostCenterGroupID == CCGroupID).FirstOrDefault();

            if (getCCGrouprName != null)
            {

                return Json(getCCGrouprName.B_CostCenterGroupName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
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


        public JsonResult GetGroupCCDataBranch(string BranchCostCenterGroupID)
        {

            List<Main_Cost_Center_DTO> main_Cost_Center_DTOs = DB.B_GroupCostCenter_Tables.Where(x => x.B_CostCenterGroupID == BranchCostCenterGroupID).Select(x => new Main_Cost_Center_DTO
            {

                GroupID = x.GroupID,

                CostCenterID = x.B_CostCenterID,

                CostCenterName = x.B_CostCenter_Table.B_CostCenterName,

                Percentage = x.B_Percentage

            }).ToList();

            return Json(main_Cost_Center_DTOs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateBranchCostGroup(string BranchCostCenterGroupID, List<B_GroupCostCenter_Table> order, List<Main_Cost_Center_DTO> deleted)
        {
            string userID = FabulousErp.Business.GetUserId();

            if (deleted != null)
            {
                foreach (var deletedItems in deleted)
                {
                    var deleteChildData = DB.B_CreateAccountCCAccount_Tables.Where(x => x.B_CostCenterGroupID == BranchCostCenterGroupID && x.B_CostCenterID == deletedItems.CostCenterID).ToList();
                    if (deleteChildData != null)
                    {
                        DB.B_CreateAccountCCAccount_Tables.RemoveRange(deleteChildData);
                        DB.SaveChanges();
                    }

                    var deleteParentData = DB.B_GroupCostCenter_Tables.Where(x => x.B_CostCenterGroupID == BranchCostCenterGroupID && x.B_CostCenterID == deletedItems.CostCenterID).FirstOrDefault();
                    if (deleteParentData != null)
                    {
                        DB.B_GroupCostCenter_Tables.Remove(deleteParentData);
                        DB.SaveChanges();
                    }
                }
            }

            //var deleteOldData = DB.B_GroupCostCenter_Tables.Where(x => x.B_CostCenterGroupID == BranchCostCenterGroupID).ToList();
            //DB.B_GroupCostCenter_Tables.RemoveRange(deleteOldData);
            //DB.SaveChanges();

            foreach (var item in order)
            {
                var updateData = DB.B_GroupCostCenter_Tables.Where(x => x.B_CostCenterGroupID == BranchCostCenterGroupID && x.B_CostCenterID == item.B_CostCenterID).FirstOrDefault();
                if (updateData != null)
                {
                    updateData.B_Percentage = item.B_Percentage;
                    updateData.MoveUserID = userID;
                    DB.SaveChanges();
                }
                else
                {
                    B_GroupCostCenter_Table b_GroupCostCenter_Table = new B_GroupCostCenter_Table()
                    {
                        B_CostCenterGroupID = BranchCostCenterGroupID,

                        B_CostCenterID = item.B_CostCenterID,

                        B_Percentage = item.B_Percentage,

                        MoveUserID = userID
                    };
                    DB.B_GroupCostCenter_Tables.Add(b_GroupCostCenter_Table);
                    DB.SaveChanges();
                }
            }

            return Json("True", JsonRequestBehavior.AllowGet);
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
        //            item.SUBMCC = Value;
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
        //        if (item.SUBMCC.ToString().Equals("True"))
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