using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CostCenter
{
    public class B_GroupCostCenter_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostCenterID { get; set; }


        [Required]
        public string B_CostCenterGroupID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string B_Percentage { get; set; }



        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }



        // every cost center can enter to several differents groups
        public virtual B_CostCenter_Table B_CostCenter_Table { get; set; }


        // Main cost center repeat with each cost center in group
        public virtual B_MainCostCenter_Table B_MainCostCenter_Table { get; set; }


        public virtual ICollection<B_CreateAccountCCAccount_Table> B_CreateAccountCCAccount_Tables { get; set; }
    }
}
