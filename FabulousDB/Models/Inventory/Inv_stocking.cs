using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_stocking
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [ForeignKey("UOM")]
        public int UOM_id { get; set; }
        [ForeignKey("Site")]
        public int Site_id { get; set; }    
        [NotMapped]
        public int Store_id { get; set; }
        public float Exist { get; set; }
        public float Adjust { get; set; }
        public float Diffrance { get; set; }
        public float Orginal_qty { get; set; }
        [Column(TypeName ="date")]
        public DateTime Transaction_date { get; set; }
        public Inv_item Item { get; set; }
        public Unit_of_measure UOM { get; set; }
        [NotMapped]
        public Inv_store Store { get; set; }  
        public Inv_store_site Site { get; set; }
    }
}
