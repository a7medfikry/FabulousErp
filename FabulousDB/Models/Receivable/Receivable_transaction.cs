using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FabulousDB.Models;
namespace FabulousErp.Receivable.Models
{
    public class Receivable_transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("Trans_doc_type")]
        public int Trans_doc_type_id { get; set; }
        [DisplayName("Document Type")]
        [Required]
        public Doc_type Doc_type { get; set; }
        [DisplayName("Description")]
        [Required]
        public string Desc { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Transaction Date")]
        public DateTime Transaction_date { get; set; }
        [DisplayName("Posting Date")]
        [Column(TypeName = "date")]
        public DateTime Posting_date { get; set; }
        [DisplayName("System Rate")]
        public decimal System_rate { get; set; } = 1;
        [DisplayName("Transaction Rate")]
        public decimal Transaction_rate { get; set; } = 1;
        [DisplayName("Currency Id")]
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        [ForeignKey("Vendor")]
        public int? Vendor_id { get; set; }
        [DisplayName("P.O Number")]
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        public string PONumber { get; set; }
        [DisplayName("V.Doc Number")]
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        public string VDocument_number { get; set; }
        [DisplayName("Document Date")]
        [Column(TypeName = "date")]
        public DateTime Doc_date { get; set; }
        [DisplayName("Due Date")]
        [Column(TypeName = "date")]
        public DateTime? Due_date { get; set; }
        public DateTime Creation_date { get; set; }
        [ForeignKey("Payment_terms")]
        [DisplayName("Payment Term")]
        public int? Payment_terms_id { get; set; }
        [DisplayName("Shipping Method")]
        [ForeignKey("Shipping_method")]
        public int? Shipping_method_id { get; set; }
        [DisplayName("Sales")]
        public decimal Purchase { get; set; }
        [DisplayName("Discount")]
        public decimal Taken_discount { get; set; }
        public decimal Tax { get; set; }
        public int Journal_number { get; set; }

        [NotMapped]
        public int Aging_period { get; set; }
        [DefaultValue(0)]
        public bool Is_void { get; set; }
        public Receivable_transactions_types Trans_doc_type { get; set; }
        public Receivable_vendore_setting Vendor { get; set; }
        public Receivable_payment_term Payment_terms { get; set; }
        public Receivable_shipping_method Shipping_method { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
        [NotMapped]
        public virtual Receivable_void Receivable_void { get; set; }
        public virtual ICollection<Inv_sales_invoice> Sales_invoice { get; set; }
    }
}
