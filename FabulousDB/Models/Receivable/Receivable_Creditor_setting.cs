using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Receivable_vendore_setting
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Customer Id")]
        [Column(TypeName ="nvarchar"),MaxLength(100)]
        [Required]
        public string Vendor_id { get; set; }
        [ForeignKey("Group_setting")]
        [DisplayName("Group id")]
        public int? Group_setting_id { get; set; }
        [DisplayName("Customer name")]
        [Column(TypeName = "nvarchar"),MaxLength(100)]
        [Required]
        public string Vendor_name { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        public string Alies { get; set; }
        [DisplayName("Currency Id")]
        [NotMapped]
        public List<string> Currency_id { get; set; }
        [DisplayName("Payment Terms")]
        [ForeignKey("Payment_terms")]
        public int? Payment_term_id { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [ForeignKey("Check_book")]
        [DisplayName("Default Checkbook")]
        public int? Def_Checkbook { get; set; }

        [DisplayName("Minimum Order")]
        public decimal? Minimum_order { get; set; }
        [DisplayName("Maximum Order")]
        public decimal? Maximum_order { get; set; }
        [DisplayName("Shipping Method")]
        [ForeignKey("Shippint_method")]
        public int? Shipping_method_id { get; set; }
        [DisplayName("Tax Group Id")]
        [ForeignKey("Tax")]
        public int? Tax_group_id { get; set; }
        [DefaultValue(false)]
        public bool Inactive { get; set; }
        [DefaultValue(false)]
        public bool Customer { get; set; }
        [DefaultValue(false)]
        public bool Revaluate { get; set; }
        [DisplayName("Amount")]
        public decimal? Credit_limit_amount { get; set; }
        [DisplayName("Amount")]
        public decimal? Min_payment_amount { get; set; }

        [DisplayName("Credit limit")]
        public Credit_limit_enum Credit_limit { get; set; }
        [DisplayName("Minimum Payment")]
        public Minimum_payment Minimum_payment { get; set; }
        [DisplayName("Payment per")]
        public Payment_per Payment_per { get; set; }

        public Receivable_shipping_method Shippint_method { get; set; }
        public Receivable_Group_setting Group_setting { get; set; }
        public Receivable_payment_term Payment_terms { get; set; }
        public C_CheckBookSetting_table Check_book { get; set; }
        public C_TaxSetting_table Tax { get; set; }
        public System.DateTime Creation_date { get; set; } = System.DateTime.Now;
        public virtual ICollection<Receivable_vendore_currencies> Receivable_vendore_currencies { get; set; }
        public virtual ICollection<Receivable_legal_info> Receivable_legal_info { get; set; }
        public virtual ICollection<Receivable_address_info> Receivable_address_info { get; set; }



    }
}