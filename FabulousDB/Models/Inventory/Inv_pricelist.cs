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
    public class Inv_pricelist
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [NotMapped]
        [DisplayName("Item Name")]
        public string ItemName { get; set; }
        [DisplayName("Price Level")]
        public Price_lvl Price_lvl { get; set; }
        [ForeignKey("Currency")]
        public string Currency_id { get; set; }
        [ForeignKey("UOM")]
        public int? Unit_of_measure_id { get; set; }
        [DisplayName("Start Quantity")]
        [Required]
        public float Start_quantity { get; set; }
        [DisplayName("End Quantity")]
        [Required]
        public float End_quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        public CurrenciesDefinition_Table Currency { get; set; }
        public Inv_item Item { get; set; }
        public Unit_of_measure UOM { get; set; }
    }
    public enum Price_lvl
    {
        Wholesale = 1,
        Retail = 2
    }
}
