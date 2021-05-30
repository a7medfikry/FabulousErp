namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay12 : DbMigration
    {
        public override void Up()
        {
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
                        Minimum_order = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Maximum_order = c.Decimal(nullable: false, precision: 20, scale: 4),
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
                .ForeignKey("dbo.Shipping_method", t => t.Shipping_method_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Tax_group_id, cascadeDelete: false)
                .Index(t => t.Group_setting_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Payment_term_id)
                .Index(t => t.Def_Checkbook)
                .Index(t => t.Shipping_method_id)
                .Index(t => t.Tax_group_id);
            
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
                        Minimum_transaction = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Maximum_transaction = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Shipping_method_id = c.Int(nullable: false),
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
                .ForeignKey("dbo.Shipping_method", t => t.Shipping_method_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Tax_group_id, cascadeDelete: false)
                .Index(t => t.Currency_id)
                .Index(t => t.Payment_terms)
                .Index(t => t.Def_Checkbook)
                .Index(t => t.Shipping_method_id)
                .Index(t => t.Tax_group_id);
            
            CreateTable(
                "dbo.Assign_payable_doc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Vendor_id = c.Int(nullable: false),
                        Trans_doc_type_id = c.Int(nullable: false),
                        Doc_type = c.Int(nullable: false),
                        Applay_date = c.DateTime(nullable: false, storeType: "date"),
                        Orginal_amount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Applay_assign = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Unassign_amount = c.Decimal(nullable: false, precision: 20, scale: 4),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: false)
                .ForeignKey("dbo.Creditor_setting", t => t.Vendor_id, cascadeDelete: false)
                .Index(t => t.Vendor_id)
                .Index(t => t.Trans_doc_type_id);
            
            CreateTable(
                "dbo.Payable_transactions_types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Trx_num = c.Int(nullable: false),
                        Documenttype = c.Int(name: "Document type", nullable: false),
                        Counter = c.Int(nullable: false),
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
                "dbo.Payable_gl_account",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account_payable_id = c.Int(),
                        Purchase_id = c.Int(),
                        Taken_discount_id = c.Int(),
                        Purchase_variance_id = c.Int(),
                        Accrued_purchase_id = c.Int(),
                        Returne_id = c.Int(),
                        Payable_Group_setting_id = c.Int(),
                        Creditor_setting_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Account_payable_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Accrued_purchase_id)
                .ForeignKey("dbo.Creditor_setting", t => t.Creditor_setting_id)
                .ForeignKey("dbo.Payable_Group_setting", t => t.Payable_Group_setting_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_variance_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Returne_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Taken_discount_id)
                .Index(t => t.Account_payable_id)
                .Index(t => t.Purchase_id)
                .Index(t => t.Taken_discount_id)
                .Index(t => t.Purchase_variance_id)
                .Index(t => t.Accrued_purchase_id)
                .Index(t => t.Returne_id)
                .Index(t => t.Payable_Group_setting_id)
                .Index(t => t.Creditor_setting_id);
            
            CreateTable(
                "dbo.Payable_payment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Payment_no = c.Int(nullable: false),
                        TransactionDate = c.DateTime(name: "Transaction Date", nullable: false, storeType: "date"),
                        PostingDate = c.DateTime(name: "Posting Date", nullable: false, storeType: "date"),
                        VendorId = c.Int(name: "Vendor Id", nullable: false),
                        PaymentType = c.Int(name: "Payment Type", nullable: false),
                        Transaction_id = c.Int(nullable: false),
                        nvarchar = c.String(maxLength: 500),
                        CheckBook = c.Int(name: "Check Book", nullable: false),
                        Withdrawnumber = c.String(name: "Withdraw number", maxLength: 200),
                        CurrencyId = c.Int(name: "Currency Id", nullable: false),
                        ChequeNumber = c.String(name: "Cheque Number", maxLength: 200),
                        DueDate = c.DateTime(name: "Due Date", nullable: false, storeType: "date"),
                        SystemRate = c.Decimal(name: "System Rate", nullable: false, precision: 20, scale: 4),
                        TransactionRate = c.Decimal(name: "Transaction Rate", nullable: false, precision: 20, scale: 4),
                        OrginalAmount = c.Decimal(name: "Orginal Amount", nullable: false, precision: 20, scale: 4),
                        TakenDiscount = c.Decimal(name: "Taken Discount", nullable: false, precision: 20, scale: 4),
                        PaidAmount = c.Decimal(name: "Paid Amount", nullable: false, precision: 20, scale: 4),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CheckBookSetting_table", t => t.CheckBook, cascadeDelete: false)
                .ForeignKey("dbo.AccountCurrencyDefinition_Table", t => t.CurrencyId, cascadeDelete: false)
                .ForeignKey("dbo.Payable_transactions_types", t => t.Transaction_id, cascadeDelete: false)
                .ForeignKey("dbo.Creditor_setting", t => t.VendorId, cascadeDelete: false)
                .Index(t => t.VendorId)
                .Index(t => t.Transaction_id)
                .Index(t => t.CheckBook)
                .Index(t => t.CurrencyId);
            
            CreateTable(
                "dbo.Payable_transaction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Trans_doc_type_id = c.Int(nullable: false),
                        Doc_type = c.Int(nullable: false),
                        Desc = c.String(),
                        Transation_date = c.DateTime(nullable: false, storeType: "date"),
                        Posting_date = c.DateTime(nullable: false, storeType: "date"),
                        System_rate = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Transaction_rate = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Vendor_id = c.Int(nullable: false),
                        PONumber = c.String(maxLength: 200),
                        VDocument_number = c.String(maxLength: 200),
                        Doc_date = c.DateTime(nullable: false, storeType: "date"),
                        Payment_terms_id = c.Int(nullable: false),
                        ShippingMethod = c.Int(name: "Shipping Method", nullable: false),
                        Purchase = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Taken_discount = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Tax = c.Decimal(nullable: false, precision: 20, scale: 4),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creditor_setting", t => t.Vendor_id, cascadeDelete: false)
                .ForeignKey("dbo.Payment_term", t => t.Payment_terms_id, cascadeDelete: false)
                .ForeignKey("dbo.Shipping_method", t => t.ShippingMethod, cascadeDelete: false)
                .ForeignKey("dbo.Payable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: false)
                .Index(t => t.Trans_doc_type_id)
                .Index(t => t.Vendor_id)
                .Index(t => t.Payment_terms_id)
                .Index(t => t.ShippingMethod);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_transaction", "Trans_doc_type_id", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Payable_transaction", "Shipping Method", "dbo.Shipping_method");
            DropForeignKey("dbo.Payable_transaction", "Payment_terms_id", "dbo.Payment_term");
            DropForeignKey("dbo.Payable_transaction", "Vendor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_payment", "Vendor Id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Payable_payment", "Currency Id", "dbo.AccountCurrencyDefinition_Table");
            DropForeignKey("dbo.Payable_payment", "Check Book", "dbo.C_CheckBookSetting_table");
            DropForeignKey("dbo.Payable_gl_account", "Taken_discount_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Returne_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Purchase_variance_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Purchase_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting");
            DropForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_gl_account", "Accrued_purchase_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Payable_gl_account", "Account_payable_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Legal_info", "Creditor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Bank_info", "Creditor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Assign_payable_doc", "Vendor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Assign_payable_doc", "Trans_doc_type_id", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Address_info", "Creditor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Creditor_setting", "Tax_group_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Creditor_setting", "Shipping_method_id", "dbo.Shipping_method");
            DropForeignKey("dbo.Creditor_setting", "Payment_term_id", "dbo.Payment_term");
            DropForeignKey("dbo.Creditor_setting", "Group_setting_id", "dbo.Payable_Group_setting");
            DropForeignKey("dbo.Payable_Group_setting", "Tax_group_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Payable_Group_setting", "Shipping_method_id", "dbo.Shipping_method");
            DropForeignKey("dbo.Payable_Group_setting", "Payment_terms", "dbo.Payment_term");
            DropForeignKey("dbo.Payable_Group_setting", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropForeignKey("dbo.Payable_Group_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropForeignKey("dbo.Creditor_setting", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropForeignKey("dbo.Creditor_setting", "Def_Checkbook", "dbo.C_CheckBookSetting_table");
            DropIndex("dbo.Payable_transaction", new[] { "Shipping Method" });
            DropIndex("dbo.Payable_transaction", new[] { "Payment_terms_id" });
            DropIndex("dbo.Payable_transaction", new[] { "Vendor_id" });
            DropIndex("dbo.Payable_transaction", new[] { "Trans_doc_type_id" });
            DropIndex("dbo.Payable_payment", new[] { "Currency Id" });
            DropIndex("dbo.Payable_payment", new[] { "Check Book" });
            DropIndex("dbo.Payable_payment", new[] { "Transaction_id" });
            DropIndex("dbo.Payable_payment", new[] { "Vendor Id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Creditor_setting_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Payable_Group_setting_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Returne_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Accrued_purchase_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Purchase_variance_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Taken_discount_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Purchase_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Account_payable_id" });
            DropIndex("dbo.Legal_info", new[] { "Creditor_id" });
            DropIndex("dbo.Bank_info", new[] { "Creditor_id" });
            DropIndex("dbo.Assign_payable_doc", new[] { "Trans_doc_type_id" });
            DropIndex("dbo.Assign_payable_doc", new[] { "Vendor_id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Tax_group_id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Shipping_method_id" });
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
            DropTable("dbo.Payable_transaction");
            DropTable("dbo.Payable_payment");
            DropTable("dbo.Payable_gl_account");
            DropTable("dbo.Legal_info");
            DropTable("dbo.Bank_info");
            DropTable("dbo.Payable_transactions_types");
            DropTable("dbo.Assign_payable_doc");
            DropTable("dbo.Payable_Group_setting");
            DropTable("dbo.Creditor_setting");
            DropTable("dbo.Address_info");
        }
    }
}
