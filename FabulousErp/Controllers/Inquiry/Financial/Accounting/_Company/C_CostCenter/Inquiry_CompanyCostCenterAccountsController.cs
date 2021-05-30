using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Company.C_CostCenter
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CompanyCostCenterAccountsController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_CompanyCostCenterAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: Inquiry_CompanyCostCenterAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICCCA")]
        public ActionResult CompanyCenterAccounts()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.CompanyID = companyID;
            string companyName = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).Select(x => x.CompanyName).First().ToString();
            ViewBag.CompanyName = companyName;
            return View();
        }

        public JsonResult GetCostCenterID(string companyID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UACompPremission_Tables.Where(x => x.UserID == userID && x.CompanyID == companyID).FirstOrDefault();
            if (check != null)
            {
                List<Inquiry_DTO> inquiry_DTO = DB.C_CostCenter_Tables.Where(x => x.CompanyID == companyID).Select(x => new Inquiry_DTO
                {
                    Cost_CenterID = x.C_CostCenterID
                }).ToList();

                return Json(inquiry_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCostCenterName(string CostID)
        {
            var list = DB.C_CostCenter_Tables.Where(x => x.C_CostCenterID == CostID).FirstOrDefault();
            if (list != null)
            {
                string CostName = list.C_CostCenterName;
                return Json(CostName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string CostID)
        {
            List<Inquiry_DTO> inquiry_DTO = DB.C_CostCenterAccounts_Tables.Where(x => x.C_CostCenterID == CostID).Select(x => new Inquiry_DTO
            {
                Cost_AccountID = x.C_CostAccountID,
                Cost_AccountName = x.C_CostAccountName
            }).ToList();
            return Json(inquiry_DTO, JsonRequestBehavior.AllowGet);
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
        //            item.ICCCA = Value;
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
        //        if (item.ICCCA.ToString().Equals("True"))
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