using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account
{
    public class C_CreateAccount_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_AID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string AccountID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string AccountName { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [ForeignKey("CompanyMainInfo_Table")]
        [Required]
        public string CompanyID { get; set; }

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
        public string C_AnalyticAccountID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_CostCenterGroupID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string C_Prefix { get; set; }






        public virtual AccountChart_Table AccountChart_Table { get; set; }


        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }


        public virtual ICollection<C_CreatAccountDist_Table> C_CreatAccountDist_Table { get; set; }

        public virtual ICollection<C_CreateAccountCCAccount_Table> C_CreateAccountCCAccount_Table { get; set; }

        public virtual ICollection<B_CreateAccount_Table> B_CreateAccount_Table { get; set; }

        public virtual ICollection<F_CreateAccount_Table> F_CreateAccount_Table { get; set; }
        public virtual ICollection<C_TaxSetting_table> C_TaxSetting_Tables { get; set; }

        //public virtual ICollection<CurrenciesDefinition_Table> CurrenciesDefinition_Table { get; set; }

        public virtual ICollection<CurrenciesDefinition_Tables> AccountCurrencyDefinition_Tables { get; set; }

        public virtual ICollection <C_CheckBookSetting_table> C_CheckBookSetting_Table { get; set; }



        public virtual ICollection<C_CurrencyCreateAccount_Table> C_CurrencyCreateAccount_Tables { get; set; }



        public virtual ICollection<C_SaveTransaction_Table> C_SaveTransaction_Tables { get; set; }

        public virtual ICollection<C_SaveAnalytic_Table> C_SaveAnalytic_Tables { get; set; }

        public virtual ICollection<C_SaveCostCenter_Table> C_SaveCostCenter_Tables { get; set; }

        public virtual ICollection<C_GeneralLedger_Table> C_GeneralLedger_Tables { get; set; }

        public virtual ICollection<C_EndingBeginingYear> C_EndingYears { get; set; }
        public virtual ICollection<Cost_center_accounts> Cost_center { get; set; }
        public virtual ICollection<Groupcostcenter_accounts> Groupcostcenter_accounts { get; set; }
    }
    public enum PayRecPrefix
    {
        Pay=1,
        Rec=2
    }
    public enum PostingType
    {
        BallanceSheet=1,
        PL=2
    }
}
