using FabulousDB.DB_Tabels.Important;
using FabulousDB.DB_Tabels.Settings;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_CostCenter;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Analytic;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CheckBook;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_CostCenter;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyBranchInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccess;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserGroup;
using FabulousDB.DB_Tabels.Transaction.Financial.Branch.Accounting;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using FabulousDB.DB_Tabels.Transaction.Financial.Factory.Accounting;
using FabulousDB.Migrations;
using FabulousDB.Models;
using FabulousDB.Models.Attachment;

//using FabulousDB.Migrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Context
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("ERPContext")
           // : base(GetCon())
        {
           //Database.SetInitializer<DBContext>(new CreateDatabaseIfNotExists<DBContext>());
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, Configuration>());
        }
        public static string GetCon()
        {
            return "Data Source=.;Initial Catalog=" + SetDataBaseName.DataBase + ";Integrated Security=False;User Id=sa; Password=admin@321;MultipleActiveResultSets=True;Application Name=EntityFramework";
            ////Initialize the SqlConnectionStringBuilder.
            //string dbServer = string.Empty;
            //string dbName = string.Empty;
            //// use it from previously built normal connection string  
            //string connectString = Convert.ToString("Data Source=FABULOUSTECH2;Initial Catalog=FabuMahmoud;user id=sa;password=123456;");
            //var sqlBuilder = new SqlConnectionStringBuilder(connectString);
            //// Set the properties for the data source.  
            //dbServer = sqlBuilder.DataSource;
            //dbName = sqlBuilder.InitialCatalog;
            //sqlBuilder.UserID = sqlBuilder.UserID;
            //sqlBuilder.Password = sqlBuilder.Password;
            //sqlBuilder.IntegratedSecurity = false;
            //sqlBuilder.MultipleActiveResultSets = true;
            //// Build the SqlConnection connection string.  
            //string providerString = Convert.ToString(sqlBuilder);
            //// Initialize the EntityConnectionStringBuilder.  
            //var entityBuilder = new EntityConnectionStringBuilder();
            ////Set the provider name.  
            //entityBuilder.Provider = "System.Data.SqlClient";
            //// Set the provider-specific connection string.  
            //entityBuilder.ProviderConnectionString = providerString;
            //// Set the Metadata location.  
            //entityBuilder.Metadata = @"metadata=res://*/;";
            //////entityBuilder.Metadata = @"res://*/EntityDataObjectName.csdl|  
            //////            res: //*/EntityDataObjectName.ssdl|  
            //////                res: //*/EntityDataObjectName.msl";
            //return entityBuilder.ToString();
        }

        //Project Forms Table From Important----------------------------------------------------------

        public DbSet<FavouritesForms_Table> FavouritesForms_Tables { get; set; }

        public DbSet<Exceptions_Table> Exceptions_Tables { get; set; }
        //----------------------------------------------------------------------------------------------

        //Company Information Tabels--------------------------------------------------------------------- 
        public DbSet<CompanyMainInfo_Table> CompanyMainInfo_Tables { get; set; }

        public DbSet<CompanyLegalInfo_Table> CompanyLegalInfo_Tables { get; set; }
        //-----------------------------------------------------------------------------------------------

        //Branch Information tabels-----------------------------------------------------------------------
        public DbSet<BranchLegalInfo_Table> BranchLegalInfo_Tables { get; set; }

        public DbSet<CompanyBranchInfo_Table> CompanyBranchInfo_Tables { get; set; }
        //-------------------------------------------------------------------------------------------------

        //Factory Information tabels-----------------------------------------------------------------------
        public DbSet<CompanyFactoryInfo_Table> CompanyFactoryInfo_Tables { get; set; }

        public DbSet<FactoryLegalInfo_Table> FactoryLegalInfo_Tables { get; set; }
        //---------------------------------------------------------------------------------------------------

        //Address Information & Communication Information tabel for all comp. and Branch and factory-------------------------------------
        public DbSet<CompanyAddressInfo_Table> CompanyAddressInfo_Tables { get; set; }

        public DbSet<FactoryAddressInfo_Table> FactoryAddressInfo_Tables { get; set; }

        public DbSet<BranchAddressInfo_Table> BranchAddressInfo_Tables { get; set; }

        public DbSet<CompanyCommInfo_Table> CompanyCommInfo_Tables { get; set; }

        public DbSet<FactoryCommInfo_Table> FactoryCommInfo_Tables { get; set; }

        public DbSet<BranchCommInfo_Table> BranchCommInfo_Tables { get; set; }
        //----------------------------------------------------------------------------------------------------
        
        //Fiscal Year Definition & Fiscal Year Periods--------------------------------
        public DbSet<FiscalDefinition_Table> FiscalDefinition_Tables { get; set; }
        public DbSet<FiscalYear_Table> FiscalYear_Tables { get; set; }
        public DbSet<Fiscal_year_area> Fiscal_year_area { get; set; }
        public DbSet<NewFiscalYear_Table> NewFiscalYear_Table { get; set; }

        public DbSet<FiscalAdjustment_Table> FiscalAdjustment_Tables { get; set; }
        public DbSet<CompanyFiscalYear_Table> CompanyFiscalYear_Tables { get; set; }
        
        //------------------------------------------------------------------------------------------------------

        //Create User Account & User Group & Group Account------------------------------
        public DbSet<CreateAccount_Table> CreateAccount_Tables { get; set; }

        public DbSet<CreateGroup_Table> CreateGroup_Tables { get; set; }

        //public DbSet<AccountGroup_Table> AccountGroup_Tables { get; set; }

        public DbSet<UserFormsAccess_Table> UserFormsAccess_Tables { get; set; }

        public DbSet<GroupFormsAccess_Table> GroupFormsAccess_Tables { get; set; }

        public DbSet<UserGroup_Table> UserGroup_Tables { get; set; }

        //------------------------------------------------------------------------------------------------

        //Create User Access , Comp. ,Branch ,Factory Access-------------------------------------------------
        public DbSet<UACompPremission_Table> UACompPremission_Tables { get; set; }

        public DbSet<UABranchPremission_Table> UABranchPremission_Tables { get; set; }

        public DbSet<UAFactoryPremission_Table> UAFactoryPremission_Tables { get; set; }
        //---------------------------------------------------------------------------------------------------

        //Accounting Currencies > Format Setting , Currencies Definition, Currencies Exchane
        public DbSet<FormateSetting_Table> FormateSetting_Tables { get; set; }

        public DbSet<CurrenciesDefinition_Table> CurrenciesDefinition_Tables { get; set; }

        public DbSet<CurrenciesExchange_Table> CurrenciesExchange_Tables { get; set; }

        public DbSet<CurrenciesDefinition_Tables> AccountCurrencyDefinition_Tables { get; set; }

        public DbSet<B_AccountCurrencyDefinition_Table> B_AccountCurrencyDefinition_Tables { get; set; }

        public DbSet<F_AccountCurrencyDefinition_Table> F_AccountCurrencyDefinition_Tables { get; set; }
        //--------------------------------------------------------------------------------------------------------
        
        //Account Chart , Segment Chart, Company Account Chart
        public DbSet<AccountChart_Table> AccountChart_Table { get; set; }

        public DbSet<SegmentAccountChart_Table> SegmentAccountChart_Table { get; set; }

        public DbSet<CompanyChartAccount_Table> CompanyChartAccount_Table { get; set; }
        //--------------------------------------------------------------------------------------------------------


        //Create Analytic Account for comp. and branch and factory , Analytic Account Distribution
        public DbSet<C_AnalyticAccount_Table> C_AnalyticAccount_Tables { get; set; }
        public DbSet<F_AnalyticAccount_Table> F_AnalyticAccount_Tables { get; set; }
        public DbSet<B_AnalyticAccount_Table> B_AnalyticAccount_Tables { get; set; }


        public DbSet<C_AnalyticDistribution_Table> C_AnalyticDistribution_Tables { get; set; }
        public DbSet<B_AnalyticDistribution_Table> B_AnalyticDistribution_Tables { get; set; }
        public DbSet<F_AnalyticDistribution_Table> F_AnalyticDistribution_Tables { get; set; }
        //---------------------------------------------------------------------------------------------------------


        //Create Cost Center for comp. and branch and factory, Create Main and Group Cost Center for all, Create cost center account for comp and branch and factory
        public DbSet<C_CostCenter_Table> C_CostCenter_Tables { get; set; }
        public DbSet<B_CostCenter_Table> B_CostCenter_Tables { get; set; }
        public DbSet<F_CostCenter_Table> F_CostCenter_Tables { get; set; }


        public DbSet<C_MainCostCenter_Table> C_MainCostCenter_Tables { get; set; }
        public DbSet<C_GroupCostCenter_Table> C_GroupCostCenter_Tables { get; set; }

        public DbSet<B_MainCostCenter_Table> B_MainCostCenter_Tables { get; set; }
        public DbSet<B_GroupCostCenter_Table> B_GroupCostCenter_Tables { get; set; }

        public DbSet<F_MainCostCenter_Table> F_MainCostCenter_Tables { get; set; }
        public DbSet<F_GroupCostCenter_Table> F_GroupCostCenter_Tables { get; set; }


        public DbSet<C_CostCenterAccounts_Table> C_CostCenterAccounts_Tables { get; set; }
        public DbSet<B_CostCenterAccounts_Table> B_CostCenterAccounts_Tables { get; set; }
        public DbSet<F_CostCenterAccounts_Table> F_CostCenterAccounts_Tables { get; set; }
        //-------------------------------------------------------------------------------------------


        // creat Account Group of chart and group content
        public DbSet<AccountGroupOfChart_Table> AccountGroupOfChart_Tables { get; set; }

        public DbSet<ChartGroupContent_Table> ChartGroupContent_Tables { get; set; }
        //-------------------------------------------------------------------------------------------


        //Company Create Account And his Analytic Distribution Percentage And Cost Accounts Percentage
        public DbSet<C_CreateAccount_Table> C_CreateAccount_Tables { get; set; }

        public DbSet<C_CreatAccountDist_Table> C_CreatAccountDist_Tables { get; set; }

        public DbSet<C_CreateAccountCCAccount_Table> C_CreateAccountCCAccount_Tables { get; set; }

        public DbSet<C_CurrencyCreateAccount_Table> C_CurrencyCreateAccount_Tables { get; set; }
        //-------------------------------------------------------------------------------------------------


        //Branch Create Account And his Analytic Distribution Percentage And Cost Accounts Percentage
        public DbSet<B_CreateAccount_Table> B_CreateAccount_Tables { get; set; }

        public DbSet<B_CreatAccountDist_Table> B_CreatAccountDist_Tables { get; set; }

        public DbSet<B_CreateAccountCCAccount_Table> B_CreateAccountCCAccount_Tables { get; set; }

        public DbSet<B_CurrencyCreateAccount_Table> B_CurrencyCreateAccount_Tables { get; set; }
        //---------------------------------------------------------------------------------------------


        //Factory Create Account And his Analytic Distribution Percentage And Cost Accounts Percentage
        public DbSet<F_CreateAccount_Table> F_CreateAccount_Tables { get; set; }

        public DbSet<F_CreatAccountDist_Table> F_CreatAccountDist_Tables { get; set; }

        public DbSet<F_CreateAccountCCAccount_Table> F_CreateAccountCCAccount_Tables { get; set; }

        public DbSet<F_CurrencyCreateAccount_Table> F_CurrencyCreateAccount_Tables { get; set; }
        //----------------------------------------------------------------------------------------------


        //Post Setting User Post Access And Setup Of It And Print Document
        public DbSet<User_Post_Table> User_Post_Tables { get; set; }

        public DbSet<PostingSetup_Table> PostingSetup_Tables { get; set; }

        public DbSet<PrintDocument_Table> PrintDocument_Tables { get; set; }
        //--------------------------------------------------------------------


        public DbSet<C_CreateBatch_Table> C_CreateBatch_Tables { get; set; }
        public DbSet<C_UserBatchApproval_Table> C_UserBatchApproval_Tables { get; set; }

        public DbSet<B_CreateBatch_Table> B_CreateBatch_Tables { get; set; }

        public DbSet<F_CreateBatch_Table> F_CreateBatch_Tables { get; set; }


        public DbSet<C_EditTRate> C_EditTRates { get; set; }

        public DbSet<B_EditTRate> B_EditTRates { get; set; }

        public DbSet<F_EditTRate> F_EditTRates { get; set; }

        //-----------------------------------------------------------------------------
        // Tax Setting
        public DbSet<C_TaxSetting_table> C_TaxSetting_Tables { get; set; }
        public DbSet<B_TaxSetting_table> B_TaxSetting_Tables { get; set; }
        public DbSet<F_TaxSetting_table> F_TaxSetting_Tables { get; set; }
        public DbSet<TaxGroup_table> TaxGroup_Tables { get; set; }

        //Tax Transaction
        public DbSet<C_PurchaseTaxHeader_Table> C_PurchaseTaxHeader_Tables { get; set; }

        public DbSet<C_PurchaseTaxDetails_Table> C_PurchaseTaxDetails_Tables { get; set; }

        //-----------------------------------------------------------------------------
        // Checkbook Setting
        public DbSet<C_CheckBookSetting_table> C_CheckBookSetting_Tables { get; set; }
        public DbSet<B_CheckBookSetting_table> B_CheckBookSetting_Tables { get; set; }
        public DbSet<F_CheckBookSetting_table> F_CheckBookSetting_Tables { get; set; }

        //------------------------------------------------------------------------------------
        public DbSet<C_GeneralJournalEntry_Table> C_GeneralJournalEntry_Tables { get; set; }
        public DbSet<C_SaveTransaction_Table> C_SaveTransaction_Tables { get; set; }
        public DbSet<C_SaveAnalytic_Table> C_SaveAnalytic_Tables { get; set; }
        public DbSet<C_SaveCostCenter_Table> C_SaveCostCenter_Tables { get; set; }
        public DbSet<C_GeneralLedger_Table> C_GeneralLedger_Tables { get; set; }

        //-------------------------------------------------------------------------------
        public DbSet<C_CheckbookTransactions_table> C_CheckbookTransactions_Tables { get; set; }
        public DbSet<C_BankReconcile_table> C_BankReconcile_Tables { get; set; }



        public DbSet<C_EndingBeginingYear> C_EndingBeginingYears { get; set; }

        //khaled Page Access start
        public DbSet<Pages> Pages { get; set; }

        public DbSet<UsersPageAccess> UsersPageAccess { get; set; }
        //khaled Page Access end

        //Balance Sheet Setting Start
        public DbSet<BPC_raw_settings> BPCRowsSettings { get; set; }
        public DbSet<BPC_Relation> BPCRelation { get; set; }

        //Balance Sheet Seeting End
        //Attchment Start
        public DbSet<Attachment_head> Attachment_head { get; set; }
        public DbSet<Attachment_files> Attachment_files { get; set; }
        //Attchment Start
        //Translate Start
        public DbSet<Translate> Translates { get; set; }
        //Translate End

        //Installment Start
        public DbSet<Installment_setting> Installment_settings { get; set; }
        public DbSet<Custom_installment> Custom_installments { get; set; }
        public DbSet<Installment_contract> Installment_contracts { get; set; }
        public DbSet<Installments> Installments { get; set; }
        //Installment End

        //DataBase Backup
        public DbSet<Client_DB> Client_DB { get; set; }
        //DataBase Backup
        
        public DbSet<Day_log> Day_log { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<Cost_center_accounts> Cost_center_accounts { get; set; }
        public DbSet<Groupcostcenter_accounts> Groupcostcenter_accounts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<decimal>().Configure(x => x.HasPrecision(20, 4));
            //Fixed Assets
            modelBuilder.Entity<Asset>()
                .Property(e => e.Acquisation_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Asset>()
                .Property(e => e.Scrap_value)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Asset>()
                .Property(e => e.Adjustment_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Asset>()
                .Property(e => e.Deprecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Additional_information)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Assets_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Assets_part_serial)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Assets_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Deprecation_record)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Asset_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Deprecation_number_of_units)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Asset_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Deprecation_temp_record)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Asset_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Depreication_assets_id_connection)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Assets_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Fixed_assets_disposel)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Assets_id);

            //modelBuilder.Entity<Asset>()
            //    .HasMany(e => e.Fixed_assets_renewal)
            //    .WithOptional(e => e.Asset)
            //    .HasForeignKey(e => e.Assets_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Fixed_assets_revaluate)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Assets_id);

            modelBuilder.Entity<Asset>()
                .HasMany(e => e.Stoking_assets)
                .WithOptional(e => e.Asset)
                .HasForeignKey(e => e.Assets_id);

            modelBuilder.Entity<Assets_class>()
                .Property(e => e.Deperecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Assets_class>()
                .HasMany(e => e.Assets)
                .WithRequired(e => e.Assets_class)
                .HasForeignKey(e => e.Assets_class_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Assets_class>()
                .HasMany(e => e.Assets_accounts)
                .WithOptional(e => e.Assets_class)
                .HasForeignKey(e => e.Class_id);

            modelBuilder.Entity<Assets_class>()
                .HasMany(e => e.Assets_main)
                .WithRequired(e => e.Assets_class)
                .HasForeignKey(e => e.Assets_class_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Assets_class>()
                .HasMany(e => e.Depreication_assets_connection)
                .WithOptional(e => e.Assets_class)
                .HasForeignKey(e => e.Assets_class_id);

            modelBuilder.Entity<Assets_class>()
                .HasMany(e => e.New_assets_transaction)
                .WithOptional(e => e.Assets_class)
                .HasForeignKey(e => e.Assets_class_id);

            modelBuilder.Entity<Assets_class>()
                .HasMany(e => e.Stoking_assets)
                .WithOptional(e => e.Assets_class)
                .HasForeignKey(e => e.Assets_class_id);

            modelBuilder.Entity<Assets_main>()
                .HasMany(e => e.Assets)
                .WithOptional(e => e.Assets_main)
                .HasForeignKey(e => e.Assets_main_id);

            //modelBuilder.Entity<Book>()
            //    .HasMany(e => e.Assets)
            //    .WithOptional(e => e.Book)
            //    .HasForeignKey(e => e.Book_id);

            modelBuilder.Entity<Delete_fixed_assets_disposel>()
                .Property(e => e.Disposal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deleted_assets>()
                .Property(e => e.Acquisation_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deleted_assets>()
                .Property(e => e.Scrap_value)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deleted_assets>()
                .Property(e => e.Adjustment_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deleted_assets>()
                .Property(e => e.Deprecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deleted_assets_class>()
                .Property(e => e.Deperecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deleted_fixed_assets_renewal>()
                .Property(e => e.Renewal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deleted_fixed_assets_renewal>()
                .Property(e => e.Deprecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation>()
                .Property(e => e.Acquisition_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation>()
                .Property(e => e.Depreciation_accumulated)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation>()
                .Property(e => e.Adjustment_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation>()
                .Property(e => e.Deprecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation>()
                .Property(e => e.Depreication_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation>()
                .Property(e => e.Special_depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation>()
                .HasMany(e => e.Deprecation_record)
                .WithOptional(e => e.Deprecation)
                .HasForeignKey(e => e.Deprecation_id);

            modelBuilder.Entity<Deprecation>()
                .HasMany(e => e.Depreication_assets_connection)
                .WithOptional(e => e.Deprecation)
                .HasForeignKey(e => e.Deprecation_id);

            modelBuilder.Entity<Deprecation>()
                .HasMany(e => e.Depreication_assets_id_connection)
                .WithOptional(e => e.Deprecation)
                .HasForeignKey(e => e.Deprecation_id);

            modelBuilder.Entity<Deprecation_periods>()
                .HasMany(e => e.Deprecations)
                .WithOptional(e => e.Deprecation_periods)
                .HasForeignKey(e => e.Period);

            modelBuilder.Entity<Deprecation_periods>()
                .HasMany(e => e.Deprecation_temp)
                .WithOptional(e => e.Deprecation_periods)
                .HasForeignKey(e => e.Period);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Assets_acquisition_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Renewal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Disposal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Total)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Beginning_deprecation_accumulated)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Renewal_depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Disposal_depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Ending_deprecication_accumulated)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .Property(e => e.Net_assets_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_record>()
                .HasMany(e => e.Deprecation_number_of_units)
                .WithOptional(e => e.Deprecation_record)
                .HasForeignKey(e => e.Deprecation_id);

            modelBuilder.Entity<Deprecation_temp>()
                .Property(e => e.Acquisition_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp>()
                .Property(e => e.Depreciation_accumulated)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp>()
                .Property(e => e.Adjustment_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp>()
                .Property(e => e.Deprecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp>()
                .Property(e => e.Depreication_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp>()
                .Property(e => e.Special_depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp>()
                .HasMany(e => e.Deprecation_temp_record)
                .WithOptional(e => e.Deprecation_temp)
                .HasForeignKey(e => e.Deprecation_id);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Assets_acquisition_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Renewal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Disposal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Total)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Beginning_deprecation_accumulated)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Renewal_depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Disposal_depreication)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Ending_deprecication_accumulated)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Deprecation_temp_record>()
                .Property(e => e.Net_assets_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Fixed_assets_disposel>()
                .Property(e => e.Disposal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Fixed_assets_renewal>()
                .Property(e => e.Renewal_amount)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Fixed_assets_renewal>()
                .Property(e => e.Deprecation_rate)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Fixed_assets_revaluate>()
                .Property(e => e.Old_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Fixed_assets_revaluate>()
                .Property(e => e.Adjustment_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Fixed_assets_revaluate>()
                .Property(e => e.Net_profit)
                .HasPrecision(20, 4);

            modelBuilder.Entity<New_assets_transaction>()
                .Property(e => e.Acquesation_cost)
                .HasPrecision(20, 4);

            //modelBuilder.Entity<New_assets_transaction>()
            //    .HasMany(e => e.Assets)
            //    .WithOptional(e => e.New_assets_transaction)
            //    .HasForeignKey(e => e.Assets_transaction_id);

            modelBuilder.Entity<Stoking_assets>()
                .HasMany(e => e.Stocking_assets_transaction)
                .WithOptional(e => e.Stoking_assets)
                .HasForeignKey(e => e.Stocking_assets_id);

            modelBuilder.Entity<Stoking_assets>()
                .HasMany(e => e.Stocking_notes)
                .WithOptional(e => e.Stoking_assets)
                .HasForeignKey(e => e.Stoking_assets_id);

            modelBuilder.Entity<Delete_fixed_assets_revaluate>()
                .Property(e => e.Old_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Delete_fixed_assets_revaluate>()
                .Property(e => e.Adjustment_cost)
                .HasPrecision(20, 4);

            modelBuilder.Entity<Delete_fixed_assets_revaluate>()
                .Property(e => e.Net_profit)
                .HasPrecision(20, 4);
            //End Fixed Assets
           
        }
        public static DBContext db()
        {
            return new DBContext();
        }
    }
}
