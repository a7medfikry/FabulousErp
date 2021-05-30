using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class ItemDetails
    {
        public decimal CostPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public bool Avaliable { get; set; }
        public List<InvSalesPo> Po_inv { get; set; }
    } 
    public class PurchasemDetails
    {
        public decimal UnitPrice { get; set; }
        public decimal CostPrice { get; set; }
        public float Qty { get; set; }
        public decimal Discount { get; set; }
        public decimal Fright { get; set; }
        public int? UOMId { get; set; }
    }
}
