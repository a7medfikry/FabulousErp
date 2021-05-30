using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Tax
{
    public class Tax
    {
        [Key]
        public int Id { get; set; }
        public Tax_doc_type? Doc_type { get; set; }
        [DisplayName("Doc. Num.")]
        public string Doc_num { get; set; }
        public DateTime Date { get; set; }
        [DisplayName("Vendor Name")]
        public string Vendor_name { get; set; }
        [DisplayName("Tax Register Number")]
        public string Tax_reg_num { get; set; }
        [DisplayName("Tax File Number")]
        public string Tax_file_number { get; set; }
        public string Address { get; set; }
        [DisplayName("National ID")]
        public string National_id { get; set; }
        [DisplayName("Mobile Number")]
        public string Mobile_number { get; set; }
        public Tax_type Tax_type { get; set; }
        public Other_tax_type Other_tax_type { get; set; }
        public string Item_name { get; set; }
        public string Item_code { get; set; }
        public Item_type Item_type { get; set; }
        [ForeignKey("Unit_of_measure")]
        [DefaultValue(0)]
        public int? Unit_of_measure_id { get; set; }
        [DefaultValue(0)]

        public decimal Quantity { get; set; }
        [DisplayName("Unit Price")]
        [DefaultValue(0)]

        public decimal Unit_price { get; set; }
        [DisplayName("Total Amount")]
        [DefaultValue(0)]
        public decimal Total_amount { get; set; }
        [DisplayName("Total Amount System Currency")]
        [DefaultValue(0)]
        public decimal Total_amount_sys_curr { get; set; }
        [DisplayName("Discount")]
        [DefaultValue(0)]
        public decimal Discount { get; set; }
        [DisplayName("Net. Amount")]
        [DefaultValue(0)]
        public decimal Net_amount { get; set; }
        [DisplayName("Tbl. Vat ID")]
        [ForeignKey("Tbl_vat")]
        [DefaultValue(0)]
        public int? Tbl_vat_id { get; set; }

        [DefaultValue(0)]
        public decimal Table_vat_amount { get; set; }
        [DefaultValue(0)]
        public decimal Table_after_vat_amount { get; set; }
        [DisplayName("Vat ID")]
        [ForeignKey("Vat")]
        public int? Vat_id { get; set; }
        public decimal? Vat_amount { get; set; }
        [ForeignKey("Dacutta")]
        public int? Dacutta_id { get; set; }
        public decimal Dacutta_amount { get; set; }
        [ForeignKey("Tax_group")]
        public int? Tax_group_id { get; set; }
        public int Journal_number { get; set; }
        public decimal Total_vat_amount { get; set; }
        public decimal Final_cost_price { get; set; }
        public string Page_key { get; set; }
        public Statment_type? Statment_type { get; set; }
        public Unit_of_measure Unit_of_measure { get; set; }
        public C_TaxSetting_table Tbl_vat { get; set; }
        public C_TaxSetting_table Vat { get; set; }
        public C_TaxSetting_table Dacutta { get; set; }
        public C_TaxSetting_table Tax_group { get; set; }
    }
    public enum Statment_type
    {
        Local=1,
        Imported=2,
        Reconcile=5
    }
    public enum Tax_doc_type
    {
        Invoice = 1,
        [Display(Name = "Credit Memo")]
        Credit_Memo = 2,
        [Display(Name = "Debit Memo")]
        Debit_Memo = 3,
        Custom_clearnce = 4,
        Return = 5
    }
    public enum Tax_type
    {
        General = 1,
        [Display(Name = "Tax Table")]
        Tax_Table = 2
    }
    public enum Other_tax_type
    {
        None = 0,
        [Display(Name = "Table First")]
        Table_first = 1,
        [Display(Name = "Table Second")]
        Table_second = 2,
    }
    public enum Item_type
    {
        Material = 3,
        Service = 4,
        Equipment = 5,
        Parts_of_Equipment = 6,
        Exemptions = 7
    }

}
