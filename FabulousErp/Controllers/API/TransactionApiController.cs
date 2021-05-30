using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousModels.APIModels;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousModels.DTOModels.Transaction.Financial;
using FabulousModels.DTOModels.Transaction.Financial.Company;
using FabulousModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Filters;
using FabulousDB.Models;
using FabulousModels.Inventory;

namespace FabulousErp.Controllers.API
{
   // [Authorize]
   [FabulousErp.MyRoleProvider.AuthorizationFilter]
    public class TransactionApiController : ApiController
    {
        DBContext DB = new DBContext();

        [HttpGet]
        public HttpResponseMessage CheckDateInPeriods(string rDate, string companyID,string AreaName=null)
        {
            try
            {
                if (string.IsNullOrEmpty(AreaName))
                {
                    AreaName = Business.GetCurrentAreaName();
                }
                var getFiscalYearOfComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).ToList();
                DateTime date = Convert.ToDateTime(rDate);
                foreach (var item in checkYearOpen)
                {
                    if (date >= Convert.ToDateTime(item.Fiscal_Year_Start) && date <= Convert.ToDateTime(item.Fiscal_Year_End))
                    {
                        int year = item.YearID;
                        var checkDateValid = DB.FiscalYear_Tables.Include(x=>x.Fiscal_year_area).ToList().Where(x => x.YearID == year && date >= Convert.ToDateTime(x.Period_Start_Date) && date <= Convert.ToDateTime(x.Period_End_Date)).FirstOrDefault();
                        if (checkDateValid.Fiscal_year_area.Any(x=>x.Allowed==true))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "DateNotValid");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            //return null;
        }

        /****************************************************************/
        [HttpGet]
        public HttpResponseMessage CheckDateInAdjustment(string rDate, string companyID)
        {
            try
            {
                //ReturnHERE
                string AreaName = FabulousErp.Business.GetCurrentAreaName();
                var getFiscalYearOfComp = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
                DateTime date = Convert.ToDateTime(rDate);
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == getFiscalYearOfComp.Fiscal_Year_ID && (x.Closed!=true) &&x.Year== date.Year.ToString()).ToList();

                foreach (var item in checkYearOpen)
                {
                    if (date >= Convert.ToDateTime(item.Fiscal_Year_Start) && date <= Convert.ToDateTime(item.Fiscal_Year_End))
                    {
                        int year = item.YearID;
                        var checkDateValid = DB.FiscalAdjustment_Tables.Include(x=>x.Fiscal_year_area).ToList().Where(x => x.YearID == year && date == Convert.ToDateTime(x.Period_Start_Date)).FirstOrDefault();
                        if (checkDateValid == null)
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable);
                        }
                        if (checkDateValid.Fiscal_year_area.Any(x=>x.Area_name==AreaName&&x.Allow_adjust))
                        {
                            return Request.CreateResponse(HttpStatusCode.OK);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NotAcceptable);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "DateNotValid");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
            //return null;
        }
        /****************************************************************/

