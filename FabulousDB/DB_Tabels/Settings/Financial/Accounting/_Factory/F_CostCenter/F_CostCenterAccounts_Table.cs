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
    public class F_CostCenterAccounts_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int F_CAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_CostAccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string F_CostAccountName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        [Required]
        public string F_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }



        public virtual F_CostCenter_Table F_CostCenter_Table { get; set; }


        public virtual ICollection<F_CreateAccountCCAccount_Table> F_CreateAccountCCAccount_Table { get; set; }
    }
}
