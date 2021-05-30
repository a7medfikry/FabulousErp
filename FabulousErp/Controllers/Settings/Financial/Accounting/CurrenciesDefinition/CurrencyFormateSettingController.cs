using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting.CurrenciesDefinition
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class CurrencyFormateSettingController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public CurrencyFormateSettingController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: CurrencyFormateSetting
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCF")]
        public ActionResult FormateSetting()
        {

            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.CompanyList = repetitionBusiness.RetrieveCompIDListCond(companyID);

            return View();
        }

        public JsonResult GetCompORGCurrency(string CompanyID)
        {
            var getCurrency = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            return Json(getCurrency.Currency, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveFormateSetting(string CompanyID, string Suffix, string Prefix, string Thousands, string Decimal, string DecimalNumbers, string Currency, string DecimalNotation)
        {

            string UserID = FabulousErp.Business.GetUserId();

            FormateSetting_Table formateSetting_Table = new FormateSetting_Table()
            {
                CompanyID = CompanyID,

                Suffix = Suffix,

                Prefix = Prefix,

                Thousands = Thousands,

                Decimal = Decimal,

                DecimalNumber = DecimalNumbers,

                Currency = Currency,

                DecimalNation = DecimalNotation,

                MoveUserID = UserID
            };

            DB.FormateSetting_Tables.Add(formateSetting_Table);
            DB.SaveChanges();

            return Json("True", JsonRequestBehavior.AllowGet);

        }
        

        public JsonResult GetFormatData(string CompanyID)
        {
            var getData = DB.FormateSetting_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            if(getData != null)
            {
                Formate_Setting_DTO formate_Setting_DTO = new Formate_Setting_DTO()
                {
                    Currency = getData.Currency,

                    DecimalNation = getData.DecimalNation,

                    DecimalNumber = getData.DecimalNumber
                };

                return Json(formate_Setting_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateFormateSetting(string CompanyID ,string DecimalNumbers, string Suffix, string Prefix, string Thousands, string Decimal, string Currency, string DecimalNotation)
        {

            string UserID = FabulousErp.Business.GetUserId();

            var UpdateFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            if(UpdateFormate.Currency == Currency && UpdateFormate.DecimalNumber == DecimalNumbers && UpdateFormate.DecimalNation == DecimalNotation)
            {
                return Json("NoChnages", JsonRequestBehavior.AllowGet);
            }
            else
            {
                UpdateFormate.Suffix = Suffix;

                UpdateFormate.Prefix = Prefix;

                UpdateFormate.Thousands = Thousands;

                UpdateFormate.Decimal = Decimal;

                UpdateFormate.DecimalNumber = DecimalNumbers;

                UpdateFormate.MoveUserID = UserID;

                UpdateFormate.Currency = Currency;

                UpdateFormate.DecimalNation = DecimalNotation;

                DB.SaveChanges();

                return Json("True", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetCompanyName(string companyID)
        {
            return Json(repetitionBusiness.GetCompanyName(companyID), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult AddFavourites(bool Value)
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item != null)
        //        {
        //            item.SCF = Value;
        //            DB.SaveChanges();
        //        }
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult CheckFavourites()
        //{
        //    string UserID = FabulousErp.Business.GetUserId();

        //    if (UserID == "SA")
        //    {
        //        return Json(JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {

        //        var item = DB.FavouritesForms_Tables.Where(x => x.UserID == UserID).FirstOrDefault();
        //        if (item.SCF.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}
    }
}