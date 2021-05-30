using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
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
    public class Installment_contract
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        [DisplayName("Contract no")]
        public string Contract_no { get; set; }
        [Display(Name ="Description")]
        [MaxLength(500)]
        public string Desc { get; set; }
        [ForeignKey("Vendore")]
        [DisplayName("Vendore")]
        public int? Vendore_id { get; set; }
        [DisplayName("Customer")]
        [ForeignKey("Customer")]
        public int? Customer_id { get; set; }
        [NotMapped]
        public string Customer_name { get; set; }
        public decimal Amount { get; set; }
        [ForeignKey("Currency")]
        [DisplayName("Currency")]
        public string Currency_id { get; set; }
        [ForeignKey("Installment_plan")]
        [DisplayName("Installment plan")]
        public int Installment_plan_id { get; set; }
        [DisplayName("Contract date")]
        public DateTime Contract_date { get; set; }
        [DisplayName("Creation date")]
        public DateTime Creation_date { get; set; } = DateTime.Now;
        public bool IsPay { get; set; } = true;
        public Pay_for Pay_for { get; set; }
        public FabulousErp.Payable.Models.Payable_creditor_setting Vendore { get; set; }
        public FabulousErp.Receivable.Models.Receivable_vendore_setting Customer { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
        public Installment_setting Installment_plan { get; set; }
    }
    public enum Pay_for
    {
        Manual = 1,
        Invoice=2,
        Balance=3
    }
}
