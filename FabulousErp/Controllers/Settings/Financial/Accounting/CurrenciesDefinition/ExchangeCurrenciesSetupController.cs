using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
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
    public class ExchangeCurrenciesSetupController : Controller
    {

        DBContext DB = new DBContext();

        // GET: ExchangeCurrencies
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCET")]
        public ActionResult ExchangeCurrencies()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var CurrencyID = DB.CurrenciesDefinition_Tables.Where(x=>x.CompanyID == companyID && x.CurrencyID != companyID).ToList();
            SelectList CurrencyList = new SelectList(CurrencyID, "CurrencyID", "CurrencyID");

            ViewBag.CurrencyList = CurrencyList;

            return View();
        }


        public JsonResult GetCurrencyName(string CurrencyID)
        {
            var GetCurrencyName = DB.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == CurrencyID).FirstOrDefault();

            Get_Small_Data_DTO get_Small_Data_DTO = new Get_Small_Data_DTO()
            {
                Name = GetCurrencyName.CurrencyName
            };

            return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetRatesData(string CurrencyID)
        {

            List<Exchange_Rates_DTO> exchange_Rates_DTOs = DB.CurrenciesExchange_Tables.Where(x => x.CurrencyID == CurrencyID).Select(x => new Exchange_Rates_DTO
            {
                ExchangeID = x.ExchangeID,

                EstablishDate = x.EstablishDate,

                ExpireDate = x.ExpireDate,

                Rate = x.Rate,

                StartDate = x.StartDate

            }).ToList();

            return Json(exchange_Rates_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveRates(string CurrencyID, double Rate, string StartDate, string ExpireDate)
        {

            DateTime start = DateTime.Parse(StartDate);
            DateTime end = DateTime.Parse(ExpireDate);

            string userID = FabulousErp.Business.GetUserId();

            if(end < start)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CurrenciesExchange_Table currenciesExchange_Table = new CurrenciesExchange_Table()
                {
                    CurrencyID = CurrencyID,

                    EstablishDate = DateTime.Now.ToShortDateString(),

                    Rate = Rate,

                    StartDate = StartDate,

                    ExpireDate = ExpireDate,

                    MoveUserID = userID
                };

                DB.CurrenciesExchange_Tables.Add(currenciesExchange_Table);
                DB.SaveChanges();

                return Json("True",JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult DeleteRate(int ExchangeID)
        {
            var deleteRate = DB.CurrenciesExchange_Tables.Where(x => x.ExchangeID == ExchangeID).FirstOrDefault();
            DB.CurrenciesExchange_Tables.Remove(deleteRate);
            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }


        public JsonResult CheckStartDate(string StartDate)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            int Start = Convert.ToDateTime(StartDate).Year;

            DateTime StartD = Convert.ToDateTime(StartDate);

            var getFysicalYearRelatedByComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

            if (getFysicalYearRelatedByComp != null)
            {

                string FysicalYear = getFysicalYearRelatedByComp.Fiscal_Year_ID;

                var checkYearOfStartExistInNewYear = DB.NewFiscalYear_Table.ToList().Where(x => x.Fiscal_Year_ID == FysicalYear && ( DateTime.Parse(x.Fiscal_Year_Start).Year == Start || DateTime.Parse(x.Fiscal_Year_End).Year == Start )).FirstOrDefault();

                if(checkYearOfStartExistInNewYear != null)
                {
                    if(checkYearOfStartExistInNewYear.Closed == false || checkYearOfStartExistInNewYear.Closed == null)
                    {
                        var checkStartOverlap = DB.NewFiscalYear_Table.ToList().Where(x => x.Fiscal_Year_ID == FysicalYear && DateTime.Parse(x.Fiscal_Year_Start) <= StartD && DateTime.Parse(x.Fiscal_Year_End) >= StartD && (x.Closed == false || x.Closed == null)).FirstOrDefault();

                        if(checkStartOverlap != null)
                        {
                            return Json("True", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("False", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("NewYearClosed", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("NewYearNotExist", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Get_Small_Data_DTO get_Small_Data_DTO = new Get_Small_Data_DTO()
                {
                    CompanyID = companyID,

                    Message = "NoYear"
                };

                return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult CheckEndDate(string EndDate)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            int End = Convert.ToDateTime(EndDate).Year;

            DateTime EndD = Convert.ToDateTime(EndDate);

            var getFysicalYearRelatedByComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

            if (getFysicalYearRelatedByComp != null)
            {

                string FysicalYear = getFysicalYearRelatedByComp.Fiscal_Year_ID;

                var checkYearOfStartExistInNewYear = DB.NewFiscalYear_Table.ToList().Where(x => x.Fiscal_Year_ID == FysicalYear && (DateTime.Parse(x.Fiscal_Year_Start).Year == End || DateTime.Parse(x.Fiscal_Year_End).Year == End)).FirstOrDefault();

                if (checkYearOfStartExistInNewYear != null)
                {
                    if (checkYearOfStartExistInNewYear.Closed == false || checkYearOfStartExistInNewYear.Closed == null)
                    {
                        var checkStartOverlap = DB.NewFiscalYear_Table.ToList().Where(x => x.Fiscal_Year_ID == FysicalYear && DateTime.Parse(x.Fiscal_Year_Start) <= EndD && DateTime.Parse(x.Fiscal_Year_End) >= EndD && (x.Closed == false || x.Closed == null)).FirstOrDefault();

                        if (checkStartOverlap != null)
                        {
                            return Json("True", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("False", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("NewYearClosed", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("NewYearNotExist", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Get_Small_Data_DTO get_Small_Data_DTO = new Get_Small_Data_DTO()
                {
                    CompanyID = companyID,

                    Message = "NoYear"
                };

                return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetCurrencyFormate()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var getFormatSetting = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

            if (getFormatSetting != null)
            {

                Formate_Setting_DTO formate_Setting_DTO = new Formate_Setting_DTO()
                {
                    DecimalNumber = getFormatSetting.DecimalNumber,

                    Prefix = getFormatSetting.Prefix,

                    Suffix = getFormatSetting.Suffix,

                    Decimal = getFormatSetting.Decimal,

                    Thousands = getFormatSetting.Thousands
                };

                return Json(formate_Setting_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
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
        //            item.SCET = Value;
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
        //        if (item.SCET.ToString().Equals("True"))
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