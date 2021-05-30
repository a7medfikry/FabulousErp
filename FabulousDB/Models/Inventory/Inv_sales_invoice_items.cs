using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Inv_sales_invoice_items
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        [DisplayName("Item Id")]
        public int Item_id { get; set; }
        [DisplayName("Item name")]
        [NotMapped]
        public string Item_name { get; set; }
        [ForeignKey("UOM")]
        public int? UOM_id { get; set; }
        [DisplayName("QTY")]
        public float Quantity { get; set; }
        public decimal Unit_price { get; set; }
        public decimal? Cost_price { get; set; }
        public decimal Total { get; set; }
        [DisplayName("Amount Sys Currency")]
        public decimal Amount_system_currency { get; set; }
        public decimal Discount { get; set; }
        public decimal Net_amount { get; set; }
        [ForeignKey("Table_vat")]
        public int? Table_vat_id { get; set; }
        public decimal? Table_vat_amount { get; set; }
        [DisplayName("Total After Vat Table")]
        [NotMapped]
        public decimal? Total_after_vat_table { get; set; }
        [NotMapped]
        public int? Po_id { get; set; }

        [ForeignKey("Vat")]
        public int? Vat_id { get; set; }
        public decimal? Vat_amount { get; set; }

        [ForeignKey("Deduct")]
        public int? Deduct_id { get; set; }
        public decimal? Deduct_amount { get; set; }
        public decimal Fright { get; set; }
        [ForeignKey("Sales_invoice")]
        public int? Sales_invoice_id { get; set; }
        public string Custom_name { get; set; }
        public C_TaxSetting_table Table_vat { get; set; }
        public C_TaxSetting_table Deduct { get; set; }
        public C_TaxSetting_table Vat { get; set; }
        public Unit_of_measure UOM { get; set; }
        public Inv_item Item { get; set; }
        public Inv_sales_invoice Sales_invoice { get; set; }
        public virtual ICollection<Inv_sales_item_serial> Serials { get; set; }
    }
}