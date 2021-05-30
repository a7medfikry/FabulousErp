using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
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
    public class B_CostCenter_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostCenterID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string B_CostCenterName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string BranchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // one branch has many of cost center
        public virtual CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }


        // every cost center can enter to several differents groups
        public virtual ICollection<B_GroupCostCenter_Table> B_GroupCostCenter_Table { get; set; }


        public virtual ICollection<B_CostCenterAccounts_Table> B_CostCenterAccounts_Table { get; set; }


        public virtual ICollection<B_CreateAccountCCAccount_Table> B_CreateAccountCCAccount_Table { get; set; }
    }
}
