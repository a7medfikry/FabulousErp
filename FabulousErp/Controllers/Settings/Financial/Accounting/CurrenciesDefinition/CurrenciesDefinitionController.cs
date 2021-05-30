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
    public class CurrenciesDefinitionController : Controller
    {

        DBContext DB = new DBContext();

        // GET: CurrenciesDefinition
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCD")]
        public ActionResult CurrenciesTypes()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var accountID = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountID).Select(x=>new { C_AID = x.C_AID , AccountID = x.AccountID + " ( " + x.AccountName + " )" }).ToList();
            SelectList accountList = new SelectList(accountID, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            return View();
        }


        public JsonResult GetCurrenciesDefinition()
        {
            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            List<Currencies_Definition_DTO> CurrenciesDefinition = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyID && x.CurrencyID != CompanyID).Select(x => new Currencies_Definition_DTO
            {
                CurrencyID = x.CurrencyID,

                CurrencyName = x.CurrencyName,

                ISOCode = x.ISOCode,

                DisActive = x.DisActive,

                //AccountID = x.C_CreateAccount_Table.AccountID

            }).ToList();

            return Json(CurrenciesDefinition, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveCurrencyRecord(string CurrencyID, string CurrencyName, string ISOCode)
        {
            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            string UserID = FabulousErp.Business.GetUserId();

            var checkDuplicateCID = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyID && x.CurrencyID == CurrencyID).FirstOrDefault();
            var checkDuplicateCN = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyID && x.CurrencyName == CurrencyName).FirstOrDefault();
            var checkDuplicateISOC = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompanyID && x.ISOCode == ISOCode).FirstOrDefault();
            var checkDuplicateCIDGlobal = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID != CompanyID && x.CurrencyID == CurrencyID).FirstOrDefault();

            if (checkDuplicateCIDGlobal != null)
            {
                return Json("DCIDG", JsonRequestBehavior.AllowGet);
            }
            else if (checkDuplicateCID != null)
            {
                return Json("DCID", JsonRequestBehavior.AllowGet);
            }
            else if (checkDuplicateCN != null)
            {
                return Json("DCN", JsonRequestBehavior.AllowGet);
            }
            else if (checkDuplicateISOC != null)
            {
                return Json("DISOC", JsonRequestBehavior.AllowGet);
            }
            else
            {
                CurrenciesDefinition_Table currenciesDefinition_Table = new CurrenciesDefinition_Table()
                {
                    CompanyID = CompanyID,

                    CurrencyID = CurrencyID,

                    CurrencyName = CurrencyName,

                    ISOCode = ISOCode,

                    MoveUserID = UserID
                };

                DB.CurrenciesDefinition_Tables.Add(currenciesDefinition_Table);
                DB.SaveChanges();

                //List<AccountCurrencyDefinition_Table> accountCurrencyDefinition_Table = new List<AccountCurrencyDefinition_Table>()
                //{
                //    new AccountCurrencyDefinition_Table{CurrencyID = currenciesDefinition_Table.CurrencyID,C_AID = C_AID},

                //    new AccountCurrencyDefinition_Table{CurrencyID = currenciesDefinition_Table.CurrencyID,C_AID = C_AID2}
                //};

                //DB.AccountCurrencyDefinition_Tables.AddRange(accountCurrencyDefinition_Table);
                //DB.SaveChanges();

                return Json("True", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteCurrenciesDefinition(string CurrencyID)
        {
            var item = DB.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == CurrencyID).FirstOrDefault();
            DB.CurrenciesDefinition_Tables.Remove(item);
            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisActiveCurrenciesDefinition(string CurrencyID)
        {
            string UserID = FabulousErp.Business.GetUserId();

            var item = DB.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == CurrencyID).FirstOrDefault();
            item.MoveUserID = UserID;
            item.DisActive = "Checked";
            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult ActiveCurrenciesDefinition(string CurrencyID)
        {
            string UserID = FabulousErp.Business.GetUserId();

            var item = DB.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == CurrencyID).FirstOrDefault();
            item.DisActive = null;
            item.MoveUserID = UserID;
            DB.SaveChanges();

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountName(int C_AID)
        {
            var getName = DB.C_CreateAccount_Tables.Where(x => x.C_AID == C_AID).FirstOrDefault();
            return Json(getName.AccountName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencyName(string currencyID)
        {
            var getName = DB.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();
            return Json(getName.CurrencyName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveAccountToCurrency(string currencyID, int c_AID, int c_AID2)
        {
            var checkDuplicate = DB.AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();

            if (checkDuplicate == null)
            {

                List<CurrenciesDefinition_Tables> accountCurrencyDefinition_Table = new List<CurrenciesDefinition_Tables>()
            {
                new CurrenciesDefinition_Tables{CurrencyID = currencyID,C_AID = c_AID,Type="Profit"},

                new CurrenciesDefinition_Tables{CurrencyID = currencyID,C_AID = c_AID2,Type="Loss"}
            };

                DB.AccountCurrencyDefinition_Tables.AddRange(accountCurrencyDefinition_Table);
                DB.SaveChanges();

                return null;
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAccountsOfCurrency(string currencyID)
        {
            var checkExist = DB.AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();
            if (checkExist != null)
            {
                List<Currencies_Definition_DTO> currencies_Definition_DTOs = DB.AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).Select(x => new Currencies_Definition_DTO
                {

                    C_AID = x.C_AID,

                    AccountID = x.C_CreateAccount_Table.AccountID,

                    AccountName = x.C_CreateAccount_Table.AccountName,

                    Type = x.Type

                }).ToList();

                return Json(currencies_Definition_DTOs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult UpdateAccountToCurrency(string currencyID, int c_AID, int c_AID2)
        {
            var updateProfit = DB.AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID && x.Type == "Profit").FirstOrDefault();
            if(updateProfit != null)
            {
                updateProfit.C_AID = c_AID;
                DB.SaveChanges();
            }

            var updateLoss = DB.AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID && x.Type == "Loss").FirstOrDefault();
            if(updateLoss != null)
            {
                updateLoss.C_AID = c_AID2;
                DB.SaveChanges();
            }
            return null;
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
        //            item.SCD = Value;
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
        //        if (item.SCD.ToString().Equals("True"))
        //        {
        //            return Json("True", JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json("False", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //}
        public ActionResult SetCurrencyUnit()
        {
            string CompId = FabulousErp.Business.GetCompanyId();
            ViewBag.CurrencyID = new SelectList(DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == CompId).ToList(), "CurrencyID", "ISOCode");

            return View();
        }
        public ActionResult SetCurrencyUnitName(CurrenciesDefinition_Table Curr)
        {
            CurrenciesDefinition_Table ThisCurr = DB.CurrenciesDefinition_Tables.FirstOrDefault(x => x.CurrencyID == Curr.CurrencyID);
            ThisCurr.Currency_small_unit_name = Curr.Currency_small_unit_name;
            ThisCurr.Currency_unit_name = Curr.Currency_unit_name;
            DB.SaveChanges();
            return Json(1);
        }
    }
}