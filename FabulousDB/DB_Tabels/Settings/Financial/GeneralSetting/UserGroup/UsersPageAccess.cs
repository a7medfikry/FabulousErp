using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup
{
    public class UsersPageAccess
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [InverseProperty("CreateAccount_Table")]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string UserID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(200)]
        public string Name { get; set; }
        [ForeignKey("Page")]
        public int Page_id { get; set; }
        [DefaultValue(false)]
        public bool View { get; set; }
        [DefaultValue(false)]
        public bool Edit { get; set; }
        [DefaultValue(false)]
        public bool Delete { get; set; }
        [DefaultValue(false)]
        public bool Update { get; set; }

        public virtual CreateAccount_Table CreateAccount_Table { get; set; }

        public virtual Pages Page { get; set; }

    }
}
