using FabulousErp.Payable.Models;
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
    public class Inv_quotation_request
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Request for Quotation NO")]
        public string Request_for_qut_no { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("Po")]
        public int? Po_id { get; set; }
        public float Within_days { get; set; }
        [NotMapped]
        public int Item_name { get; set; }
        [ForeignKey("Vendore")]
        public int Vendore_id { get; set; }
        public DateTime Delivery_Date { get; set; }
        public Request_from Request_from { get; set; }
        public Payable_creditor_setting Vendore { get; set; }
        public Inv_purchase_request Po { get; set; }
        public virtual ICollection<Inv_quotation_request_item> Inv_quotation_request_item { get; set; }
    }
}
