using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
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
    public class C_GeneralJournalEntry_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_PostingNumber { get; set; }


        public int C_JournalEntryNumber { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        [Required]
        public string C_PostingKey { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(30)]
        [Required]
        public string C_TransactionType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string C_PostingDate { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string C_TransactionDate { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string C_Refrence { get; set; }


        public double? C_TotalDebit { get; set; }


        public double? C_TotalCredit { get; set; }


        public int? C_NoOfAcc { get; set; }


        public bool C_RoutinJournal { get; set; }


        public DateTime C { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        public int? C_CBID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }


        public double C_SystemRate { get; set; }


        public double C_TransactionRate { get; set; }


        public bool Post { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string UserID { get; set; }


        public int? VoidPostingNum { get; set; }

        public int C_Posting { get; set; }



        [ForeignKey("VoidPostingNum")]
        public virtual C_GeneralJournalEntry_Table GeneralJournalEntry_Table { get; set; }

        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

        public virtual C_CreateBatch_Table C_CreateBatch_Table { get; set; }

        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }

        public virtual CreateAccount_Table CreateAccount_Table { get; set; }



        public virtual ICollection<C_SaveTransaction_Table> C_SaveTransaction_Tables { get; set; }

        public virtual ICollection<C_SaveAnalytic_Table> C_SaveAnalytic_Tables { get; set; }

        public virtual ICollection<C_SaveCostCenter_Table> C_SaveCostCenter_Tables { get; set; }

        public virtual ICollection<C_GeneralLedger_Table> C_GeneralLedger_Tables { get; set; }

        public virtual ICollection<C_CheckbookTransactions_table> C_CheckbookTransactions_Tables { get; set; }

        //public virtual ICollection<C_EndingBeginingYear> C_EndingBeginingYears { get; set; }

        public virtual C_PurchaseTaxHeader_Table C_PurchaseTaxHeader_Table { get; set; }
    }
}
