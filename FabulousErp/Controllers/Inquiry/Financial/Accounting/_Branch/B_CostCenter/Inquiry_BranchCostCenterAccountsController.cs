using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting._Branch.B_CostCenter
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_BranchCostCenterAccountsController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public Inquiry_BranchCostCenterAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Inquiry_BranchCostCenterAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IBCCA")]
        public ActionResult BranchCenterAccounts()
        {
            ViewBag.BranchName = "";
            return View();
        }

        public JsonResult GetBranchName(string branchID)
        {
            var list = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == branchID).FirstOrDefault();
            if (list != null)
            {
                string BranchName = list.BranchName;
                return Json(BranchName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetBranchCostCenterID(string branchID)
        {
            DB.Configuration.ProxyCreationEnabled = false;

            string userID = FabulousErp.Business.GetUserId();
            var check = DB.UABranchPremission_Tables.Where(x => x.UserID == userID && x.BranchID == branchID).FirstOrDefault();
            if (check != null)
            {
                List<Inquiry_DTO> inquiry_DTO = DB.B_CostCenter_Tables.Where(x => x.BranchID == branchID).Select(x => new Inquiry_DTO
                {
                    Cost_CenterID = x.B_CostCenterID
                }).ToList();

                return Json(inquiry_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBranchCostCenterName(string CostID)
        {
            var list = DB.B_CostCenter_Tables.Where(x => x.B_CostCenterID == CostID).FirstOrDefault();
            if (list != null)
            {
                string CostName = list.B_CostCenterName;
                return Json(CostName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string CostID)
        {
            List<Inquiry_DTO> inquiry_DTO = DB.B_CostCenterAccounts_Tables.Where(x => x.B_CostCenterID == CostID).Select(x => new Inquiry_DTO
            {
                Cost_AccountID = x.B_CostAccountID,
                Cost_AccountName = x.B_CostAccountName
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
        //            item.IBCCA = Value;
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
        //        if (item.IBCCA.ToString().Equals("True"))
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