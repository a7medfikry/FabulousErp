using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousErp.Receivable.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models.Inventory
{
    public class Inv_receivable_num
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Check_book")]
        public int Checkbook_id { get; set; }
        [ForeignKey("Sales")]
        public int Inv_sales_id { get; set; }
        [ForeignKey("Rec_inv")]
        public int Inv_num_id { get; set; }
        public C_CheckbookTransactions_table Check_book { get; set; }
        public Receivable_transactions_types Rec_inv { get; set; }
        public Inv_sales_invoice Sales { get; set; }
    }
}
