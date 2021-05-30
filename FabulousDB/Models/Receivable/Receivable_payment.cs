using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousErp.Receivable.Models
{
    public class Receivable_payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Payment No")]
        public int? Payment_no { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Transaction Date")]
        public DateTime Transaction_date { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Posting Date")]
        public DateTime Posting_date { get; set; }
        [DisplayName("Vendor Id")]
        [ForeignKey("Vendor")]
        public int Vendor_id { get; set; }
        [ForeignKey("Tranaction")]
        public int? Transaction_id { get; set; }
        [ForeignKey("Tranaction_p")]
        public int? Transaction_p_id { get; set; }
        [Column("Reference"), MaxLength(500)]
        [Required]
        public string Reference { get; set; }
        [ForeignKey("CheckBook_setting")]
        [DisplayName("Check Book")]
        public int Check_book_id { get; set; }
        [ForeignKey("CheckBook_transaction")]
        [DisplayName("Check Book Transaction")]
        public int? Check_book_transaction_id { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        [DisplayName("Withdraw Num")]
        public string Withdraw_number { get; set; }
        [DisplayName("Currency Id")]
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        [DisplayName("Cheque Number")]
        public string Cheque_number { get; set; }
        [Column(TypeName = "date")]
        [DisplayName("Due Date")]
        public DateTime? Due_date { get; set; }
        public DateTime Creation_date { get; set; } = DateTime.Now;
        [DisplayName("System Rate")]
        public decimal System_rate { get; set; } = 1;
        [DisplayName("Transaction Rate")]
        public decimal Transaction_rate { get; set; } = 1;
        [DisplayName("Orginal Amount")]
        public decimal Orginal_amount { get; set; }
        [DisplayName("Taken Discount")]
        [NotMapped]
        public decimal Taken_discount { get; set; }
        //[DisplayName("Paid Amount")]
        //public decimal Paid_amount { get; set; }
        [DisplayName("Cash Type")]
        public int Journal_number { get; set; }
        [ForeignKey("Trans_doc_type")]
        public int Trans_doc_type_id { get; set; }
        public int? Installment_id { get; set; }
        [DefaultValue(0)]
        public bool Is_void { get; set; } = false;
        [ForeignKey("Trans_doc_type_id")]
        public Receivable_transactions_types Trans_doc_type { get; set; }
        public Cash_type Cash_type { get; set; }
        [NotMapped]
        public bool IsInstallment { get; set; } = false;
        [ForeignKey("Transaction_p_id")]
        public Receivable_transaction Tranaction_p { get; set; }
        [ForeignKey("Transaction_id")]
        public Receivable_transaction Tranaction { get; set; }
        public Receivable_vendore_setting Vendor { get; set; }
        public C_CheckBookSetting_table CheckBook_setting { get; set; }
        public C_CheckbookTransactions_table CheckBook_transaction { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; } 
        [NotMapped]
        public Installments Installment { get;set;}
        [NotMapped]
        public virtual Receivable_void Receivable_void { get; set; }
        [NotMapped]
        public string Profitable_user { get; set; }
        [NotMapped]
        public int? Trx_trans_doc_type_id { get; set; }
        [NotMapped]
        public int PostingNumber { get; set; }
        [NotMapped]
        public string UserId { get; set; }
        [NotMapped]
        public string Payment_To_Recieved_From { get; set; }
    }
    public enum Payment_type_enum
    {
        Any = 1,
        Transaction = 2
    }
   
}
