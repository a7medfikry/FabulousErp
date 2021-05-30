using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup
{
    public class GroupFormsAccess_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GFAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string GroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string FormCode { get; set; }

        [Required]
        public string FormName { get; set; }

        //type 1 from company and 2 from branch and 3 from factory
        public int Type { get; set; }

        public virtual CreateGroup_Table CreateGroup_Table { get; set; }
    }
}
