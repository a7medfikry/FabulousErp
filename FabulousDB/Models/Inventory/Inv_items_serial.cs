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
    public class Inv_items_serial
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [MaxLength(500)]
        [DisplayName("Description")]
        public string Desc { get; set; }
        [DisplayName("Lot Description")]
        public string Lot_Desc { get; set; }
        public int Lot_id { get; set; }
        public int Quantity { get; set; }
        public item_serial_type Type { get; set; }
        public string Serial_number_from { get; set; }
        public string Serial_number_To { get; set; }
        [NotMapped]
        public bool Increment { get; set; }
        public Inv_item Item { get; set; }
    }
    public enum item_serial_type
    {
        Numric = 1,
        Alpha = 2,
        Symbol = 3,
        User_Date = 4

    }
}
