using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FabulousDB.DB_Context;
using FabulousModels.ViewModels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.FiscalPeriods
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class FiscalDefController : Controller
    {
        IRepetitionBusiness repetitionBusiness;

        public FiscalDefController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFYD")]
        public ActionResult FiscalYearDef()
        {

            ViewBag.CNFiscalYearID = repetitionBusiness.RetrieveFiscalYearIDList();

            return View();
        }
        
        [HttpPost]
        public ActionResult FiscalYearDefenition(Fiscal_Definition fiscalDefinition)
        {
            if (ModelState.IsValid)
            {
                var ID = DB.FiscalDefinition_Tables.Where(x => x.Fiscal_Year_ID == fiscalDefinition.Fiscal_Year_ID).FirstOrDefault();
                if (ID != null)
                {
                    return Json("False", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FiscalDefinition_Table fiscalDefinition_Table = new FiscalDefinition_Table()
                    {
                        Fiscal_Year_ID = fiscalDefinition.Fiscal_Year_ID,

                        Fiscal_Year_Name = fiscalDefinition.Fiscal_Year_Name,

                        Number_Of_Periods = fiscalDefinition.Number_Of_Periods,

                        Number_Of_Adjustment_Periods = fiscalDefinition.Number_Of_Adjustment_Periods
                    };

                    DB.FiscalDefinition_Tables.Add(fiscalDefinition_Table);
                    DB.SaveChanges();
                    return Json("True", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(JsonRequestBehavior.AllowGet);
            }
        }
    }
}