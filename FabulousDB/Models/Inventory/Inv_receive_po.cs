using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousErp.Payable.Models;
using FabulousErp.Receivable.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_receive_po
    {
        [Key]
        public int Id { get; set; }
        public Doc_type? Doc_type { get; set; }
        public int? Gr_num { get; set; }
        [ForeignKey("PO")]
      //  [Required]
      //  [NotMapped]
        public int? PO_id { get; set; }
        [ForeignKey("Store")]
        [Required]
        public int Store_id { get; set; }
        [ForeignKey("Site")]
        [Required]
        public int Site_id { get; set; }
        [Column(TypeName = "date")]
        [NotMapped]
        public DateTime Doc_date { get; set; } = DateTime.Now;
        public DateTime Creation_date { get; set; } = DateTime.Now;
        [ForeignKey("Vendore")]
        public int? Vendore_id { get; set; }
        [ForeignKey("Customer")]
        public int? Customer_id { get; set; }
        [NotMapped]
        public int? Batch_id { get; set; }
        [NotMapped]
        public string Buyer { get; set; }
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
        public Payable_creditor_setting Vendore { get; set; }
        public Receivable_vendore_setting Customer { get; set; }
        public Inv_store Store { get; set; }
        public Inv_store_site Site { get; set; }
        public Inv_po PO { get; set; }
        [Column(TypeName ="date")]
        public DateTime? Posting_date { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Transaction_date { get; set; }
        [ForeignKey("Payment_terms")]
        [DisplayName("Payment Term")]
        public int? Payment_terms_id { get; set; }
        [DisplayName("V.Doc Number")]
        [NotMapped]
        public string Vendore_doc_number { get; set; }
        [ForeignKey("Payable")]
        public int? Payable_id { get; set; }  
        [ForeignKey("Receivable")]
        public int? Receivable_id { get; set; }
        [NotMapped]
        [Required]
        public string Desc { get; set; }
        [NotMapped]
        public Price_lvl Price_lvl { get; set; }
        [NotMapped]
        [Required]
        [DisplayName("Due_date")]
        public DateTime Due_date { get; set; }
        [DisplayName("Shipping Method")]
        public int? Shipping_method_id { get; set; }
        public int Posting_number { get; set; }

        [NotMapped]
        public int Journal_number { get; set; }
        [NotMapped]
        public decimal Tax { get; set; }
        [NotMapped]
        public decimal Net_amount { get; set; }
        [NotMapped]
        public decimal Discount { get; set; }
        [NotMapped]
        public  string Po_inv { get; set; } 
        [NotMapped]
        public  List<InvSalesPo> Po_inv_list { get; set; }
        [NotMapped]
        public decimal? Payamnet { get; set; }
        [ForeignKey("Profit_user")]
        public string Profit_user_id { get; set; }
        public CreateAccount_Table Profit_user { get; set; }
        public Payable_transaction Payable { get; set; }
        public Receivable_transaction Receivable { get; set; }
        public Payable_payment_term Payment_terms { get; set; }
        public Payable_shipping_method Shipping_methods { get; set; }
        [NotMapped]
        public int Checkbook_id { get; set; }
        public virtual ICollection<Inv_receive_po_items> Items { get; set; }
        public virtual ICollection<Inv_sales_receivs_pos> Po_sales { get; set; }

        public virtual ICollection<Inv_po_item_transfer> Transfer { get; set; }
        public virtual ICollection<Inv_item_adjustment> Adjustment { get; set; }

        [NotMapped]
        public decimal Total { get; set; }
        [NotMapped]
        public decimal System_rate { get; set; } = 1;
        [NotMapped]
        public decimal Transaction_rate { get; set; } = 1;
        public virtual ICollection<Purchase_Installment_contract_invoice> Purchase_Installment_contract_invoice { get; set; }

        public Inv_receive_po Copy()
        {
            return (Inv_receive_po)this.MemberwiseClone();
        }
    }
}
