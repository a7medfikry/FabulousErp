using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousErp.MyRoleProvider;
using FabulousModels.ViewModels.Settings.Financial.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_CheckBook
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_CheckBookSettingController : Controller
    {
        DBContext DB = new DBContext();
        // GET: C_CheckBookSetting
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCCB")]
        public ActionResult CompanyCheckBook()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var list = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList Currency = new SelectList(list, "CurrencyID", "ISOCode");
            ViewBag.Currency = Currency;

            var list2 = DB.C_CreateAccount_Tables
                .Where(x => x.CompanyID == companyID && (x.C_AnalyticAccountID == null || x.C_AnalyticAccountID == "") && (x.C_CostCenterID == null || x.C_CostCenterID == "") && (x.C_CostCenterGroupID == null || x.C_CostCenterGroupID == "") && (x.C_Prefix == "Pay" || x.C_Prefix == "Rec" || x.C_Prefix == "Asset" || x.C_Prefix == "Tax"))
                .ToList();
            SelectList AccountID = new SelectList(list2, "C_AID", "AccountID");
            ViewBag.AccountID = AccountID;

            var list3 = DB.UACompPremission_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList UserIDs = new SelectList(list3, "UserID", "UserID");
            ViewBag.UserIDs = UserIDs;

            var list4 = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID).ToList();
            SelectList checkbookSearch = new SelectList(list4, "C_CheckbookID", "C_CheckbookID");
            ViewBag.checkbookSearch = checkbookSearch;

            return View();
        }
        public JsonResult GetCompanyAccountsID()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<Checkbook_DTO> Checkbook_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).Select(x => new Checkbook_DTO
            {
                Company_AccountsID = x.AccountID,
                AccountName = x.AccountName,
                AccountID = x.C_AID
            }).ToList().OrderBy(x=>Convert.ToUInt32(x.Company_AccountsID.Replace("-","")))
            .ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAccountName(int AccountID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            List<Checkbook_DTO> Checkbook_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.C_AID == AccountID).Select(x => new Checkbook_DTO
            {
                AccountName = x.AccountName
            }).ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveCheckbookSetting(string CheckbookID, string CheckbookName, bool? Status, string CheckbookType, string Currency, int AccountID, double? MinAmount, double? MaxAmount, int NextWithdraw, int NextDeposit, double? CurrentBalance, double? CurrentCash, double? LastReconsileBalance, double? LastReconsileDate, string BankName, int? BankAccountNumber, string BankAccountName, string BranchName, string SwiftCode, string IBAN, string UserID, string Password)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            var check = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CheckbookID == CheckbookID).FirstOrDefault();
            if (check != null)
            {
                return Json("CheckbookFalse", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //var check2 = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_AID == AccountID).FirstOrDefault();
                //if (check2 != null)
                //{
                //    return Json("AccountIDExist", JsonRequestBehavior.AllowGet);
                //}
                //else
                {
                    C_CheckBookSetting_table c_CheckBookSetting_Table = new C_CheckBookSetting_table()
                    {
                        CompanyID = companyID,
                        C_CheckbookID = CheckbookID,
                        C_CheckbookName = CheckbookName,
                        C_CheckbookStatus = Status,
                        C_CheckbookType = CheckbookType,
                        CurrencyID = Currency,
                        C_AID = AccountID,
                        C_CheckbookMinAmount = MinAmount,
                        C_CheckbookMaxAmount = MaxAmount,
                        C_NextWithdrawNumber = NextWithdraw,
                        C_NextDepositNumber = NextDeposit,
                        C_CurrentCheckbookBalance = CurrentBalance,
                        C_CurrentCashAccountBalance = CurrentCash,
                        C_LastReconcileBalance = LastReconsileBalance,
                        C_LastReconcileDate = LastReconsileDate,
                        C_BankName = BankName,
                        C_BankAccountNumber = BankAccountNumber,
                        C_BankAccountName = BankAccountName,
                        C_BranchName = BranchName,
                        C_SwiftCode = SwiftCode,
                        C_IBAN = IBAN,
                        C_UserIDAccess = UserID,
                        C_PasswordAccess = Password
                    };
                    DB.C_CheckBookSetting_Tables.Add(c_CheckBookSetting_Table);
                    DB.SaveChanges();

                    var check3 = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.C_AID == AccountID).FirstOrDefault();
                    if (check3 != null)
                    {
                        check3.C_Prefix = "Cash";
                        DB.SaveChanges();
                    }
                }
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchCheckbook(string CheckbookID)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            decimal totalBalance = 0;
            double accountBalance = 0;
            if (DB.C_CheckbookTransactions_Tables.Where(x => x.CompanyID == companyID && x.C_CheckBookSetting_Table.C_CheckbookID == CheckbookID).Any())
            {
                totalBalance = DB.C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckBookSetting_Table.C_CheckbookID == CheckbookID)
                .Select(x => x.C_Balance)
                .Sum();
            }
            var currentCBBalance = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CheckbookID == CheckbookID).FirstOrDefault();
            currentCBBalance.C_CurrentCheckbookBalance = Convert.ToDouble(totalBalance);
            DB.SaveChanges();

            var list = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CheckbookID == CheckbookID).FirstOrDefault();
            if (list != null)
            {
                var accountID = list.C_AID;
                var accountCheck = DB.C_GeneralLedger_Tables.Where(x => x.C_AID == accountID).FirstOrDefault();
                if (accountCheck != null)
                {
                    accountBalance = DB.C_GeneralLedger_Tables.Where(x => x.C_AID == accountID).Sum(x => x.Ballance);
                }
                else
                {
                    accountBalance = 0;
                }
            }

            var data = DB.C_CheckBookSetting_Tables.Where(x => x.C_CheckbookID == CheckbookID && x.CompanyID == companyID).FirstOrDefault();
            Checkbook_DTO Checkbook_DTO = new Checkbook_DTO()
            {
                CheckbookID = data.C_CheckbookID,
                CheckbookName = data.C_CheckbookName,
                AccountID = data.C_AID,
                AccountName = data.C_CreateAccount_Table.AccountName,
                Status = data.C_CheckbookStatus,
                BankAccountName = data.C_BankAccountName,
                BankAccountNumber = data.C_BankAccountNumber,
                BankName = data.C_BankName,
                BranchName = data.C_BranchName,
                CheckbookType = data.C_CheckbookType,
                CurrencyID = data.CurrencyID,
                CurrentCashBalance = data.C_CurrentCashAccountBalance,
                CurrentCheckbookBalance = Convert.ToDouble(data.C_CurrentCheckbookBalance),
                IBAN = data.C_IBAN,
                LastReconsileBalance = data.C_LastReconcileBalance,
                LastReconsileDate = data.C_LastReconcileDate,
                MaxAmount = data.C_CheckbookMaxAmount,
                MinAmount = data.C_CheckbookMinAmount,
                NextDeposit = data.C_NextDepositNumber,
                NextWithDraw = data.C_NextWithdrawNumber,
                Password = data.C_PasswordAccess,
                SwiftCode = data.C_SwiftCode,
                UserID = data.C_UserIDAccess,
                cashAccountBalance = accountBalance
            };

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckAccountCurrency(int AccountID, string currencyID)
        {
            var check = DB.C_CurrencyCreateAccount_Tables.Where(x => x.C_AID == AccountID && x.CurrencyID == currencyID).FirstOrDefault();
            if (check != null)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateCheckbookSetting(string CheckbookID, string CheckbookName, bool? Status, string CheckbookType, string Currency, int AccountID, double? MinAmount, double? MaxAmount, int NextWithdraw, int NextDeposit, double? CurrentBalance, double? CurrentCash, double? LastReconsileBalance, double? LastReconsileDate, string BankName, int? BankAccountNumber, string BankAccountName, string BranchName, string SwiftCode, string IBAN, string UserID, string Password)
        {
            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            var check = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == CompanyID && x.C_CheckbookID == CheckbookID).FirstOrDefault();
            if (check != null)
            {
                check.C_AID = AccountID;
                check.C_CheckbookName = CheckbookName;
                check.C_CheckbookStatus = Status;
                check.C_CheckbookType = CheckbookType;
                check.CurrencyID = Currency;
                check.C_CheckbookMinAmount = MinAmount;
                check.C_CheckbookMaxAmount = MaxAmount;
                check.C_NextWithdrawNumber = NextWithdraw;
                check.C_NextDepositNumber = NextDeposit;
                check.C_CurrentCheckbookBalance = CurrentBalance;
                check.C_CurrentCashAccountBalance = CurrentCash;
                check.C_LastReconcileBalance = LastReconsileBalance;
                check.C_LastReconcileDate = LastReconsileDate;
                check.C_BankName = BankName;
                check.C_BankAccountNumber = BankAccountNumber;
                check.C_BankAccountName = BankAccountName;
                check.C_BranchName = BranchName;
                check.C_SwiftCode = SwiftCode;
                check.C_IBAN = IBAN;
                check.C_UserIDAccess = UserID;
                check.C_PasswordAccess = Password;

                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteCheckbook(string CheckbookID)
        {
            var item = DB.C_CheckBookSetting_Tables.Where(x => x.C_CheckbookID == CheckbookID).FirstOrDefault();
            DB.C_CheckBookSetting_Tables.Remove(item);
            DB.SaveChanges();

            return Json("True", JsonRequestBehavior.AllowGet);
        }
    }
}