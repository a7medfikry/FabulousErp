//using FabulousDB.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Related_rec_trans
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Transaction")]
        public int Transaction_id { get; set; }

        [ForeignKey("Payment")]
        public int Payment_id { get; set; }
        public Receivable_transactions_types Transaction { get; set; }
        public Receivable_transactions_types Payment { get; set; }
    }
}