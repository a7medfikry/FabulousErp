using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class ItemStockDetails
    {
        [Key]
        public string Serial_no { get; set; }
        public DateTime? Warranty_start { get; set; }
        public DateTime? Warranty_end { get; set; }
        public List<Inv_receive_expiry> Expiery_date { get; set; }
    }
}
