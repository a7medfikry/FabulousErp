using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Installments
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Contract")]
        public int Contract_id { get; set; }
        [MaxLength(500)]
        public string Cheque_number { get; set; }
        [MaxLength(500)]
        public string Refrence { get; set; }
        public decimal Amount { get; set; }
        public float Percentage { get; set; }
        public DateTime? Cheque_Date { get; set; }
        public bool Paid { get; set; } = false;
        public bool Historical { get; set; } = false;
        [ForeignKey("Check_book_trx")]
        public int? Check_book_trx_id { get; set; }
        public Installment_contract Contract { get; set; }
        public C_CheckbookTransactions_table Check_book_trx { get; set; }
    }
}
