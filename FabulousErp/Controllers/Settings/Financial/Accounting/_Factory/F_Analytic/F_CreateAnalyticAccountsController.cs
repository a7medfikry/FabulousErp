using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_Analytic
{
    [AuthorizationFilter]
    public class F_CreateAnalyticAccountsController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_CreateAnalyticAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        // GET: F_CreateAnalyticAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFAA")]
        public ActionResult FactoryAnalyticAccounts()
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
            else if(factoryID != null)
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

        public JsonResult AddAnalyticAccounts(string FactoryID, string AnalyticID, string AnalyticName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.F_AnalyticAccount_Tables.Where(x => x.F_AnalyticAccountID == AnalyticID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_AnalyticAccount_Table analyticAccount_Table = new F_AnalyticAccount_Table()
                {

                    F_AnalyticAccountID = AnalyticID,

                    F_AnalyticAccountName = AnalyticName,

                    FactoryID = FactoryID,

                    MoveUserID = userID

                };

                DB.F_AnalyticAccount_Tables.Add(analyticAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }

        public JsonResult GetAnalyticAccounts(string factoryID)
        {
            IQueryable<Analytic_To_Account_DTO> analyticData = DB.F_AnalyticAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Analytic_To_Account_DTO
            {
                AnalyticAccountID = x.F_AnalyticAccountID,

                AnalyticAccountName = x.F_AnalyticAccountName
            });

            return Json(analyticData, JsonRequestBehavior.AllowGet);
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
        //            item.SFAA = Value;
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
        //        if (item.SFAA.ToString().Equals("True"))
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