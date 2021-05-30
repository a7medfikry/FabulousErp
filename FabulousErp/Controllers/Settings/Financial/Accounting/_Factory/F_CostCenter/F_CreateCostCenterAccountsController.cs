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
    public class F_CreateCostCenterAccountsController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_CreateCostCenterAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: F_CreateCostCenterAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFCCA")]
        public ActionResult FactoryCCAccounts()
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


        public JsonResult GetFactoryCCAccountsData(string FactoryCostCenterID)
        {

            List<Cost_Center_Accounts_DTO> cost_Center_Accounts = DB.F_CostCenterAccounts_Tables.Where(x => x.F_CostCenterID == FactoryCostCenterID).Select(x => new Cost_Center_Accounts_DTO
            {

                CostAccountID = x.F_CostAccountID,

                CostAccountName = x.F_CostAccountName

            }).ToList();

            return Json(cost_Center_Accounts, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveRecordFactoryCCAccounts(string CostCenterID, string CostAccountID, string CostAccountName)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.F_CostCenterAccounts_Tables.Where(x => x.F_CostAccountID == CostAccountID && x.F_CostCenterID == CostCenterID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_CostCenterAccounts_Table f_CostCenterAccounts_Table = new F_CostCenterAccounts_Table()
                {
                    F_CostCenterID = CostCenterID,

                    F_CostAccountID = CostAccountID,

                    F_CostAccountName = CostAccountName,

                    MoveUserID = userID
                };

                DB.F_CostCenterAccounts_Tables.Add(f_CostCenterAccounts_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult DeleteCostAccountFactory(string CCAccountID)
        {
            var deleteCostAccount = DB.F_CostCenterAccounts_Tables.Where(x => x.F_CostAccountID == CCAccountID).FirstOrDefault();

            DB.F_CostCenterAccounts_Tables.Remove(deleteCostAccount);
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
        //            item.SFCCA = Value;
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
        //        if (item.SFCCA.ToString().Equals("True"))
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