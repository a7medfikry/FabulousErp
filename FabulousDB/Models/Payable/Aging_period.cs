using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_aging_period
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(100)]
        public string Name { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        [NotMapped]
        public decimal Amount { get; set; }
        [NotMapped]
        public int NumberOfTransactions { get; set; }
        [NotMapped]
        public int NumberOfPayments { get; set; }
        [NotMapped]
        public decimal ThisTransactionAmount { get; set; }
        [NotMapped]
        public decimal ThisPaymentAmount { get; set; }
        [NotMapped]
        public DateTime ActionDate { get; set; }
       
    }
}