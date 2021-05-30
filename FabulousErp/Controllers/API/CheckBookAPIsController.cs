using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousModels.DTOModels.Transaction.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FabulousErp.Controllers.API
{
   // [Authorize]
    public class CheckBookAPIsController : ApiController
    {
        DBContext DB = new DBContext();

        [HttpGet]
        public HttpResponseMessage GetCheckbookData(int checkbookID, string companyID)
        {
            try
            {
                var data = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID).FirstOrDefault();
                CheckbookTransactions_DTO checkbookTransactions_DTO = new CheckbookTransactions_DTO()
                {
                    CheckbookName = data.C_CheckbookName,
                    CurrencyID = data.CurrencyID,
                    CurrencyName = data.CurrenciesDefinition_Table.ISOCode,
                    Company_AccountsName = data.C_CreateAccount_Table.AccountName,
                    C_AID = data.C_AID,
                    Company_AccountsID = data.C_CreateAccount_Table.AccountID,
                    MaxAmount = data.C_CheckbookMaxAmount,
                    MinAmount = data.C_CheckbookMinAmount,
                    NextDepositNumber = data.C_NextDepositNumber,
                    NextWithdrawNumber = data.C_NextWithdrawNumber
                };
                return Request.CreateResponse(HttpStatusCode.OK, checkbookTransactions_DTO);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckbookSecurity(int checkbookID, string companyID)
        {
            try
            {
                string pass = "";
                string userID = "";
                var check = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID).FirstOrDefault();
                if (check != null)
                {
                    if (check.C_PasswordAccess != "")
                    {
                        pass = "Exist";
                    }
                    else if (check.C_UserIDAccess != "")
                    {
                        if (check.C_UserIDAccess == FabulousErp.Business.GetUserId())
                        {
                            userID = "UserIDAccess";
                        }
                        else
                        {
                            userID = "NoPermit";
                        }
                    }
                    CheckbookTransactions_DTO CheckbookTransactions_DTO = new CheckbookTransactions_DTO()
                    {
                        CB_Password = pass,
                        CB_UserID = userID
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, CheckbookTransactions_DTO);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckbookSecurityCheck(string companyID, int checkbookID, string password)
        {
            try
            {
                var check = DB.C_CheckBookSetting_Tables.Where(x => x.CompanyID == companyID && x.C_CBSID == checkbookID && x.C_PasswordAccess == password).FirstOrDefault();
                if (check != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Valid Password");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Check Your Password");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage DocumentNumberCheck(string documentNumber, string companyID)
        {
            var check = DB.C_CheckbookTransactions_Tables
                .Where(x => x.CompanyID == companyID && x.C_CheckNumber == documentNumber).FirstOrDefault();
            if (check != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Exist");
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.Accepted, "NotExist");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetProfitLossAccount(string currencyID, string companyID)
        {
            List<CheckbookTransactions_DTO> checkbookTransactions_DTOs = DB.AccountCurrencyDefinition_Tables.Where(x => x.CurrencyID == currencyID).Select(x => new CheckbookTransactions_DTO
            {
                C_AID = x.C_AID,
                AccountType = x.Type,
                Currency_AccountsID = x.C_CreateAccount_Table.AccountID,
                Company_AccountsName = x.C_CreateAccount_Table.AccountName
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, checkbookTransactions_DTOs);
        }
        public int GetCheckbookByUser(string UserId)
        {
            C_CheckBookSetting_table CheckBook=
                DB.C_CheckBookSetting_Tables.FirstOrDefault(x => x.C_UserIDAccess == UserId);
            if (CheckBook != null)
            {
                return CheckBook.C_CBSID;
            }
            else
            {
                return -1;
            }
        }

        //[HttpGet]
        //public HttpRequestMessage GetCheckbook_Transactions_Data(string companyID, int journalEntryNumber)
        //{

        //}






    }
}
