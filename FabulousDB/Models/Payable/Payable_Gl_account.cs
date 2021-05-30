using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_gl_account
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Account Payable")]
        [ForeignKey("Account_payable")]
        public int Account_payable_id { get; set; }
        [ForeignKey("Purchase")]
        [DisplayName("Purchase")]
        public int? Purchase_id { get; set; }
        [DisplayName("Taken Discount")]
        [ForeignKey("Taken_discount")]

        public int? Taken_discount_id { get; set; }
        [DisplayName("Purchase Variance")]
        [ForeignKey("Purchase_variance")]

        public int? Purchase_variance_id { get; set; }
        [DisplayName("Accrued Purchase")]
        [ForeignKey("Accrued_purchase")]

        public int? Accrued_purchase_id { get; set; }
        [ForeignKey("Returne")]
        [DisplayName("Returne")]
        public int? Returne_id { get; set; }
        [ForeignKey("Payable_Group_setting")]
        public int? Payable_Group_setting_id { get; set; }
        [ForeignKey("Creditor_settings")]
        public int? Creditor_setting_id { get; set; }
        public Payable_Group_setting Payable_Group_setting { get; set; }
        public Payable_creditor_setting Creditor_settings { get; set; }
        public C_CreateAccount_Table Account_payable { get; set; }
        public C_CreateAccount_Table Purchase { get; set; }
        public C_CreateAccount_Table Taken_discount { get; set; }
        public C_CreateAccount_Table Purchase_variance { get; set; }
        public C_CreateAccount_Table Accrued_purchase { get; set; }
        public C_CreateAccount_Table Returne { get; set; }

    }
}