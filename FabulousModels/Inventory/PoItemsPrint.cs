using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class PoItemsPrint
    {
        public string Item_id { get; set; }
        public string Item_name { get; set; }
        public string UOM { get; set; }
        public float Qty { get; set; }
        public decimal Unit_price { get; set; }
        public decimal Discount { get; set; }
        public decimal Fright { get; set; }
        public decimal? Tax { get; set; }
        public DateTime? Date { get; set; } = new DateTime(1, 1, 1);
    }
    public class ItemSerial
    {
        public string Item_id { get; set; }
        public string Serial { get; set; }
        public DateTime? Warranty_start { get; set; }
        public DateTime? Warranty_end { get; set; }
        public List<Inv_receive_expiry> Expiery_date { get; set; }
        public bool Has_warranty { get; set; } = false;
        public bool Has_expiery { get; set; } = false;
    }
}
