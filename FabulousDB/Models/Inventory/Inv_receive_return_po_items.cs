using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Inv_receive_return_po_items
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Po_items")]
        public int Po_items_id { get; set; }
        [DisplayName("QTY")]
        public float Quantity { get; set; }
        public decimal Unit_price { get; set; }
        [NotMapped]
        public decimal Total { get; set; }
        public Inv_receive_po_items Po_items { get; set; }
    }
}