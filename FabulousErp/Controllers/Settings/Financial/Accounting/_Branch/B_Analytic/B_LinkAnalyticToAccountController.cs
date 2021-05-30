using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Branch.B_Analytic
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_LinkAnalyticToAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_LinkAnalyticToAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_LinkAnalyticToAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBLAA")]
        public ActionResult BranchLinkAnalyticToAccount()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;

            if (companyID != null)
            {
                ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDListCond(companyID);
            }
            else if (branchID != null)
            {
                ViewBag.BranchID = branchID;
                ViewBag.BranchName = repetitionBusiness.GetBranchName(branchID);

                var getAnalytic = DB.B_AnalyticAccount_Tables.Where(x => x.BranchID == branchID).ToList();
                SelectList analyticList = new SelectList(getAnalytic, "B_AnalyticAccountID", "B_AnalyticAccountID");
                ViewBag.AnalyticList = analyticList;
            }

            return View();
        }


        public JsonResult GetBranchName(string branchID)
        {
            return Json(repetitionBusiness.GetBranchName(branchID), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAnalyticAccounts(string branchID)
        {
            List<Analytic_To_Account_DTO> analytic_To_Account_DTOs = DB.B_AnalyticAccount_Tables.Where(x => x.BranchID == branchID).Select(x => new Analytic_To_Account_DTO
            {
                AnalyticID = x.B_AnalyticAccountID

            }).ToList();

            return Json(analytic_To_Account_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAnalyticName(string analyticID)
        {
            var getName = DB.B_AnalyticAccount_Tables.Where(x => x.B_AnalyticAccountID == analyticID).FirstOrDefault();
            return Json(getName.B_AnalyticAccountName, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAccounts(string analyticID)
        {
            List<Analytic_To_Account_DTO> analytic_To_Account_DTOs = DB.B_CreateAccount_Tables.Where(x => x.B_AnalyticAccountID == "" || x.B_AnalyticAccountID == analyticID).OrderBy(x => x.AccountID).Select(x => new Analytic_To_Account_DTO
            {
                AID = x.B_AID,

                AccountID = x.AccountID,

                AccountName = x.AccountName,

                AnalyticID = x.B_AnalyticAccountID
            }).ToList();

            return Json(analytic_To_Account_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddAnalyticToAcc(int b_AID, string analyticID)
        {
            var update = DB.B_CreateAccount_Tables.Where(x => x.B_AID == b_AID).FirstOrDefault();
            update.B_AnalyticAccountID = analyticID;
            DB.SaveChanges();
            return null;
        }

        public JsonResult RemoveAnalyticFromAcc(int b_AID)
        {
            var update = DB.B_CreateAccount_Tables.Where(x => x.B_AID == b_AID).FirstOrDefault();
            update.B_AnalyticAccountID = "";
            DB.SaveChanges();
            return null;
        }

    }
}