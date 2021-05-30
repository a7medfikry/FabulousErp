using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Branch.Accounting
{
    public class B_CreateBatch_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int B_CBID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string BranchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(11)]
        [Required]
        public string B_Module { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string B_BatchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string B_BatchDescription { get; set; }


        public bool? Approval { get; set; }

        public bool? NotApproval { get; set; }

        public virtual CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }
    }
}
