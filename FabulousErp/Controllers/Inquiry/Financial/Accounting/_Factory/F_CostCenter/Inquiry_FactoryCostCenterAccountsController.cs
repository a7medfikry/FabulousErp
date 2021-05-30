using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Factory.F_CostCenter
{
    [AuthorizationFilter]
    public class Inquiry_FactoryCostCenterAccountsController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_FactoryCostCenterAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_FactoryCostCenterAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IFCCA")]
        public ActionResult FactoryCenterAccounts()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.CodeList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
         
            return View();
        }

        public JsonResult GetFactoryName(string factoryID)
        {
            var list = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factoryID).FirstOrDefault();
            if (list != null)
            {
                string FactoryName = list.FactoryName;
                return Json(FactoryName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetFactoryCostCenterID(string factoryID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == factoryID).FirstOrDefault();
            if (check != null)
            {
                List<Inquiry_DTO> inquiry_DTO = DB.F_CostCenter_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Inquiry_DTO
                {
                    Cost_CenterID = x.F_CostCenterID
                }).ToList();

                return Json(inquiry_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFactoryCostCenterName(string CostID)
        {
            var list = DB.F_CostCenter_Tables.Where(x => x.F_CostCenterID == CostID).FirstOrDefault();
            if (list != null)
            {
                string CostName = list.F_CostCenterName;
                return Json(CostName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string CostID)
        {
            List<Inquiry_DTO> inquiry_DTO = DB.F_CostCenterAccounts_Tables.Where(x => x.F_CostCenterID == CostID).Select(x => new Inquiry_DTO
            {
                Cost_AccountID = x.F_CostAccountID,
                Cost_AccountName = x.F_CostAccountName
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
        //            item.IFCCA = Value;
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
        //        if (item.IFCCA.ToString().Equals("True"))
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