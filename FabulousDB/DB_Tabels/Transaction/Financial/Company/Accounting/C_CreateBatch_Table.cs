using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
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
    public class C_CreateBatch_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_CBID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(11)]
        [Required]
        public string C_Module { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        [Required]
        public string C_BatchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string C_BatchDescription { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string C_BatchCreationDate { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string C_BatchLocation { get; set; }


        public bool? Approval { get; set; }

        public bool? NotApproval { get; set; }

        public bool? Removed { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string UserID { get; set; }




        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }


        public virtual ICollection<C_GeneralJournalEntry_Table> C_GeneralJournalEntry_Tables { get; set; }

        public virtual CreateAccount_Table CreateAccount_Table { get; set; }

        public virtual C_UserBatchApproval_Table C_UserBatchApproval_Table { get; set; }
    }
}
