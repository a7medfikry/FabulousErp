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
    public class C_CreateMainCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_CreateMainCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: C_CreateMainCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCMCC")]
        public ActionResult CompMainCostCenter()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            //ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);
            ViewBag.CompanyID = companyID;
            ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);

            return View();
        }


        public JsonResult FilterCostCenterIDForComp(string CompanyID)
        {
            CompanyID = FabulousErp.Business.GetCompanyId();
            return Json(repetitionBusiness.FilterCostCenterIDForComp(CompanyID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompCostCenter(string CostCenterID)
        {
            return Json(repetitionBusiness.GetCompCostCenterName(CostCenterID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckDuplicateCostCenterGroupIDComp(string CostCenterGroupID)
        {

            var check = DB.C_MainCostCenter_Tables.Where(x => x.C_CostCenterGroupID == CostCenterGroupID).FirstOrDefault();

            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }


        public ActionResult SaveCompCostGroup(string CostCenterGroupID, string CostCenterGroupName, string CompanyID, C_GroupCostCenter_Table[] order)
        {

            string userID = FabulousErp.Business.GetUserId();

            C_MainCostCenter_Table c_MainCostCenter_Table = new C_MainCostCenter_Table()
            {
                C_CostCenterGroupID = CostCenterGroupID,

                C_CostCenterGroupName = CostCenterGroupName,

                CompanyID = CompanyID,

                MoveUserID = userID
            };
            DB.C_MainCostCenter_Tables.Add(c_MainCostCenter_Table);
            DB.SaveChanges();

            foreach (var item in order)
            {
                C_GroupCostCenter_Table c_GroupCostCenter_Table = new C_GroupCostCenter_Table()
                {
                    C_CostCenterGroupID = CostCenterGroupID,

                    C_CostCenterID = item.C_CostCenterID,

                    C_Percentage = item.C_Percentage,

                    MoveUserID = userID
                };
                DB.C_GroupCostCenter_Tables.Add(c_GroupCostCenter_Table);
                DB.SaveChanges();
            }

            return Json("True", JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSavedMainCC(string companyID)
        {
            IQueryable<Main_Cost_Center_DTO> main_Cost_Center_DTOs = DB.C_MainCostCenter_Tables.Where(x => x.CompanyID == companyID).Select(x => new Main_Cost_Center_DTO
            {
                MainCostCenterID = x.C_CostCenterGroupID,

                MainCostCenterName = x.C_CostCenterGroupName
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
        //            item.SCMCC = Value;
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
        //        if (item.SCMCC.ToString().Equals("True"))
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