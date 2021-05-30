using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Inv_old_receive_item_serial
    {
        public int Id { get; set; }
        [ForeignKey("Serial_item")]
        public int Serial_item_id { get; set; } 
        [ForeignKey("Po")]
        public int Old_po_id { get; set; }
        public Inv_receive_po Po { get; set; }
        public DateTime Transfer_date { get; set; } = DateTime.Now;
        public Inv_receive_item_serial Serial_item { get; set; }
    }
}