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
    public class Inv_sales
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Sales order No")]
        public string Request { get; set; }
        public DateTime Date { get; set; }
        public string Sales_person { get; set; }
        public int Within_days { get; set; }
        [ForeignKey("Items")]
        public int Item_id { get; set; }
        [NotMapped]
        public int Item_name { get; set; }
        public int Quantity { get; set; }
        public DateTime Delivery_Date { get; set; }
        public Request_from Request_from { get; set; }
        public Inv_item Items { get; set; }
    }
    public enum Request_from
    {
        Sales=1,
        Inventory=2,
        Production=3
    }
}
