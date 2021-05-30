using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CostCenter
{
    public class F_GroupCostCenter_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_CostCenterGroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string F_Percentage { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // every cost center can enter to several differents groups
        public virtual F_CostCenter_Table F_CostCenter_Table { get; set; }


        // Main cost center repeat with each cost center in group
        public virtual F_MainCostCenter_Table F_MainCostCenter_Table { get; set; }


        public virtual ICollection<F_CreateAccountCCAccount_Table> F_CreateAccountCCAccount_Tables { get; set; }
    }
}
