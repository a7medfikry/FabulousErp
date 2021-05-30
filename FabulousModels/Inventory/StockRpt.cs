using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class StockRpt
    {
        public int Id { get; set; }
        [Key]
        public string Item_id { get; set; }
        public string Item_name { get; set; }
        public string UOM { get; set; }
        public float Avaliable { get; set; }
        public decimal Unit_cost { get; set; }
        public decimal Amount { get; set; }
        public int? Po { get; set; }
    }
}
