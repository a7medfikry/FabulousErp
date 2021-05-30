using FabulousDB.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Inv_quotation_request_item
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("item")]
        public int item_id { get; set; }
        public double Quntaty { get; set; }
        public Inv_item item { get; set; }
        [ForeignKey("Quotation_request")]
        public int Quotation_request_id { get; set; }
        public Inv_quotation_request Quotation_request { get; set; }
    }
}