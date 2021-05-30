namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Additional_information",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_id = c.Int(),
                        Field_name = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asset", t => t.Assets_id)
                .Index(t => t.Assets_id);
            
            CreateTable(
                "dbo.Asset",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_class_id = c.Int(nullable: false),
                        Assets_main_id = c.Int(),
                        Description = c.String(nullable: false, maxLength: 200),
                        Foreign_name = c.String(maxLength: 200),
                        Type = c.Int(nullable: false),
                        Proparty_type = c.Int(nullable: false),
                        Acquisation_cost = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Scrap_value = c.Decimal(precision: 20, scale: 4),
                        Start_use = c.DateTime(nullable: false, storeType: "date"),
                        Start_derecation_date = c.DateTime(nullable: false, storeType: "date"),
                        Number_of_units = c.Int(),
                        Deactive_depraction = c.Boolean(nullable: false),
                        Fully_depraction = c.Boolean(nullable: false),
                        Disposal = c.Boolean(nullable: false),
                        Date_of_orgin = c.DateTime(nullable: false, storeType: "date"),
                        Adjustment_cost = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Date_of_Adjustmemt = c.DateTime(nullable: false, storeType: "date"),
                        Use_life = c.DateTime(nullable: false, storeType: "date"),
                        Deprecation_method = c.Int(nullable: false),
                        Include_scerap_value = c.Boolean(nullable: false),
                        Assets_number = c.String(maxLength: 200),
                        Deprecation_rate = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Book_id = c.Int(nullable: false),
                        Assets_transaction_id = c.Int(nullable: false),
                        Company_id = c.String(maxLength: 10),
                        Creation_date = c.DateTime(),
                        Purchase_types = c.Int(),
                        Payable_Inv_trans = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets_class", t => t.Assets_class_id)
                .ForeignKey("dbo.Assets_main", t => t.Assets_main_id)
                .ForeignKey("dbo.New_assets_transaction", t => t.Assets_transaction_id, cascadeDelete: false)
                .ForeignKey("dbo.Book", t => t.Book_id, cascadeDelete: false)
                .Index(t => t.Assets_class_id)
                .Index(t => t.Assets_main_id)
                .Index(t => t.Book_id)
                .Index(t => t.Assets_transaction_id);
            
            CreateTable(
                "dbo.Assets_class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Class_id = c.String(nullable: false, maxLength: 200),
                        Description = c.String(nullable: false, maxLength: 200),
                        Deprecation_method = c.Int(),
                        Deperecation_rate = c.Decimal(precision: 20, scale: 4),
                        Active = c.Boolean(nullable: false),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Assets_accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Class_id = c.Int(),
                        Cost_account = c.Int(nullable: false),
                        Deprecation_accumulated_account = c.Int(nullable: false),
                        Profit_account = c.Int(),
                        Lose_account = c.Int(),
                        Payable_account = c.Int(),
                        Revaluation_account = c.Int(),
                        Deprcation = c.Int(nullable: false),
                        Retirment = c.Int(),
                        Accrued = c.Int(),
                        Receivable_account = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets_class", t => t.Class_id)
                .Index(t => t.Class_id);
            
            CreateTable(
                "dbo.Assets_main",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_class_id = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                        Number_of_parts = c.Int(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        Assets_number = c.String(nullable: false, maxLength: 200),
                        Assets_custom_id = c.Int(),
                        Company_id = c.String(maxLength: 10)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets_class", t => t.Assets_class_id)
                .Index(t => t.Assets_class_id);
            
            CreateTable(
                "dbo.Depreication_assets_connection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deprecation_id = c.Int(),
                        Assets_class_id = c.Int(),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deprecation", t => t.Deprecation_id)
                .ForeignKey("dbo.Assets_class", t => t.Assets_class_id)
                .Index(t => t.Deprecation_id)
                .Index(t => t.Assets_class_id);
            
            CreateTable(
                "dbo.Deprecation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deprecation_no = c.Int(),
                        Transaction_date = c.DateTime(),
                        Deprecation_date = c.DateTime(nullable: false, storeType: "date"),
                        Period = c.Int(),
                        Is_assets_class = c.Boolean(),
                        Acquisition_cost = c.Decimal(precision: 20, scale: 4),
                        Depreciation_accumulated = c.Decimal(precision: 20, scale: 4),
                        Adjustment_cost = c.Decimal(precision: 20, scale: 4),
                        Deprecation_rate = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Depreication_cost = c.Decimal(precision: 20, scale: 4),
                        Special_depreication = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Period_id = c.Int(),
                        Month = c.Int(),
                        Year = c.Int(),
                        Createion_date = c.DateTime(),
                        Jornal_number = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deprecation_periods", t => t.Period)
                .Index(t => t.Period);
            
            CreateTable(
                "dbo.Deprecation_periods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        text = c.String(maxLength: 20),
                        Period_start = c.DateTime(storeType: "date"),
                        Period_end = c.DateTime(storeType: "date"),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Deprecation_temp",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deprecation_no = c.Int(),
                        Transaction_date = c.DateTime(),
                        Deprecation_date = c.DateTime(nullable: false, storeType: "date"),
                        Period = c.Int(),
                        Is_assets_class = c.Boolean(),
                        Acquisition_cost = c.Decimal(precision: 20, scale: 4),
                        Depreciation_accumulated = c.Decimal(precision: 20, scale: 4),
                        Adjustment_cost = c.Decimal(precision: 20, scale: 4),
                        Deprecation_rate = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Depreication_cost = c.Decimal(precision: 20, scale: 4),
                        Special_depreication = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Period_id = c.Int(),
                        Month = c.Int(),
                        Year = c.Int(),
                        Createion_date = c.DateTime(),
                        Jornal_number = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deprecation_periods", t => t.Period)
                .Index(t => t.Period);
            
            CreateTable(
                "dbo.Deprecation_temp_record",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asset_id = c.Int(),
                        Date = c.DateTime(storeType: "date"),
                        Assets_acquisition_cost = c.Decimal(precision: 20, scale: 4),
                        Renewal_amount = c.Decimal(precision: 20, scale: 4),
                        Disposal_amount = c.Decimal(precision: 20, scale: 4),
                        Total = c.Decimal(precision: 20, scale: 4),
                        Beginning_deprecation_accumulated = c.Decimal(precision: 20, scale: 4),
                        Depreication = c.Decimal(precision: 20, scale: 4),
                        Renewal_depreication = c.Decimal(precision: 20, scale: 4),
                        Disposal_depreication = c.Decimal(precision: 20, scale: 4),
                        Ending_deprecication_accumulated = c.Decimal(precision: 20, scale: 4),
                        Net_assets_cost = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Deprecation_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deprecation_temp", t => t.Deprecation_id)
                .ForeignKey("dbo.Asset", t => t.Asset_id)
                .Index(t => t.Asset_id)
                .Index(t => t.Deprecation_id);
            
            CreateTable(
                "dbo.Deprecation_record",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Asset_id = c.Int(),
                        Date = c.DateTime(storeType: "date"),
                        Assets_acquisition_cost = c.Decimal(precision: 20, scale: 4),
                        Renewal_amount = c.Decimal(precision: 20, scale: 4),
                        Disposal_amount = c.Decimal(precision: 20, scale: 4),
                        Total = c.Decimal(precision: 20, scale: 4),
                        Beginning_deprecation_accumulated = c.Decimal(precision: 20, scale: 4),
                        Depreication = c.Decimal(precision: 20, scale: 4),
                        Renewal_depreication = c.Decimal(precision: 20, scale: 4),
                        Disposal_depreication = c.Decimal(precision: 20, scale: 4),
                        Ending_deprecication_accumulated = c.Decimal(precision: 20, scale: 4),
                        Net_assets_cost = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Deprecation_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deprecation", t => t.Deprecation_id)
                .ForeignKey("dbo.Asset", t => t.Asset_id)
                .Index(t => t.Asset_id)
                .Index(t => t.Deprecation_id);
            
            CreateTable(
                "dbo.Deprecation_number_of_units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 100),
                        Date = c.DateTime(nullable: false),
                        Asset_id = c.Int(),
                        Deprecation_number_of_unit = c.Int(),
                        Deprecation_id = c.Int(),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deprecation_record", t => t.Deprecation_id)
                .ForeignKey("dbo.Asset", t => t.Asset_id)
                .Index(t => t.Asset_id)
                .Index(t => t.Deprecation_id);
            
            CreateTable(
                "dbo.Depreication_assets_id_connection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deprecation_id = c.Int(),
                        Assets_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deprecation", t => t.Deprecation_id)
                .ForeignKey("dbo.Asset", t => t.Assets_id)
                .Index(t => t.Deprecation_id)
                .Index(t => t.Assets_id);
            
            CreateTable(
                "dbo.New_assets_transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Acquesation_cost = c.Decimal(precision: 20, scale: 4),
                        Date_of_orgin = c.DateTime(nullable: false, storeType: "date"),
                        Currency_id = c.String(maxLength: 50),
                        Reference = c.String(maxLength: 200),
                        Vendor_id = c.Int(),
                        Type = c.Int(),
                        Assets_class_id = c.Int(),
                        Company_id = c.String(maxLength: 10),
                        Gl_transaction_id = c.Int(),
                        Transaction_date = c.DateTime(storeType: "date"),
                        Posting_date = c.DateTime(storeType: "date"),
                        IsVoid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets_class", t => t.Assets_class_id)
                .Index(t => t.Assets_class_id);
            
            CreateTable(
                "dbo.Stoking_assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_id = c.Int(),
                        Assets_class_id = c.Int(),
                        Company_id = c.String(maxLength: 10),
                        Added_date = c.DateTime(),
                        Serial = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assets_class", t => t.Assets_class_id)
                .ForeignKey("dbo.Asset", t => t.Assets_id)
                .Index(t => t.Assets_id)
                .Index(t => t.Assets_class_id);
            
            CreateTable(
                "dbo.Stocking_assets_transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Stocking_no = c.String(maxLength: 200),
                        Reconcile_date = c.DateTime(nullable: false),
                        Transaction_date = c.DateTime(storeType: "date"),
                        Status = c.Int(),
                        Reconcile = c.Boolean(),
                        Stocking_assets_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stoking_assets", t => t.Stocking_assets_id)
                .Index(t => t.Stocking_assets_id);
            
            CreateTable(
                "dbo.Stocking_notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Note = c.String(maxLength: 500),
                        Important = c.Boolean(),
                        Stoking_assets_id = c.Int(),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stoking_assets", t => t.Stoking_assets_id)
                .Index(t => t.Stoking_assets_id);
            
            CreateTable(
                "dbo.Assets_part_serial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_id = c.Int(),
                        Part_number = c.Int(),
                        Serial = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asset", t => t.Assets_id)
                .Index(t => t.Assets_id);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 200),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fixed_assets_disposel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Disposal_no = c.Int(),
                        Transaction_date = c.DateTime(),
                        Disposal_date = c.DateTime(storeType: "date"),
                        Depreication_up_to_date = c.DateTime(storeType: "date"),
                        Assets_id = c.Int(),
                        Disposal_amount = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Gl_transaction_id = c.Int(),
                        Currency_id = c.String(maxLength: 50),
                        Reference = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asset", t => t.Assets_id)
                .Index(t => t.Assets_id);
            
            CreateTable(
                "dbo.Fixed_assets_renewal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Renewal_no = c.Int(),
                        Descroption = c.String(maxLength: 500),
                        Assets_id = c.Int(nullable: false),
                        Renewal_amount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Deprecation_rate = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Gl_transaction_id = c.Int(),
                        Use_life = c.Double(),
                        Renwal_date = c.DateTime(),
                        Currency_id = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asset", t => t.Assets_id, cascadeDelete: false)
                .Index(t => t.Assets_id);
            
            CreateTable(
                "dbo.Fixed_assets_revaluate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Revaluate_no = c.String(maxLength: 200),
                        Transaction_date = c.DateTime(),
                        Revaluate_date = c.DateTime(storeType: "date"),
                        Assets_id = c.Int(),
                        Old_cost = c.Decimal(precision: 20, scale: 4),
                        Old_use_life = c.DateTime(storeType: "date"),
                        Adjustment_cost = c.Decimal(precision: 20, scale: 4),
                        Net_profit = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Gl_transaction_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asset", t => t.Assets_id)
                .Index(t => t.Assets_id);
            
            CreateTable(
                "dbo.Delete_assets_main",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_class_id = c.Int(nullable: false),
                        Description = c.String(maxLength: 200),
                        Number_of_parts = c.Int(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                        Assets_number = c.String(maxLength: 200),
                        Assets_custom_id = c.Int(),
                        Company_id = c.String(maxLength: 10),
                        Deleted_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Delete_fixed_assets_disposel",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Disposal_no = c.Int(),
                        Transaction_date = c.DateTime(),
                        Disposal_date = c.DateTime(storeType: "date"),
                        Depreication_up_to_date = c.DateTime(storeType: "date"),
                        Assets_id = c.Int(),
                        Disposal_amount = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Gl_transaction_id = c.Int(),
                        Currency_id = c.String(maxLength: 50),
                        Reference = c.String(nullable: false, maxLength: 200),
                        Delete_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Delete_fixed_assets_revaluate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Delete_date = c.DateTime(nullable: false,defaultValue:DateTime.Now,defaultValueSql:"GetDate()"),
                        Revaluate_no = c.String(maxLength: 200),
                        Transaction_date = c.DateTime(),
                        Revaluate_date = c.DateTime(storeType: "date"),
                        Assets_id = c.Int(),
                        Old_cost = c.Decimal(precision: 20, scale: 4),
                        Old_use_life = c.DateTime(storeType: "date"),
                        Adjustment_cost = c.Decimal(precision: 20, scale: 4),
                        Net_profit = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t =>  t.Id);
            
            CreateTable(
                "dbo.Deleted_assets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_class_id = c.Int(nullable: false),
                        Assets_main_id = c.Int(),
                        Description = c.String(nullable: false, maxLength: 200),
                        Foreign_name = c.String(maxLength: 200),
                        Type = c.Int(nullable: false),
                        Proparty_type = c.Int(nullable: false),
                        Acquisation_cost = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Scrap_value = c.Decimal(precision: 20, scale: 4),
                        Start_use = c.DateTime(nullable: false, storeType: "date"),
                        Start_derecation_date = c.DateTime(nullable: false, storeType: "date"),
                        Number_of_units = c.Int(),
                        Deactive_depraction = c.Boolean(nullable: false),
                        Fully_depraction = c.Boolean(nullable: false),
                        Disposal = c.Boolean(nullable: false),
                        Date_of_orgin = c.DateTime(nullable: false, storeType: "date"),
                        Adjustment_cost = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Date_of_Adjustmemt = c.DateTime(nullable: false, storeType: "date"),
                        Use_life = c.DateTime(nullable: false, storeType: "date"),
                        Deprecation_method = c.Int(nullable: false),
                        Include_scerap_value = c.Boolean(nullable: false),
                        Assets_number = c.String(maxLength: 200),
                        Deprecation_rate = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Book_id = c.Int(),
                        Assets_transaction_id = c.Int(),
                        Company_id = c.String(maxLength: 10),
                        Gl_transaction_id = c.Int(),
                        Deleted_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Deleted_assets_class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Class_id = c.String(maxLength: 200),
                        Description = c.String(maxLength: 200),
                        Deprecation_method = c.Int(),
                        Deperecation_rate = c.Decimal(precision: 20, scale: 4),
                        Active = c.Boolean(nullable: false),
                        Company_id = c.String(maxLength: 10),
                        Deleted_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Deleted_assets_serial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Assets_id = c.Int(),
                        Part_number = c.Int(),
                        Serial = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Deleted_fixed_assets_renewal",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Renewal_no = c.Int(),
                        Descroption = c.String(maxLength: 500),
                        Assets_id = c.Int(),
                        Renewal_amount = c.Decimal(precision: 20, scale: 4),
                        Deprecation_rate = c.Decimal(precision: 20, scale: 4),
                        Company_id = c.String(maxLength: 10),
                        Gl_transaction_id = c.Int(),
                        Use_life = c.Double(),
                        Renwal_date = c.DateTime(),
                        Currency_id = c.String(maxLength: 20),
                        Delete_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Deprecation_Setting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Auto_or_manual = c.Int(nullable: false),
                        Deprecation_calcualtion = c.Int(nullable: false),
                        Deprecation_jv = c.Int(nullable: false),
                        Change_deprecation_method = c.Boolean(nullable: false),
                        Can_add_assets_info = c.Boolean(nullable: false),
                        Company_id = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stoking_assets", "Assets_id", "dbo.Asset");
            DropForeignKey("dbo.Fixed_assets_revaluate", "Assets_id", "dbo.Asset");
            DropForeignKey("dbo.Fixed_assets_renewal", "Assets_id", "dbo.Asset");
            DropForeignKey("dbo.Fixed_assets_disposel", "Assets_id", "dbo.Asset");
            DropForeignKey("dbo.Depreication_assets_id_connection", "Assets_id", "dbo.Asset");
            DropForeignKey("dbo.Deprecation_temp_record", "Asset_id", "dbo.Asset");
            DropForeignKey("dbo.Deprecation_record", "Asset_id", "dbo.Asset");
            DropForeignKey("dbo.Deprecation_number_of_units", "Asset_id", "dbo.Asset");
            DropForeignKey("dbo.Asset", "Book_id", "dbo.Book");
            DropForeignKey("dbo.Assets_part_serial", "Assets_id", "dbo.Asset");
            DropForeignKey("dbo.Stoking_assets", "Assets_class_id", "dbo.Assets_class");
            DropForeignKey("dbo.Stocking_notes", "Stoking_assets_id", "dbo.Stoking_assets");
            DropForeignKey("dbo.Stocking_assets_transaction", "Stocking_assets_id", "dbo.Stoking_assets");
            DropForeignKey("dbo.New_assets_transaction", "Assets_class_id", "dbo.Assets_class");
            DropForeignKey("dbo.Asset", "Assets_transaction_id", "dbo.New_assets_transaction");
            DropForeignKey("dbo.Depreication_assets_connection", "Assets_class_id", "dbo.Assets_class");
            DropForeignKey("dbo.Depreication_assets_id_connection", "Deprecation_id", "dbo.Deprecation");
            DropForeignKey("dbo.Depreication_assets_connection", "Deprecation_id", "dbo.Deprecation");
            DropForeignKey("dbo.Deprecation_record", "Deprecation_id", "dbo.Deprecation");
            DropForeignKey("dbo.Deprecation_number_of_units", "Deprecation_id", "dbo.Deprecation_record");
            DropForeignKey("dbo.Deprecation", "Period", "dbo.Deprecation_periods");
            DropForeignKey("dbo.Deprecation_temp", "Period", "dbo.Deprecation_periods");
            DropForeignKey("dbo.Deprecation_temp_record", "Deprecation_id", "dbo.Deprecation_temp");
            DropForeignKey("dbo.Assets_main", "Assets_class_id", "dbo.Assets_class");
            DropForeignKey("dbo.Asset", "Assets_main_id", "dbo.Assets_main");
            DropForeignKey("dbo.Assets_accounts", "Class_id", "dbo.Assets_class");
            DropForeignKey("dbo.Asset", "Assets_class_id", "dbo.Assets_class");
            DropForeignKey("dbo.Additional_information", "Assets_id", "dbo.Asset");
            DropIndex("dbo.Fixed_assets_revaluate", new[] { "Assets_id" });
            DropIndex("dbo.Fixed_assets_renewal", new[] { "Assets_id" });
            DropIndex("dbo.Fixed_assets_disposel", new[] { "Assets_id" });
            DropIndex("dbo.Assets_part_serial", new[] { "Assets_id" });
            DropIndex("dbo.Stocking_notes", new[] { "Stoking_assets_id" });
            DropIndex("dbo.Stocking_assets_transaction", new[] { "Stocking_assets_id" });
            DropIndex("dbo.Stoking_assets", new[] { "Assets_class_id" });
            DropIndex("dbo.Stoking_assets", new[] { "Assets_id" });
            DropIndex("dbo.New_assets_transaction", new[] { "Assets_class_id" });
            DropIndex("dbo.Depreication_assets_id_connection", new[] { "Assets_id" });
            DropIndex("dbo.Depreication_assets_id_connection", new[] { "Deprecation_id" });
            DropIndex("dbo.Deprecation_number_of_units", new[] { "Deprecation_id" });
            DropIndex("dbo.Deprecation_number_of_units", new[] { "Asset_id" });
            DropIndex("dbo.Deprecation_record", new[] { "Deprecation_id" });
            DropIndex("dbo.Deprecation_record", new[] { "Asset_id" });
            DropIndex("dbo.Deprecation_temp_record", new[] { "Deprecation_id" });
            DropIndex("dbo.Deprecation_temp_record", new[] { "Asset_id" });
            DropIndex("dbo.Deprecation_temp", new[] { "Period" });
            DropIndex("dbo.Deprecation", new[] { "Period" });
            DropIndex("dbo.Depreication_assets_connection", new[] { "Assets_class_id" });
            DropIndex("dbo.Depreication_assets_connection", new[] { "Deprecation_id" });
            DropIndex("dbo.Assets_main", new[] { "Assets_class_id" });
            DropIndex("dbo.Assets_accounts", new[] { "Class_id" });
            DropIndex("dbo.Asset", new[] { "Assets_transaction_id" });
            DropIndex("dbo.Asset", new[] { "Book_id" });
            DropIndex("dbo.Asset", new[] { "Assets_main_id" });
            DropIndex("dbo.Asset", new[] { "Assets_class_id" });
            DropIndex("dbo.Additional_information", new[] { "Assets_id" });
            DropTable("dbo.Deprecation_Setting");
            DropTable("dbo.Deleted_fixed_assets_renewal");
            DropTable("dbo.Deleted_assets_serial");
            DropTable("dbo.Deleted_assets_class");
            DropTable("dbo.Deleted_assets");
            DropTable("dbo.Delete_fixed_assets_revaluate");
            DropTable("dbo.Delete_fixed_assets_disposel");
            DropTable("dbo.Delete_assets_main");
            DropTable("dbo.Fixed_assets_revaluate");
            DropTable("dbo.Fixed_assets_renewal");
            DropTable("dbo.Fixed_assets_disposel");
            DropTable("dbo.Book");
            DropTable("dbo.Assets_part_serial");
            DropTable("dbo.Stocking_notes");
            DropTable("dbo.Stocking_assets_transaction");
            DropTable("dbo.Stoking_assets");
            DropTable("dbo.New_assets_transaction");
            DropTable("dbo.Depreication_assets_id_connection");
            DropTable("dbo.Deprecation_number_of_units");
            DropTable("dbo.Deprecation_record");
            DropTable("dbo.Deprecation_temp_record");
            DropTable("dbo.Deprecation_temp");
            DropTable("dbo.Deprecation_periods");
            DropTable("dbo.Deprecation");
            DropTable("dbo.Depreication_assets_connection");
            DropTable("dbo.Assets_main");
            DropTable("dbo.Assets_accounts");
            DropTable("dbo.Assets_class");
            DropTable("dbo.Asset");
            DropTable("dbo.Additional_information");
        }
    }
}
