using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation
{
    public class CompanyMainInfo_Table
    {
        
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string CompanyName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CompanyAlies { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string CountryName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string Language { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string Currency { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string CompanyMainActivity { get; set; }


        public bool? Status { get; set; }

        public string LogoPath { get; set; }

        public string LogoName { get; set; }

        public byte[] LogoByte { get; set; }


        //one to one ' Main Company To his Legal Info '
        public virtual CompanyLegalInfo_Table CompanyLegalInfo_Table { get; set; }

        //one to one ' Main Company To his Address Info '
        public virtual CompanyAddressInfo_Table AddressInformation_Table { get; set; }

        //one to one ' Main Company To his Communication Info '
        public virtual CompanyCommInfo_Table CompanyCommInfo_Table { get; set; }

        //one to one ' Main Company to his Account Chart '
        public virtual CompanyChartAccount_Table CompanyChartAccount_Table { get; set; }

        //one to many 'Company has a collection of branches'
        public virtual ICollection<CompanyBranchInfo_Table> CompanyBranchInfo_Table { get; set; }
        public virtual ICollection<UABranchPremission_Table> UABranchPremission_Table { get; set; }

        //one to many 'Company has a collection of Factories'
        public virtual ICollection<CompanyFactoryInfo_Table> CompanyFactoryInfo_Table { get; set; }
        public virtual ICollection<UAFactoryPremission_Table> UAFactoryPremission_Table { get; set; }


        //many to many
        public virtual ICollection<UACompPremission_Table> UACompPremission_Table { get; set; }

        //one to many to fiscal year
        public virtual ICollection<CompanyFiscalYear_Table> CompanyFiscalYear_Table { get; set; }

        // one company have one formate setting
        public virtual FormateSetting_Table FormateSetting_Table { get; set; }


        // one company have many of currency definition
        public virtual ICollection<CurrenciesDefinition_Table> CurrenciesDefinition_Table { get; set; }


        // one company have many of analytic accounts
        public virtual ICollection<C_AnalyticAccount_Table> C_AnalyticAccount_Table { get; set; }



        //one company has many of cost center
        public virtual ICollection<C_CostCenter_Table> C_CostCenter_Table { get; set; }


        // one company has many of Main Cost Center
        public virtual ICollection<C_MainCostCenter_Table> C_MainCostCenter_Table { get; set; }



        public virtual ICollection<C_CreateAccount_Table> C_CreateAccount_Table { get; set; }


        public virtual ICollection<PostingSetup_Table> PostingSetup_Table { get; set; }


        public virtual ICollection<PrintDocument_Table> PrintDocument_Table { get; set; }


        public virtual ICollection<C_CreateBatch_Table> C_CreateBatch_Table { get; set; }


        public virtual ICollection<C_EditTRate> C_EditTRate { get; set; }
        public virtual ICollection<C_TaxSetting_table> C_TaxSetting_Tables { get; set; }
        public virtual ICollection<C_CheckBookSetting_table> C_CheckBookSetting_Tables { get; set; }


        public virtual ICollection<C_GeneralJournalEntry_Table> C_GeneralJournalEntry_Tables { get; set; }


        // Checkbook Transactions
        public virtual ICollection<C_CheckbookTransactions_table> C_CheckbookTransactions_Tables { get; set; }
        public virtual ICollection<C_BankReconcile_table> C_BankReconcile_Tables { get; set; }

        //public virtual ICollection<C_TaxSetting_table> TaxGroup_Tables { get; set; }
    }
}
