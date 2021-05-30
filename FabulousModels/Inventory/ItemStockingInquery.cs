using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class ItemStockingInquery
    {
        public string Item_id { get; set; }
        public string Item_name { get; set; }
        public string UOM { get; set; }
        public float Avaliable { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime? SalesDate { get; set; }
        public DateTime? Pur_ws { get; set; }
        public DateTime? Pur_we { get; set; }
        public DateTime? Sales_ws { get; set; }
        public DateTime? Sales_we { get; set; }
    }
}
