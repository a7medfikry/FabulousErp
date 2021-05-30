using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CostCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account
{
    public class B_CreateAccountCCAccount_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int B_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(15)]
        [Required]
        public string CostCenterType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostCenterGroupID { get; set; }


        public int? B_CAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string Percentage { get; set; }


        public int? GroupID { get; set; }


        public virtual B_CreateAccount_Table B_CreateAccount_Table { get; set; }


        public virtual B_CostCenter_Table B_CostCenter_Table { get; set; }


        public virtual B_CostCenterAccounts_Table B_CostCenterAccounts_Table { get; set; }


        public virtual B_GroupCostCenter_Table B_GroupCostCenter_Table { get; set; }
    }
}
