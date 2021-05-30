using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_po_GS
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Next PO No")]
        public int Next_po_no { get; set; }
        [Display(Name = "Next Pr No")]
        public int Next_pr_no { get; set; }
        [DisplayName("Allow Receiving Without Invoice")]
        public bool Allow_receiv_without_inv { get; set; }

        [DisplayName("Allow Receiving Part Of PO")]
        public bool Allow_receiv_part_of_po { get; set; }

        [DisplayName("Allow View J.V")]
        public bool Allow_View_jv { get; set; }

        [DisplayName("Show Items Cost In Receiving")]
        public bool Show_items_cost_in_receiving { get; set; }

        [DisplayName("Passwords For Unhold")]
        public bool passwords_for_unhold_check { get; set; }
        [DisplayName("Passwords For Unhold")]
        [MaxLength(200)]
        public string passwords_for_unhold { get; set; }
        [DisplayName("Allow Generate Automatic PO")]
        public bool Allow_generate_automatic_po { get; set; }
    }
}
