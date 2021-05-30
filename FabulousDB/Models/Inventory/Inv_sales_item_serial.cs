using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{ 
    public class Inv_sales_item_serial
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [ForeignKey("Sales")]
        public int Sales_id { get; set; }
        [MaxLength(200)]
        public string Serial { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Start_warranty { get; set; }
        [Column(TypeName = "date")]
        public DateTime? End_warranty { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Expiry_date { get; set; }
        public Inv_sales_invoice_items Item { get; set; }
        public Inv_sales_invoice Sales { get; set; }
    }
}