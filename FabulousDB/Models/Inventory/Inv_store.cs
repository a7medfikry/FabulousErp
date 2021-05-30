using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
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
    public class Inv_store
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        [DisplayName("Store Id")]
        [Required]
        public string Store_id { get; set; }
        
        [MaxLength(500)]
        [DisplayName("Store Name")]
        public string Store_name { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(200)]
        public string City { get; set; }
        [MaxLength(200)]
        public string State { get; set; }
        [MaxLength(200)]
        public string Country { get; set; }
        [MaxLength(200)]
        public string Phone { get; set; }
        [MaxLength(200)]
        public string Mobile { get; set; }
        [MaxLength(200)]
        public string Fax { get; set; }
        [MaxLength(200)]
        public string Contact_person { get; set; }
        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }
        public bool Inactive { get; set; } = false;
        [DisplayName("Next Gr No.")]
        public int Next_gr_no { get; set; }
        [Display(Name = "Next Goods Out No")]
        public int Next_goods_no { get; set; }
        [ForeignKey("Store_gl_account")]
        public int? Store_gl_account_id { get; set; }

        public C_CreateAccount_Table Store_gl_account { get; set; }

        public virtual ICollection<Inv_store_site> Sites { get; set; }
        public virtual ICollection<Inv_item_store_site> Item_store_site { get; set; }

    }
}
