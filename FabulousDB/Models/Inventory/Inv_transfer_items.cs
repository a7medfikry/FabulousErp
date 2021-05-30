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
    public class Inv_transfer_items
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500)]
        [DisplayName("Transfer No")]
        public string Transfer_no { get; set; }
        public DateTime Transfer_date { get; set; }
        public DateTime Posting_date { get; set; }
        [DisplayName("From Store ID")]
        [ForeignKey("From_Store")]
        public int From_store_id { get; set; }
        public Inv_store Store { get; set; }
        [DisplayName("To Store ID")]
        [ForeignKey("To_Store")]
        public int To_store_id { get; set; }
        [DisplayName("Reference")]
        [MaxLength(500)]
        public string Ref { get; set; }
        [ForeignKey("item")]
        public int item_id { get; set; }
        public int Quantity { get; set; }
        public decimal Unite_cost { get; set; }
        [ForeignKey("Lot")]
        public int Lot_id { get; set; }
        [NotMapped]
        public string Serial_number_from { get; set; }
        [NotMapped]
        public string Serial_number_To { get; set; }
        public Inv_items_serial Lot { get; set; }
        public Inv_item item { get; set; }
        public Inv_store From_Store { get; set; }
        public Inv_store To_Store { get; set; }
    }
}
