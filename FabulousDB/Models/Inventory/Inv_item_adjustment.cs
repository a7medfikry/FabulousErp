using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
  
    public class Inv_item_adjustment
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [NotMapped]
        public int Store_id { get; set; }
        [NotMapped]
        public int Site_id { get; set; }
        public float Damage_qty { get; set; }
        public float Loss_qty { get; set; }
        public float Earn_qty { get; set; } 
        public decimal Damage_amount { get; set; }
        public decimal Loss_amount { get; set; }
        public decimal Earn_amount { get; set; }
        public string Desc { get; set; }
        [NotMapped]
        public DateTime Transaction_date { get; set; }
        [NotMapped]
        public DateTime Posting_date { get; set; }
        public DateTime Adjustment_date { get; set; }
        [ForeignKey("Po")]
        public int Po_id { get; set; }
        public int Adjustment_num { get; set; }
        [ForeignKey("UOM")]
        public int UOM_id { get; set; }
        [NotMapped]
        public decimal Unit_price { get; set; }
        [NotMapped]
        public decimal Total { get; set; }
        public Inv_item Item { get; set; }
        public int Posting_num { get; set; }
        //public Inv_store Store { get; set; }
        //public Inv_store_site Site { get; set; }
        public Inv_receive_po Po { get; set; }
        public Unit_of_measure UOM { get; set; }
        public virtual ICollection<Inv_adjustment_relation> Relations { get; set; }
    }
}
