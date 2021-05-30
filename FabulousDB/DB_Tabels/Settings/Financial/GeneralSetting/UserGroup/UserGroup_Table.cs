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
    public class UserGroup_Table
    {
        [Key, ForeignKey("CreateAccount_Table")]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string UserID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string GroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string Date { get; set; }


        public virtual CreateAccount_Table CreateAccount_Table { get; set; }

        public virtual CreateGroup_Table CreateGroup_Table { get; set; }
    }
}
