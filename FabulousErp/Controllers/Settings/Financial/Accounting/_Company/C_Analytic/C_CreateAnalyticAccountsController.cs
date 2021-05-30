using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_Analytic
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_CreateAnalyticAccountsController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_CreateAnalyticAccountsController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: CreateAnalyticAccounts
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCAA")]
        public ActionResult CompanyAnalyticAccounts()
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

        //    if(checkAccess != null)
        //    {
        //        return Json(repetitionBusiness.GetCompanyName(CompanyID), JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("False", JsonRequestBehavior.AllowGet);
        //    }
        //}


        public JsonResult AddAnalyticAccounts(string CompanyID, string AnalyticID, string AnalyticName)
        {

            string userID = FabulousErp.Business.GetUserId();

            var checkDuplicate = DB.C_AnalyticAccount_Tables.Where(x => x.C_AnalyticAccountID == AnalyticID).FirstOrDefault();

            if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_AnalyticAccount_Table analyticAccount_Table = new C_AnalyticAccount_Table()
                {

                    C_AnalyticAccountID = AnalyticID,

                    C_AnalyticAccountName = AnalyticName,

                    CompanyID = CompanyID,

                    MoveUserID = userID

                };

                DB.C_AnalyticAccount_Tables.Add(analyticAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }


        public JsonResult GetAnalyticAccounts(string companyID)
        {
            IQueryable<Analytic_To_Account_DTO> analyticData = DB.C_AnalyticAccount_Tables.Where(x => x.CompanyID == companyID).Select(x => new Analytic_To_Account_DTO
            {
                AnalyticAccountID = x.C_AnalyticAccountID,

                AnalyticAccountName = x.C_AnalyticAccountName
            });

            return Json(analyticData, JsonRequestBehavior.AllowGet);
        } 
        
        public JsonResult DelAnalyticAccounts(string AnalyticAccountID)
        {
            DB.C_AnalyticAccount_Tables.Remove(
             DB.C_AnalyticAccount_Tables.FirstOrDefault(x => x.C_AnalyticAccountID == AnalyticAccountID)
            );
            DB.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
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
        //            item.SCAA = Value;
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
        //        if (item.SCAA.ToString().Equals("True"))
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