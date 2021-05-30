using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
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
    public class C_CostCenterAccounts_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_CAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostAccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string C_CostAccountName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        [Required]
        public string C_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        public virtual C_CostCenter_Table C_CostCenter_Table { get; set; }

        public virtual ICollection<C_CreateAccountCCAccount_Table> C_CreateAccountCCAccount_Table { get; set; }


        public virtual ICollection<C_SaveCostCenter_Table> C_SaveCostCenter_Tables { get; set; }
    }
}
