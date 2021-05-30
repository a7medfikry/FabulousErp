using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_goods_out
    {
        [Key]
        public int Id { get; set; }
        public string Gr_no { get; set; }
        public int Request_no { get; set; }
        [ForeignKey("Store")]
        public int Store_id { get; set; }
        [ForeignKey("Site")]
        public int Site_id { get; set; }
        public DateTime Date { get; set; }
        [DisplayName("Vendor Doc")]
        public string Vendor_doc{ get; set; }
        [DisplayName("Po_no")]
        public int Po_no{ get; set; }
        public Inv_store Store { get; set; }
        public Inv_store Site { get; set; }
        // public Inv_purchase_request Request_no { get; set; }
    }
}
