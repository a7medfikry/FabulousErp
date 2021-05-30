using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Inv_purchase_request_items
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("item")]
        public int item_id { get; set; }
        public double Quntaty { get; set; }
        public Inv_item item { get; set; }
        [ForeignKey("Purchase_request")]
        public int Purchase_request_id { get; set; } 
        public Inv_purchase_request Purchase_request { get; set; } 
    }
}