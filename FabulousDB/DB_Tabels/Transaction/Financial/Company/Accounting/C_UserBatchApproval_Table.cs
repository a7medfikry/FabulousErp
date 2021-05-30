using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_UserBatchApproval_Table
    {
        [Key, ForeignKey("C_CreateBatch_Table")]
        public int C_CBID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string UserID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string ApprovedDate { get; set; }


        public virtual C_CreateBatch_Table C_CreateBatch_Table { get; set; }

        public virtual CreateAccount_Table CreateAccount_Table { get; set; }
    }
}