        [HttpGet]
        public HttpResponseMessage GetExchangeCurrencyRate(string currencyID, string postingDate)
        {
            try
            {
                DateTime date = Convert.ToDateTime(postingDate);

                List<TCGE_DTO> tCGE_DTO = DB.CurrenciesExchange_Tables.ToList().Where(x => x.CurrencyID == currencyID && date >= Convert.ToDateTime(x.StartDate) && date <= Convert.ToDateTime(x.ExpireDate)).Select(x => new TCGE_DTO
                {
                    Rate = x.Rate,

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTO);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCurrencyFormate(string companyID=null)
        {
            try
            {
                if (companyID == null)
                {
                    companyID = FabulousErp.Business.GetCompanyId();
                }
                var getFormatSetting = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

                if (getFormatSetting != null)
                {
                    if (string.IsNullOrEmpty(getFormatSetting.Suffix))
                    {
                        getFormatSetting.Suffix = " EGP";
                    }
                    Formate_Setting_DTO formate_Setting_DTO = new Formate_Setting_DTO()
                    {
                        DecimalNumber = getFormatSetting.DecimalNumber,

                        Prefix = getFormatSetting.Prefix,

                        Suffix = getFormatSetting.Suffix,

                        Decimal = getFormatSetting.Decimal,

                        Thousands = getFormatSetting.Thousands,
                        
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, formate_Setting_DTO);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // Buttom Part Of Transaction
        [HttpGet]
        public HttpResponseMessage GetFinancialAccounts(string companyID)
        {
            try
            {
                List<TCGE_DTO> tCGE_DTOs = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new TCGE_DTO
                {

                    C_AID = x.C_AID,

                    AccountID = x.AccountID,

                    AccountName = x.AccountName

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTOs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        //K Update
        [HttpGet]

        public HttpResponseMessage CheckIfAnalaltyic(int C_aid)
        {
            return Request.CreateResponse(HttpStatusCode.OK, DB.C_CreateAccount_Tables.Where(x=>x.C_AID==C_aid)
                .FirstOrDefault().C_AnalyticAccountID);
        }
        //K Update
        [HttpGet]
        public HttpResponseMessage GetAccountDetails(int c_AID, string currencyID,bool IsAdjust=false,bool GetData=false)
        {
            try
            {
                var getData = DB.C_CreateAccount_Tables.Include(x=>x.Cost_center)
                    .Include(x=>x.Groupcostcenter_accounts).Include(x=>x.Groupcostcenter_accounts.Select(z=>z.Group_costcenter))
                    .Where(x => x.C_AID == c_AID).FirstOrDefault();

                var checkCurrency = DB.C_CurrencyCreateAccount_Tables.Where(x => x.C_AID == c_AID && x.CurrencyID == currencyID).FirstOrDefault();
                string CostCenterType = getData.Groupcostcenter_accounts.Where(x=>x.Account_id==c_AID).Any() ? "MainCostCenter" : getData.Cost_center.Where(x => x.Account_id == c_AID).Any() ? "CostCenter" : "";
                if (IsAdjust)
                {
                    if (getData.AllowAdjusment.HasValue&& getData.AllowAdjusment.Value==true)
                    {
                        TCGE_DTO tCGE_DTO = new TCGE_DTO()
                        {
                            AccountName = getData.AccountName,

                            //CurrencyID = getData.Currency,

                            AccountType = getData.AccountType,

                            SupportDocument = getData.SupportDocument,

                            C_AnalyticAccountID = getData.C_AnalyticAccountID,

                            CostCenterType = CostCenterType,

                            C_CostCenterID = getData.Cost_center.Where(x => x.Account_id == c_AID).ToList().DefaultIfEmpty(new FabulousDB.Models.Cost_center_accounts { Cost_center_id = "" }).FirstOrDefault().Cost_center_id,

                            C_CostCenterGroupID = getData.Groupcostcenter_accounts.Where(x => x.Account_id == c_AID).ToList().DefaultIfEmpty(new Groupcostcenter_accounts {Group_costcenter=new
                            FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter.C_GroupCostCenter_Table
                            {C_CostCenterGroupID=""} }).FirstOrDefault().Group_costcenter.C_CostCenterGroupID,

                            MaiximumAmount = getData.MaximumAmountPerTransaction,

                            MinimumAmount = getData.MinimumAmountPerTransaction
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTO);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "You Can't Choose This UnAdjust Account..!");

                    }
                }
                if (getData.ConsolidationAccount == true && !GetData)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "You Can't Choose This Conslidation Account..!");
                }
                else if (getData.ReconcileAccount == true && !GetData)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "You Can't Choose This Reconcile Account..!");
                }
                else if (checkCurrency == null && !GetData)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "This Account Currency Not Matching With Transaction Choosen Currency..!");
                }
                else
                {
                    TCGE_DTO tCGE_DTO = new TCGE_DTO()
                    {
                        AccountName = getData.AccountName,
                        //CurrencyID = getData.Currency,
                        AccountType = getData.AccountType,
                        SupportDocument = getData.SupportDocument,
                        C_AnalyticAccountID = getData.C_AnalyticAccountID,
                        CostCenterType = CostCenterType,
                        C_CostCenterID = getData.Cost_center.Where(x => x.Account_id == c_AID).ToList().DefaultIfEmpty(new FabulousDB.Models.Cost_center_accounts { Cost_center_id = "" }).FirstOrDefault().Cost_center_id,
                        C_CostCenterGroupID = getData.Groupcostcenter_accounts.Where(x => x.Account_id == c_AID).ToList().DefaultIfEmpty(new Groupcostcenter_accounts
                        {
                            Group_costcenter = new
                            FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter.C_GroupCostCenter_Table
                            { C_CostCenterGroupID = "" }
                        }).FirstOrDefault().Group_costcenter.C_CostCenterGroupID,
                        MaiximumAmount = getData.MaximumAmountPerTransaction,
                        MinimumAmount = getData.MinimumAmountPerTransaction
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTO);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage CheckAccountHasCostOrAnayltic([FromBody]int[] C_AID)
        {
            var getData = DB.C_CreateAccount_Tables.Include(x => x.Cost_center)
                   .Include(x => x.Groupcostcenter_accounts).Include(x => x.Groupcostcenter_accounts.Select(z => z.Group_costcenter))
                   .Where(x => C_AID.Contains(x.C_AID)).ToList();
            List<HasCostCenterHasAnayltic> Res = getData.Select(x => new HasCostCenterHasAnayltic
            {
                Aid=x.C_AID,
                HasAnayltic=!string.IsNullOrEmpty(x.C_AnalyticAccountID),
                HasCostCenter= x.Groupcostcenter_accounts.Where(z => z.Account_id == x.C_AID).Any() ? true : x.Cost_center.Where(z => z.Account_id == x.C_AID).Any() ? true : false,
                CostCetnerType= x.Groupcostcenter_accounts.Where(z => z.Account_id == x.C_AID).Any() ? "MainCostCenter" : x.Cost_center.Where(z => z.Account_id == x.C_AID).Any() ? "CostCenter" : "",
            }).ToList();
            
            foreach (HasCostCenterHasAnayltic i in Res)
            {
                if (i.CostCetnerType== "MainCostCenter")
                {
                    i.CostGroupId = getData
                       .SelectMany(x=>x.Groupcostcenter_accounts)
                       .Where(x => x.Account_id == i.Aid)
                        .DefaultIfEmpty(new Groupcostcenter_accounts
                    {
                        Group_costcenter = new
                            FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter.C_GroupCostCenter_Table
                        { C_CostCenterGroupID = "" }
                    }).FirstOrDefault().Group_costcenter.C_CostCenterGroupID;
                }
                else if (i.CostCetnerType == "CostCenter")
                {
                    i.CostCenterId = getData.SelectMany(x=>x.Cost_center)
                        .Where(x => x.Account_id == i.Aid)
                        .ToList()
                        .DefaultIfEmpty(new FabulousDB.Models.Cost_center_accounts { Cost_center_id = "" })
                        .FirstOrDefault().Cost_center_id;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, Res);
        }
        [HttpPost]
        public HttpResponseMessage CheckAccountCostAndAnayltic([FromBody]int[] C_AID)
        {
            var getData = DB.C_CreateAccount_Tables.Include(x => x.Cost_center)
                   .Include(x => x.Groupcostcenter_accounts).Include(x => x.Groupcostcenter_accounts.Select(z => z.Group_costcenter))
                   .Where(x => C_AID.Contains(x.C_AID)).ToList();
            List<HasCostCenterHasAnayltic> Res = getData.Select(x => new HasCostCenterHasAnayltic
            {
                Aid=x.C_AID,
                HasAnayltic=!string.IsNullOrEmpty(x.C_AnalyticAccountID),
                HasCostCenter= x.Groupcostcenter_accounts.Where(z => z.Account_id == x.C_AID).Any() ? true : x.Cost_center.Where(z => z.Account_id == x.C_AID).Any() ? true : false,
                CostCetnerType= x.Groupcostcenter_accounts.Where(z => z.Account_id == x.C_AID).Any() ? "MainCostCenter" : x.Cost_center.Where(z => z.Account_id == x.C_AID).Any() ? "CostCenter" : "",
            }).ToList();
            
            foreach (HasCostCenterHasAnayltic i in Res)
            {
                if (i.CostCetnerType== "MainCostCenter")
                {
                    i.CostCenterId = getData
                       .SelectMany(x=>x.Groupcostcenter_accounts)
                       .Where(x => C_AID.Contains(x.Account_id))
                        .DefaultIfEmpty(new Groupcostcenter_accounts
                    {
                        Group_costcenter = new
                            FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter.C_GroupCostCenter_Table
                        { C_CostCenterGroupID = "" }
                    }).FirstOrDefault().Group_costcenter.C_CostCenterGroupID;
                }
                else if (i.CostCetnerType == "CostCenter")
                {
                    i.CostCenterId = getData.SelectMany(x=>x.Cost_center)
                        .Where(x => C_AID.Contains(x.Account_id))
                        .ToList()
                        .DefaultIfEmpty(new FabulousDB.Models.Cost_center_accounts { Cost_center_id = "" })
                        .FirstOrDefault().Cost_center_id;
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, Res);
        }

        [HttpGet]
        public HttpResponseMessage CheckExistDistribution(int c_AID)
        {
            try
            {
                List<TCGE_DTO> tCGE_DTOs = DB.C_CreatAccountDist_Tables.Where(x => x.C_AID == c_AID).Select(x => new TCGE_DTO
                {
                    C_DistID = x.C_DistID,

                    C_AccountDistributionID = x.C_AnalyticDistribution_Table.C_AccountDistributionID,

                    C_AccountDistributionName = x.C_AnalyticDistribution_Table.C_AccountDistributionName,

                    Percentage = x.Percentage

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTOs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetDistributionOfAccountAnalytic(string analyticID,int AccId)
        {
            try
            {
                string AnaylticId= DB.C_CreateAccount_Tables.Find(AccId).C_AnalyticAccountID;
                if (string.IsNullOrEmpty(analyticID))
                {
                    analyticID = AnaylticId;
                }
                List<TCGE_DTO> tCGE_DTOs = DB.C_AnalyticDistribution_Tables.Where(x => x.C_AnalyticAccountID == analyticID).Select(x => new TCGE_DTO
                {
                    C_DistID2 = x.C_DistID,

                    C_AccountDistributionID2 = x.C_AccountDistributionID

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTOs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetDistributionName(int distributionID)
        {
            try
            {
                var getName = DB.C_AnalyticDistribution_Tables.Where(x => x.C_DistID == distributionID).FirstOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, getName.C_AccountDistributionName);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAccountsOfCostCenter(string costCenterID)
        {
            try
            {
                List<TCGE_DTO> tCGE_DTO = DB.C_CostCenterAccounts_Tables
                    .Where(x => x.C_CostCenterID == costCenterID).Select(x => new TCGE_DTO
                {
                    C_CAID = x.C_CAID,

                    C_CostAccountID = x.C_CostAccountID

                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTO);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCostAccountName(int costAccountID)
        {
            try
            {
                var getName = DB.C_CostCenterAccounts_Tables.Where(x => x.C_CAID == costAccountID).FirstOrDefault();
                return Request.CreateResponse(HttpStatusCode.OK, getName.C_CostAccountName);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckExistCostAccounts(int c_AID, string costCenterType)
        {
            try
            {
                if (costCenterType == "CostCenter")
                {
                    List<TCGE_DTO> tCGE_DTOs = DB.C_CreateAccountCCAccount_Tables.Where(x => x.C_AID == c_AID && x.CostCenterType == costCenterType).Select(x => new TCGE_DTO
                    {

                        C_CAID2 = x.C_CAID,

                        C_CostAccountID2 = x.C_CostCenterAccounts_Table.C_CostAccountID,

                        C_CostAccountName = x.C_CostCenterAccounts_Table.C_CostAccountName,

                        Percentage = x.Percentage

                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTOs);
                }
                else if (costCenterType == "MainCostCenter")
                {

                    List<TCGE_DTO> tCGE_DTOs = DB.C_CreateAccountCCAccount_Tables.Where(x => x.C_AID == c_AID && x.CostCenterType == costCenterType).Select(x => new TCGE_DTO
                    {
                        C_CostCenterID = x.C_GroupCostCenter_Table.C_CostCenter_Table.C_CostCenterID,

                        C_CostCenterName = x.C_GroupCostCenter_Table.C_CostCenter_Table.C_CostCenterName,

                        CostCenterIDPercentage = x.C_GroupCostCenter_Table.C_Percentage,

                        C_CAID2 = x.C_CAID,

                        C_CostAccountID2 = x.C_CostCenterAccounts_Table.C_CostAccountID,

                        C_CostAccountName = x.C_CostCenterAccounts_Table.C_CostAccountName,

                        Percentage = x.Percentage

                    }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTOs);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCostCenterIDFromMain(string mainCostCenter)
        {
            try
            {
                List<TCGE_DTO> tCGE_DTOs = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterGroupID == mainCostCenter).Select(x => new TCGE_DTO
                {
                    C_CostCenterID = x.C_CostCenterID

                }).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTOs);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetCostCenterIDDetails(string costCenterID, string mainCostCenter)
        {
            try
            {
                var getData = DB.C_GroupCostCenter_Tables.Where(x => x.C_CostCenterID == costCenterID && x.C_CostCenterGroupID == mainCostCenter).FirstOrDefault();

                TCGE_DTO tCGE_DTO = new TCGE_DTO()
                {
                    CostCenterName = getData.C_CostCenter_Table.C_CostCenterName,

                    Percentage = getData.C_Percentage
                };

                return Request.CreateResponse(HttpStatusCode.OK, tCGE_DTO);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [AcceptVerbs("GET", "POST")]
        public HttpResponseMessage InsertTransactionData([FromUri]string companyID, [FromUri]int inputType, [FromUri]int? voidPostingNum, [FromBody]TransactionApiData data,bool IsApiCall=true)
        {
            try
            {
                int journalEntryNumber = 1;

                double totalDebit = 0;
                double totalCredit = 0;
                int numOfTransactions = data.SaveTransaction.Length;

                foreach (var tItemForMain in data.SaveTransaction)
                {
                    totalDebit += tItemForMain.C_Debit.GetValueOrDefault();
                    totalCredit += tItemForMain.C_Credit.GetValueOrDefault();
                }

                var generateJENum = DB.C_GeneralJournalEntry_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

                if (generateJENum != null)
                {
                    journalEntryNumber = DB.C_GeneralJournalEntry_Tables.Where(x => x.CompanyID == companyID).Max(x => x.C_JournalEntryNumber) + 1;
                }

                bool post = false;
                int posting = 0;
                if (inputType == 2) //Post
                {
                    post = true;
                    var postingCheck = DB.C_GeneralJournalEntry_Tables
                        .Where(x => x.CompanyID == companyID && x.Post == true)
                        .OrderByDescending(x => x.C_Posting)
                        .FirstOrDefault();
                    if (postingCheck != null)
                    {
                        posting = postingCheck.C_Posting + 1;
                    }
                    else
                    {
                        posting = 1;
                    }
                }

                int? generatedBatchID = data.SaveHeader.C_CBID;

                if (inputType == 1 /*&& data.SaveHeader.C_PostingKey != "TCGE"*/)
                {
                    generatedBatchID = GenerateBatchID(companyID, data.SaveHeader.C_PostingKey);
                }


                C_GeneralJournalEntry_Table c_GeneralJournalEntry_Table = new C_GeneralJournalEntry_Table()
                {
                    C_JournalEntryNumber = journalEntryNumber,

                    C_PostingKey = data.SaveHeader.C_PostingKey,

                    C_TransactionType = data.SaveHeader.C_TransactionType,

                    C_PostingDate = data.SaveHeader.C_PostingDate,

                    C_TransactionDate = data.SaveHeader.C_TransactionDate,

                    C_Refrence = data.SaveHeader.C_Refrence,

                    C_TotalDebit = totalDebit,

                    C_TotalCredit = totalCredit,

                    CompanyID = companyID,

                    C_NoOfAcc = numOfTransactions,

                    C = DateTime.Now,

                    C_RoutinJournal = false,

                    C_CBID = generatedBatchID,

                    CurrencyID = data.SaveHeader.CurrencyID,

                    C_SystemRate = data.SaveHeader.C_SystemRate,

                    C_TransactionRate = data.SaveHeader.C_TransactionRate,

                    Post = post,

                    C_Posting = posting,

                    UserID = FabulousErp.Business.GetUserId(),

                    VoidPostingNum = voidPostingNum
                };
                DB.C_GeneralJournalEntry_Tables.Add(c_GeneralJournalEntry_Table);
                if (inputType == 1) //Save
                {
                    foreach (var transactionItems in data.SaveTransaction)
                    {
                        C_SaveTransaction_Table c_SaveTransaction_Table = new C_SaveTransaction_Table()
                        {
                            C_PostingNumber = c_GeneralJournalEntry_Table.C_PostingNumber,

                            C_AID = transactionItems.C_AID,

                            C_Describtion = transactionItems.C_Describtion,

                            C_Document = transactionItems.C_Document,

                            C_OriginalAmount = transactionItems.C_OriginalAmount,

                            C_Debit = transactionItems.C_Debit,

                            C_Credit = transactionItems.C_Credit,

                            Ballance = transactionItems.C_Debit.GetValueOrDefault() - transactionItems.C_Credit.GetValueOrDefault()
                        };
                        DB.C_SaveTransaction_Tables.Add(c_SaveTransaction_Table);
                    }
                }
                else if (inputType == 2) //Post
                {
                    foreach (var transactionItems in data.SaveTransaction)
                    {
                        C_GeneralLedger_Table c_GeneralLedger = new C_GeneralLedger_Table()
                        {
                            C_PostingNumber = c_GeneralJournalEntry_Table.C_PostingNumber,

                            C_AID = transactionItems.C_AID,

                            C_Describtion =string.IsNullOrWhiteSpace(transactionItems.C_Describtion)? "N/A" : transactionItems.C_Describtion,

                            C_Document = transactionItems.C_Document,

                            C_OriginalAmount = transactionItems.C_OriginalAmount,

                            C_Debit = transactionItems.C_Debit,

                            C_Credit = transactionItems.C_Credit,

                            Ballance = transactionItems.C_Debit.GetValueOrDefault() - transactionItems.C_Credit.GetValueOrDefault()
                        };
                        DB.C_GeneralLedger_Tables.Add(c_GeneralLedger);
                    }
                }

                
                if (data.SaveAnalytic != null)
                {
                    foreach (var analyticItems in data.SaveAnalytic)
                    {
                        C_SaveAnalytic_Table c_SaveAnalytic_Table = new C_SaveAnalytic_Table()
                        {
                            C_PostingNumber = c_GeneralJournalEntry_Table.C_PostingNumber,

                            C_AID = analyticItems.C_AID,

                            Describtion = string.IsNullOrWhiteSpace(analyticItems.Describtion) ? "N/A" : analyticItems.Describtion,

                            C_AnalyticAccountID = analyticItems.C_AnalyticAccountID,

                            C_DistID = analyticItems.C_DistID,

                            C_Amount = analyticItems.C_Amount,

                            C_Debit = analyticItems.C_Debit,

                            C_Credit = analyticItems.C_Credit,

                            Ballance = analyticItems.C_Debit.GetValueOrDefault() - analyticItems.C_Credit.GetValueOrDefault(),

                            Post = post,

                            C_Percentage = analyticItems.C_Percentage
                        };
                        DB.C_SaveAnalytic_Tables.Add(c_SaveAnalytic_Table);
                    }
                }

                if (data.SaveCost != null)
                {
                    foreach (var costItems in data.SaveCost)
                    {
                        string mainCostCenter = costItems.C_CostCenterGroupID;
                        if (string.IsNullOrEmpty(costItems.C_CostCenterGroupID))
                        {
                            mainCostCenter = null;
                        }

                        C_SaveCostCenter_Table c_SaveCostCenter_Table = new C_SaveCostCenter_Table()
                        {
                            C_PostingNumber = c_GeneralJournalEntry_Table.C_PostingNumber,

                            C_AID = costItems.C_AID,

                            Describtion = string.IsNullOrWhiteSpace(costItems.Describtion) ? "N/A" : costItems.Describtion,

                            C_CostCenterID = costItems.C_CostCenterID,

                            C_CAID = costItems.C_CAID,

                            C_Amount = costItems.C_Amount,

                            C_Debit = costItems.C_Debit,

                            C_Credit = costItems.C_Credit,

                            Ballance = costItems.C_Debit.GetValueOrDefault() - costItems.C_Credit.GetValueOrDefault(),

                            Post = post,

                            C_Percentage = costItems.C_Percentage,

                            C_CostCenterGroupID = mainCostCenter,

                            CostCenterPercentage = costItems.CostCenterPercentage
                        };
                        DB.C_SaveCostCenter_Tables.Add(c_SaveCostCenter_Table);
                    }
                }

                if (voidPostingNum.HasValue)
                {
                    var updateVoidedPostingNum = DB.C_GeneralJournalEntry_Tables.Where(x => x.C_PostingNumber == voidPostingNum).FirstOrDefault();
                    updateVoidedPostingNum.VoidPostingNum = c_GeneralJournalEntry_Table.C_PostingNumber;
                }

                DB.SaveChanges();

                TCGE_DTO ids = new TCGE_DTO()
                {
                    JournalEntryNumber = c_GeneralJournalEntry_Table.C_JournalEntryNumber,

                    PostingNumber = c_GeneralJournalEntry_Table.C_PostingNumber
                };
                if (IsApiCall)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, ids);
                }
                else
                {
                    return new HttpResponseMessage
                    {
                       ReasonPhrase= ids.PostingNumber.ToString()
                    };
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [AcceptVerbs("GET", "POST")]
        public HttpResponseMessage SaveSimpleTransaction([FromUri]string companyID, [FromBody]TransactionApiData data)
        {
            try
            {
                int journalEntryNumber = 1;

                double totalDebit = 0;
                double totalCredit = 0;
                int numOfTransactions = data.SaveTransaction.Length;

                if (data.SaveTransaction != null)
                {
                    foreach (var tItemForMain in data.SaveTransaction)
                    {
                        totalDebit += tItemForMain.C_Debit.GetValueOrDefault();
                        totalCredit += tItemForMain.C_Credit.GetValueOrDefault();
                    }
                }

                var generateJENum = DB.C_GeneralJournalEntry_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();

                if (generateJENum != null)
                {
                    journalEntryNumber = DB.C_GeneralJournalEntry_Tables.Where(x => x.CompanyID == companyID).Max(x => x.C_JournalEntryNumber) + 1;
                }

                C_GeneralJournalEntry_Table c_GeneralJournalEntry_Table = new C_GeneralJournalEntry_Table()
                {
                    C_JournalEntryNumber = journalEntryNumber,

                    C_PostingKey = data.SaveHeader.C_PostingKey,

                    C_TransactionType = data.SaveHeader.C_TransactionType,

                    C_PostingDate = data.SaveHeader.C_PostingDate,

                    C_TransactionDate = data.SaveHeader.C_TransactionDate,

                    C_Refrence = data.SaveHeader.C_Refrence,

                    C_TotalDebit = totalDebit,

                    C_TotalCredit = totalCredit,

                    CompanyID = companyID,

                    C_NoOfAcc = numOfTransactions,

                    C = DateTime.Now,

                    C_RoutinJournal = false,

                    CurrencyID = data.SaveHeader.CurrencyID,

                    C_SystemRate = data.SaveHeader.C_SystemRate,

                    C_TransactionRate = data.SaveHeader.C_TransactionRate,

                    Post = true,

                    UserID = FabulousErp.Business.GetUserId()
                };
                DB.C_GeneralJournalEntry_Tables.Add(c_GeneralJournalEntry_Table);

                if (data.SaveTransaction != null)
                {
                    foreach (var transactionItems in data.SaveTransaction)
                    {
                        C_GeneralLedger_Table c_GeneralLedger = new C_GeneralLedger_Table()
                        {
                            C_PostingNumber = c_GeneralJournalEntry_Table.C_PostingNumber,

                            C_AID = transactionItems.C_AID,

                            C_Describtion = transactionItems.C_Describtion,

                            C_Document = transactionItems.C_Document,

                            C_OriginalAmount = transactionItems.C_OriginalAmount,

                            C_Debit = transactionItems.C_Debit,

                            C_Credit = transactionItems.C_Credit,

                            Ballance = transactionItems.C_Debit.GetValueOrDefault() - transactionItems.C_Credit.GetValueOrDefault()
                        };
                        DB.C_GeneralLedger_Tables.Add(c_GeneralLedger);
                    }
                }

                DB.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, c_GeneralJournalEntry_Table.C_PostingNumber);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage SaveNewTransactionData([FromUri]string companyID, [FromUri]int postingNumber, [FromBody]TransactionApiData data)
        {
            double totalDebit = 0;
            double totalCredit = 0;
            int numOfTransactions = data.SaveTransaction.Length;

            foreach (var tItemForMain in data.SaveTransaction)
            {
                totalDebit += tItemForMain.C_Debit.GetValueOrDefault();
                totalCredit += tItemForMain.C_Credit.GetValueOrDefault();
            }

            var headerData = DB.C_GeneralJournalEntry_Tables.Where(x => x.C_PostingNumber == postingNumber).FirstOrDefault();
            headerData.C_TotalDebit = totalDebit;
            headerData.C_TotalCredit = totalCredit;
            headerData.C_NoOfAcc = numOfTransactions;

            IQueryable<C_SaveTransaction_Table> tansRange = DB.C_SaveTransaction_Tables.Where(x => x.C_PostingNumber == postingNumber && x.C_GeneralJournalEntry_Table.CompanyID == companyID);
            DB.C_SaveTransaction_Tables.RemoveRange(tansRange);

            IQueryable<C_SaveAnalytic_Table> analyticRange = DB.C_SaveAnalytic_Tables.Where(x => x.C_PostingNumber == postingNumber && x.C_GeneralJournalEntry_Table.CompanyID == companyID);
            DB.C_SaveAnalytic_Tables.RemoveRange(analyticRange);

            IQueryable<C_SaveCostCenter_Table> costCenterRange = DB.C_SaveCostCenter_Tables.Where(x => x.C_PostingNumber == postingNumber && x.C_GeneralJournalEntry_Table.CompanyID == companyID);
            DB.C_SaveCostCenter_Tables.RemoveRange(costCenterRange);

            foreach (var transactionItems in data.SaveTransaction)
            {
                C_SaveTransaction_Table c_SaveTransaction_Table = new C_SaveTransaction_Table()
                {
                    C_PostingNumber = postingNumber,

                    C_AID = transactionItems.C_AID,

                    C_Describtion = transactionItems.C_Describtion,

                    C_Document = transactionItems.C_Document,

                    C_OriginalAmount = transactionItems.C_OriginalAmount,

                    C_Debit = transactionItems.C_Debit,

                    C_Credit = transactionItems.C_Credit,

                    Ballance = transactionItems.C_Debit.GetValueOrDefault() - transactionItems.C_Credit.GetValueOrDefault()
                };
                DB.C_SaveTransaction_Tables.Add(c_SaveTransaction_Table);
            }

            if (data.SaveAnalytic != null)
            {
                foreach (var analyticItems in data.SaveAnalytic)
                {
                    C_SaveAnalytic_Table c_SaveAnalytic_Table = new C_SaveAnalytic_Table()
                    {
                        C_PostingNumber = postingNumber,

                        C_AID = analyticItems.C_AID,

                        Describtion = analyticItems.Describtion,

                        C_AnalyticAccountID = analyticItems.C_AnalyticAccountID,

                        C_DistID = analyticItems.C_DistID,

                        C_Amount = analyticItems.C_Amount,

                        C_Debit = analyticItems.C_Debit,

                        C_Credit = analyticItems.C_Credit,

                        Ballance = analyticItems.C_Debit.GetValueOrDefault() - analyticItems.C_Credit.GetValueOrDefault(),

                        Post = false,

                        C_Percentage = analyticItems.C_Percentage
                    };
                    DB.C_SaveAnalytic_Tables.Add(c_SaveAnalytic_Table);
                }
            }

            if (data.SaveCost != null)
            {
                foreach (var costItems in data.SaveCost)
                {
                    string mainCostCenter = costItems.C_CostCenterGroupID;
                    if (costItems.C_CostCenterGroupID.Length == 0)
                    {
                        mainCostCenter = null;
                    }

                    C_SaveCostCenter_Table c_SaveCostCenter_Table = new C_SaveCostCenter_Table()
                    {
                        C_PostingNumber = postingNumber,

                        C_AID = costItems.C_AID,

                        Describtion = costItems.Describtion,

                        C_CostCenterID = costItems.C_CostCenterID,

                        C_CAID = costItems.C_CAID,

                        C_Amount = costItems.C_Amount,

                        C_Debit = costItems.C_Debit,

                        C_Credit = costItems.C_Credit,

                        Ballance = costItems.C_Debit.GetValueOrDefault() - costItems.C_Credit.GetValueOrDefault(),

                        Post = false,

                        C_Percentage = costItems.C_Percentage,

                        C_CostCenterGroupID = mainCostCenter,

                        CostCenterPercentage = costItems.CostCenterPercentage
                    };
                    DB.C_SaveCostCenter_Tables.Add(c_SaveCostCenter_Table);
                }
            }

            DB.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, "Data Updated Successfully");
        }

        private int GenerateBatchID(string companyID, string postingKey)
        {
            int batchID = 1;

            var check = DB.C_CreateBatch_Tables.Where(x => x.CompanyID == companyID && x.C_BatchLocation == postingKey).FirstOrDefault();

            if (check != null)
            {
                batchID = DB.C_CreateBatch_Tables.ToList().Where(x => x.CompanyID == companyID && x.C_BatchLocation == postingKey).Max(x => Int32.Parse(x.C_BatchID)) + 1;
            }


            var postingSetup = Business.GetPostingSetup();//  Business.GetPostingSetup();

            bool notApproval = false;
            bool approval = false;
            if (postingSetup.Batch == "C2")
            {
                notApproval = true;
                approval = false;
            }
            else
            {
                approval = true;
                notApproval = false;
            }

            C_CreateBatch_Table c_CreateBatch_Table = new C_CreateBatch_Table()
            {
                C_BatchID = batchID.ToString(),

                CompanyID = companyID,

                C_BatchDescription = postingKey + " " + batchID,

                C_BatchLocation = postingKey,

                C_BatchCreationDate = DateTime.Now.ToShortDateString(),

                C_Module = "Financial",

                NotApproval = notApproval,

                Approval = approval,

                Removed = false,

                UserID = FabulousErp.Business.GetUserId()
            };

            DB.C_CreateBatch_Tables.Add(c_CreateBatch_Table);
            DB.SaveChanges();

            return c_CreateBatch_Table.C_CBID;
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionData(int postingNumber)
        {
            RetrieveTransactionData getData = new RetrieveTransactionData
            {
                ShowHeader = DB.C_GeneralJournalEntry_Tables.Where(x => x.C_PostingNumber == postingNumber).Select(x => new ShowHeader
                {
                    PostingDate = x.C_PostingDate,
                    TransactionDate = x.C_TransactionDate,
                    CurrencyID = x.CurrencyID,
                    SystemRate = x.C_SystemRate,
                    TransactionRate = x.C_TransactionRate,
                    Reference = x.C_Refrence,
                    PostingKey = x.C_PostingKey,
                    ISO = x.CurrenciesDefinition_Table.ISOCode,
                    VoidJENum = x.GeneralJournalEntry_Table.C_JournalEntryNumber,
                    VoidDate = x.GeneralJournalEntry_Table.C_PostingDate,
                    VoidPostingKey = x.GeneralJournalEntry_Table.C_PostingKey
                }).FirstOrDefault(),

                ShowTransactions = DB.C_SaveTransaction_Tables.Where(x => x.C_PostingNumber == postingNumber).Select(x => new ShowTransaction
                {
                    AID = x.C_AID,
                    AccountID = x.C_CreateAccount_Table.AccountID,
                    OriginalAmount = x.C_OriginalAmount,
                    Credit = x.C_Credit,
                    Debit = x.C_Debit,
                    Describtion = x.C_Describtion,
                    Document = x.C_Document,
                    AccountName = x.C_CreateAccount_Table.AccountName
                }).ToList(),

                ShowGeneralLedger = DB.C_GeneralLedger_Tables.Where(x => x.C_PostingNumber == postingNumber).Select(x => new ShowTransaction
                {
                    AID = x.C_AID,
                    AccountID = x.C_CreateAccount_Table.AccountID,
                    OriginalAmount = x.C_OriginalAmount,
                    Credit = x.C_Credit,
                    Debit = x.C_Debit,
                    Describtion = x.C_Describtion,
                    Document = x.C_Document,
                    AccountName = x.C_CreateAccount_Table.AccountName
                }).ToList(),

                ShowAnalytics = DB.C_SaveAnalytic_Tables.Where(x => x.C_PostingNumber == postingNumber).Select(x => new ShowAnalytic
                {
                    AID = x.C_AID,
                    Amount = x.C_Amount,
                    AnalyticID = x.C_AnalyticAccountID,
                    Credit = x.C_Credit,
                    Debit = x.C_Debit,
                    Describtion = x.Describtion,
                    DistID = x.C_DistID,
                    DistributionID = x.C_AnalyticDistribution_Table.C_AccountDistributionID,
                    DistributionName = x.C_AnalyticDistribution_Table.C_AccountDistributionName,
                    Percentage = x.C_Percentage
                }).ToList(),

                ShowCostCenters = DB.C_SaveCostCenter_Tables.Where(x => x.C_PostingNumber == postingNumber).Select(x => new ShowCostCenter
                {
                    AID = x.C_AID,
                    Percentage = x.C_Percentage,
                    Amount = x.C_Amount,
                    Credit = x.C_Credit,
                    Debit = x.C_Debit,
                    Describtion = x.Describtion,
                    CostCenterID = x.C_CostCenterID,
                    CostCenterName = x.C_CostCenter_Table.C_CostCenterName,
                    CAID = x.C_CAID,
                    CostAccountID = x.C_CostCenterAccounts_Table.C_CostAccountID,
                    CostAccountName = x.C_CostCenterAccounts_Table.C_CostAccountName,
                    MainCostCenterID = x.C_CostCenterGroupID,
                    CostCenterIDPercentage = x.CostCenterPercentage
                }).ToList(),

                CheckbookData = DB.C_CheckbookTransactions_Tables.Where(x => x.C_PostingNumber == postingNumber).Select(x => new CheckbookData
                {
                    C_CBT = x.C_CBT,
                    C_CBSID = x.C_CBSID,
                    CheckbookName = x.C_CheckBookSetting_Table.C_CheckbookName,
                    Payment_To_Recieved_From = x.C_Payment_To_Recieved_From,
                    NextDepositNumber = x.C_CheckBookSetting_Table.C_NextDepositNumber,
                    NextWithdrawNumber = x.C_CheckBookSetting_Table.C_NextWithdrawNumber,
                    C_DocumentType = x.C_DocumentType,
                    C_Balance = x.C_Balance,
                    C_CheckNumber = x.C_CheckNumber,
                    C_DueDate = x.C_DueDate,
                    C_TransactionDate = x.C_TransactionDate,
                    C_PostingDate = x.C_PostingDate
                }).FirstOrDefault(),

                TransferData = DB.C_CheckbookTransactions_Tables.Where(x => x.C_PostingNumber == postingNumber).Select(x => new TransferData
                {
                    C_CBT = x.C_CBT,
                    C_CBSID = x.C_CBSID,
                    CheckbookName = x.C_CheckBookSetting_Table.C_CheckbookName,
                    CurrencyID = x.CurrencyID,
                    ISO = x.CurrenciesDefinition_Table.ISOCode,
                    C_DocumentType = x.C_DocumentType,
                    C_Reference = x.C_Reference,
                    C_SystemRate = x.C_SystemRate,
                    C_TransactionRate = x.C_TransactionRate,
                    C_Difference = x.C_Difference,
                    C_Balance = x.C_Balance,
                    RecieptCheck = x.C_Reciept,
                    PaymentCheck = x.C_Payment,
                }).ToList()
            };

            return Request.CreateResponse(HttpStatusCode.OK, getData);
        }


    }
}
