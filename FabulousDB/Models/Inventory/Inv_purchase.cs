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
    public class Inv_purchase
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Purchase Request NO.")]
        public string Request { get; set; }
        public DateTime Date { get; set; }
        public int Within_days { get; set; }
        [DisplayName("Delivery Date")]
        public DateTime Delivery_date { get; set; }
        public Inv_sales_send_to Send_to { get; set; }
        [ForeignKey("Items")]
        public int Item_id { get; set; }
        [NotMapped]
        public int Item_name { get; set; }
        public int Quantity { get; set; }
        public Inv_item Items { get; set; }
    }
    public enum Inv_sales_send_to
    {
        Purchase = 1,
        Person=2
    }
}
