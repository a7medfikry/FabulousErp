using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_receive_expiry
    {
        [Key]
        public int Id { get; set; }
        public float Number { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Item_serial")]
        public int Serial_id { get; set; }
        public Inv_receive_item_serial Item_serial { get; set; }
    }
}
