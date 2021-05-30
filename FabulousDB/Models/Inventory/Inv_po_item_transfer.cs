using FabulousDB.Models;
using FabulousDB.Models.Inventory;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_po_item_transfer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [ForeignKey("From_store")]
        public int From_store_id { get; set; }
        [ForeignKey("From_site")]
        public int From_site_id { get; set; }
        [ForeignKey("To_store")]
        public int To_store_id { get; set; }
        [ForeignKey("To_site")]
        public int To_site_id { get; set; }
        public float Transfer_qty { get; set; }
        public float Orginal_Qty { get; set; }
        public string Desc { get; set; } 
        [NotMapped]
        public DateTime Transaction_date { get; set; }
        [NotMapped]
        public DateTime Posting_date { get; set; }
        public DateTime Transfer_date { get; set; }
        public bool Site_transfer { get; set; }
        [ForeignKey("Po")]
        public int Po_id { get; set; }
        [NotMapped]
        public int Main_po { get; set; }
        [ForeignKey("Sales")]
        public int? Sales_id { get; set; }
        public int Transfer_num { get; set; }
        [ForeignKey("UOM")]
        public int UOM_id { get; set; }
        [NotMapped]
        public decimal Unit_price { get; set; } 
        [NotMapped]
        public decimal Total { get; set; }
        public Inv_item Item { get; set; }
        public Inv_store From_store { get; set; }
        public Inv_store To_store { get; set; }
        public Inv_store_site From_site { get; set; }
        public Inv_store_site To_site { get; set; }
        public Inv_receive_po Po { get; set; }
        public Inv_sales_invoice Sales { get; set; }
        public Unit_of_measure UOM { get; set; }
        public virtual ICollection<Inv_transfer_relation> Relations { get; set; }
    }
}
