using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Important;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CreateAccount;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Company.C_Account
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_CreateAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public C_CreateAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();


        // GET: C_CreateAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SCCA")]
        public ActionResult CompanyAccount()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            ViewBag.CompanyList = companyID;

            return View();
        }


        public JsonResult GetCompanyName()
        {
            string CompanyID = (string)FabulousErp.Business.GetCompanyId();

            var getCompName = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            if (getCompName != null)
            {

                var getChartData = DB.CompanyChartAccount_Table.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

                if (getChartData != null)
                {

                    CBF_Accounts_DTO cBF_Accounts_DTO = new CBF_Accounts_DTO()
                    {
                        CBFName = getCompName.CompanyName,

                        ChartOfAccountID = getChartData.AccountChartID,

                        ChartOfAccountName = getChartData.AccountChart_Table.AccountChartName
                    };

                    return Json(cBF_Accounts_DTO, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NoChart", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return null;
            }
        }


        public JsonResult GetViewOfChart(string ChartOfAccountID)
        {

            var check = DB.SegmentAccountChart_Table.Where(x => x.AccountChartID == ChartOfAccountID).FirstOrDefault();

            if (check != null)
            {

                List<Chart_View_DTO> chart_View_DTO = DB.SegmentAccountChart_Table.Where(x => x.AccountChartID == ChartOfAccountID).Select(x => new Chart_View_DTO
                {

                    NumberOfSegment = x.AccountChart_Table.NumberOfSegment,

                    Seperator = x.AccountChart_Table.Separate,

                    Length = x.Length

                }).ToList();

                return Json(chart_View_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var getData = DB.AccountChart_Table.Where(x => x.AccountChartID == ChartOfAccountID).FirstOrDefault();

                Chart_View_DTO chart_View = new Chart_View_DTO()
                {
                    Length = getData.AccountLength,

                    Status = "False"
                };

                return Json(chart_View, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetFromTo(string AccountsGroup)
        {
            var getFromTo = DB.ChartGroupContent_Tables.Where(x => x.AccountName == AccountsGroup).FirstOrDefault();

            if (getFromTo != null)
            {

                Get_Small_Data_DTO get_Small_Data_DTO = new Get_Small_Data_DTO()
                {
                    From = getFromTo.AccountFrom,

                    To = getFromTo.AccountTo,

                    FromWithSep = getFromTo.AccountFromWithSep,

                    ToWithSep = getFromTo.AccountToWithSep
                };

                return Json(get_Small_Data_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult GetAccountsGroup(string ChartOfAccountID)
        {
            var getGroupID = DB.AccountGroupOfChart_Tables.Where(x => x.AccountChartID == ChartOfAccountID).FirstOrDefault();

            if (getGroupID != null)
            {

                string groupID = getGroupID.AccountGroupChartID;

                var getGroupContent = DB.ChartGroupContent_Tables.Where(x => x.AccountGroupChartID == groupID).ToList();

                List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.ChartGroupContent_Tables.Where(x => x.AccountGroupChartID == groupID).Select(x => new RetrieveDataToAccount_DTO
                {
                    AccountGroupName = x.AccountName

                }).ToList();

                return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult GetAnalyticAccount(string CompanyID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.C_AnalyticAccount_Tables.Where(x => x.CompanyID == CompanyID).Select(x => new RetrieveDataToAccount_DTO
            {

                AAccountID = x.C_AnalyticAccountID

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAnalyticAccountName(string AnalyticAccountID)
        {
            var getName = DB.C_AnalyticAccount_Tables.Where(x => x.C_AnalyticAccountID == AnalyticAccountID).FirstOrDefault();

            return Json(getName.C_AnalyticAccountName, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCostCenter(string CompanyID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.C_CostCenter_Tables.Where(x => x.CompanyID == CompanyID).Select(x => new RetrieveDataToAccount_DTO
            {

                CostCenterID = x.C_CostCenterID

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCostCenterName(string CostCenterID)
        {
            var getName = DB.C_CostCenter_Tables.Where(x => x.C_CostCenterID == CostCenterID).FirstOrDefault();

            return Json(getName.C_CostCenterName, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetMainCostCenter(string CompanyID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.C_MainCostCenter_Tables.Where(x => x.CompanyID == CompanyID).Select(x => new RetrieveDataToAccount_DTO
            {

                MainCostCenterID = x.C_CostCenterGroupID,

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMainCostCenterName(string MainCostCenterID)
        {
            var getName = DB.C_MainCostCenter_Tables.Where(x => x.C_CostCenterGroupID == MainCostCenterID).FirstOrDefault();

            return Json(getName.C_CostCenterGroupName, JsonRequestBehavior.AllowGet);
        }




        public JsonResult SaveCompAccountInfo(string AccountID, string AccountName, string AccountsGroup, string CompanyID, string ChartAccountID, bool DisActive, string AccountType, string PostingType, bool ReconcileAccount, bool AllowAdjusment, bool Reevaluate, bool ConslidationAccount, double? MaxAmountPerTransaction, double? MinAmountPerTransaction,
                                              string AnalyticAccountID, string CostCenterType, string CostCenterID, string MainCostCenterID, double AccountIDWithoutSep, bool? FinancialArea, bool? SalesArea, bool? PurshacingArea, bool? InventoryArea, bool SupportDocument)
        {

            var getGroupID = DB.AccountGroupOfChart_Tables.Where(x => x.AccountChartID == ChartAccountID).FirstOrDefault();

            var checkRange = DB.ChartGroupContent_Tables.Where(x => x.AccountName == AccountsGroup && x.AccountGroupChartID == getGroupID.AccountGroupChartID && AccountIDWithoutSep >= x.AccountFrom && AccountIDWithoutSep <= x.AccountTo).FirstOrDefault();

            var checkDuplicate = DB.C_CreateAccount_Tables.Where(x => x.AccountID == AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

            if (checkRange == null)
            {
                return Json("Out", JsonRequestBehavior.AllowGet);
            }
            else if (checkDuplicate != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                C_CreateAccount_Table c_CreateAccount_Table = new C_CreateAccount_Table()
                {
                    AccountID = AccountID,

                    AccountName = AccountName,

                    AccountsGroup = AccountsGroup,

                    CompanyID = CompanyID,

                    AccountChartID = ChartAccountID,

                    //Currency = Currency,

                    DisActive = DisActive,

                    AccountType = AccountType,

                    PostingType = PostingType,

                    ReconcileAccount = ReconcileAccount,

                    AllowAdjusment = AllowAdjusment,

                    Reevaluate = Reevaluate,

                    ConsolidationAccount = ConslidationAccount,

                    MaximumAmountPerTransaction = MaxAmountPerTransaction,

                    MinimumAmountPerTransaction = MinAmountPerTransaction,

                    EstablishDate = DateTime.Now.ToShortDateString(),

                    C_AnalyticAccountID = "",//AnalyticAccountID,

                    CostCenterType = CostCenterType,

                    C_CostCenterID = "",//CostCenterID,

                    C_CostCenterGroupID = "",//MainCostCenterID,

                    FinancialArea = FinancialArea,

                    InventoryArea = InventoryArea,

                    PurshacingArea = PurshacingArea,

                    SalesArea = SalesArea,

                    SupportDocument = SupportDocument
                };

                DB.C_CreateAccount_Tables.Add(c_CreateAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetCompAccountData(int AccountID, string ChartAccountID)
        {
            var accountData = DB.C_CreateAccount_Tables.Where(x => x.C_AID == AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

            if (accountData != null)
            {
                RetrieveDataToAccount_DTO retrieveDataToAccount_DTO = new RetrieveDataToAccount_DTO()
                {
                    AccountName = accountData.AccountName,
                    AccountsGroup = accountData.AccountsGroup,
                    DisActive = accountData.DisActive,
                    //Currency = accountData.Currency,
                    AccountType = accountData.AccountType,
                    PostingType = accountData.PostingType,
                    ReconcileAccount = accountData.ReconcileAccount,
                    AllowAdjusment = accountData.AllowAdjusment,
                    Reevaluate = accountData.Reevaluate,
                    ConsolidationAccount = accountData.ConsolidationAccount,
                    MaximumAmountPerTransaction = accountData.MaximumAmountPerTransaction,
                    MinimumAmountPerTransaction = accountData.MinimumAmountPerTransaction,
                    CostCenterType = accountData.CostCenterType,
                    R_AnalyticAccountID = accountData.C_AnalyticAccountID,
                    R_CostCenterID = accountData.C_CostCenterID,
                    R_CostCenterGroupID = accountData.C_CostCenterGroupID,
                    PurshacingArea = accountData.PurshacingArea,
                    InventoryArea = accountData.InventoryArea,
                    FinancialArea = accountData.FinancialArea,
                    SalesArea = accountData.SalesArea,
                    SupportDocument = accountData.SupportDocument,
                    C_Prefix = accountData.C_Prefix
                };

                return Json(retrieveDataToAccount_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult UpdateCompAccountInfo(int AccountID, bool DisActive, string AccountType, string PostingType, bool ReconcileAccount, bool AllowAdjusment, bool Reevaluate, bool ConslidationAccount, double? MaxAmountPerTransaction, double? MinAmountPerTransaction,
                                              string AnalyticAccountID, string CostCenterType, string CostCenterID, string MainCostCenterID, string ChartAccountID, bool? FinancialArea, bool? SalesArea, bool? PurshacingArea, bool? InventoryArea, bool? SupportDocument)
        {
            var getAccount = DB.C_CreateAccount_Tables.Where(x => x.C_AID == AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

            if (getAccount != null)
            {

                var getID = getAccount.C_AID;
                var updateAccount = DB.C_CreateAccount_Tables.Where(x => x.C_AID == getID).FirstOrDefault();

                //updateAccount.Currency = Currency;
                updateAccount.DisActive = DisActive;
                updateAccount.AccountType = AccountType;
                updateAccount.PostingType = PostingType;
                updateAccount.ReconcileAccount = ReconcileAccount;
                updateAccount.AllowAdjusment = AllowAdjusment;
                updateAccount.Reevaluate = Reevaluate;
                updateAccount.ConsolidationAccount = ConslidationAccount;
                updateAccount.MaximumAmountPerTransaction = MaxAmountPerTransaction;
                updateAccount.MinimumAmountPerTransaction = MinAmountPerTransaction;
                updateAccount.C_AnalyticAccountID = ""; //AnalyticAccountID;
                updateAccount.CostCenterType = CostCenterType;
                updateAccount.C_CostCenterID = "";//CostCenterID;
                updateAccount.C_CostCenterGroupID = "";//MainCostCenterID;
                updateAccount.FinancialArea = FinancialArea;
                updateAccount.SalesArea = SalesArea;
                updateAccount.PurshacingArea = PurshacingArea;
                updateAccount.InventoryArea = InventoryArea;
                updateAccount.PurshacingArea = PurshacingArea;
                updateAccount.SupportDocument = SupportDocument;

                DB.SaveChanges();

                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
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


        public FileResult DownloadExcel()
        {
            string path = "/UploadedFiles/CompanyAccounts.xlsx";
            return File(path, "application/vnd.ms-excel", "CompanyAccounts.xlsx");
        }


        public JsonResult UploadExcel(Import_Excel_DTO File, string CompanyID, string ChartAccountID)
        {
            HttpPostedFileBase FileUpload = File.AccountsExcel;

            var CompCurrency = DB.CompanyMainInfo_Tables.Where(x => x.CompanyID == CompanyID).FirstOrDefault();

            List<string> data = new List<string>();

            if (FileUpload != null)
            {
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/UploadedFiles/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";

                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var artistAlbums = from a in excelFile.Worksheet<C_CreateAccount_Table_excel>(sheetName) select a;

                    var valid = true;

                    foreach (var a in artistAlbums)
                    {
                        //check exist of account group in this chart
                        var getGroupID = DB.AccountGroupOfChart_Tables.Where(x => x.AccountChartID == ChartAccountID).FirstOrDefault();
                        string groupID = "";

                        if (getGroupID != null)
                        {
                            groupID = getGroupID.AccountGroupChartID;
                        }

                        var checkAccountGroup = DB.ChartGroupContent_Tables.ToList().Where(x => x.AccountName == a.AG && x.AccountGroupChartID == groupID).FirstOrDefault();

                        //check Range of Account Id To Group
                        string AccountID = "";

                        string[] chars = new string[] { "-", "/", ".", "," };
                        try
                        {
                            for (int i = 0; i < chars.Length; i++)
                            {
                                if (a.A.Contains(chars[i]))

                                    AccountID = a.A.Replace(chars[i], "");
                            }

                        }
                        catch
                        {

                        }
                    
                        var checkRange = DB.ChartGroupContent_Tables.ToList().Where(x => x.AccountName == a.AG && Double.Parse(AccountID) >= x.AccountFrom && Double.Parse(AccountID) <= x.AccountTo).FirstOrDefault();

                        var checkDuplicate = DB.C_CreateAccount_Tables.Where(x => x.AccountID == AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();


                        if (checkDuplicate != null)
                        {
                            return Json("There Exist Account ID Not Valid : " + a.A + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.A == "" || a.A == null)
                        {
                            return Json("Account ID is Required .. In Account Name " + a.AN + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.AN == "" || a.AN == null)
                        {
                            return Json("Account Name is Required  .. In Account ID " + a.A + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.AT == "" || a.AT == null)
                        {
                            return Json("Account Type is Required  .. In Account Name " + a.AN + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.PostType == "" || a.PostType == null)
                        {
                            return Json("Posting Type is Required  .. In Account Name " + a.AN + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.AG == "" || a.AG == null)
                        {
                            return Json("Accounts Group is Required  .. In Account Name " + a.AN + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (!new[] { "Debit", "Credit" }.Contains(a.AT))
                        {
                            return Json("Accounts Type Must Be Credit Or Debit  .. In Account Name " + a.AN + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (!new[] { "B", "P" }.Contains(a.PostType))
                        {
                            return Json("Posting Type Must Be BallanceSheet Or PL  .. In Account Name " + a.AN + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (checkAccountGroup == null)
                        {
                            return Json("Check Account Group Not Exist In System  .. In Account Name " + a.AN + "");
                            valid = false;
                        }
                        else if (checkRange == null)
                        {
                            return Json("There Exist Account out of Range with Name " + a.AN + " From Group " + a.AG + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                    }
                    //deleting excel file from folder
                    /*
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    */

                    if (valid == true)
                    {
                        foreach (var a in artistAlbums)
                        {
                            var checkDuplicate = DB.C_CreateAccount_Tables.Where(x => x.AccountID == a.A && x.AccountChartID == ChartAccountID).FirstOrDefault();

                            if (checkDuplicate != null)
                            {
                                return Json("There Exist Account ID Not Valid Delete or Change it : " + a.A + "", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                try
                                {
                                    C_CreateAccount_Table TU = new C_CreateAccount_Table()
                                    {
                                        AccountID = a.A,
                                        AccountName = a.AN,
                                        AccountType = a.AT,
                                        PostingType = a.PostType.Replace("B", PostingType.BallanceSheet.ToString()).Replace("P", PostingType.PL.ToString()),
                                        AccountsGroup = a.AG,
                                        EstablishDate = DateTime.Now.ToShortDateString(),
                                        CompanyID = CompanyID,
                                        AccountChartID = ChartAccountID,
                                        //Currency = CompCurrency.Currency,
                                        DisActive = false,
                                        C_AnalyticAccountID = "",
                                        C_CostCenterID="",
                                        C_CostCenterGroupID = ""
                                    };
                                    DB.C_CreateAccount_Tables.Add(TU);

                                    DB.SaveChanges();
                                }
                                catch (DbEntityValidationException ex)
                                {
                                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                    {

                                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                                        {

                                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);

                                        }

                                    }
                                    return Json(ex.ToString(), JsonRequestBehavior.AllowGet);

                                }
                                catch (Exception ex)
                                {
                                    return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        return Json("success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    //alert message for invalid file format  
                    return Json("Only Excel file format is allowed", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Please choose Excel file", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetCompanyAccounts(int? sortType)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            if(sortType == 1)
            {
                List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountID).Select(x => new RetrieveDataToAccount_DTO
                {
                    C_AID = x.C_AID,

                    C_AccountID = x.AccountID,

                    AccountName = x.AccountName

                }).ToList();

                return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
            else if(sortType == 2)
            {
                List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).OrderBy(x => x.AccountName).Select(x => new RetrieveDataToAccount_DTO
                {
                    C_AID = x.C_AID,

                    C_AccountID = x.AccountID,

                    AccountName = x.AccountName

                }).ToList();

                return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).Select(x => new RetrieveDataToAccount_DTO
                {
                    C_AID = x.C_AID,

                    C_AccountID = x.AccountID,

                    AccountName = x.AccountName

                }).ToList();

                return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllCurrencies()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).Select(x => new RetrieveDataToAccount_DTO
            {

                CurrencyID = x.CurrencyID,

                ISOCode = x.ISOCode

            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountName(int c_AID)
        {
            var getName = DB.C_CreateAccount_Tables.Where(x => x.C_AID == c_AID).FirstOrDefault();
            return Json(getName.AccountName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountCurrencies(int c_AID)
        {
            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.C_CurrencyCreateAccount_Tables.Where(x => x.C_AID == c_AID).Select(x => new RetrieveDataToAccount_DTO
            {
                CurrencyID = x.CurrencyID
            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult SaveAccountCurrencies(int c_AID, C_CurrencyCreateAccount_Table[] currencies)
        //{
        //    var getOldData = DB.C_CurrencyCreateAccount_Tables.Where(x => x.C_AID == c_AID).ToList();
        //    if (getOldData != null)
        //    {
        //        DB.C_CurrencyCreateAccount_Tables.RemoveRange(getOldData);
        //    }

        //    foreach (var item in currencies)
        //    {
        //        C_CurrencyCreateAccount_Table c_CurrencyCreateAccount_Table = new C_CurrencyCreateAccount_Table()
        //        {
        //            C_AID = c_AID,

        //            CurrencyID = item.CurrencyID
        //        };
        //        DB.C_CurrencyCreateAccount_Tables.Add(c_CurrencyCreateAccount_Table);
        //        DB.SaveChanges();
        //    }
        //    return null;
        //}


        public JsonResult SaveCurrencyToAccount(int c_AID, string currencyID)
        {
            C_CurrencyCreateAccount_Table c_CurrencyCreateAccount_Table = new C_CurrencyCreateAccount_Table()
            {
                C_AID = c_AID,

                CurrencyID = currencyID
            };
            DB.C_CurrencyCreateAccount_Tables.Add(c_CurrencyCreateAccount_Table);
            DB.SaveChanges();
            return null;
        }

        public JsonResult RemoveCurrencyFromAccount(int c_AID, string currencyID)
        {
            var item = DB.C_CurrencyCreateAccount_Tables.Where(x => x.C_AID == c_AID && x.CurrencyID == currencyID).FirstOrDefault();
            DB.C_CurrencyCreateAccount_Tables.Remove(item);
            DB.SaveChanges();
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
        //            item.SCCA = Value;
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
        //        if (item.SCCA.ToString().Equals("True"))
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
    public class C_CreateAccount_Table_excel
    {
        public string A { get; set; }
        public string AN { get; set; }
        public string AG { get; set; } = "1";
        public string AT { get; set; } 
        public string PostType { get; set; } 
        public string P { get; set; }
        public string B { get; set; }
        public string D { get; set; }
        public string C { get; set; }
    }
}