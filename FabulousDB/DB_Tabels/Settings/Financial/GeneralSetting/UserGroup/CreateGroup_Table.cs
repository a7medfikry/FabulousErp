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
    public class CreateGroup_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string GroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        [Required]
        public string GroupName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string GroupDescription { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        [Required]
        public string Date { get; set; }

        //1 from company 2 from Branch 3 from factory
        public int FromCBF { get; set; }


        public Nullable<bool> DisActive { get; set; }


        public Nullable<bool> Deleted { get; set; }



        //one to many from group to user
        public virtual ICollection<CreateAccount_Table> CreateAccount_Table { get; set; }
        // 3public ICollection<AccountGroup_Table> AccountGroup_Table { get; set; }

        public virtual ICollection<GroupFormsAccess_Table> GroupFormsAccess_Tables { get; set; }

        public virtual ICollection<UserGroup_Table> UserGroup_Tables { get; set; }
    }
}
