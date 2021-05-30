using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_transactions_types
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Trx_num { get; set; }
        [Column("Document type")]
        public Doc_type Doc_type { get; set; }
        public int Counter { get; set; }
        public TrxPay Origin { get; set; }
        public virtual ICollection<Payable_transaction> Payable_transaction { get; set; }
        public virtual ICollection<Payable_payment> Payable_payment { get; set; }
    }
 
}
