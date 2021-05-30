using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_CheckbookSettingController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public Inquiry_CheckbookSettingController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: Inquiry_CheckbookSetting
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "ICCBS")]
        public ActionResult CompanyCB()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.CompanyID = companyID;
            string companyName = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == companyID).Select(x => x.CompanyName).First().ToString();
            ViewBag.CompanyName = companyName;
            return View();
        }
        public ActionResult BranchCB()
        {
            ViewBag.BranchName = "";
            return View();
        }
        public ActionResult FactoryCB()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.FactoryID = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
          
            return View();
        }
        public JsonResult GetCompanyCheckbook()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<Inquiry_DTO> Inquiry_DTO = DB.C_CheckBookSetting_Tables
                .Where(x => x.CompanyID == companyID)
                .Select(x => new Inquiry_DTO
                {
                    CheckbookID = x.C_CheckbookID,
                    CheckbookName = x.C_CheckbookName,
                    CheckbookType = x.C_CheckbookType,
                    CheckbookStatus = x.C_CheckbookStatus,
                    CheckbookCurrency = x.CurrenciesDefinition_Table.CurrencyName,
                    CheckbookAccountID = x.C_CreateAccount_Table.AccountID
                }).ToList();
            return Json(Inquiry_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBranchCheckbook(string branchID)
        {
            List<Inquiry_DTO> Inquiry_DTO = DB.B_CheckBookSetting_Tables
                .Where(x => x.BranchID == branchID)
                .Select(x => new Inquiry_DTO
                {
                    Branch_Factory_Name = x.CompanyBranchInfo_Table.BranchName,
                    CheckbookID = x.B_CheckbookID,
                    CheckbookName = x.B_CheckbookName,
                    CheckbookType = x.B_CheckbookType,
                    CheckbookStatus = x.B_CheckbookStatus,
                    CheckbookCurrency = x.CurrenciesDefinition_Table.CurrencyName,
                    CheckbookAccountID = x.B_CreateAccount_Table.AccountID
                }).ToList();
            return Json(Inquiry_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFactoryCheckbook(string factoryID)
        {
            List<Inquiry_DTO> Inquiry_DTO = DB.F_CheckBookSetting_Tables
                .Where(x => x.FactoryID == factoryID)
                .Select(x => new Inquiry_DTO
                {
                    Branch_Factory_Name = x.CompanyFactoryInfo_Table.FactoryName,
                    CheckbookID = x.F_CheckbookID,
                    CheckbookName = x.F_CheckbookName,
                    CheckbookType = x.F_CheckbookType,
                    CheckbookStatus = x.F_CheckbookStatus,
                    CheckbookCurrency = x.CurrenciesDefinition_Table.CurrencyName,
                    CheckbookAccountID = x.F_CreateAccount_Table.AccountID
                }).ToList();
            return Json(Inquiry_DTO, JsonRequestBehavior.AllowGet);
        }



    }
}
