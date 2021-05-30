using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class QtyAvaliable
    {
        [Key]
        public int Id { get; set; }
        public string Item_id { get; set; }
        public string Item_name { get; set; }
        public string UOM { get; set; }
        public float Total_qty_in { get; set; }
        public float Total_qty_out { get; set; }
        public float Qty_avaliable { get; set; }
    }
}
