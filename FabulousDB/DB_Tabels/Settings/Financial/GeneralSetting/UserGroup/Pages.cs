using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup
{
   public  class Pages
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(200)]
        [Required]
        public string Link { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(200)]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Page_section { get; set; }
    }
}
