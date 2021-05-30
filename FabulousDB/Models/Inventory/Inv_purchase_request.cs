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
    public class Inv_purchase_request
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Pr_number { get; set; }
        public int Within_days { get; set; }
        [ForeignKey("Store")]
        public int Store_id { get; set; }
        [ForeignKey("Site")]
        public int Site_id { get; set; }
        public Inv_store Store { get; set; }
        public Inv_store_site Site { get; set; }
        [DisplayName("Delivery Date")]
        public DateTime Within_days_date { get; set; }
        public virtual ICollection<Inv_purchase_request_items> Inv_purchase_request_items { get; set; }
    }
}
