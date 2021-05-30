using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter
{
    public class C_CostCenter_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string C_CostCenterName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }



        // one company has many of cost center
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }


        // every cost center can enter to several differents groups
        public virtual ICollection<C_GroupCostCenter_Table> C_GroupCostCenter_Table { get; set; }


        public virtual ICollection<C_CostCenterAccounts_Table> C_CostCenterAccounts_Table { get; set; }


        public virtual ICollection<C_CreateAccountCCAccount_Table> C_CreateAccountCCAccount_Table { get; set; }



        public virtual ICollection<C_SaveCostCenter_Table> C_SaveCostCenter_Tables { get; set; }

    }
}
