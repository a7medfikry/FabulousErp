﻿using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_Assign_void
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [DefaultValue("1")]
        public DateTime? Creation_date { get; set; }
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
        public Payable_creditor_setting Vendor { get; set; }
        public Payable_transactions_types Trans_doc_type { get; set; }
        public Payable_transactions_types Trans_doc_type_to { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
    }
}