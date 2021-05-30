using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_sales_GS
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Allow Insert Item")]
        public bool Allow_insert_item_no_enough_store { get; set; } = false;

        [DisplayName("Allow Proforma Invoice")]
        public bool Allow_proforma_inv { get; set; } = false;

        [DisplayName("Allow View J.V")]
        public bool Allow_View_jv { get; set; } = false;

        [DisplayName("Override price in price list")]
        public bool Override_price_in_price_list { get; set; } = false;
        [DisplayName("Allow price lower cost")]
        public bool Allow_price_lower_cost { get; set; } = false;
        [DisplayName("Allow Edit Sales Price")]
        public bool Allow_edit_sales_price { get; set; } = false;

        [DisplayName("Passwords For Unhold")]
        public bool passwords_for_unhold_check { get; set; } = false;
        [DisplayName("Passwords For Unhold")]
        [MaxLength(200)]
        public string passwords_for_unhold { get; set; }
        [DisplayName("Allow Generate Automatic PO")]
        public bool Allow_generate_automatic_po { get; set; } = false; 
        [DisplayName("Show Cost Price")]
        public bool Show_cost_price { get; set; } = false;
    }
}
