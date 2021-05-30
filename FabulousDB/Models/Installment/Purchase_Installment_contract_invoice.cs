using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public  class Purchase_Installment_contract_invoice
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Contract")]
        public int Contract_id { get; set; }
        [ForeignKey("Purchase_invoice")]
        public int Invoice_id { get; set; }
        public Installment_contract Contract { get; set; }
        public Inv_receive_po Purchase_invoice { get; set; }
    }
}
