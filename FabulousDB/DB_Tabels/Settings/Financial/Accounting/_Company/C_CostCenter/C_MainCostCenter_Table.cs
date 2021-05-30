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
    public class C_MainCostCenter_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterGroupID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string C_CostCenterGroupName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }



        // Main cost center repeat with each cost center in group
        public virtual ICollection<C_GroupCostCenter_Table> C_GroupCostCenter_Table { get; set; }


        // one company has many of cost center
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }


        public virtual ICollection<C_SaveCostCenter_Table> C_SaveCostCenter_Tables { get; set; }
    }
}
