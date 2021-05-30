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
    public class UserFormsAccess_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UFAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string UserID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string FormCode { get; set; }

        [Required]
        public string FormName { get; set; }

        //type 1 from company and 2 from branch and 3 from factory
        public int Type { get; set; }

        public virtual CreateAccount_Table CreateAccount_Table { get; set;  }
    }
}
