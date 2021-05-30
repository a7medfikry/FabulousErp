namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.Payment_term",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   Terms_id = c.String(maxLength: 50),
                   Inactive = c.Boolean(nullable: false),
                   Amount_type = c.Int(nullable: false),
                   Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                   Net_Days = c.Int(nullable: false),
                   Total_Days = c.Int(nullable: false),
                   Date_option = c.Int(nullable: false),
               })
               .PrimaryKey(t => t.Id);

            CreateTable(
             "dbo.Shippint_method",
             c => new
             {
                 Id = c.Int(nullable: false, identity: true),
                 Shipping_method = c.String(maxLength: 200),
                 Description = c.String(maxLength: 200),
             })
             .PrimaryKey(t => t.Id);


            CreateTable(
            "dbo.Payable_Group_setting",
            c => new
            {
                Id = c.Int(nullable: false, identity: true),
                Group_id = c.String(maxLength: 50),
                Group_description = c.String(maxLength: 200),
                Currency_id = c.Int(nullable: false),
                Payment_terms = c.Int(nullable: false),
                Def_Checkbook = c.Int(nullable: false),
                Password = c.String(maxLength: 50),
                Minimum_transaction = c.Decimal(nullable: false, precision: 18, scale: 2),
                Maximum_transaction = c.Decimal(nullable: false, precision: 18, scale: 2),
                Shipping_method = c.Int(nullable: false),
                Tax_group_id = c.Int(nullable: false),
                Inactive = c.Boolean(nullable: false),
                Credit_limit = c.Int(nullable: false),
                Minimum_payment = c.Int(nullable: false),
                Payment_per = c.Int(nullable: false),
                Revaluate = c.Boolean(nullable: false),
            })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.C_CheckBookSetting_table", t => t.Def_Checkbook, cascadeDelete: false)
            .ForeignKey("dbo.AccountCurrencyDefinition_Table", t => t.Currency_id, cascadeDelete: false)
            .ForeignKey("dbo.Payment_term", t => t.Payment_terms, cascadeDelete: false)
            .ForeignKey("dbo.Shippint_method", t => t.Shipping_method, cascadeDelete: false)
            .ForeignKey("dbo.C_TaxSetting_table", t => t.Tax_group_id, cascadeDelete: false)
            .Index(t => t.Currency_id)
            .Index(t => t.Payment_terms)
            .Index(t => t.Def_Checkbook)
            .Index(t => t.Shipping_method)
            .Index(t => t.Tax_group_id);

            CreateTable(
                "dbo.Creditor_setting",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Vendor_id = c.String(maxLength: 100),
                    Group_setting_id = c.Int(nullable: false),
                    Vendor_name = c.String(maxLength: 100),
                    Alies = c.String(maxLength: 50),
                    Currency_id = c.Int(nullable: false),
                    Payment_term_id = c.Int(nullable: false),
                    Password = c.String(maxLength: 50),
                    Def_Checkbook = c.Int(nullable: false),
                    Minimum_order = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Maximum_order = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Shipping_method_id = c.Int(nullable: false),
                    Tax_group_id = c.Int(nullable: false),
                    Inactive = c.Boolean(nullable: false),
                    Customer = c.Boolean(nullable: false),
                    Revaluate = c.Boolean(nullable: false),
                    Credit_limit = c.Int(nullable: false),
                    Minimum_payment = c.Int(nullable: false),
                    Payment_per = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CheckBookSetting_table", t => t.Def_Checkbook, cascadeDelete: false)
                .ForeignKey("dbo.AccountCurrencyDefinition_Table", t => t.Currency_id, cascadeDelete: false)
                .ForeignKey("dbo.Payable_Group_setting", t => t.Group_setting_id, cascadeDelete: false)
                .ForeignKey("dbo.Payment_term", t => t.Payment_term_id, cascadeDelete: false)
                .ForeignKey("dbo.Shippint_method", t => t.Shipping_method_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Tax_group_id, cascadeDelete: false)
                .Index(t => t.Group_setting_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Payment_term_id)
                .Index(t => t.Def_Checkbook)
                .Index(t => t.Shipping_method_id)
                .Index(t => t.Tax_group_id);

            CreateTable(
                "dbo.Address_info",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 50),
                        State = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        Post_code = c.String(maxLength: 50),
                        Phone_number = c.String(maxLength: 100),
                        Fax = c.String(maxLength: 100),
                        Contact_person = c.String(maxLength: 100),
                        Mobile_number = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Creditor_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creditor_setting", t => t.Creditor_id, cascadeDelete: false)
                .Index(t => t.Creditor_id);
            
            
            
        
            
           
            
         
            CreateTable(
                "dbo.Aging_period",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        From = c.Int(nullable: false),
                        To = c.Int(nullable: false),
                        Date_option = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bank_info",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cheque_name = c.String(maxLength: 50),
                        Bank_name = c.String(maxLength: 50),
                        Branch = c.String(maxLength: 100),
                        Account_name = c.String(maxLength: 50),
                        Account_number = c.String(maxLength: 50),
                        Swift_code = c.String(maxLength: 100),
                        Bank_address = c.String(maxLength: 200),
                        Iban = c.String(maxLength: 100),
                        Creditor_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creditor_setting", t => t.Creditor_id, cascadeDelete: false)
                .Index(t => t.Creditor_id);
            
            CreateTable(
                "dbo.Genral_setting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Doc_type = c.Int(nullable: false),
                        Checked = c.Boolean(nullable: false),
                        Next_number = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gl_account",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group_setting_id = c.Int(),
                        Creditor_setting_id = c.Int(),
                        Account_payable_C_AID = c.Int(),
                        Accrued_purchase_C_AID = c.Int(),
                        Purchase_C_AID = c.Int(),
                        Purchase_variance_C_AID = c.Int(),
                        Returne_C_AID = c.Int(),
                        Taken_discount_C_AID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Account_payable_C_AID)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Accrued_purchase_C_AID)
                .ForeignKey("dbo.Creditor_setting", t => t.Creditor_setting_id)
                .ForeignKey("dbo.Payable_Group_setting", t => t.Group_setting_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_C_AID)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_variance_C_AID)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Returne_C_AID)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Taken_discount_C_AID)
                .Index(t => t.Group_setting_id)
                .Index(t => t.Creditor_setting_id)
                .Index(t => t.Account_payable_C_AID)
                .Index(t => t.Accrued_purchase_C_AID)
                .Index(t => t.Purchase_C_AID)
                .Index(t => t.Purchase_variance_C_AID)
                .Index(t => t.Returne_C_AID)
                .Index(t => t.Taken_discount_C_AID);
            
            CreateTable(
                "dbo.Legal_info",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tax_file_no = c.String(maxLength: 200),
                        Tax_Id = c.String(maxLength: 200),
                        Commercial_register = c.String(maxLength: 200),
                        Social_insurance = c.String(maxLength: 200),
                        Creditor_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creditor_setting", t => t.Creditor_id, cascadeDelete: false)
                .Index(t => t.Creditor_id);
            
            CreateTable(
                "dbo.Other_option",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Option = c.Int(nullable: false),
                        Checked = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Legal_info", "Creditor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Gl_account", "Taken_discount_C_AID", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Gl_account", "Returne_C_AID", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Gl_account", "Purchase_variance_C_AID", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Gl_account", "Purchase_C_AID", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Gl_account", "Group_setting_id", "dbo.Payable_Group_setting");
            DropForeignKey("dbo.Gl_account", "Creditor_setting_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Gl_account", "Accrued_purchase_C_AID", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Gl_account", "Account_payable_C_AID", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Bank_info", "Creditor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Address_info", "Creditor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Creditor_setting", "Tax_group_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Creditor_setting", "Shipping_method_id", "dbo.Shippint_method");
            DropForeignKey("dbo.Creditor_setting", "Payment_term_id", "dbo.Payment_term");
            DropForeignKey("dbo.Creditor_setting", "Group_setting_id", "dbo.Payable_Group_setting");
            DropForeignKey("dbo.Payable_Group_setting", "Tax_group_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Payable_Group_setting", "Shipping_method", "dbo.Shippint_method");
            DropForeignKey("dbo.Payable_Group_setting", "Payment_terms", "dbo.Payment_term");
            DropForeignKey("dbo.Payable_Group_setting", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropForeignKey("dbo.Payable_Group_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropForeignKey("dbo.Creditor_setting", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropForeignKey("dbo.Creditor_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropIndex("dbo.Legal_info", new[] { "Creditor_id" });
            DropIndex("dbo.Gl_account", new[] { "Taken_discount_C_AID" });
            DropIndex("dbo.Gl_account", new[] { "Returne_C_AID" });
            DropIndex("dbo.Gl_account", new[] { "Purchase_variance_C_AID" });
            DropIndex("dbo.Gl_account", new[] { "Purchase_C_AID" });
            DropIndex("dbo.Gl_account", new[] { "Accrued_purchase_C_AID" });
            DropIndex("dbo.Gl_account", new[] { "Account_payable_C_AID" });
            DropIndex("dbo.Gl_account", new[] { "Creditor_setting_id" });
            DropIndex("dbo.Gl_account", new[] { "Group_setting_id" });
            DropIndex("dbo.Bank_info", new[] { "Creditor_id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Tax_group_id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Shipping_method" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Def_Checkbook" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Payment_terms" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Currency_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Tax_group_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Shipping_method_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Def_Checkbook" });
            DropIndex("dbo.Creditor_setting", new[] { "Payment_term_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Currency_id" });
            DropIndex("dbo.Creditor_setting", new[] { "Group_setting_id" });
            DropIndex("dbo.Address_info", new[] { "Creditor_id" });
            DropTable("dbo.Other_option");
            DropTable("dbo.Legal_info");
            DropTable("dbo.Gl_account");
            DropTable("dbo.Genral_setting");
            DropTable("dbo.Bank_info");
            DropTable("dbo.Aging_period");
            DropTable("dbo.Shippint_method");
            DropTable("dbo.Payment_term");
            DropTable("dbo.Payable_Group_setting");
            DropTable("dbo.Creditor_setting");
            DropTable("dbo.Address_info");
        }
    }
}
