using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class TransferItems
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [ForeignKey("Store")]
        public int Old_item_store { get; set; }
        [ForeignKey("Site")]
        public int Old_item_site { get; set; }
        [ForeignKey("Store")]
        public int New_item_store { get; set; }
        [ForeignKey("Site")]
        public int New_item_site { get; set; }
        public float Transfer_qty { get; set; }
        public float Orginal_Qty { get; set; }
        public bool Site_transfer { get; set; }
        public Inv_item Item { get; set; }
        public Inv_store Store { get; set; }
        public Inv_store_site Site { get; set; }
    }
}
