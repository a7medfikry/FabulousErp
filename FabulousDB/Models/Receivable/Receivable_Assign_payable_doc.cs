using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Assign_Receivable_doc
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Assign_no { get; set; }
        [DisplayName("Vendor Id")]
        [ForeignKey("Vendor")]
        public int Vendor_id { get; set; }
        [ForeignKey("Trans_doc_type")]
        public int Trans_doc_type_id { get; set; }
        [ForeignKey("Trans_doc_type_to")]
        public int Trans_doc_type_id_to { get; set; }
        [DisplayName("Doc. type")]
        public Doc_type Doc_type { get; set; }
        [DisplayName("Doc. Num.")]
        public string Doc_Num { get; set; }

        [DisplayName("Doc. Currency")]
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        [DisplayName("Applay Date")]
        [Column(TypeName = "date")]
        
        public DateTime Applay_date { get; set; }
        public DateTime Creation_date { get; set; }
        [DisplayName("Orginal Amount")]
        public decimal Orginal_amount { get; set; }
        [DisplayName("Applay Assign")]
        public decimal Applay_assign { get; set; }
        [DisplayName("Unassign")]
        public decimal Unassign_amount { get; set; }
        [DisplayName("Taken Discount")]
        public decimal Taken_discount { get; set; }
        public decimal Earn_or_lose { get; set; }
        public int JournalEntry { get; set; }
        public decimal Transaction_rate { get; set; }
        [DefaultValue(0)]
        public bool Is_void { get; set; }
        public Receivable_vendore_setting Vendor { get; set; }
        public Receivable_transactions_types Trans_doc_type { get; set; }
        public Receivable_transactions_types Trans_doc_type_to { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }

    }
}