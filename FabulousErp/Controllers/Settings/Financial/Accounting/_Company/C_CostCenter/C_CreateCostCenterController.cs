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
    public class C_CreateCostCenterController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_CreateCostCenterController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();


        // GET: C_CreateCostCenter
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCCC")]
        public ActionResult CompanyCostCenter()
        {

            string companyID = (string)FabulousErp.Business.GetCompanyId();

            //ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);
            ViewBag.CompanyID = companyID;
            ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);

            return View();
        }


        public JsonResult AddCompCostCenter(string CompanyID, string CostCenterID, string CostCenterName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.C_CostCenter_Tables.Where(x => x.C_CostCenterID == CostCenterID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_CostCenter_Table c_CostCenter_Table = new C_CostCenter_Table()
                {
                    CompanyID = CompanyID,

                    C_CostCenterID = CostCenterID,

                    C_CostCenterName = CostCenterName,

                    MoveUserID = userID
                };

                DB.C_CostCenter_Tables.Add(c_CostCenter_Table);
                DB.SaveChanges();

                return null;
            }

        }


        public JsonResult GetCostCenter(/*string companyID*/)
        {
            string companyID = Business.GetCompanyId();
            IQueryable<Cost_Center_Accounts_DTO> costData = DB.C_CostCenter_Tables.Where(x => x.CompanyID == companyID).Select(x => new Cost_Center_Accounts_DTO
            {
                CostCenterID = x.C_CostCenterID,

                CostCenterName = x.C_CostCenterName
            });

            return Json(costData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DelCostCenter(string Id)
        {
            try
            {
                DB.C_CostCenter_Tables.Remove(DB.C_CostCenter_Tables.FirstOrDefault
               (x => x.C_CostCenterID == Id));
                return Json(DB.SaveChanges());
            }
            catch
            {
                return Json(0);
            }
         
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
        //            item.SCCC = Value;
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
        //        if (item.SCCC.ToString().Equals("True"))
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