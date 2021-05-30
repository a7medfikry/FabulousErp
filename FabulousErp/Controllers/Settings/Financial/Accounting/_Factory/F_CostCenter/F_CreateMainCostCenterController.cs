using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CostCenter;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CostCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_CostCenter
{
    [AuthorizationFilter]
    public class F_CreateMainCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_CreateMainCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: F_CreateMainCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFMCC")]
        public ActionResult FactoryMainCostCenter()
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


        public JsonResult FilterCostCenterIDForFactory(string FactoryID)
        {
            return Json(repetitionBusiness.FilterCostCenterIDForFactory(FactoryID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFactoryCostCenter(string CostCenterID)
        {
            return Json(repetitionBusiness.GetFactoryCostCenterName(CostCenterID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckDuplicateCostCenterGroupIDFactory(string CostCenterGroupID)
        {

            var check = DB.F_MainCostCenter_Tables.Where(x => x.F_CostCenterGroupID == CostCenterGroupID).FirstOrDefault();

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public ActionResult SaveFactoryCostGroup(string CostCenterGroupID, string CostCenterGroupName, string FactoryID, F_GroupCostCenter_Table[] order)
        {

            string userID = FabulousErp.Business.GetUserId();

            F_MainCostCenter_Table f_MainCostCenter_Table = new F_MainCostCenter_Table()
            {
                F_CostCenterGroupID = CostCenterGroupID,

                F_CostCenterGroupName = CostCenterGroupName,

                FactoryID = FactoryID,

                MoveUserID = userID
            };
            DB.F_MainCostCenter_Tables.Add(f_MainCostCenter_Table);
            DB.SaveChanges();

            foreach (var item in order)
            {
                F_GroupCostCenter_Table f_GroupCostCenter_Table = new F_GroupCostCenter_Table()
                {
                    F_CostCenterGroupID = CostCenterGroupID,

                    F_CostCenterID = item.F_CostCenterID,

                    F_Percentage = item.F_Percentage,

                    MoveUserID = userID
                };
                DB.F_GroupCostCenter_Tables.Add(f_GroupCostCenter_Table);
                DB.SaveChanges();
            }

            return Json("True", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSavedMainCC(string factoryID)
        {
            IQueryable<Main_Cost_Center_DTO> main_Cost_Center_DTOs = DB.F_MainCostCenter_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Main_Cost_Center_DTO
            {
                MainCostCenterID = x.F_CostCenterGroupID,

                MainCostCenterName = x.F_CostCenterGroupName
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
        //            item.SFMCC = Value;
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
        //        if (item.SFMCC.ToString().Equals("True"))
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