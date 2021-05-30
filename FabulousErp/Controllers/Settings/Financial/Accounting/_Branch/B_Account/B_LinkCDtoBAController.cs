using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Branch.B_Account
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_LinkCDtoBAController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public B_LinkCDtoBAController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: B_LinkCDtoBA
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "CDTBA")]
        public ActionResult CurrencyDefinitionToBranchAccount()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            string branchID = null;

            if (companyID != null)
            {
                ViewBag.BranchList = repetitionBusiness.RetrieveBranchIDListCond(companyID);

                var accountID = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).ToList();
                SelectList accountList = new SelectList(accountID, "C_AID", "AccountID");
                ViewBag.AccountList = accountList;

                ViewBag.CurrencyList = GetCurrencies(companyID);
            }
            else if (branchID != null)
            {
                ViewBag.BranchList = branchID;

                ViewBag.BranchName = repetitionBusiness.GetBranchName(branchID);

                var getCompanyOfBranch = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == branchID).FirstOrDefault();

                ViewBag.CurrencyList = GetCurrencies(getCompanyOfBranch.CompanyID);
            }

            return View();
        }

        public JsonResult GetBranchName(string branchID)
        {
            string userID = FabulousErp.Business.GetUserId();
            var checkAccess = DB.UABranchPremission_Tables.Where(x => x.BranchID == branchID && x.UserID == userID).FirstOrDefault();
            if(checkAccess != null)
            {
                return Json(repetitionBusiness.GetBranchName(branchID), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("NotAccess", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetBranchAccounts(string branchID)
        {
            IQueryable<Currencies_Definition_DTO> currencies_Definition_s = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID).Select(x => new Currencies_Definition_DTO
            {
                B_AID = x.B_AID,

                AccountID = x.AccountID,

                AccountName = x.AccountName
            });

            return Json(currencies_Definition_s, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountName(int b_AID)
        {
            var getName = DB.B_CreateAccount_Tables.Where(x => x.B_AID == b_AID).FirstOrDefault();
            return Json(getName.AccountName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencyName(string currencyID)
        {
            var getName = DB.CurrenciesDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();
            return Json(getName.CurrencyName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountsOfCurrency(string currencyID)
        {
            var checkExist = DB.B_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();
            if (checkExist != null)
            {
                List<Currencies_Definition_DTO> currencies_Definition_DTOs = DB.B_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).Select(x => new Currencies_Definition_DTO
                {

                    B_AID = x.B_AID,

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

        public JsonResult SaveAccountToCurrency(string currencyID, int b_AID, int b_AID2)
        {
            var checkDuplicate = DB.B_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).FirstOrDefault();

            if (checkDuplicate == null)
            {

                List<B_AccountCurrencyDefinition_Table> accountCurrencyDefinition_Table = new List<B_AccountCurrencyDefinition_Table>()
            {
                new B_AccountCurrencyDefinition_Table{CurrencyID = currencyID,B_AID = b_AID,Type="Profit"},

                new B_AccountCurrencyDefinition_Table{CurrencyID = currencyID,B_AID = b_AID2,Type="Loss"}
            };

                DB.B_AccountCurrencyDefinition_Tables.AddRange(accountCurrencyDefinition_Table);
                DB.SaveChanges();

                return null;
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateAccountToCurrency(string currencyID, int b_AID, int b_AID2)
        {
            var updateProfit = DB.B_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID && x.Type == "Profit").FirstOrDefault();
            if (updateProfit != null)
            {
                updateProfit.B_AID = b_AID;
                DB.SaveChanges();
            }

            var updateLoss = DB.B_AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID && x.Type == "Loss").FirstOrDefault();
            if (updateLoss != null)
            {
                updateLoss.B_AID = b_AID2;
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