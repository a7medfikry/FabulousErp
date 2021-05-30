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
    public class Inv_po_items
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
        public int UOM_id { get; set; }
        [DisplayName("QTY")]
        public float Quantity { get; set; }
        [DisplayName("Unit Price")]

        public decimal Unit_price { get; set; }
        public decimal Total { get; set; }
        [DisplayName("Amount Sys Currency")]
        public decimal Amount_system_currency { get; set; }
        public decimal Discount { get; set; }
        public decimal Net_amount { get; set; }
        [ForeignKey("Table_vat")]
        [DisplayName("Table Vat")]
        public int Table_vat_id { get; set; }
        [DisplayName("Table Vat Amount")]
        public decimal Table_vat_amount { get; set; }
        [DisplayName("Total After Vat Table")]
        [NotMapped]
        public decimal Total_after_vat_table { get; set; }

        [ForeignKey("Vat")]
        public int Vat_id { get; set; }
        [DisplayName("Vat Amount")]
        public decimal Vat_amount { get; set; }

        [ForeignKey("Deduct")]
        [DisplayName("Deduct Id")]

        public int Deduct_id { get; set; }
        [DisplayName("Deduct amount")]

        public decimal Deduct_amount { get; set; }
       [ForeignKey("Inv_po")]
       [DisplayName("PO Id")]
        public int Po_id { get; set; }
        public C_TaxSetting_table Table_vat { get; set; }
        public C_TaxSetting_table Deduct { get; set; }
        public C_TaxSetting_table Vat { get; set; }
        public Unit_of_measure UOM { get; set; }
        public Inv_item Item { get; set; }
        public Inv_po Inv_po { get; set; }
    }
}
