using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_receive_item_serial
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Item_id { get; set; }
        [ForeignKey("Po")]
        public int Po_id { get; set; }
        [MaxLength(200)]
        [Index(IsClustered = false,IsUnique =true)]
        public string Serial { get; set; }
        public DateTime? Start_warranty { get; set; }
        public DateTime? End_warranty { get; set; }
        //public DateTime? Expiry_date { get; set; }
        public Inv_receive_po_items Item { get; set; }
        public Inv_receive_po Po { get; set; }
        public bool Sold { get; set; } = false;
        public virtual ICollection<Inv_receive_expiry> Expiry { get; set; }
    }
}
