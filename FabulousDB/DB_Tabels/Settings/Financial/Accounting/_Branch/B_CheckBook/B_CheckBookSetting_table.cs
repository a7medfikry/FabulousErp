using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CheckBook
{
   public class B_CheckBookSetting_table
    {
        [Key]
        [ForeignKey("B_CreateAccount_Table")]
        public int B_AID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string BranchID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string B_CheckbookID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string B_CheckbookName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CheckbookType { get; set; }
        public bool? B_CheckbookStatus { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }
        public double? B_CheckbookMaxAmount { get; set; }
        public double? B_CheckbookMinAmount { get; set; }
        public int B_NextWithdrawNumber { get; set; }
        public int B_NextDepositNumber { get; set; }
        public double? B_CurrentCheckbookBalance { get; set; }
        public double? B_CurrentCashAccountBalance { get; set; }
        public double? B_LastReconcileBalance { get; set; }
        public double? B_LastReconcileDate { get; set; }
        public string B_BankName { get; set; }
        public int? B_BankAccountNumber { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string B_BankAccountName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string B_BranchName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string B_SwiftCode { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string B_IBAN { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string B_UserIDAccess { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string B_PasswordAccess { get; set; }
        public virtual CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }
        public virtual B_CreateAccount_Table B_CreateAccount_Table { get; set; }
        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }
    }
}
