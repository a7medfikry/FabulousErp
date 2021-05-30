using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
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
    public class Inv_receive_po_items
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
        [NotMapped]
        public decimal? Cost_price { get; set; }
        [NotMapped]
        public decimal Total { get {return Unit_price* (decimal)Quantity; } set { value = Unit_price * (decimal)Quantity; } }
        [DisplayName("Amount Sys Currency")]
        [NotMapped]
        public decimal Amount_system_currency { get; set; }
        public decimal Discount { get; set; }
        public decimal Fright { get; set; }
        public decimal Net_amount { get; set; }
        [NotMapped]
       // [ForeignKey("Table_vat")]
        public int? Table_vat_id { get; set; }
        public decimal? Table_vat_amount { get; set; }
        [DisplayName("Total After Vat Table")]
        [NotMapped]
        public decimal? Total_after_vat_table { get; set; }

        //[ForeignKey("Vat")]
        [NotMapped]

        public int? Vat_id { get; set; }
        public decimal? Vat_amount { get; set; }

        //[ForeignKey("Deduct")]
        [NotMapped]
        public int? Deduct_id { get; set; }
        public decimal? Deduct_amount { get; set; }
        [ForeignKey("Receive_po")]
        public int? Receive_po_id { get; set; }
        [ForeignKey("Site")]
        public int? Site_id { get; set; }
        //public C_TaxSetting_table Table_vat { get; set; }
        //public C_TaxSetting_table Deduct { get; set; }
        //public C_TaxSetting_table Vat { get; set; }
        public Unit_of_measure UOM { get; set; }
        public Inv_item Item { get; set; }
        public Inv_receive_po Receive_po { get; set; }
        public Inv_store_site Site { get; set; }
        public virtual ICollection<Inv_receive_item_serial> Item_serial { get; set; }

        public Inv_receive_po_items Copy()
        {
            return (Inv_receive_po_items)this.MemberwiseClone();
        }
    }
}
