using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account
{
    public class B_CreateAccount_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int B_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string AccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string AccountName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string BranchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string AccountChartID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string AccountsGroup { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(12)]
        [Required]
        public string EstablishDate { get; set; }


        public bool? DisActive { get; set; }


        //[Column(TypeName = "nvarchar"), MaxLength(10)]
        //[Required]
        //public string Currency { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(7)]
        [Required]
        public string AccountType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(15)]
        [Required]
        public string PostingType { get; set; }


        public bool? ReconcileAccount { get; set; }

        public bool? AllowAdjusment { get; set; }

        public bool? Reevaluate { get; set; }

        public bool? ConsolidationAccount { get; set; }

        public bool? SupportDocument { get; set; }



        public bool? FinancialArea { get; set; }
        public bool? SalesArea { get; set; }
        public bool? PurshacingArea { get; set; }
        public bool? InventoryArea { get; set; }



        public double? MaximumAmountPerTransaction { get; set; }


        public double? MinimumAmountPerTransaction { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(15)]
        public string CostCenterType { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_AnalyticAccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostCenterID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string B_CostCenterGroupID { get; set; }


        public int? C_AID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string CompanyAccountID { get; set; }


        public virtual AccountChart_Table AccountChart_Table { get; set; }

        public virtual CompanyBranchInfo_Table CompanyBranchInfo_Table { get; set; }


        public virtual ICollection<B_CreatAccountDist_Table> B_CreatAccountDist_Table { get; set; }

        public virtual ICollection<B_CreateAccountCCAccount_Table> B_CreateAccountCCAccount_Table { get; set; }

        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }

        public virtual ICollection<F_CreateAccount_Table> F_CreateAccount_Table { get; set; }

        public virtual ICollection<B_TaxSetting_table> B_TaxSetting_Tables { get; set; }
        public virtual B_CheckBookSetting_table B_CheckBookSetting_Table { get; set; }



        public virtual ICollection<B_CurrencyCreateAccount_Table> B_CurrencyCreateAccount_Tables { get; set; }


        public virtual ICollection<B_AccountCurrencyDefinition_Table> B_AccountCurrencyDefinition_Tables { get; set; }
    }
}
