using FabulousDB.DB_Context;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Inquiry.Financial.GeneralSetting.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Inquiry.Financial.GeneralSetting.Tax
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class Inquiry_BranchTaxController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public Inquiry_BranchTaxController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: Inquiry_BranchTax
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IBT")]
        public ActionResult BranchTax()
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

        public JsonResult GetData(string branchID)
        {
            List<Inquiry_TaxDTO> inquiry_TaxDTOs = new List<Inquiry_TaxDTO>();
            var list = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID).ToList();
            foreach (var items in list)
            {
                foreach (var items2 in items.B_TaxSetting_Tables)
                {
                    Inquiry_TaxDTO inquiry_TaxDTO = new Inquiry_TaxDTO()
                    {
                        AccountID = items2.B_CreateAccount_Table.AccountID,
                        TaxCode = items2.C_TaxSetting_Table.C_Taxcode,
                        TaxDescribtion = items2.C_TaxSetting_Table.C_Taxdescribtion,
                        TaxPercentage = items2.C_TaxSetting_Table.C_Taxpercentage
                    };
                    inquiry_TaxDTOs.Add(inquiry_TaxDTO);
                }
            }
            return Json(inquiry_TaxDTOs, JsonRequestBehavior.AllowGet);
        }






    }
}