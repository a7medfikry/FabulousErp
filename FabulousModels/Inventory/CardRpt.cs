using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class CardRpt
    {
        [Key]
        public int Id { get; set; }
        public int Action_num { get; set; }
        public float Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Total { get; set; }
        public decimal Fright { get; set; }
        public decimal Discount { get; set; }
        public float Avaliable { get; set; }
        public GRGO Type { get; set; }
        public DateTime Action_date { get; set; }
        public int Store_id { get; set; }
        public string Store { get; set; }
        public int? Site_id { get; set; }
        public int? Po_receive_id { get; set; }
        public int? Sales_id { get; set; }
        public bool Remove { get; set; } = false;
        public bool TakeUnit { get; set; } = false;
        public string ItemId { get; set; }
        public string UOM { get; set; }
        public string Doc_date { get; set; }
        public DateTime Creation_date { get; set; }
    }
    public enum GRGO
    {
        GR=1,
        GO=2,
        RGO=3,
        Transfer_in=4,
        Transfer_out=5,  
        Adjustment_in=6,
        Adjustment_out = 7,
    }
}
