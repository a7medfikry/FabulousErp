using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using FabulousDB.DB_Tabels.Tax;
using FabulousDB.Models;
using FabulousErp.MyRoleProvider;
using FabulousModels.DTOModels.Settings.Financial.GeneralSettings.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Controllers.Transaction.Financial.Company.Accounting
{
    [BranchSecurityFilter]
    [FactorySecurityFilter]
    [AuthorizationFilter]
    public class C_TaxTransactionController : Controller
    {
        DBContext DB = new DBContext();

        // GET: C_TaxTransaction
        [HttpGet]
       // [Authorize]
        public ActionResult CompanyTaxTransaction()
        {
            string companyID = (string)FabulousErp.Business.GetCompanyId();
            
            var getCurrencyID = DB.CurrenciesDefinition_Tables.Where(x => x.CompanyID == companyID).ToList();
            if (getCurrencyID != null)
            {
                SelectList currencyIDList = new SelectList(getCurrencyID, "CurrencyID", "ISOCode");
                ViewBag.CurrencyIDList = currencyIDList;
            }

            var SPSCheck =  Business.GetPostingSetup();
            if (SPSCheck != null)
            {
                ViewBag.EPD = SPSCheck.EditPostingDate;
                ViewBag.PT = SPSCheck.PostingType;
            }
            else
            {
                ViewBag.FJEPer = "NoPS";
            }

            var FiscalyearCheck = DB.CompanyFiscalYear_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (FiscalyearCheck != null)
            {
                var checkYearOpen = DB.NewFiscalYear_Table.Where(x => x.CheckDate == "True" && x.Fiscal_Year_ID == FiscalyearCheck.Fiscal_Year_ID && (x.Closed == null || x.Closed == false)).FirstOrDefault();
                if (checkYearOpen != null)
                {
                    ViewBag.CheckYear = "Exist";
                }
            }

            var checkCurrencyFormate = DB.FormateSetting_Tables.Where(x => x.CompanyID == companyID).FirstOrDefault();
            if (checkCurrencyFormate != null)
            {
                ViewBag.FormateSetting = "True";
            }

            var checkEditTransactionRate = DB.C_EditTRates.Where(x => x.CompanyID == companyID && x.Module == "Financial" && x.TransactionFormName == "Company Journal Entry Transaction").FirstOrDefault();
            if (checkEditTransactionRate != null)
            {
                if (checkEditTransactionRate.AllowUserE == true)
                {
                    ViewBag.AllowUserERate = "True";
                }
            }

            var getCompanyTaxGroup = DB.TaxGroup_Tables.Where(x => x.CompanyID == companyID && x.C_TaxGrouptype != "General").Select(x => new { tG_ID = x.TG_ID, TaxGroupID = x.TaxGroupID + " - " + x.C_TaxGrouptype }).ToList();
            if (Request["Tax"]!= "Sales")
            {
                SelectList groupTaxList = new SelectList(getCompanyTaxGroup.Where(x => x.tG_ID == 1), "tG_ID", "TaxGroupID", 1);
                ViewBag.GroupTaxList = groupTaxList;
                ViewBag.Title = "Company Purchase Tax";
            }
            else
            {
                SelectList groupTaxList = new SelectList(getCompanyTaxGroup.Where(x => x.tG_ID == 2), "tG_ID", "TaxGroupID", 2);
                ViewBag.GroupTaxList = groupTaxList;
                ViewBag.Title = "Company Sales Tax";

            }

            //Accounts in financial and not has consalidation and not related with checkbook
            var getCompanyAccounts = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true && x.ConsolidationAccount == false && !x.C_CheckBookSetting_Table.Any()).Select(x => new { AID = x.C_AID, AccountName = x.AccountID + " ( " + x.AccountName + " )" }).ToList();
            SelectList accountsList = new SelectList(getCompanyAccounts, "AID", "AccountName");
            ViewBag.AccountsList = accountsList;


            var accountsData = DB.C_CreateAccount_Tables.Where(x => x.CompanyID == companyID && x.FinancialArea == true).OrderBy(x => x.AccountID).Select(x => new { AccountID = x.AccountID + " ( " + x.AccountName + " )", x.C_AID }).ToList();
            SelectList accountList = new SelectList(accountsData, "C_AID", "AccountID");
            ViewBag.AccountList = accountList;

            ViewBag.PostingToOrThrow = Business.PostingToOrThrow();
            ViewBag.PageKey = "Tax";
            return View();
        }
        public JsonResult GetTaxCodeByGroup(int taxGroupID)
        {
            List<TaxCodeByGroup_DTO> c_TaxSetting_Tables = DB.C_TaxSetting_Tables.Where(x => x.TG_ID == taxGroupID).Select(x => new TaxCodeByGroup_DTO
            {
                CT_ID = x.CT_ID,
                C_Taxcode = x.C_Taxcode
            }).ToList();

            return Json(c_TaxSetting_Tables, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTaxCodeValue(int taxCodeID)
        {
            TaxCodeByGroup_DTO taxCodeByGroup_DTO = DB.C_TaxSetting_Tables.Where(x => x.CT_ID == taxCodeID).Select(x => new TaxCodeByGroup_DTO
            {

                Amount = x.C_TaxAmount,

                Percentage = x.C_Taxpercentage,

                C_AID = x.C_AID,

                AccountID = x.C_CreateAccount_Table.AccountID,

                AccountName = x.C_CreateAccount_Table.AccountName,
                
                MinAmount=x.C_MinTaxable,
                MaxAmount=x.C_MaxTaxable

            }).FirstOrDefault();

            return Json(taxCodeByGroup_DTO, JsonRequestBehavior.AllowGet);
        } 
        [HttpPost]
        public JsonResult AddTax (List<FabulousDB.DB_Tabels.Tax.Tax> Tax)
        {
            try
            {
                foreach (FabulousDB.DB_Tabels.Tax.Tax i in Tax)
                {
                    if (i.Doc_type==Tax_doc_type.Invoice
                        || i.Doc_type==Tax_doc_type.Credit_Memo
                        || i.Doc_type == Tax_doc_type.Debit_Memo)
                    {
                        i.Statment_type = Statment_type.Local;
                    }
                    else if (i.Doc_type == Tax_doc_type.Custom_clearnce)
                    {
                        i.Statment_type = Statment_type.Imported;
                    }
                    else if (i.Doc_type == Tax_doc_type.Return)
                    {
                        i.Statment_type = Statment_type.Reconcile;
                    }
                    foreach (System.Reflection.PropertyInfo prop in i.GetType().GetProperties())
                    {
                        try
                        {
                            string type = prop.GetValue(i).ToString();
                            if (type == "undefined")
                            {
                                prop.SetValue(i, "");
                            }
                        }
                        catch
                        {

                        }
                    }
                    if (i.Date == new DateTime(1, 1, 1, 0, 0, 0))
                    {
                        i.Date = DateTime.Now;
                    }
                    if (i.Unit_price < 0)
                    {
                        i.Unit_price = -i.Unit_price;
                    }
                    if (i.Total_amount < 0)
                    {
                        i.Total_amount = -i.Total_amount;
                    }
                    if (i.Net_amount < 0)
                    {
                        i.Net_amount = -i.Net_amount;
                    }
                    if (i.Doc_type == Tax_doc_type.Return)
                    {
                        if (i.Table_after_vat_amount > 0)
                        {
                            i.Table_after_vat_amount = -i.Table_after_vat_amount;
                        }
                        if (i.Dacutta_amount > 0)
                        {
                            i.Dacutta_amount = -i.Dacutta_amount;
                        } 
                        if (i.Vat_amount > 0)
                        {
                            i.Vat_amount = -i.Vat_amount;
                        } 
                        if (i.Total_vat_amount > 0)
                        {
                            i.Total_vat_amount = -i.Total_vat_amount;
                        }
                        if (i.Net_amount > 0)
                        {
                            i.Net_amount = -i.Net_amount;
                        }
                        if (i.Unit_price > 0)
                        {
                            i.Unit_price = -i.Unit_price;
                        }
                        if (i.Total_amount > 0)
                        {
                            i.Total_amount = -i.Total_amount;
                        }
                    }
                }
                DB.Taxs.AddRange(Tax);
                DB.SaveChanges();
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(0);
            }
        }
        public PartialViewResult MainTax(string Tax,string PostingNumber=null)
        {
            if (!string.IsNullOrEmpty(Tax))
            {
                ViewBag.Tax = Tax;
            }
            if (!string.IsNullOrEmpty(Request["Tax"]))
            {
                ViewBag.Tax = Request["Tax"];
            }
            ViewBag.UnitOfMeasure = new SelectList(DB.Unit_of_measures, "Id", "Unit_id");
            ViewBag.PostingNumber = PostingNumber;
            return PartialView();
        }
        public PartialViewResult TaxTable(int? PostingNumber = null)
        {
            List<FabulousModels.Tax.TaxTable> Taxes = new List<FabulousModels.Tax.TaxTable>();
            if (PostingNumber.HasValue)
            {
                ViewBag.Print = true;
               int JournalEntry=  Business.GetJournalEntry(PostingNumber);
                Taxes.AddRange(DB.Taxs.Where(x =>x.Vat_amount!=0&&x.Table_vat_amount!=0&&x.Dacutta_amount!=0&& x.Journal_number == JournalEntry)
                    .Select(x => new FabulousModels.Tax.TaxTable
                    {
                        Action = "",
                        Deducte_amount = x.Dacutta_amount,
                        Deducte_tax_id = x.Dacutta_id,
                        Discount = x.Discount,
                        Item_name = x.Item_name,
                        Net_amount = x.Net_amount,
                        Orginal_amount = x.Net_amount,
                        Quantity = x.Quantity,
                        Total_amount = x.Total_amount,
                        Unit_price =Math.Round((x.Total_amount-x.Discount)/x.Quantity,2),
                        UOM = x.Unit_of_measure_id,
                        Vat_amount = x.Total_vat_amount
                    }));
            }
            return PartialView(Taxes);
        }
    }

}