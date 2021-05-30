using FabulousDB.DB_Context;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
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
using System.Data.Entity;
using System.Data.Sql;
using Models;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_CheckbookTransactions_table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_CBT { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }
        public int C_PostingNumber { get; set; }
        public int C_DocumentNumber { get; set; }
        public int? C_CBSID { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string C_PostingKey { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string C_TransactionDate { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string C_PostingDate { get; set; }
        public double? C_SystemRate { get; set; }
        public double? C_TransactionRate { get; set; }
        public double? C_Difference { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string C_Reference { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(3)]
        public string C_DocumentType { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string C_Payment_To_Recieved_From { get; set; }
        public decimal? C_Reciept { get; set; }
        public decimal? C_Payment { get; set; }
        public decimal C_Balance { get; set; }
        public string C_CheckNumber { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string C_DueDate { get; set; }
        public bool? C_Reconcile { get; set; }
        public int? C_ReconcileNumber { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string UserID { get; set; }
        public DateTime C_DateTime { get; set; }
        public int? C_CBTVoid { get; set; }





        [ForeignKey("C_CBTVoid")]
        public virtual C_CheckbookTransactions_table C_CheckbookTransactions_Table { get; set; }
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
        public virtual C_CheckBookSetting_table C_CheckBookSetting_Table { get; set; }
        public virtual CreateAccount_Table CreateAccount_Table { get; set; }
        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }
        public virtual C_GeneralJournalEntry_Table C_GeneralJournalEntry_Table { get; set; }


    }
}
