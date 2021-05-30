using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_movment_GS
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Allow adjustment")]
        public bool Allow_adjustment { get; set; }
        [Display(Name = "Next Adjustment No")]
        public int Next_adjustment_no { get; set; }
        public bool Allow_Transfer { get; set; }
        [DisplayName("Next Transfer No")]
        public int Next_transfer_no { get; set; }
        [MaxLength(200)]
        [DisplayName("Adjustment Password")]
        public string Adjustment_password { get; set; }
        [MaxLength(200)]
        public string Transfer_password { get; set; }
        //[DisplayName("Automatic safety stock")]
        //public bool Automatic_safety_stock { get; set; }
    }
}
