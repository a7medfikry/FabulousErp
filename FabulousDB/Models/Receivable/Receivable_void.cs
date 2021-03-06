using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Receivable_void
    {
        [Key]
        public int Id { get; set; }
        public DateTime Transaction_date { get; set; }
        public DateTime Posting_date { get; set; }
        [ForeignKey("Trx")]
        public int? Trx_id { get; set; }
        [ForeignKey("Payment")]
        public Receivable_transactions_types Trx { get; set; }
    }
}