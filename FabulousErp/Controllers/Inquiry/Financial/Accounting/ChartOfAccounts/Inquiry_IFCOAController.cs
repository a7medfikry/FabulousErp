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
    public class Inquiry_IFCOAController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public Inquiry_IFCOAController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: Inquiry_IFCOA
        public ActionResult FactoryChartOfAccounts()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.FactoryID = repetitionBusiness.RetrieveFactoryIDListCond(companyID);
            }
          
            return View();
        }

        public JsonResult GetFactoryName(string factoryID)
        {
            var list = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factoryID).FirstOrDefault();
            if (list != null)
            {
                string FactoryName = list.FactoryName;
                return Json(FactoryName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetFactoryData(string factoryID, int? sortValue)
        {
            if (sortValue == 1)
            {
                List<Inquiry_DTO> Inquiry_DTO = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).OrderBy(x => x.AccountID).Select(x => new Inquiry_DTO
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
                List<Inquiry_DTO> Inquiry_DTO = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).OrderBy(x => x.AccountName).Select(x => new Inquiry_DTO
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
                List<Inquiry_DTO> Inquiry_DTO = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Inquiry_DTO
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