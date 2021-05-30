using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Inv_item_deduct_tax
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Deduct")]
        public int Deduct_id { get; set; }
        [ForeignKey("item")]
        public int item_id { get; set; }
        public C_TaxSetting_table Deduct { get; set; }
        public Inv_item item { get; set; }

    }
}