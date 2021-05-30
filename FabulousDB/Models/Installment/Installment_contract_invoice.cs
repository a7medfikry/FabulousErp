using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Installment_contract_invoice
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Contract")]
        public int Contract_id { get; set; }
        [ForeignKey("Sales_invoice")]
        public int Invoice_id { get; set; }  
        public Installment_contract Contract { get; set; }
        public Inv_sales_invoice Sales_invoice { get; set; }
    }
}
