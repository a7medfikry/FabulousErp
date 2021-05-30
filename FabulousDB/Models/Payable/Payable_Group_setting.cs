using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_Group_setting
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Group Id")]
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        [Required]
        public string Group_id { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(200)]
        [DisplayName("Group Description")]
        public string Group_description { get; set; }
        [DisplayName("Currency Id")]
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        [ForeignKey("Payment_term")]
        [DisplayName("Payment Terms")]
        public int? Payment_terms { get; set; }
        [ForeignKey("Check_book")]
        [DisplayName("Default Checkbook")]
        public int? Def_Checkbook { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [DisplayName("Minimum Transaction")]
        public decimal? Minimum_transaction { get; set; }
        [DisplayName("Maximum Transaction")]
        public decimal? Maximum_transaction { get; set; }

        [DisplayName("Shipping M.")]
        [ForeignKey("Shipping_method")]
        public int? Shipping_method_id { get; set; }
        [DisplayName("Tax Group Id")]
        [ForeignKey("Tax")]
        public int? Tax_group_id { get; set; }
        [DefaultValue(false)]
        public bool Inactive { get; set; }
        [DisplayName("Credit limit")]
        public Credit_limit_enum? Credit_limit { get; set; }
        [DisplayName("Minimum Payment")]
        public Minimum_payment? Minimum_payment { get; set; }
        [DisplayName("Payment per")]
        public Payment_per? Payment_per { get; set; }
        [DefaultValue(false)]
        public bool? Revaluate { get; set; }
        [DisplayName("Amount")]
        public decimal? Credit_limit_amount { get; set; }
        [DisplayName("Amount")]
        public decimal? Min_payment_amount { get; set; }

        public Payable_shipping_method Shipping_method { get; set; }
        public C_TaxSetting_table Tax { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
        public Payable_payment_term Payment_term { get; set; }
        public C_CheckBookSetting_table Check_book { get; set; }

    }


}