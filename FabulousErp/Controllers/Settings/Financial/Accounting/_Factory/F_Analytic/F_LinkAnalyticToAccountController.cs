using FabulousDB.DB_Context;
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
    public class F_LinkAnalyticToAccountController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public F_LinkAnalyticToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: F_LinkAnalyticToAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFLAA")]
        public ActionResult FactoryLinkAnalyticToAccount()
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
                ViewBag.FactoryID = factoryID;
                ViewBag.FactoryName = repetitionBusiness.GetFactoryName(factoryID);

                var getAnalytic = DB.F_AnalyticAccount_Tables.Where(x => x.FactoryID == factoryID).ToList();
                SelectList analyticList = new SelectList(getAnalytic, "F_AnalyticAccountID", "F_AnalyticAccountID");
                ViewBag.AnalyticList = analyticList;
            }

            return View();
        }


        public JsonResult GetFactoryName(string factoryID)
        {
            return Json(repetitionBusiness.GetFactoryName(factoryID), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAnalyticAccounts(string factoryID)
        {
            List<Analytic_To_Account_DTO> analytic_To_Account_DTOs = DB.F_AnalyticAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Analytic_To_Account_DTO
            {
                AnalyticID = x.F_AnalyticAccountID

            }).ToList();

            return Json(analytic_To_Account_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAnalyticName(string analyticID)
        {
            var getName = DB.F_AnalyticAccount_Tables.Where(x => x.F_AnalyticAccountID == analyticID).FirstOrDefault();
            return Json(getName.F_AnalyticAccountName, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAccounts(string analyticID)
        {
            List<Analytic_To_Account_DTO> analytic_To_Account_DTOs = DB.F_CreateAccount_Tables.Where(x => x.F_AnalyticAccountID == "" || x.F_AnalyticAccountID == analyticID).OrderBy(x => x.AccountID).Select(x => new Analytic_To_Account_DTO
            {
                AID = x.F_AID,

                AccountID = x.AccountID,

                AccountName = x.AccountName,

                AnalyticID = x.F_AnalyticAccountID
            }).ToList();

            return Json(analytic_To_Account_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddAnalyticToAcc(int f_AID, string analyticID)
        {
            var update = DB.F_CreateAccount_Tables.Where(x => x.F_AID == f_AID).FirstOrDefault();
            update.F_AnalyticAccountID = analyticID;
            DB.SaveChanges();
            return null;
        }

        public JsonResult RemoveAnalyticFromAcc(int f_AID)
        {
            var update = DB.F_CreateAccount_Tables.Where(x => x.F_AID == f_AID).FirstOrDefault();
            update.F_AnalyticAccountID = "";
            DB.SaveChanges();
            return null;
        }

    }
}