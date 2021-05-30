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
    public class Inv_store_site
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        [DisplayName("Site Id")]
        [Required]
        public string Site_id { get; set; }
        [MaxLength(500)]
        [DisplayName("Site Name")]
        public string Site_name { get; set; }
        [ForeignKey("Store")]
        [Required]
        public int Store_id { get; set; }
        public bool Inactive { get; set; } = false;
        public Inv_store Store { get; set; }
        public virtual ICollection<Inv_item_store_site> Item_store_site { get; set; }

    }
}
