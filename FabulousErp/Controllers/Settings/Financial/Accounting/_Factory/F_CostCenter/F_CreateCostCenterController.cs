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
    public class F_CreateCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_CreateCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: F_CreateCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFCC")]
        public ActionResult FactoryCostCenter()
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


        public JsonResult GetFactoryName(string FactoryID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.FactoryID == FactoryID && x.UserID == userID).FirstOrDefault();

            if(checkAccess != null)
            {
                return Json(repetitionBusiness.GetFactoryName(FactoryID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SaveFactoryCostCenter(string FactoryID, string CostCenterID, string CostCenterName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.F_CostCenter_Tables.Where(x => x.F_CostCenterID == CostCenterID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_CostCenter_Table f_CostCenter_Table = new F_CostCenter_Table()
                {
                    FactoryID = FactoryID,

                    F_CostCenterID = CostCenterID,

                    F_CostCenterName = CostCenterName,

                    MoveUserID = userID
                };

                DB.F_CostCenter_Tables.Add(f_CostCenter_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult GetCostCenter(string factoryID)
        {
            IQueryable<Cost_Center_Accounts_DTO> costData = DB.F_CostCenter_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Cost_Center_Accounts_DTO
            {
                CostCenterID = x.F_CostCenterID,

                CostCenterName = x.F_CostCenterName
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
        //            item.SFCC = Value;
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
        //        if (item.SFCC.ToString().Equals("True"))
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