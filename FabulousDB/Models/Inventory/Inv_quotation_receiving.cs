using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousErp.Payable.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_quotation_receiving
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Pr_no")]
        public int Pr_no_id { get; set; }
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        [ForeignKey("Qutation")]
        public int Qutation_num_id { get; set; }
        [ForeignKey("Vendore")]
        public int Vendore_id { get; set; }
        [NotMapped]
        public string Vendore_name { get; set; }
        public int Payment_term { get; set; }
        public Payable_creditor_setting Vendore { get; set; }
        public Inv_quotation_request Qutation { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
        public Inv_purchase_request Pr_no { get; set; }


    }
}
