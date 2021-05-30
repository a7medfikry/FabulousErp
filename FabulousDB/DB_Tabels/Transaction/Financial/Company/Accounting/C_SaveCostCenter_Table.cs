using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_SaveCostCenter_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_SCC { get; set; }


        public int C_PostingNumber { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterID { get; set; }


        public int? C_CAID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string Describtion { get; set; }


        public double C_Percentage { get; set; }


        public double C_Amount { get; set; }


        public double? C_Debit { get; set; }


        public double? C_Credit { get; set; }


        public double Ballance { get; set; }


        public bool Post { get; set; }


        public int? C_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterGroupID { get; set; }


        public double? CostCenterPercentage { get; set; }



        public virtual C_GeneralJournalEntry_Table C_GeneralJournalEntry_Table { get; set; }

        public virtual C_CostCenter_Table C_CostCenter_Table { get; set; }

        public virtual C_CostCenterAccounts_Table C_CostCenterAccounts_Table { get; set; }

        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }

        public virtual C_MainCostCenter_Table C_MainCostCenter_Table { get; set; }
    }
}
