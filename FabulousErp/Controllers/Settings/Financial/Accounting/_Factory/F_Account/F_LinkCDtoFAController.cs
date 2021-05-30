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

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_Account
{
    [AuthorizationFilter]
    public class F_LinkCDtoFAController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_LinkCDtoFAController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();

        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "CDTFA")]
        // GET: F_LinkCDtoFA
        public ActionResult CurrencyDefinitionToFactoryAccount()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID =null;

            string factoryID = null;

            if (companyID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);

                ViewBag.CurrencyList = GetCurrencies(companyID);
            }
            else if (branchID != null)
            {
                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCondByB(branchID);

                string branchCompany = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == branchID).FirstOrDefault().CompanyID;
                ViewBag.CurrencyList = GetCurrencies(branchCompany);
            }
            else if (factoryID != null)
            {
                ViewBag.FactoryList = factoryID;

                ViewBag.FactoryName = repetitionBusiness.GetFactoryName(factoryID);

                string factoryCompany = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factoryID).FirstOrDefault().CompanyID;
                ViewBag.CurrencyList = GetCurrencies(factoryCompany);
            }

            return View();
        }


        public JsonResult GetFactoryName(string factoryID)
        {
            string userID = FabulousErp.Business.GetUserId();
            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.FactoryID == factoryID && x.UserID == userID).FirstOrDefault();
            if (checkAccess != null)
            {
                return Json(repetitionBusiness.GetFactoryName(factoryID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NotAccess", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetFactoryAccounts(string factoryID)
        {
            IQueryable<Currencies_Definition_DTO> currencies_Definition_s = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Currencies_Definition_DTO
            {
                F_AID = x.F_AID,

                AccountID = x.AccountID,

                AccountName = x.AccountName
            });

            return Json(currencies_Definition_s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountName(int f_AID)
        {
            var getName = DB.F_CreateAccount_Tables.Where(x => x.F_AID == f_AID).FirstOrDefault();
            return Json(getName.AccountName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencyName(string currencyID)
        {
            var getName = DB.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();
            return Json(getName.CurrencyName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountsOfCurrency(string currencyID)
        {
            var checkExist = DB.F_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();
            if (checkExist != null)
            {
                List<Currencies_Definition_DTO> currencies_Definition_DTOs = DB.F_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).Select(x => new Currencies_Definition_DTO
                {

                    F_AID = x.F_AID,

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

        public JsonResult SaveAccountToCurrency(string currencyID, int f_AID, int f_AID2)
        {
            var checkDuplicate = DB.F_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();

            if (checkDuplicate == null)
            {

                List<F_AccountCurrencyDefinition_Table> accountCurrencyDefinition_Table = new List<F_AccountCurrencyDefinition_Table>()
            {
                new F_AccountCurrencyDefinition_Table{CurrencyID = currencyID,F_AID = f_AID,Type="Profit"},

                new F_AccountCurrencyDefinition_Table{CurrencyID = currencyID,F_AID = f_AID2,Type="Loss"}
            };

                DB.F_AccountCurrencyDefinition_Tables.AddRange(accountCurrencyDefinition_Table);
                DB.SaveChanges();

                return null;
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateAccountToCurrency(string currencyID, int f_AID, int f_AID2)
        {
            var updateProfit = DB.F_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID && x.Type == "Profit").FirstOrDefault();
            if (updateProfit != null)
            {
                updateProfit.F_AID = f_AID;
                DB.SaveChanges();
            }

            var updateLoss = DB.F_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID && x.Type == "Loss").FirstOrDefault();
            if (updateLoss != null)
            {
                updateLoss.F_AID = f_AID2;
                DB.SaveChanges();
            }
            return null;
        }


        private SelectList GetCurrencies(string companyID)
        {
            IQueryable<CurrenciesDefinition_Table> currenciesID = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID && x.CurrencyID != companyID);
            SelectList currencyList = new SelectList(currenciesID, "CurrencyID", "ISOCode");
            return currencyList;
        }

    }
}