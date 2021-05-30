using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
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

namespace FabulousErp.Controllers.Settings.Financial.Accounting._Factory.F_Account
{
    [AuthorizationFilter]
    public class F_CreateAccountController : Controller
    {

        IRepetitionBusiness repetitionBusiness;

        public F_CreateAccountController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }


        DBContext DB = new DBContext();


        // GET: F_CreateAccount
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "SFCA")]
        public ActionResult FactoryAccount()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                ViewBag.CompanyList = companyID;

                ViewBag.FactoryList = repetitionBusiness.RetrieveFactoryIDListCond(companyID);

            }
            return View();
        }



        public JsonResult GetCompanyName(string CompanyID)
        {
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



        public JsonResult GetFactoryName(string FactoryID)
        {
            string userID = FabulousErp.Business.GetUserId();

            var checkAccess = DB.UAFactoryPremission_Tables.Where(x => x.UserID == userID && x.FactoryID == FactoryID).FirstOrDefault();

            if (checkAccess != null)
            {
                var getFactory = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();

                return Json(getFactory.FactoryName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult CheckFactortRelated(string FactoryID)
        {
            var check = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == FactoryID).FirstOrDefault();

            if (check.BranchID == null || check.BranchID == "")
            {
                List<RetrieveDataToAccount_DTO> dataToAccount_DTOs = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == check.CompanyID).Select(x => new RetrieveDataToAccount_DTO
                {
                    C_AccountID = x.AccountID,

                    C_AID = x.C_AID,

                    AccountName = x.AccountName,

                    TFactory = "Company"

                }).ToList();


                return Json(dataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<RetrieveDataToAccount_DTO> retrieveDatas = DB.B_CreateAccount_Tables.Where(x => x.BranchID == check.BranchID).Select(x => new RetrieveDataToAccount_DTO
                {

                    B_AccountID = x.AccountID,

                    B_AID = x.B_AID,

                    AccountName = x.AccountName,

                    TFactory = "Branch"

                }).ToList();

                return Json(retrieveDatas, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult GetAnalyticAccount(string FactoryID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.F_AnalyticAccount_Tables.Where(x => x.FactoryID == FactoryID).Select(x => new RetrieveDataToAccount_DTO
            {

                AAccountID = x.F_AnalyticAccountID

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetAnalyticAccountName(string AnalyticAccountID)
        {
            var getName = DB.F_AnalyticAccount_Tables.Where(x => x.F_AnalyticAccountID == AnalyticAccountID).FirstOrDefault();

            return Json(getName.F_AnalyticAccountName, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCostCenter(string FactoryID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.F_CostCenter_Tables.Where(x => x.FactoryID == FactoryID).Select(x => new RetrieveDataToAccount_DTO
            {

                CostCenterID = x.F_CostCenterID

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCostCenterName(string CostCenterID)
        {
            var getName = DB.F_CostCenter_Tables.Where(x => x.F_CostCenterID == CostCenterID).FirstOrDefault();

            return Json(getName.F_CostCenterName, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetMainCostCenter(string FactoryID)
        {
            List<RetrieveDataToAccount_DTO> chart_Group_Content_DTOs = DB.F_MainCostCenter_Tables.Where(x => x.FactoryID == FactoryID).Select(x => new RetrieveDataToAccount_DTO
            {

                MainCostCenterID = x.F_CostCenterGroupID

            }).ToList();

            return Json(chart_Group_Content_DTOs, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMainCostCenterName(string MainCostCenterID)
        {
            var getName = DB.F_MainCostCenter_Tables.Where(x => x.F_CostCenterGroupID == MainCostCenterID).FirstOrDefault();

            return Json(getName.F_CostCenterGroupName, JsonRequestBehavior.AllowGet);
        }



        public JsonResult SaveCompAccountInfo(string AccountID, string AccountName, string AccountsGroup, string FactoryID, string ChartAccountID, bool DisActive, string AccountType, string PostingType, bool ReconcileAccount, bool AllowAdjusment, bool Reevaluate, bool ConslidationAccount, double MaxAmountPerTransaction, double MinAmountPerTransaction,
                                             string AnalyticAccountID, string CostCenterType, string CostCenterID, string MainCostCenterID, double AccountIDWithoutSep, int? CompanyAccountID, int? BranchAccountID, bool? FinancialArea, bool? SalesArea, bool? PurshacingArea, bool? InventoryArea, bool SupportDocument)
        {

            var getGroupID = DB.AccountGroupOfChart_Tables.Where(x => x.AccountChartID == ChartAccountID).FirstOrDefault();

            var checkRange = DB.ChartGroupContent_Tables.Where(x => x.AccountName == AccountsGroup && x.AccountGroupChartID == getGroupID.AccountGroupChartID && AccountIDWithoutSep >= x.AccountFrom && AccountIDWithoutSep <= x.AccountTo).FirstOrDefault();

            var checkDuplicate = DB.F_CreateAccount_Tables.Where(x => x.AccountID == AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

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
                F_CreateAccount_Table f_CreateAccount_Table = new F_CreateAccount_Table()
                {
                    AccountID = AccountID,

                    AccountName = AccountName,

                    AccountsGroup = AccountsGroup,

                    FactoryID = FactoryID,

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

                    F_AnalyticAccountID = AnalyticAccountID,

                    CostCenterType = CostCenterType,

                    F_CostCenterID = CostCenterID,

                    F_CostCenterGroupID = MainCostCenterID,

                    C_AID = CompanyAccountID,

                    B_AID = BranchAccountID,

                    FinancialArea = FinancialArea,

                    InventoryArea = InventoryArea,

                    PurshacingArea = PurshacingArea,

                    SalesArea = SalesArea,

                    SupportDocument = SupportDocument
                };

                DB.F_CreateAccount_Tables.Add(f_CreateAccount_Table);
                DB.SaveChanges();

                return null;
            }
        }



        public JsonResult GetCompAccountData(int AccountID, string ChartAccountID)
        {
            var accountData = DB.F_CreateAccount_Tables.Where(x => x.F_AID == AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

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

                    R_AnalyticAccountID = accountData.F_AnalyticAccountID,

                    R_CostCenterID = accountData.F_CostCenterID,

                    R_CostCenterGroupID = accountData.F_CostCenterGroupID,

                    C_AID = accountData.C_AID.GetValueOrDefault(),

                    B_AID = accountData.B_AID.GetValueOrDefault(),

                    SalesArea = accountData.SalesArea,

                    PurshacingArea = accountData.PurshacingArea,

                    InventoryArea = accountData.InventoryArea,

                    FinancialArea = accountData.FinancialArea,

                    SupportDocument = accountData.SupportDocument
                };

                return Json(retrieveDataToAccount_DTO, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult UpdateCompAccountInfo(int AccountID, bool DisActive, string AccountType, string PostingType, bool ReconcileAccount, bool AllowAdjusment, bool Reevaluate, bool ConslidationAccount, double MaxAmountPerTransaction, double MinAmountPerTransaction,
                                              string AnalyticAccountID, string CostCenterType, string CostCenterID, string MainCostCenterID, string ChartAccountID, bool? FinancialArea, bool? SalesArea, bool? PurshacingArea, bool? InventoryArea, bool SupportDocument)
        {
            var getAccount = DB.F_CreateAccount_Tables.Where(x => x.F_AID == AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

            if (getAccount != null)
            {

                var getID = getAccount.F_AID;
                var updateAccount = DB.F_CreateAccount_Tables.Where(x => x.F_AID == getID).FirstOrDefault();

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
                updateAccount.F_AnalyticAccountID = AnalyticAccountID;
                updateAccount.CostCenterType = CostCenterType;
                updateAccount.F_CostCenterID = CostCenterID;
                updateAccount.F_CostCenterGroupID = MainCostCenterID;
                updateAccount.FinancialArea = FinancialArea;
                updateAccount.InventoryArea = InventoryArea;
                updateAccount.PurshacingArea = PurshacingArea;
                updateAccount.SalesArea = SalesArea;
                updateAccount.SupportDocument = SupportDocument;

                DB.SaveChanges();

                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }



        public JsonResult GetCurrencyFormate(string companyID)
        {
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
            string path = "/UploadedFiles/FactoryAccounts.xlsx";
            return File(path, "application/vnd.ms-excel", "FactoryAccounts.xlsx");
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
                    var artistAlbums = from a in excelFile.Worksheet<F_CreateAccount_Table>(sheetName) select a;

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

                        var checkAccountGroup = DB.ChartGroupContent_Tables.ToList().Where(x => x.AccountName == a.AccountsGroup && x.AccountGroupChartID == groupID).FirstOrDefault();

                        var checkCompanyAccount = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == CompanyID && x.AccountID == a.CompanyAccountID).FirstOrDefault();

                        var checkBranchAccount = DB.B_CreateAccount_Tables.Where(x => x.AccountChartID == ChartAccountID && x.AccountID == a.BranchAccountID).FirstOrDefault();

                        //check Range of Account Id To Group
                        string AccountID = "";

                        string[] chars = new string[] { "-", "/", ".", "," };

                        for (int i = 0; i < chars.Length; i++)
                        {
                            if (a.AccountID.Contains(chars[i]))

                                AccountID = a.AccountID.Replace(chars[i], "");
                        }

                        var checkRange = DB.ChartGroupContent_Tables.ToList().Where(x => x.AccountName == a.AccountsGroup && Double.Parse(AccountID) >= x.AccountFrom && Double.Parse(AccountID) <= x.AccountTo).FirstOrDefault();

                        var checkExistOFFactory = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == a.FactoryID).FirstOrDefault();

                        var checkDuplicate = DB.F_CreateAccount_Tables.Where(x => x.AccountID == a.AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

                        var checkRelatedOfFactory = DB.CompanyFactoryInfo_Tables.Where(x => x.FactoryID == a.FactoryID).FirstOrDefault();

                        if (checkDuplicate != null)
                        {
                            return Json("There Exist Account ID Not Valid : " + a.AccountID + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.FactoryID == "" || a.FactoryID == null)
                        {
                            return Json("Factory ID is Required .. In Account ID " + a.AccountID + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (checkExistOFFactory == null)
                        {
                            return Json("Factory ID Not Exist In System .. ID : " + a.FactoryID + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.AccountID == "" || a.AccountID == null)
                        {
                            return Json("Account ID is Required .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.AccountName == "" || a.AccountName == null)
                        {
                            return Json("Account Name is Required  .. In Account ID " + a.AccountID + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.AccountType == "" || a.AccountType == null)
                        {
                            return Json("Account Type is Required  .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.PostingType == "" || a.PostingType == null)
                        {
                            return Json("Posting Type is Required  .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.AccountsGroup == "" || a.AccountsGroup == null)
                        {
                            return Json("Accounts Group is Required  .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (!new[] { "Debit", "Credit" }.Contains(a.AccountType))
                        {
                            return Json("Accounts Type Must Be Credit Or Debit  .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (!new[] { "BallanceSheet", "PL" }.Contains(a.PostingType))
                        {
                            return Json("Posting Type Must Be BallanceSheet Or PL  .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (checkAccountGroup == null)
                        {
                            return Json("Check Account Group Not Exist In System  .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (checkRange == null)
                        {
                            return Json("There Exist Account out of Range with Name " + a.AccountName + " From Group " + a.AccountsGroup + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.CompanyAccountID == null && a.BranchAccountID == null)
                        {
                            return Json("CompanyAccount ID OR BranchAccountID Is Required .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.CompanyAccountID != null && a.BranchAccountID != null)
                        {
                            return Json("You Must Fill CompanyAccountID or BranchAccountID Not Two Together .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.CompanyAccountID == null && checkRelatedOfFactory.BranchID == null)
                        {
                            return Json("This Factory Related With Company Directly You Should Fill CompanyAccountID Not BranchAccountID .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.BranchAccountID == null && checkRelatedOfFactory.BranchID != null)
                        {
                            return Json("This Factory Related With Branch Directly You Should Fill BranchAccountID Not CompanyAccountID .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.CompanyAccountID != null && checkCompanyAccount == null)
                        {
                            return Json("Company Account ID not Exist In System .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
                            valid = false;
                        }
                        else if (a.BranchAccountID != null && checkBranchAccount == null)
                        {
                            return Json("Branch Account ID not Exist In System .. In Account Name " + a.AccountName + "", JsonRequestBehavior.AllowGet);
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

                            var checkDuplicate = DB.F_CreateAccount_Tables.Where(x => x.AccountID == a.AccountID && x.AccountChartID == ChartAccountID).FirstOrDefault();

                            if (checkDuplicate != null)
                            {
                                return Json("There Exist Account ID Not Valid Delete or Change it : " + a.AccountID + "", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                int? C_AID = 0;
                                var getIdentityIDOfCompanyAccount = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == CompanyID && x.AccountID == a.CompanyAccountID).FirstOrDefault();
                                if (getIdentityIDOfCompanyAccount != null)
                                {
                                    C_AID = getIdentityIDOfCompanyAccount.C_AID;
                                }
                                else
                                {
                                    C_AID = null;
                                }

                                int? B_AID = 0;
                                var getIdentityIDOfBranchAccount = DB.B_CreateAccount_Tables.Where(x => x.AccountChartID == ChartAccountID && x.AccountID == a.BranchAccountID).FirstOrDefault();
                                if (getIdentityIDOfBranchAccount != null)
                                {
                                    B_AID = getIdentityIDOfBranchAccount.B_AID;
                                }
                                else
                                {
                                    B_AID = null;
                                }

                                try
                                {
                                    F_CreateAccount_Table TU = new F_CreateAccount_Table()
                                    {
                                        AccountID = a.AccountID,
                                        AccountName = a.AccountName,
                                        AccountType = a.AccountType,
                                        PostingType = a.PostingType,
                                        AccountsGroup = a.AccountsGroup,
                                        EstablishDate = DateTime.Now.ToShortDateString(),
                                        FactoryID = a.FactoryID,
                                        AccountChartID = ChartAccountID,
                                        //Currency = CompCurrency.Currency,
                                        DisActive = false,
                                        C_AID = C_AID,
                                        B_AID = B_AID
                                    };
                                    DB.F_CreateAccount_Tables.Add(TU);

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
                                }
                                catch (Exception)
                                {
                                    return Json("There Exist Duplicate In Account ID " + a.AccountID + " Delete Or Change it", JsonRequestBehavior.AllowGet);
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



        public JsonResult GetFactoryAccounts(int? sortType, string factoryID)
        {
            if (sortType == 1)
            {
                List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).OrderBy(x => x.AccountID).Select(x => new RetrieveDataToAccount_DTO
                {
                    F_AID = x.F_AID,

                    F_AccountID = x.AccountID,

                    AccountName = x.AccountName

                }).ToList();

                return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
            else if (sortType == 2)
            {
                List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).OrderBy(x => x.AccountName).Select(x => new RetrieveDataToAccount_DTO
                {
                    F_AID = x.F_AID,

                    F_AccountID = x.AccountID,

                    AccountName = x.AccountName

                }).ToList();

                return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new RetrieveDataToAccount_DTO
                {
                    F_AID = x.F_AID,

                    F_AccountID = x.AccountID,

                    AccountName = x.AccountName

                }).ToList();

                return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetAllCurrencies()
        {

            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();

                List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).Select(x => new RetrieveDataToAccount_DTO
                {

                    CurrencyID = x.CurrencyID,

                    ISOCode = x.ISOCode

                }).ToList();

                return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }


        }

        public JsonResult GetAccountName(int f_AID)
        {
            var getName = DB.F_CreateAccount_Tables.Where(x => x.F_AID == f_AID).FirstOrDefault();
            return Json(getName.AccountName, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAccountCurrencies(int f_AID)
        {
            List<RetrieveDataToAccount_DTO> retrieveDataToAccount_DTOs = DB.F_CurrencyCreateAccount_Tables.Where(x => x.F_AID == f_AID).Select(x => new RetrieveDataToAccount_DTO
            {
                CurrencyID = x.CurrencyID
            }).ToList();

            return Json(retrieveDataToAccount_DTOs, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult SaveAccountCurrencies(int f_AID, F_CurrencyCreateAccount_Table[] currencies)
        //{
        //    var getOldData = DB.F_CurrencyCreateAccount_Tables.Where(x => x.F_AID == f_AID).ToList();
        //    if (getOldData != null)
        //    {
        //        DB.F_CurrencyCreateAccount_Tables.RemoveRange(getOldData);
        //    }

        //    foreach (var item in currencies)
        //    {
        //        F_CurrencyCreateAccount_Table f_CurrencyCreateAccount_Table = new F_CurrencyCreateAccount_Table()
        //        {
        //            F_AID = f_AID,

        //            CurrencyID = item.CurrencyID
        //        };
        //        DB.F_CurrencyCreateAccount_Tables.Add(f_CurrencyCreateAccount_Table);
        //        DB.SaveChanges();
        //    }
        //    return null;
        //}

        public JsonResult SaveCurrencyToAccount(int f_AID, string currencyID)
        {
            F_CurrencyCreateAccount_Table f_CurrencyCreateAccount_Table = new F_CurrencyCreateAccount_Table()
            {
                F_AID = f_AID,

                CurrencyID = currencyID
            };
            DB.F_CurrencyCreateAccount_Tables.Add(f_CurrencyCreateAccount_Table);
            DB.SaveChanges();
            return null;
        }

        public JsonResult RemoveCurrencyFromAccount(int f_AID, string currencyID)
        {
            var item = DB.F_CurrencyCreateAccount_Tables.Where(x => x.F_AID == f_AID && x.CurrencyID == currencyID).FirstOrDefault();
            DB.F_CurrencyCreateAccount_Tables.Remove(item);
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
        //            item.SFCA = Value;
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
        //        if (item.SFCA.ToString().Equals("True"))
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