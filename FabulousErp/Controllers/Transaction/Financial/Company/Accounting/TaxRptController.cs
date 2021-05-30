using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Tax;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FabulousErp.Tax
{
    public class TaxRptController : Controller
    {
        DBContext db = new DBContext();
        // GET: TaxRpt
        public ActionResult Purchase()
        {
            if (Request["Tax"] != "Sales")
            {
                var Year = db.Taxs.Where(x => x.Tax_group_id == 1).Select(x => new { Year = x.Date.Year }).ToList().Distinct();

                ViewBag.Year = new SelectList(Year, "Year", "Year");


                var Monthes = db.Taxs.Where(x => x.Tax_group_id == 1 && x.Date != null).OrderBy(x=>x.Date)
                    .ToList().Select(x => new { Month = x.Date.Month,MonthName=
                      System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(x.Date.Month)
                    }).ToList().Distinct();



                ViewBag.Monthes = new SelectList(Monthes, "Month", "MonthName");
                ViewBag.Type = 1;
            }
            else
            {
                var Year = db.Taxs.Where(x => x.Tax_group_id == 2).Select(x => new { Year = x.Date.Year }).ToList().Distinct();

                ViewBag.Year = new SelectList(Year, "Year", "Year");


                var Monthes = db.Taxs.Where(x => x.Tax_group_id == 2 && x.Date != null)
                     .ToList().Select(x => new { Month = x.Date.Month,
                    MonthName =
                   System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(x.Date.Month)
                }).ToList().Distinct();
                ViewBag.Monthes = new SelectList(Monthes, "Month", "MonthName");
                ViewBag.Type = 2;
            }
            return View();
        }
        public PartialViewResult PurchaseRes(int? Year=null,int? Month=null,int Type=1)
        {
            List<Purches_tax_rpt> PurRpt = new List<Purches_tax_rpt>();
            if (Month == null&&Year!=null)
            {
                PurRpt = db.Taxs.Where(x => x.Tax_group_id == Type && x.Date.Year == Year)
             .Include(x => x.Vat).Include(x => x.Tbl_vat)
             .ToList().Select(x => new Purches_tax_rpt
             {
                 Tax_doc_type =(char.IsDigit(x.Doc_type.ToString(),0))? (Tax_doc_type)x.Doc_type : x.Doc_type,
                 Address = x.Address,
                 Date = x.Date,
                 Doc_num = x.Doc_num,
                 Item_name = x.Item_name,
                 Item_quentaty = x.Quantity,
                 Item_tbl_type = x.Other_tax_type,
                 Item_type = x.Item_type,
                 Mobile_num = x.Mobile_number,
                 Net_amount = x.Net_amount - x.Discount,
                 Vendore_name =x.Vendor_name,
                 Tax_file_num = x.Tax_file_number,
                 Tax_registration_num = x.Tax_reg_num,
                 Tax_amount = x.Total_vat_amount + x.Table_vat_amount,
                 Tax_cat =Math.Round((x.Total_vat_amount+ x.Table_vat_amount) /(x.Net_amount-x.Discount),2), //(((x.Table_vat_amount * 100) + 100) * Convert.ToDecimal((x.Vat != null) ? ((x.Vat.C_Taxpercentage != null) ? x.Vat.C_Taxpercentage : 0) : 0)
                 //+ (Convert.ToDecimal((x.Tbl_vat != null) ? x.Tbl_vat.C_Taxpercentage : 0) * 100)) / 100,
                 National_id = x.National_id,
                 Total = x.Total_amount+x.Total_vat_amount - x.Discount,
                 Tax_group_id=x.Tax_group_id,
                 Tax_type = x.Tax_type
             }).ToList();
            }
            else
            {
                PurRpt = db.Taxs.Where(x => x.Tax_group_id == Type && x.Date!=null)
             .Include(x => x.Vat).Include(x => x.Tbl_vat)
             .ToList().Where(x=>x.Date.Month== Month&&x.Date.Year== Year).Select(x => new Purches_tax_rpt
             {
                 Tax_doc_type = (char.IsDigit(x.Doc_type.ToString(), 0)) ? (Tax_doc_type)x.Doc_type : x.Doc_type,
                 Address = x.Address,
                 Date = x.Date,
                 Doc_num = x.Doc_num,
                 Item_name = x.Item_name,
                 Item_quentaty = x.Quantity,
                 Item_tbl_type = x.Other_tax_type,
                 Item_type = x.Item_type,
                 Mobile_num = x.Mobile_number,
                 Net_amount = x.Net_amount-x.Discount,
                 Vendore_name = x.Vendor_name,
                 Tax_file_num = x.Tax_file_number,
                 Tax_registration_num = x.Tax_reg_num,
                 Tax_amount = x.Total_vat_amount + x.Table_vat_amount,
                 Tax_cat = Math.Round((x.Total_vat_amount + x.Table_vat_amount) / (x.Net_amount - x.Discount), 2), //(((x.Table_vat_amount * 100) + 100) * Convert.ToDecimal((x.Vat != null) ? ((x.Vat.C_Taxpercentage != null) ? x.Vat.C_Taxpercentage : 0) : 0)
                 //+ (Convert.ToDecimal((x.Tbl_vat != null) ? x.Tbl_vat.C_Taxpercentage : 0) * 100)) / 100,
                 National_id = x.National_id,
                 Total = x.Total_amount + x.Total_vat_amount - x.Discount,
                 Tax_group_id = x.Tax_group_id,
                 Tax_type=x.Tax_type
             }).ToList();
            }
            foreach (Purches_tax_rpt i in PurRpt.Where(x=>!string.IsNullOrEmpty(x.Vendore_name)))
            {
                if (i.Vendore_name.All(x => char.IsDigit(x)))
                {
                    if (i.Tax_group_id == 1)
                    {
                        int VendoreId = Convert.ToInt32(i.Vendore_name);
                        Payable.Models.Payable_creditor_setting C = DBContext.db().Payable_creditor_setting.Include(x=>x.Payable_legal_info)
                            .Include(x=>x.Payable_address_info).FirstOrDefault(x=>x.Id==VendoreId);

                        if (C != null)
                        {
                            i.Vendore_name = C.Vendor_id;
                            i.Tax_file_num = C.Payable_legal_info.FirstOrDefault().Tax_file_no;
                            i.Address = C.Payable_address_info.FirstOrDefault().Address + " "
                                + C.Payable_address_info.FirstOrDefault().Country + " "
                                + C.Payable_address_info.FirstOrDefault().City;
                            i.Tax_registration_num = C.Payable_legal_info.FirstOrDefault().Tax_Id;
                        }
                    }
                    else if (i.Tax_group_id == 2)
                    {
                        int VendoreId = Convert.ToInt32(i.Vendore_name);
                        Receivable.Models.Receivable_vendore_setting C = DBContext.db().Receivable_vendore_settings.Include(x => x.Receivable_legal_info)
                            .Include(x => x.Receivable_address_info).FirstOrDefault(x => x.Id == VendoreId);

                        if (C != null)
                        {
                            i.Vendore_name = C.Vendor_id;
                            i.Tax_file_num = C.Receivable_legal_info.FirstOrDefault().Tax_file_no;
                            i.Address = C.Receivable_address_info.FirstOrDefault().Address + " "
                                + C.Receivable_address_info.FirstOrDefault().Country + " "
                                + C.Receivable_address_info.FirstOrDefault().City;
                            i.Tax_registration_num = C.Receivable_legal_info.FirstOrDefault().Tax_Id;
                        }
                    }
                }
            }
            return PartialView(PurRpt);
        }

        public ActionResult WithHoldingTrans()
        {
            var Year = db.Taxs.Select(x => new { Year = x.Date.Year }).ToList().Distinct();

            ViewBag.Year = new SelectList(Year, "Year", "Year");

            ViewBag.Period = new SelectList(Enum.GetValues(typeof(Withhold_period)).Cast<Withhold_period>().Select(v => new SelectListItem
             {
                 Text = v.ToString(),
                 Value = ((int)v).ToString()
             }).ToList(), "Value", "Text");


            return View();
        }

        public PartialViewResult WithHoldingTransRes(Withhold_period? Period, int? Year=null)
        {
            List<Withholding_rpt> Res = new List<Withholding_rpt>();
            List<FabulousDB.DB_Tabels.Tax.Tax> Taxes = db.Taxs.Include(x => x.Vat).Include(x => x.Tbl_vat).Include(x=>x.Dacutta).ToList();
            if (Period== Withhold_period.الفترة_الاولي)
            {
                Taxes = Taxes.Where(x => (x.Date >= new DateTime(Year.Value, 1, 1) && x.Date <= new DateTime(Year.Value, 3, 31))).ToList();
            }
            else if (Period == Withhold_period.الفترة_الثانية)
            {
                Taxes = Taxes.Where(x => (x.Date >= new DateTime(Year.Value, 4, 1) && x.Date <= new DateTime(Year.Value, 6, 30))).ToList();

            }
            else if (Period == Withhold_period.الفترة_الثالثة)
            {
                Taxes = Taxes.Where(x => (x.Date >= new DateTime(Year.Value, 7, 1) && x.Date <= new DateTime(Year.Value, 9, 30))).ToList();

            }
            else if (Period == Withhold_period.الفترة_الرابعة)
            {
                Taxes = Taxes.Where(x => (x.Date >= new DateTime(Year.Value, 10, 1) && x.Date <= new DateTime(Year.Value, 12, 31))).ToList();
            }
         

            if (Year == null)
            {
                Res = Taxes
                    .Where(x=>x.Tax_group_id==1)
                      .ToList().Select(x => new Withholding_rpt
                      {
                        // Period = (Period.HasValue)? Period.Value.ToString().Replace("_"," "):"",
                          Period = (x.Date >= new DateTime(DateTime.Now.Year, 1, 1) && x.Date <= new DateTime(DateTime.Now.Year, 3, 31)) ? "الفترة الاولي" :
                                   (x.Date >= new DateTime(DateTime.Now.Year, 4, 1) && x.Date <= new DateTime(DateTime.Now.Year, 6, 30)) ? "الفترة الثانية" :
                                   (x.Date >= new DateTime(DateTime.Now.Year, 7, 1) && x.Date <= new DateTime(DateTime.Now.Year, 9, 30)) ? "الفترة الثالثة" :
                                   (x.Date >= new DateTime(DateTime.Now.Year, 10, 1) && x.Date <= new DateTime(DateTime.Now.Year, 12, 31)) ? "الفترة الرابعة" :
                                   "",
                          Year = x.Date.Year,
                          Reg_num = x.Tax_reg_num,
                          File_num = x.Tax_file_number,
                          Mission_num = "",
                          Mission_date = x.Date,
                          Mission_type = (x.Tbl_vat!=null)?x.Tbl_vat.Transaction_type.ToString():"",
                          Total =x.Total_amount_sys_curr,
                          Discount_type = "",
                          Net_total = x.Total_amount_sys_curr,
                          Discount_percentage = (x.Dacutta != null) ? ((x.Dacutta.C_Taxpercentage != null ? x.Dacutta.C_Taxpercentage : 0)) : 0,
                          Daccouta_amount = x.Dacutta_amount,
                          Vendore_name = x.Vendor_name,
                          National_id = x.National_id,
                          Address = x.Address,
                          Currency_iso = db.C_GeneralJournalEntry_Tables.Where(z => z.C_JournalEntryNumber == x.Journal_number).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { CurrenciesDefinition_Table = new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" } })
                         .FirstOrDefault().CurrenciesDefinition_Table.ISOCode,
                          Tax_group_id = x.Tax_group_id,
                          
                      }).ToList();  
            }
            else
            {
                Res = Taxes.Where(x => x.Tax_group_id == 1)
                     .ToList().Where(x => x.Date.Year == Year.Value).Select(x => new Withholding_rpt
                     {
                         // Period = (Period.HasValue)? Period.Value.ToString().Replace("_"," "):"",
                         Period = (x.Date >= new DateTime(DateTime.Now.Year, 1, 1) && x.Date <= new DateTime(DateTime.Now.Year, 3, 31)) ? "الفترة الاولي" :
                                   (x.Date >= new DateTime(DateTime.Now.Year, 4, 1) && x.Date <= new DateTime(DateTime.Now.Year, 6, 30)) ? "الفترة الثانية" :
                                   (x.Date >= new DateTime(DateTime.Now.Year, 7, 1) && x.Date <= new DateTime(DateTime.Now.Year, 9, 30)) ? "الفترة الثالثة" :
                                   (x.Date >= new DateTime(DateTime.Now.Year, 10, 1) && x.Date <= new DateTime(DateTime.Now.Year, 12, 31)) ? "الفترة الرابعة" :
                                   "",
                         Year = x.Date.Year,
                         Reg_num = x.Tax_reg_num,
                         File_num = x.Tax_file_number,
                         Mission_num = "",
                         Mission_date = x.Date,
                         Mission_type = (x.Tbl_vat != null) ? x.Tbl_vat.Transaction_type.ToString() : "",
                         Total =x.Total_amount_sys_curr,
                         Discount_type = "",
                         Net_total = x.Total_amount_sys_curr,
                         Discount_percentage = (x.Dacutta != null) ? ((x.Dacutta.C_Taxpercentage != null ? x.Dacutta.C_Taxpercentage : 0)) : 0,
                         Daccouta_amount = x.Dacutta_amount,

                         Vendore_name = x.Vendor_name,
                         National_id = x.National_id,
                         Address = x.Address,
                         Currency_iso = db.C_GeneralJournalEntry_Tables.Where(z => z.C_JournalEntryNumber == x.Journal_number).ToList().DefaultIfEmpty(new FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting.C_GeneralJournalEntry_Table { CurrenciesDefinition_Table = new FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition.CurrenciesDefinition_Table { ISOCode = "Egp" } })
                        .FirstOrDefault().CurrenciesDefinition_Table.ISOCode,
                         Tax_group_id=x.Tax_group_id
                     }).ToList();
            }
            foreach (Withholding_rpt i in Res.Where(x => !string.IsNullOrEmpty(x.Vendore_name)))
            {
                if (i.Vendore_name.All(x => char.IsDigit(x)))
                {
                    if (i.Tax_group_id == 1)
                    {
                        int VendoreId = Convert.ToInt32(i.Vendore_name);
                        Payable.Models.Payable_creditor_setting C = DBContext.db().Payable_creditor_setting.Include(x => x.Payable_legal_info)
                            .Include(x => x.Payable_address_info).FirstOrDefault(x => x.Id == VendoreId);

                        if (C != null)
                        {
                            i.Vendore_name = C.Vendor_id;
                            i.File_num = C.Payable_legal_info.FirstOrDefault().Tax_file_no;
                            i.Address = C.Payable_address_info.FirstOrDefault().Address + " "
                                + C.Payable_address_info.FirstOrDefault().Country + " "
                                + C.Payable_address_info.FirstOrDefault().City;
                            i.Reg_num = C.Payable_legal_info.FirstOrDefault().Tax_Id;
                        }
                    }
                    else if (i.Tax_group_id == 2)
                    {
                        int VendoreId = Convert.ToInt32(i.Vendore_name);
                        Receivable.Models.Receivable_vendore_setting C = DBContext.db().Receivable_vendore_settings.Include(x => x.Receivable_legal_info)
                            .Include(x => x.Receivable_address_info).FirstOrDefault(x => x.Id == VendoreId);

                        if (C != null)
                        {
                            i.Vendore_name = C.Vendor_id;
                            i.File_num = C.Receivable_legal_info.FirstOrDefault().Tax_file_no;
                            i.Address = C.Receivable_address_info.FirstOrDefault().Address + " "
                                + C.Receivable_address_info.FirstOrDefault().Country + " "
                                + C.Receivable_address_info.FirstOrDefault().City;
                            i.Reg_num = C.Receivable_legal_info.FirstOrDefault().Tax_Id;
                        }
                    }
                }
            }
            return PartialView(Res);
        }
    
    }
    public class Purches_tax_rpt
    {
        public Tax_doc_type? Tax_doc_type { get; set; }
        public Tax_type Tax_type { get; set; }
        public Other_tax_type Item_tbl_type { get; set; }
        public string Doc_num { get; set; }
        public string Vendore_name { get; set; }
        public string Tax_registration_num { get; set; }
        public string Tax_file_num { get; set; }
        public string Address { get; set; }
        public string Mobile_num { get; set; }
        public DateTime Date { get; set; }
        public string Item_name { get; set; }
        public Item_type Item_type { get; set; }
        public decimal? Tax_cat { get; set; }
        public decimal Item_quentaty { get; set; }
        public decimal Net_amount { get; set; }
        public decimal? Tax_amount { get; set; }
        public decimal Total { get; set; }
        public string National_id { get; set; }
        public int? Tax_group_id { get; set; }
    }

    public class Withholding_rpt
    {
        public string Period { get; set; }
        public int Year { get; set; }
        public string Reg_num { get; set; }
        public string File_num { get; set; }
        public string Mission_num { get; set; }
        public DateTime Mission_date { get; set; }
        public string Mission_type { get; set; }
        public decimal Total { get; set; }
        public string Discount_type { get; set; }
        public decimal Net_total { get; set; }

        public double? Discount_percentage { get; set; }
        public decimal? Daccouta_amount { get; set; }
        public string Vendore_name { get; set; }
        public string Address { get; set; }
        public string National_id { get; set; }
        public string Currency_iso { get; set; }
        public int? Tax_group_id { get; set; }
    }
    
    public enum Withhold_period
    {
        الفترة_الاولي=1,
        الفترة_الثانية=2,
        الفترة_الثالثة=3,
        الفترة_الرابعة=4
    }
}