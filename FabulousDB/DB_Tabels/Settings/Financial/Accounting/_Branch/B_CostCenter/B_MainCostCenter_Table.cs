using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CostCenter
{
    public class B_MainCostCenter_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostCenterGroupID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string B_CostCenterGroupName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string BranchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // Main cost center repeat with each cost center in group
        public virtual ICollection<B_GroupCostCenter_Table> B_GroupCostCenter_Table { get; set; }


        // one branch has many of Main cost center
        public virtual CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }
    }
}
