using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter
{
    public class C_GroupCostCenter_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterGroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string C_Percentage { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // Main cost center repeat with each cost center in group
        public virtual C_MainCostCenter_Table C_MainCostCenter_Table { get; set; }


        // every cost center can enter to several differents groups
        public virtual C_CostCenter_Table C_CostCenter_Table { get; set; }


        public virtual ICollection<C_CreateAccountCCAccount_Table> C_CreateAccountCCAccount_Tables { get; set; }
    }
}
