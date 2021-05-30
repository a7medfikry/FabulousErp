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
   public  class Inv_po
    {
        [Key]
        public int Id { get; set; }
        public int Po_num { get; set; }
        public int? Po_type { get; set; }
        [ForeignKey("Pr_no")]
        public int? Pr_no_id { get; set; }
        [Column(TypeName ="date")]
        public DateTime Date { get; set; } = DateTime.Now;
        [ForeignKey("Vendore")]
        [DisplayName("Vendore id")]
        public int Vendore_id { get; set; }
        [NotMapped]
        [DisplayName("Vendore name")]
        public string Vendore_name { get; set; }
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        [DisplayName("System Rate")]
        public decimal System_rate { get; set; }
        [DisplayName("Transaction rate")]
        public decimal Transaction_rate { get; set; }
        public decimal Difference { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
        public Payable_creditor_setting Vendore { get; set; }
        public Inv_purchase_request Pr_no { get; set; }
        public virtual ICollection<Inv_po_items> Items { get; set; }
    }
}
