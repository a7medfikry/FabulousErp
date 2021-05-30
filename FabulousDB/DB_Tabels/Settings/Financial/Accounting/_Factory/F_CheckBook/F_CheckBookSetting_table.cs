using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CheckBook
{
    public class F_CheckBookSetting_table
    {
        [Key]
        [ForeignKey("F_CreateAccount_Table")]
        public int F_AID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string FactoryID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string F_CheckbookID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string F_CheckbookName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string F_CheckbookType { get; set; }
        public bool? F_CheckbookStatus { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }
        public double? F_CheckbookMaxAmount { get; set; }
        public double? F_CheckbookMinAmount { get; set; }
        public int F_NextWithdrawNumber { get; set; }
        public int F_NextDepositNumber { get; set; }
        public double? F_CurrentCheckbookBalance { get; set; }
        public double? F_CurrentCashAccountBalance { get; set; }
        public double? F_LastReconcileBalance { get; set; }
        public double? F_LastReconcileDate { get; set; }
        public string F_BankName { get; set; }
        public int? F_BankAccountNumber { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string F_BankAccountName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string F_BranchName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string F_SwiftCode { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string F_IBAN { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string F_UserIDAccess { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string F_PasswordAccess { get; set; }
        public virtual CompanyFactoryInfo_Table CompanyFactoryInfo_Table { get; set; }
        public virtual F_CreateAccount_Table F_CreateAccount_Table { get; set; }
        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }
    }
}
