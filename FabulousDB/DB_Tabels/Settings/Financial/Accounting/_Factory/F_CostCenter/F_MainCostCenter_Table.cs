using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CostCenter
{
    public class F_MainCostCenter_Table
    {

        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_CostCenterGroupID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string F_CostCenterGroupName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string FactoryID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // Main cost center repeat with each cost center in group
        public virtual ICollection<F_GroupCostCenter_Table> F_GroupCostCenter_Table { get; set; }


        // one factory has many of main cost center
        public virtual CompanyFactoryInfo_Table CompanyFactoryInfo_Table { get; set; }
    }
}
