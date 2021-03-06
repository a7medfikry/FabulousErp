using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Inquiry.Financial.GeneralSetting.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.Tax
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CompanyTaxController : Controller
    {
        DBContext DB = new DBContext();

        // GET: Inquiry_CompanyTax
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICT")]
        public ActionResult CompanyTax()
        {
            var userID = FabulousErp.Business.GetUserId();
            var list = DB.UACompPremission_Tables.Where(x => x.UserID == userID).ToList();
            SelectList CompanyID = new SelectList(list, "CompanyID", "CompanyID",Business.GetCompanyId());
            ViewBag.CompanyID = CompanyID;
            return View();
        }

        public JsonResult GetCompanyName(string companyID)
        {
            var list = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (list != null)
            {
                string CompanyName = list.CompanyName;
                return Json(CompanyName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetData(string companyID)
        {
            List<Inquiry_TaxDTO> inquiry_TaxDTOs = DB.C_TaxSetting_Tables.Where(x => x.CompanyID == companyID).Select(x => new Inquiry_TaxDTO
            {
                TaxCode = x.C_Taxcode,
                AccountID = x.C_CreateAccount_Table.AccountID,
                TaxDescribtion = x.C_Taxdescribtion,
                TaxPercentage = x.C_Taxpercentage
            }).ToList();
            return Json(inquiry_TaxDTOs, JsonRequestBehavior.AllowGet);
        }



    }
}