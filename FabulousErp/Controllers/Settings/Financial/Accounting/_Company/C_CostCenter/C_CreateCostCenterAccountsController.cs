using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CostCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_CostCenter
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_CreateCostCenterAccountsController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_CreateCostCenterAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: C_CreateCostCenterAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCCCA")]
        public ActionResult CompCCAccounts()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            //ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);
            ViewBag.CompanyID = companyID;
            ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);

            return View();
        }

        //public JsonResult GetCompanyName(string CompanyID)
        //{
        //    string userID = FabulousErp.Business.GetUserId();

        //    var checkAccess = DB.UACompPremission_Tables.Where(x => x.CompanyID == CompanyID && x.UserID == userID).FirstOrDefault();

        //    if (checkAccess != null)
        //    {
        //        return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("False", JsonRequestBehavior.AllowGet);
        //    }
        //}


        public JsonResult FilterCostCenterIDForComp(string CompanyID)
        {
            return Json(repetitionBusiness.FilterCostCenterIDForComp(CompanyID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCompCostCenter(string CostCenterID)
        {
            return Json(repetitionBusiness.GetCompCostCenterName(CostCenterID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCompCCAccountsData(string CompCostCenterID)
        {

            List<Cost_Center_Accounts_DTO> cost_Center_Accounts = DB.C_CostCenterAccounts_Tables.Where(x => x.C_CostCenterID == CompCostCenterID).Select(x => new Cost_Center_Accounts_DTO
            {

                CostAccountID = x.C_CostAccountID,

                CostAccountName = x.C_CostAccountName

            }).ToList();

            return Json(cost_Center_Accounts, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveRecordCompCCAccounts(string CostCenterID, string CostAccountID, string CostAccountName)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.C_CostCenterAccounts_Tables.Where(x => x.C_CostAccountID == CostAccountID && x.C_CostCenterID == CostCenterID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_CostCenterAccounts_Table c_CostCenterAccounts_Table = new C_CostCenterAccounts_Table()
                {
                    C_CostCenterID = CostCenterID,

                    C_CostAccountID = CostAccountID,

                    C_CostAccountName = CostAccountName,

                    MoveUserID = userID
                };

                DB.C_CostCenterAccounts_Tables.Add(c_CostCenterAccounts_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult DeleteCostAccountComp(string CCAccountID)
        {
            var deleteCostAccount = DB.C_CostCenterAccounts_Tables.Where(x => x.C_CostAccountID == CCAccountID).FirstOrDefault();

            DB.C_CostCenterAccounts_Tables.Remove(deleteCostAccount);
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
        //            item.SCCCA = Value;
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
        //        if (item.SCCCA.ToString().Equals("True"))
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