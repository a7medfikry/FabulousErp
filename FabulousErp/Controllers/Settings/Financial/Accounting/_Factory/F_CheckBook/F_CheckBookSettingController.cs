using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CheckBook;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.ViewModels.Settings.Financial.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_CheckBook
{
    [AuthorizationFilter]
    public class F_CheckBookSettingController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public F_CheckBookSettingController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: F_CheckBookSetting
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFCB")]
        public ActionResult FactoryCheckBook()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.FactoryID = repetitionBusiness.RetrieveFactoryIDListCond(companyID);

                var list = DB.UAFactoryPremission_Tables.Where(x => x.CompanyID == companyID).ToList();
                SelectList UserIDs = new SelectList(list, "UserID", "UserID");
                ViewBag.UserIDs = UserIDs;
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

        public JsonResult GetCurrency(string factoryID)
        {
            var check = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == factoryID).FirstOrDefault();
            if (check != null)
            {
                var companyID = check.CompanyID;
                List<Checkbook_DTO> Checkbook_DTO = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).Select(x => new Checkbook_DTO
                {
                    CurrencyID = x.CurrencyID,
                    CurrencyName = x.ISOCode
                }).ToList();
                return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetAccountsID(string factoryID)
        {
            List<Checkbook_DTO> Checkbook_DTO = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Checkbook_DTO
            {
                Factory_AccountsID = x.AccountID,
                AccountName = x.AccountName,
                AccountID = x.F_AID
            }).ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountName(int AccountID, string factoryID)
        {
            List<Checkbook_DTO> Checkbook_DTO = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID && x.F_AID == AccountID).Select(x => new Checkbook_DTO
            {
                AccountName = x.AccountName,
            }).ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckAccountCurrency(int AccountID, string currencyID)
        {
            var check = DB.F_CurrencyCreateAccount_Tables.Where(x => x.F_AID == AccountID && x.CurrencyID == currencyID).FirstOrDefault();
            if (check != null)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveCheckbookSetting(string FactoryID, string CheckbookID, string CheckbookName, bool? Status, string CheckbookType, string Currency, int AccountID, double? MinAmount, double? MaxAmount, int NextWithdraw, int NextDeposit, double? CurrentBalance, double? CurrentCash, double? LastReconsileBalance, double? LastReconsileDate, string BankName, int? BankAccountNumber, string BankAccountName, string BranchName, string SwiftCode, string IBAN, string UserID, string Password)
        {
            var check = DB.F_CheckBookSetting_Tables.Where(x => x.FactoryID == FactoryID && x.F_CheckbookID == CheckbookID).FirstOrDefault();
            if (check != null)
            {
                return Json("CheckbookFalse", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var check2 = DB.F_CheckBookSetting_Tables.Where(x => x.FactoryID == FactoryID && x.F_AID == AccountID).FirstOrDefault();
                if (check2 != null)
                {
                    return Json("AccountIDExist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    F_CheckBookSetting_table f_CheckBookSetting_Table = new F_CheckBookSetting_table()
                    {
                        FactoryID = FactoryID,
                        F_CheckbookID = CheckbookID,
                        F_CheckbookName = CheckbookName,
                        F_CheckbookStatus = Status,
                        F_CheckbookType = CheckbookType,
                        CurrencyID = Currency,
                        F_AID = AccountID,
                        F_CheckbookMinAmount = MinAmount,
                        F_CheckbookMaxAmount = MaxAmount,
                        F_NextWithdrawNumber = NextWithdraw,
                        F_NextDepositNumber = NextDeposit,
                        F_CurrentCheckbookBalance = CurrentBalance,
                        F_CurrentCashAccountBalance = CurrentCash,
                        F_LastReconcileBalance = LastReconsileBalance,
                        F_LastReconcileDate = LastReconsileDate,
                        F_BankName = BankName,
                        F_BankAccountNumber = BankAccountNumber,
                        F_BankAccountName = BankAccountName,
                        F_BranchName = BranchName,
                        F_SwiftCode = SwiftCode,
                        F_IBAN = IBAN,
                        F_UserIDAccess = UserID,
                        F_PasswordAccess = Password
                    };
                    DB.F_CheckBookSetting_Tables.Add(f_CheckBookSetting_Table);
                    DB.SaveChanges();
                }
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFactoryCheckbook(string factoryID)
        {
            List<Checkbook_DTO> Checkbook_DTO = DB.F_CheckBookSetting_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Checkbook_DTO
            {
                CheckbookID = x.F_CheckbookID
            }).ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCheckbook(string factoryID, string CheckbookID)
        {
            var data = DB.F_CheckBookSetting_Tables.Where(x => x.F_CheckbookID == CheckbookID && x.FactoryID == factoryID).FirstOrDefault();
            Checkbook_DTO Checkbook_DTO = new Checkbook_DTO()
            {
                CheckbookID = data.F_CheckbookID,
                CheckbookName = data.F_CheckbookName,
                AccountID = data.F_AID,
                AccountName = data.F_CreateAccount_Table.AccountName,
                Status = data.F_CheckbookStatus,
                BankAccountName = data.F_BankAccountName,
                BankAccountNumber = data.F_BankAccountNumber,
                BankName = data.F_BankName,
                BranchName = data.F_BranchName,
                CheckbookType = data.F_CheckbookType,
                CurrencyID = data.CurrencyID,
                CurrentCashBalance = data.F_CurrentCashAccountBalance,
                CurrentCheckbookBalance = Convert.ToDouble(data.F_CurrentCheckbookBalance),
                IBAN = data.F_IBAN,
                LastReconsileBalance = data.F_LastReconcileBalance,
                LastReconsileDate = data.F_LastReconcileDate,
                MaxAmount = data.F_CheckbookMaxAmount,
                MinAmount = data.F_CheckbookMinAmount,
                NextDeposit = data.F_NextDepositNumber,
                NextWithDraw = data.F_NextWithdrawNumber,
                Password = data.F_PasswordAccess,
                SwiftCode = data.F_SwiftCode,
                UserID = data.F_UserIDAccess
            };
            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCheckbookSetting(string FactoryID, string CheckbookID, string CheckbookName, bool? Status, string CheckbookType, string Currency, int AccountID, double? MinAmount, double? MaxAmount, int NextWithdraw, int NextDeposit, double? CurrentBalance, double? CurrentCash, double? LastReconsileBalance, double? LastReconsileDate, string BankName, int? BankAccountNumber, string BankAccountName, string BranchName, string SwiftCode, string IBAN, string UserID, string Password)
        {
            var check = DB.F_CheckBookSetting_Tables.Where(x => x.FactoryID == FactoryID && x.F_CheckbookID == CheckbookID).FirstOrDefault();
            if (check != null)
            {
                check.F_CheckbookName = CheckbookName;
                check.F_CheckbookStatus = Status;
                check.F_CheckbookType = CheckbookType;
                check.CurrencyID = Currency;
                check.F_AID = AccountID;
                check.F_CheckbookMinAmount = MinAmount;
                check.F_CheckbookMaxAmount = MaxAmount;
                check.F_NextWithdrawNumber = NextWithdraw;
                check.F_NextDepositNumber = NextDeposit;
                check.F_CurrentCheckbookBalance = CurrentBalance;
                check.F_CurrentCashAccountBalance = CurrentCash;
                check.F_LastReconcileBalance = LastReconsileBalance;
                check.F_LastReconcileDate = LastReconsileDate;
                check.F_BankName = BankName;
                check.F_BankAccountNumber = BankAccountNumber;
                check.F_BankAccountName = BankAccountName;
                check.F_BranchName = BranchName;
                check.F_SwiftCode = SwiftCode;
                check.F_IBAN = IBAN;
                check.F_UserIDAccess = UserID;
                check.F_PasswordAccess = Password;

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
            var item = DB.F_CheckBookSetting_Tables.Where(x => x.F_CheckbookID == CheckbookID).FirstOrDefault();
            DB.F_CheckBookSetting_Tables.Remove(item);
            DB.SaveChanges();

            return Json("True", JsonRequestBehavior.AllowGet);
        }














    }
}