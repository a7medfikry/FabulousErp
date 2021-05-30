using FabulousDB.DB_Context;
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
    public class C_LinkAnalyticToAccountController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public C_LinkAnalyticToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: C_LinkAnalyticToAccount
        [HttpGet]
       // [Authorize]
        public ActionResult CompanyLinkAnalyticToAccount()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.CompanyID = companyID;
            ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);

            var getAnalytic = DB.C_AnalyticAccount_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList analyticList = new SelectList(getAnalytic, "C_AnalyticAccountID", "C_AnalyticAccountID");
            ViewBag.AnalyticList = analyticList;

            return View();
        }

        public JsonResult GetAnalyticName(string analyticID)
        {
            var getName = DB.C_AnalyticAccount_Tables.Where(x => x.C_AnalyticAccountID == analyticID).FirstOrDefault();
            return Json(getName.C_AnalyticAccountName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccounts(string analyticID)
        {
            List<Analytic_To_Account_DTO> analytic_To_Account_DTOs = DB.C_CreateAccount_Tables.Where(x => x.C_Prefix == null && (x.C_AnalyticAccountID == "" || x.C_AnalyticAccountID == analyticID)).OrderBy(x => x.AccountID).Select(x => new Analytic_To_Account_DTO
            {
                AID = x.C_AID,

                AccountID = x.AccountID,

                AccountName = x.AccountName,

                AnalyticID = x.C_AnalyticAccountID
            }).ToList();

            return Json(analytic_To_Account_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAnalyticToAcc(int c_AID, string analyticID)
        {
            var update = DB.C_CreateAccount_Tables.Where(x => x.C_AID == c_AID).FirstOrDefault();
            update.C_AnalyticAccountID = analyticID;
            DB.SaveChanges();
            return null;
        }

        public JsonResult RemoveAnalyticFromAcc(int c_AID)
        {
            var update = DB.C_CreateAccount_Tables.Where(x => x.C_AID == c_AID).FirstOrDefault();
            update.C_AnalyticAccountID = "";
            DB.SaveChanges();
            return null;
        }

    }
}