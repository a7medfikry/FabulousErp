using FabulousDB.DB_Context;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial.Accounting.ChartOfAccounts
{
    public class Inquiry_ICCOAController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public Inquiry_ICCOAController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: Inquiry_ICOA
        public ActionResult CompanyChartOfAccounts()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            ViewBag.BranchID = repetitionBusiness.RetrieveBranchIDListCond(companyID);
            ViewBag.FactoryID = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            return View();
        }

        public JsonResult GetCompanyData(int? sortValue)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            if (sortValue == 1)
            {
                List<Inquiry_DTO> Inquiry_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountID).Select(x => new Inquiry_DTO
                {
                    AccountID = x.AccountID,
                    AccountName = x.AccountName,
                    AccountType = x.AccountType,
                    AccountGroup = x.AccountsGroup,
                    PostingType = x.PostingType
                }).ToList();
                return Json(Inquiry_DTO, JsonRequestBehavior.AllowGet);
            }
            else if (sortValue == 2)
            {
                List<Inquiry_DTO> Inquiry_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountName).Select(x => new Inquiry_DTO
                {
                    AccountID = x.AccountID,
                    AccountName = x.AccountName,
                    AccountType = x.AccountType,
                    AccountGroup = x.AccountsGroup,
                    PostingType = x.PostingType
                }).ToList();
                return Json(Inquiry_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<Inquiry_DTO> Inquiry_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).Select(x => new Inquiry_DTO
                {
                    AccountID = x.AccountID,
                    AccountName = x.AccountName,
                    AccountType = x.AccountType,
                    AccountGroup = x.AccountsGroup,
                    PostingType = x.PostingType
                }).ToList();
                return Json(Inquiry_DTO, JsonRequestBehavior.AllowGet);
            }
        }

 

    }
}