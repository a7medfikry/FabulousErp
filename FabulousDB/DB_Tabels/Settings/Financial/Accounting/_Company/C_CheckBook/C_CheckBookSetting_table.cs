using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook
{
    public class C_CheckBookSetting_table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_CBSID { get; set; }

        [Index(IsUnique = false)]
        public int C_AID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string C_CheckbookID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_CheckbookName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CheckbookType { get; set; }
        public bool? C_CheckbookStatus { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }
        public double? C_CheckbookMaxAmount { get; set; }
        public double? C_CheckbookMinAmount { get; set; }
        public int C_NextWithdrawNumber { get; set; }
        public int C_NextDepositNumber { get; set; }

        public double? C_CurrentCheckbookBalance { get; set; }
        public double? C_CurrentCashAccountBalance { get; set; }
        public double? C_LastReconcileBalance { get; set; }
        public double? C_LastReconcileDate { get; set; }
        public string C_BankName { get; set; }
        public int? C_BankAccountNumber { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_BankAccountName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_BranchName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_SwiftCode { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_IBAN { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_UserIDAccess { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_PasswordAccess { get; set; }


        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }
        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }


        public virtual ICollection<C_CheckbookTransactions_table> C_CheckbookTransactions_Tables { get; set; }
        public virtual ICollection<C_BankReconcile_table> C_BankReconcile_Tables { get; set; }

    }
}
