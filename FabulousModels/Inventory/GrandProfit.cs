using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class GrandProfit
    {
        [Key]
        public int Id { get; set; }
        public string Item_id { get; set; }
        public string Item_name { get; set; }
        public float Qty_out { get; set; }
        public float RePoQty { get; set; }
        public float SalesQty { get; set; }
        public decimal Cost_of_qty_out { get; set; }
        public decimal Total_Revenue_from_sales { get; set; }
        public decimal Grand_profit { get; set; }
        public DateTime? Transaction_date { get; set; }
        public Doc_type? Doc_type { get; set; }
        public string  Doc_num { get; set; }
        public string Customer_name { get; set; }
        public int? Sales_id { get; set; }
    }
}
