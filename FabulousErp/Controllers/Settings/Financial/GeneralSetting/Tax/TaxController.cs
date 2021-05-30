using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using FabulousErp.MyRoleProvider;
using FabulousErp.Repository.Interface;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Settings.Financial.GeneralSetting.Tax
{
    [BranchSecurityFilter] //for block this page from branch login
    [FactorySecurityFilter] //for block this page from factory login
    [AuthorizationFilter] //for logout user if disactive or deleted or session empty
    public class TaxController : Controller
    {
        IRepetitionBusiness repetitionBusiness;
        public TaxController(IRepetitionBusiness repetitionBusiness)
        {
            this.repetitionBusiness = repetitionBusiness;
        }

        DBContext DB = new DBContext();

        // GET: Tax
        [HttpGet]
       // [Authorize]
        [CustomAuthorize(Roles = "STA")]
        public ActionResult Tax_Setting()
        {
            if (FabulousErp.Business.GetCompanyId() != null)
            {
                string companyID = (string)FabulousErp.Business.GetCompanyId();
                ViewBag.CompanyID = companyID;
                ViewBag.CompanyName = repetitionBusiness.GetCompanyName(companyID);
                ViewBag.BranchID = repetitionBusiness.RetrieveBranchIDListCond(companyID);
                ViewBag.FactoryID = repetitionBusiness.RetrieveFactoryIDListCond(companyID);

                var list = DB.C_TaxSetting_Tables.Where(x => x.CompanyID == companyID).Select(x=>new { CT_ID=x.CT_ID, C_Taxcode= x.C_Taxcode }).ToList();
                SelectList CompanyTaxCode = new SelectList(list, "CT_ID", "C_Taxcode");
                ViewBag.CompanyTaxCode = CompanyTaxCode;

                var AccountIDCheck = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
                if (AccountIDCheck != null)
                {
                    ViewBag.AccountIDCheck = "Exist";
                }
                else
                {
                    ViewBag.AccountIDCheck = "NotExist";
                }

                //Accounts in financial and has reconcile and not related with checkbook
                var getCompanyAccounts = DB.C_CreateAccount_Tables
                    .Where(x => x.CompanyID == companyID
                    && (x.C_AnalyticAccountID == null || x.C_AnalyticAccountID == "")
                    && (x.C_CostCenterID == null || x.C_CostCenterID == "")
                    && (x.C_CostCenterGroupID == null || x.C_CostCenterGroupID == "")
                    && (x.C_Prefix == "Tax" || x.C_Prefix == null)
                    && x.FinancialArea == true && x.ReconcileAccount == true &&
                    !x.C_CheckBookSetting_Table.Any())
                    .Select(x => new { AID = x.C_AID, AccountName = x.AccountID + " ( " + x.AccountName + " )" })
                  ;
                SelectList accountsList = new SelectList(getCompanyAccounts, "AID", "AccountName");
                ViewBag.AccountsList = accountsList;

                var getCompanyTaxs = DB.C_TaxSetting_Tables.Where(x => x.CompanyID == companyID).Select(x => new { CT_ID = x.CT_ID, C_Taxcode = x.C_Taxcode }).ToList();
                SelectList taxsList = new SelectList(getCompanyTaxs, "CT_ID", "C_Taxcode");
                ViewBag.TaxsList = taxsList;

                var getCompanyTaxGroup = DB.TaxGroup_Tables.Where(x => x.CompanyID == companyID).ToList().Select(x => new { tG_ID = x.TG_ID,TaxGroupID = BusController.Translate(x.TaxGroupID) + " - " + BusController.Translate(x.C_TaxGrouptype) }).ToList();
                SelectList groupTaxList = new SelectList(getCompanyTaxGroup, "tG_ID", "TaxGroupID");
                ViewBag.GroupTaxList = groupTaxList;
                ViewBag.Transaction_type = new SelectList(Enum.GetNames(typeof(Transaction_type)).Select(x => new {Id=x,Name =BusController.Translate(x) }), "Id", "Name");
            }
            return View();
        }

        public JsonResult GetCompanyAccountName(int companyaccountID)
        {
            var list = DB.C_CreateAccount_Tables.Where(x => x.C_AID == companyaccountID).FirstOrDefault();
            if (list != null)
            {
                string companyaccountName = list.AccountName;
                return Json(companyaccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult SaveCompanyTax(string Taxcode, string TaxDescribtion, int taxGroupID, double? TaxPercentage, double? TaxAmount, int AccountID, string CompanyID, double? MinAmount, double? MaxAmount, bool? PrintDocument,
            Transaction_type TransactionType=0)
        {
            var check = DB.C_TaxSetting_Tables.Where(x => x.C_AID == AccountID && x.C_Taxcode == Taxcode).Any();
            if (check != false)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var check2 = DB.C_TaxSetting_Tables.Where(x => x.CompanyID == CompanyID && x.C_Taxcode == Taxcode).Any();
                if (check2 != false)
                {
                    return Json("falsetax", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    C_TaxSetting_table c_TaxSetting_Table = new C_TaxSetting_table()
                    {
                        C_Taxcode = Taxcode,
                        C_Taxdescribtion = TaxDescribtion,
                        TG_ID = taxGroupID,
                        //C_Taxtype = Taxtype,
                        C_Taxpercentage = TaxPercentage,
                        C_TaxAmount = TaxAmount,
                        C_AID = AccountID,
                        CompanyID = CompanyID,
                        C_Printdocument = PrintDocument,
                        C_MinTaxable = MinAmount,
                        C_MaxTaxable = MaxAmount,
                        Transaction_type= TransactionType
                    };
                    try
                    {
                        DB.C_CreateAccount_Tables.Find(AccountID).C_Prefix = "Tax";
                    }
                    catch
                    {

                    }
                    DB.C_TaxSetting_Tables.Add(c_TaxSetting_Table);
                    DB.SaveChanges();
                }
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveBranchTax(int Taxcode, int AccountID)
        {
            var check = DB.B_TaxSetting_Tables.Where(x => x.B_AID == AccountID && x.CT_ID == Taxcode).FirstOrDefault();
            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                B_TaxSetting_table b_TaxSetting_Table = new B_TaxSetting_table()
                {
                    CT_ID = Taxcode,
                    B_AID = AccountID
                };
                DB.B_TaxSetting_Tables.Add(b_TaxSetting_Table);
                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult SaveFactoryTax(int Taxcode, int AccountID)
        {
            var check = DB.F_TaxSetting_Tables.Where(x => x.F_AID == AccountID && x.CT_ID == Taxcode).FirstOrDefault();
            if (check != null)
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
            else
            {
                F_TaxSetting_table f_TaxSetting_Table = new F_TaxSetting_table()
                {
                    CT_ID = Taxcode,
                    F_AID = AccountID
                };
                DB.F_TaxSetting_Tables.Add(f_TaxSetting_Table);
                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult TaxSearch(int taxcode, string companyID)
        {
            var data = DB.C_TaxSetting_Tables.Where(x => x.CT_ID == taxcode && x.CompanyID == companyID).FirstOrDefault();
            Tax_DTO tax_DTO = new Tax_DTO()
            {
                AccountID = data.C_AID,
                AccountName = data.C_CreateAccount_Table.AccountName,
                //TaxType = data.C_Taxtype,
                TaxGroupID = data.TG_ID,
                TaxDescribtion = data.C_Taxdescribtion,
                TaxPercentage = data.C_Taxpercentage,
                TaxAmount = data.C_TaxAmount,
                MinTaxable = data.C_MinTaxable,
                MaxTaxable = data.C_MaxTaxable,
                PrintDocument = data.C_Printdocument,
                Transaction_type= data.Transaction_type.ToString(),
                SelectPrintDocument = data.C_Selectprintdocument
            };
            return Json(tax_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CompanyTaxUpdate(int Taxcode, string TaxDescribtion, int taxGroupID, double? TaxPercentage, double? TaxAmount, int AccountID, string AccountName, string CompanyID, double? MinAmount, double? MaxAmount, bool? PrintDocument, Transaction_type Transaction_type)
        {
            var check = DB.C_TaxSetting_Tables.Where(x => x.CT_ID == Taxcode).FirstOrDefault();
            if (check != null)
            {
                check.CT_ID = Taxcode;
                check.C_Taxdescribtion = TaxDescribtion;
                //check.C_Taxtype = Taxtype;
                check.TG_ID = taxGroupID;
                check.C_Taxpercentage = TaxPercentage;
                check.C_TaxAmount = TaxAmount;
                check.C_AID = AccountID;
                check.CompanyID = CompanyID;
                check.C_Printdocument = PrintDocument;
                check.C_MinTaxable = MinAmount;
                check.C_MaxTaxable = MaxAmount;
                check.Transaction_type = Transaction_type;
                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult BranchTaxUpdate(int Taxcode, int AccountID)
        {
            var check = DB.B_TaxSetting_Tables.Where(x => x.B_AID == AccountID).FirstOrDefault();
            if (check != null)
            {
                //check.B_AID = AccountID;
                check.CT_ID = Taxcode;
                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult FactoryTaxUpdate(int Taxcode, int AccountID)
        {
            var check = DB.F_TaxSetting_Tables.Where(x => x.F_AID == AccountID).FirstOrDefault();
            if (check != null)
            {
                //check.F_AID = AccountID;
                check.CT_ID = Taxcode;
                DB.SaveChanges();
                return Json("True", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("False", JsonRequestBehavior.AllowGet);
            }
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

        public JsonResult GetBranchAccountsID(string branchID)
        {
            List<Tax_DTO> Tax_DTO = DB.B_CreateAccount_Tables.Where(x => x.BranchID == branchID).Select(x => new Tax_DTO
            {
                Branch_AccountsID = x.AccountID,
                AccountName = x.AccountName,
                B_AID = x.B_AID
            }).ToList();

            return Json(Tax_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBranchAccountName(int branchaccountID)
        {
            var list = DB.B_CreateAccount_Tables.Where(x => x.B_AID == branchaccountID).FirstOrDefault();
            if (list != null)
            {
                string branchaccountName = list.AccountName;
                return Json(branchaccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
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

        public JsonResult GetFactoryAccountsID(string factoryID)
        {
            List<Tax_DTO> Tax_DTO = DB.F_CreateAccount_Tables.Where(x => x.FactoryID == factoryID).Select(x => new Tax_DTO
            {
                Factory_AccountsID = x.AccountID,
                AccountName = x.AccountName,
                F_AID = x.F_AID
            }).ToList();

            return Json(Tax_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFactoryAccountName(int factoryaccountID)
        {
            var list = DB.F_CreateAccount_Tables.Where(x => x.F_AID == factoryaccountID).FirstOrDefault();
            if (list != null)
            {
                string factoryaccountName = list.AccountName;
                return Json(factoryaccountName, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }

        public JsonResult GetDetails(int taxcode)
        {
            Tax_DTO Tax_DTO = DB.C_TaxSetting_Tables.Where(x => x.CT_ID == taxcode).Select(x => new Tax_DTO
            {
                //TaxType = x.C_Taxtype,
                TaxGroupID = x.TG_ID,
                TaxDescribtion = x.C_Taxdescribtion,
            }).FirstOrDefault();
            return Json(Tax_DTO, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddTaxGroup(string taxGroupID, string description, string type)
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();

            var checkDuplicate = DB.TaxGroup_Tables.Where(x => x.CompanyID == companyID && x.TaxGroupID == taxGroupID).FirstOrDefault();
            if (checkDuplicate != null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                TaxGroup_table taxGroup_Table = new TaxGroup_table()
                {
                    CompanyID = companyID,

                    C_TaxGrouptype = type,

                    TaxGroupID = taxGroupID,

                    TaxGroupDescribtion = description
                };
                DB.TaxGroup_Tables.Add(taxGroup_Table);
                DB.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult GetCompanyTaxCode(string companyID)
        //{
        //    List<Tax_DTO> Tax_DTO = DB.C_TaxSetting_Tables.Where(x => x.CompanyID == companyID).Select(x => new Tax_DTO
        //    {
        //        CompanyTaxs = x.C_Taxcode,
        //        CT_ID = x.CT_ID
        //    }).ToList();

        //    return Json(Tax_DTO, JsonRequestBehavior.AllowGet);
        //}

        //public JsonResult GetCompanyAccountsID(string companyID)
        //{
        //    List<Tax_DTO> Tax_DTO = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID).Select(x => new Tax_DTO
        //    {
        //        Company_AccountsID = x.AccountID,
        //        AccountName = x.AccountName,
        //        C_AID = x.C_AID
        //    }).ToList();

        //    return Json(Tax_DTO, JsonRequestBehavior.AllowGet);
        //}
       
    }
}