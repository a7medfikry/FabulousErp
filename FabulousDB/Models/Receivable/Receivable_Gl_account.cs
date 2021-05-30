using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Receivable_gl_account
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Account Receivable")]
        [ForeignKey("Account_Receivable")]
        public int Account_Receivable_id { get; set; }
        [ForeignKey("Sales")]
        [DisplayName("Sales")]
        public int? Sales_id { get; set; }
        [DisplayName("Discount")]
        [ForeignKey("Discount")]
        public int? Discount_id { get; set; }
        [DisplayName("Sales Variance")]
        [ForeignKey("Sales_variance")]
        public int? Sales_variance_id { get; set; }
        [DisplayName("Accrued Sales")]
        [ForeignKey("Accrued_sales")]
        public int? Accrued_sales_id { get; set; }
        [ForeignKey("Returne")]
        [DisplayName("Returne")]
        public int? Returne_id { get; set; }
        [ForeignKey("Receivable_Group_setting")]
        public int? Receivable_Group_setting_id { get; set; }
        [ForeignKey("Creditor_settings")]
        [DisplayName("Customer Settings")]
        public int? Creditor_setting_id { get; set; }
        public Receivable_Group_setting Receivable_Group_setting { get; set; }
        public Receivable_vendore_setting Creditor_settings { get; set; }
        public C_CreateAccount_Table Account_Receivable { get; set; }
        public C_CreateAccount_Table Sales { get; set; }
        public C_CreateAccount_Table Discount { get; set; }
        public C_CreateAccount_Table Sales_variance { get; set; }
        public C_CreateAccount_Table Accrued_sales { get; set; }
        public C_CreateAccount_Table Returne { get; set; }

    }
}