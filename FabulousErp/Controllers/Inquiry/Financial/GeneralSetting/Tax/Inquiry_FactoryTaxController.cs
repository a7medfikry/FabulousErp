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
    [AuthorizationFilter]
    public class Inquiry_FactoryTaxController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public Inquiry_FactoryTaxController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: Inquiry_FactoryTax
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "IFT")]
        public ActionResult FactoryTax()
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

        public JsonResult GetData(string factoryID)
        {
            List<Inquiry_TaxDTO> inquiry_TaxDTOs = new List<Inquiry_TaxDTO>();
            var list = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).ToList();
            foreach (var items in list)
            {
                foreach (var items2 in items.F_TaxSetting_Tables)
                {
                    Inquiry_TaxDTO inquiry_TaxDTO = new Inquiry_TaxDTO()
                    {
                        AccountID = items2.F_CreateAccount_Table.AccountID,
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