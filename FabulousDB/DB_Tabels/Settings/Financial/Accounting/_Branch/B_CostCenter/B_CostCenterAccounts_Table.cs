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
    public class B_CostCenterAccounts_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int B_CAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostAccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string B_CostAccountName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        [Required]
        public string B_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        public virtual B_CostCenter_Table B_CostCenter_Table { get; set; }


        public virtual ICollection<B_CreateAccountCCAccount_Table> B_CreateAccountCCAccount_Table { get; set; }
    }
}
