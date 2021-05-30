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
    public class Inquiry_IBCOAController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public Inquiry_IBCOAController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: Inquiry_IBCOA
        public ActionResult BranchChartOfAccounts()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.BranchID = repetitionBusiness.RetrieveBranchIDListCond(companyID);
            }
            
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

        public JsonResult GetBranchData(string branchID, int? sortValue)
        {
            if (sortValue == 1)
            {
                List<Inquiry_DTO> Inquiry_DTO = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID).OrderBy(x => x.AccountID).Select(x => new Inquiry_DTO
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
                List<Inquiry_DTO> Inquiry_DTO = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID).OrderBy(x => x.AccountName).Select(x => new Inquiry_DTO
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
                List<Inquiry_DTO> Inquiry_DTO = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID).Select(x => new Inquiry_DTO
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