using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CostCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account
{
    public class F_CreateAccountCCAccount_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int F_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(15)]
        [Required]
        public string CostCenterType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_CostCenterGroupID { get; set; }


        public int? F_CAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string Percentage { get; set; }


        public int? GroupID { get; set; }


        public virtual F_CreateAccount_Table F_CreateAccount_Table { get; set; }


        public virtual F_CostCenter_Table F_CostCenter_Table { get; set; }


        public virtual F_CostCenterAccounts_Table F_CostCenterAccounts_Table { get; set; }


        public virtual F_GroupCostCenter_Table F_GroupCostCenter_Table { get; set; }
    }
}
