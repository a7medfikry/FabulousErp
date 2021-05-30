using FabulousDB.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Inv_item_store_site
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [ForeignKey("Store")]
        public int? Store_id { get; set; }
        [ForeignKey("Site")]
        public int? Site_id { get; set; }
        public Inv_store Store { get; set; }
        public Inv_store_site Site { get; set; }
        public Inv_item Item { get; set; }
    }
}