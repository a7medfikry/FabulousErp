using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CheckBook;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.ViewModels.Settings.Financial.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Branch.B_CheckBook
{
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class B_CheckBookSettingController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public B_CheckBookSettingController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }
        DBContext DB = new DBContext();

        // GET: B_CheckBookSetting
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SBCB")]
        public ActionResult BranchCheckBook()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.BranchID = repetitionBusiness.RetrieveBranchIDListCond(companyID);

                var list = DB.UACompPremission_Tables.Where(x => x.CompanyID == companyID).ToList();
                SelectList UserIDs = new SelectList(list, "UserID", "UserID");
                ViewBag.UserIDs = UserIDs;
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

        public JsonResult GetCurrency(string branchID)
        {
            var check = DB.CompanyBranchInfo_Tables.Where(x => x.BranchID == branchID).FirstOrDefault();
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

        public JsonResult GetAccountsID(string branchID)
        {
            List<Checkbook_DTO> Checkbook_DTO = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID).Select(x => new Checkbook_DTO
            {
                Branch_AccountsID = x.AccountID,
                AccountName = x.AccountName,
                AccountID = x.B_AID
            }).ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountName(int AccountID, string branchID)
        {
            List<Checkbook_DTO> Checkbook_DTO = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID && x.B_AID == AccountID).Select(x => new Checkbook_DTO
            {
                AccountName = x.AccountName,
            }).ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckAccountCurrency(int AccountID, string currencyID)
        {
            var check = DB.B_CurrencyCreateAccount_Tables.Where(x => x.B_AID == AccountID && x.CurrencyID == currencyID).FirstOrDefault();
            if (check != null)
            {
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveCheckbookSetting(string BranchID, string CheckbookID, string CheckbookName, bool? Status, string CheckbookType, string Currency, int AccountID, double? MinAmount, double? MaxAmount, int NextWithdraw, int NextDeposit, double? CurrentBalance, double? CurrentCash, double? LastReconsileBalance, double? LastReconsileDate, string BankName, int? BankAccountNumber, string BankAccountName, string BranchName, string SwiftCode, string IBAN, string UserID, string Password)
        {
            var check = DB.B_CheckBookSetting_Tables.Where(x => x.BranchID == BranchID && x.B_CheckbookID == CheckbookID).FirstOrDefault();
            if (check != null)
            {
                return Json("CheckbookFalse", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var check2 = DB.B_CheckBookSetting_Tables.Where(x => x.BranchID == BranchID && x.B_AID == AccountID).FirstOrDefault();
                if (check2 != null)
                {
                    return Json("AccountIDExist", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    B_CheckBookSetting_table b_CheckBookSetting_Table = new B_CheckBookSetting_table()
                    {
                        BranchID = BranchID,
                        B_CheckbookID = CheckbookID,
                        B_CheckbookName = CheckbookName,
                        B_CheckbookStatus = Status,
                        B_CheckbookType = CheckbookType,
                        CurrencyID = Currency,
                        B_AID = AccountID,
                        B_CheckbookMinAmount = MinAmount,
                        B_CheckbookMaxAmount = MaxAmount,
                        B_NextWithdrawNumber = NextWithdraw,
                        B_NextDepositNumber = NextDeposit,
                        B_CurrentCheckbookBalance = CurrentBalance,
                        B_CurrentCashAccountBalance = CurrentCash,
                        B_LastReconcileBalance = LastReconsileBalance,
                        B_LastReconcileDate = LastReconsileDate,
                        B_BankName = BankName,
                        B_BankAccountNumber = BankAccountNumber,
                        B_BankAccountName = BankAccountName,
                        B_BranchName = BranchName,
                        B_SwiftCode = SwiftCode,
                        B_IBAN = IBAN,
                        B_UserIDAccess = UserID,
                        B_PasswordAccess = Password
                    };
                    DB.B_CheckBookSetting_Tables.Add(b_CheckBookSetting_Table);
                    DB.SaveChanges();
                }
            }
            return Json("True", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchCheckbook(string branchID)
        {
            List<Checkbook_DTO> Checkbook_DTO = DB.B_CheckBookSetting_Tables.Where(x => x.BranchID == branchID).Select(x => new Checkbook_DTO
            {
                CheckbookID = x.B_CheckbookID
            }).ToList();

            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchCheckbook(string branchID, string CheckbookID)
        {
            var data = DB.B_CheckBookSetting_Tables.Where(x => x.B_CheckbookID == CheckbookID && x.BranchID == branchID).FirstOrDefault();
            Checkbook_DTO Checkbook_DTO = new Checkbook_DTO()
            {
                CheckbookID = data.B_CheckbookID,
                CheckbookName = data.B_CheckbookName,
                AccountID = data.B_AID,
                AccountName = data.B_CreateAccount_Table.AccountName,
                Status = data.B_CheckbookStatus,
                BankAccountName = data.B_BankAccountName,
                BankAccountNumber = data.B_BankAccountNumber,
                BankName = data.B_BankName,
                BranchName = data.B_BranchName,
                CheckbookType = data.B_CheckbookType,
                CurrencyID = data.CurrencyID,
                CurrentCashBalance = data.B_CurrentCashAccountBalance,
                CurrentCheckbookBalance = Convert.ToDouble(data.B_CurrentCheckbookBalance),
                IBAN = data.B_IBAN,
                LastReconsileBalance = data.B_LastReconcileBalance,
                LastReconsileDate = data.B_LastReconcileDate,
                MaxAmount = data.B_CheckbookMaxAmount,
                MinAmount = data.B_CheckbookMinAmount,
                NextDeposit = data.B_NextDepositNumber,
                NextWithDraw = data.B_NextWithdrawNumber,
                Password = data.B_PasswordAccess,
                SwiftCode = data.B_SwiftCode,
                UserID = data.B_UserIDAccess
            };
            return Json(Checkbook_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateCheckbookSetting(string BranchID, string CheckbookID, string CheckbookName, bool? Status, string CheckbookType, string Currency, int AccountID, double? MinAmount, double? MaxAmount, int NextWithdraw, int NextDeposit, double? CurrentBalance, double? CurrentCash, double? LastReconsileBalance, double? LastReconsileDate, string BankName, int? BankAccountNumber, string BankAccountName, string BranchName, string SwiftCode, string IBAN, string UserID, string Password)
        {
            var check = DB.B_CheckBookSetting_Tables.Where(x => x.BranchID == BranchID && x.B_CheckbookID == CheckbookID).FirstOrDefault();
            if (check != null)
            {
                check.B_CheckbookName = CheckbookName;
                check.B_CheckbookStatus = Status;
                check.B_CheckbookType = CheckbookType;
                check.CurrencyID = Currency;
                check.B_AID = AccountID;
                check.B_CheckbookMinAmount = MinAmount;
                check.B_CheckbookMaxAmount = MaxAmount;
                check.B_NextWithdrawNumber = NextWithdraw;
                check.B_NextDepositNumber = NextDeposit;
                check.B_CurrentCheckbookBalance = CurrentBalance;
                check.B_CurrentCashAccountBalance = CurrentCash;
                check.B_LastReconcileBalance = LastReconsileBalance;
                check.B_LastReconcileDate = LastReconsileDate;
                check.B_BankName = BankName;
                check.B_BankAccountNumber = BankAccountNumber;
                check.B_BankAccountName = BankAccountName;
                check.B_BranchName = BranchName;
                check.B_SwiftCode = SwiftCode;
                check.B_IBAN = IBAN;
                check.B_UserIDAccess = UserID;
                check.B_PasswordAccess = Password;

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
            var item = DB.B_CheckBookSetting_Tables.Where(x => x.B_CheckbookID == CheckbookID).FirstOrDefault();
            DB.B_CheckBookSetting_Tables.Remove(item);
            DB.SaveChanges();

            return Json("True", JsonRequestBehavior.AllowGet);
        }









    }
}