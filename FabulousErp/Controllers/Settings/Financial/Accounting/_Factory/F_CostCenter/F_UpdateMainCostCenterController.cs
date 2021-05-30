using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CostCenter;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CostCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_CostCenter
{
    [AuthorizationFilter]
    public class F_UpdateMainCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_UpdateMainCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: F_UpdateMainCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SUFMCC")]
        public ActionResult FactoryUpdateGroupCC()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;

            string factoryID = null;

            if (companyID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
            else if (branchID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCondByB(branchID);
            }
            else if (factoryID != null)
            {
                ViewBag.FactoryList = factoryID;

                ViewBag.FactoryName = repetitionBusiness.GetFactoryName(factoryID);
            }

            return View();
        }

        public JsonResult GetFatoryName(string FactoryID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.FactoryID == FactoryID && x.UserID == userID).FirstOrDefault();

            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetFactoryName(FactoryID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult FilterCostCenterGroupIDFrorFactory(string FactoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            List<Get_Small_Data_DTO> get_Small_Data_DTO = DB.F_MainCostCenter_Tables.Where(x => x.FactoryID == FactoryID).Select(x => new Get_Small_Data_DTO
            {

                CostCenterGroupID = x.F_CostCenterGroupID

            }).ToList();

            return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCCGroupIDForFactory(string CCGroupID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            var getCCGrouprName = DB.F_MainCostCenter_Tables.Where(x => x.F_CostCenterGroupID == CCGroupID).FirstOrDefault();

            if (getCCGrouprName != null)
            {

                return Json(getCCGrouprName.F_CostCenterGroupName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public JsonResult FilterCostCenterIDForFactory(string FactoryID)
        {
            return Json(repetitionBusiness.FilterCostCenterIDForFactory(FactoryID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFactoryCostCenter(string CostCenterID)
        {
            return Json(repetitionBusiness.GetFactoryCostCenterName(CostCenterID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetGroupCCDataFactory(string FactoryCostCenterGroupID)
        {

            List<Main_Cost_Center_DTO> main_Cost_Center_DTOs = DB.F_GroupCostCenter_Tables.Where(x => x.F_CostCenterGroupID == FactoryCostCenterGroupID).Select(x => new Main_Cost_Center_DTO
            {

                GroupID = x.GroupID,

                CostCenterID = x.F_CostCenterID,

                CostCenterName = x.F_CostCenter_Table.F_CostCenterName,

                Percentage = x.F_Percentage

            }).ToList();

            return Json(main_Cost_Center_DTOs, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateFactoryCostGroup(string FactoryCostCenterGroupID, F_GroupCostCenter_Table[] order, List<Main_Cost_Center_DTO> deleted)
        {
            if (deleted != null)
            {
                foreach (var deletedItems in deleted)
                {
                    var deleteChildData = DB.F_CreateAccountCCAccount_Tables.Where(x => x.F_CostCenterGroupID == FactoryCostCenterGroupID && x.F_CostCenterID == deletedItems.CostCenterID).ToList();
                    if (deleteChildData != null)
                    {
                        DB.F_CreateAccountCCAccount_Tables.RemoveRange(deleteChildData);
                        DB.SaveChanges();
                    }

                    var deleteParentData = DB.F_GroupCostCenter_Tables.Where(x => x.F_CostCenterGroupID == FactoryCostCenterGroupID && x.F_CostCenterID == deletedItems.CostCenterID).FirstOrDefault();
                    if (deleteParentData != null)
                    {
                        DB.F_GroupCostCenter_Tables.Remove(deleteParentData);
                        DB.SaveChanges();
                    }
                }
            }

            //var deleteOldData = DB.F_GroupCostCenter_Tables.Where(x => x.F_CostCenterGroupID == FactoryCostCenterGroupID).ToList();
            //DB.F_GroupCostCenter_Tables.RemoveRange(deleteOldData);
            //DB.SaveChanges();

            string userID = FabulousErp.Business.GetUserId();

            foreach (var item in order)
            {
                var updateData = DB.F_GroupCostCenter_Tables.Where(x => x.F_CostCenterGroupID == FactoryCostCenterGroupID && x.F_CostCenterID == item.F_CostCenterID).FirstOrDefault();
                if (updateData != null)
                {
                    updateData.F_Percentage = item.F_Percentage;
                    updateData.MoveUserID = userID;
                    DB.SaveChanges();
                }
                else
                {
                    F_GroupCostCenter_Table f_GroupCostCenter_Table = new F_GroupCostCenter_Table()
                    {
                        F_CostCenterGroupID = FactoryCostCenterGroupID,

                        F_CostCenterID = item.F_CostCenterID,

                        F_Percentage = item.F_Percentage,

                        MoveUserID = userID
                    };
                    DB.F_GroupCostCenter_Tables.Add(f_GroupCostCenter_Table);
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
        //            item.SUFMCC = Value;
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
        //        if (item.SUFMCC.ToString().Equals("True"))
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