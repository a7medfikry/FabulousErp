using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Receivable_vendore_currencies
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Vendore")]
        [ForeignKey("Vendore")]
        public int Vendore_id { get; set; }

        [DisplayName("Currency Id")]
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        public Receivable_vendore_setting Vendore { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }

    }
}