using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account
{
    public class C_CreateAccountCCAccount_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int C_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(15)]
        [Required]
        public string CostCenterType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterGroupID { get; set; }


        public int? C_CAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string Percentage { get; set; }


        public int? GroupID { get; set; }


        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }

        public virtual C_CostCenter_Table C_CostCenter_Table { get; set; }

        public virtual C_CostCenterAccounts_Table C_CostCenterAccounts_Table { get; set; }

        public virtual C_GroupCostCenter_Table C_GroupCostCenter_Table { get; set; }
    }
}
