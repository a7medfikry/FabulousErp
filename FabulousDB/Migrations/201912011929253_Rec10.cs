namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assign_Receivable_doc",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Assign_no = c.Int(nullable: false),
                    Vendor_id = c.Int(nullable: false),
                    Trans_doc_type_id = c.Int(nullable: false),
                    Trans_doc_type_id_to = c.Int(nullable: false),
                    Doc_type = c.Int(nullable: false),
                    Doc_Num = c.String(),
                    Currency_id = c.String(maxLength: 50),
                    Applay_date = c.DateTime(nullable: false, storeType: "date"),
                    Creation_date = c.DateTime(nullable: false),
                    Orginal_amount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Applay_assign = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Unassign_amount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Taken_discount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Earn_or_lose = c.Decimal(nullable: false, precision: 30, scale: 9),
                    JournalEntry = c.Int(nullable: false),
                    Transaction_rate = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Is_void = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Trans_doc_type_id_to, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Vendor_id, cascadeDelete: false)
                .Index(t => t.Vendor_id)
                .Index(t => t.Trans_doc_type_id)
                .Index(t => t.Trans_doc_type_id_to)
                .Index(t => t.Currency_id);

           
            CreateTable(
                "dbo.Receivable_transactions_types",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Trx_num = c.Int(nullable: false),
                    Documenttype = c.Int(name: "Document type", nullable: false),
                    Counter = c.Int(nullable: false),
                    Origin = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_payment",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Payment_no = c.Int(),
                    Transaction_date = c.DateTime(nullable: false, storeType: "date"),
                    Posting_date = c.DateTime(nullable: false, storeType: "date"),
                    Vendor_id = c.Int(nullable: false),
                    Transaction_id = c.Int(),
                    Reference = c.String(nullable: false, maxLength: 500),
                    Check_book_id = c.Int(nullable: false),
                    Check_book_transaction_id = c.Int(),
                    Withdraw_number = c.String(maxLength: 200),
                    Currency_id = c.String(maxLength: 50),
                    Cheque_number = c.String(maxLength: 200),
                    Due_date = c.DateTime(storeType: "date"),
                    Creation_date = c.DateTime(nullable: false),
                    System_rate = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Transaction_rate = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Orginal_amount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Taken_discount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Journal_number = c.Int(nullable: false),
                    Trans_doc_type_id = c.Int(nullable: false),
                    Is_void = c.Boolean(nullable: false),
                    Cash_type = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CheckBookSetting_table", t => t.Check_book_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CheckbookTransactions_table", t => t.Check_book_transaction_id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Receivable_transaction", t => t.Transaction_id)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Vendor_id, cascadeDelete: false)
                .Index(t => t.Vendor_id)
                .Index(t => t.Transaction_id)
                .Index(t => t.Check_book_id)
                .Index(t => t.Check_book_transaction_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Trans_doc_type_id);

            CreateTable(
                "dbo.Receivable_transaction",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Trans_doc_type_id = c.Int(nullable: false),
                    Doc_type = c.Int(nullable: false),
                    Desc = c.String(nullable: false),
                    Transaction_date = c.DateTime(nullable: false, storeType: "date"),
                    Posting_date = c.DateTime(nullable: false, storeType: "date"),
                    System_rate = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Transaction_rate = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Currency_id = c.String(maxLength: 50),
                    Vendor_id = c.Int(),
                    PONumber = c.String(maxLength: 200),
                    VDocument_number = c.String(maxLength: 200),
                    Doc_date = c.DateTime(nullable: false, storeType: "date"),
                    Due_date = c.DateTime(storeType: "date"),
                    Creation_date = c.DateTime(nullable: false),
                    Payment_terms_id = c.Int(),
                    Shipping_method_id = c.Int(),
                    Purchase = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Taken_discount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Tax = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Journal_number = c.Int(nullable: false),
                    Is_void = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Receivable_payment_term", t => t.Payment_terms_id)
                .ForeignKey("dbo.Receivable_shipping_method", t => t.Shipping_method_id)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Vendor_id)
                .Index(t => t.Trans_doc_type_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Vendor_id)
                .Index(t => t.Payment_terms_id)
                .Index(t => t.Shipping_method_id);

            CreateTable(
                "dbo.Receivable_payment_term",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Terms_id = c.String(maxLength: 50),
                    Inactive = c.Boolean(nullable: false),
                    Amount_type = c.Int(nullable: false),
                    Amount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Net_Days = c.Int(nullable: false),
                    Total_Days = c.Int(nullable: false),
                    Date_option = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_shipping_method",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Ship_method = c.String(maxLength: 200),
                    Description = c.String(maxLength: 200),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_vendore_setting",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Vendor_id = c.String(nullable: false, maxLength: 100),
                    Group_setting_id = c.Int(),
                    Vendor_name = c.String(maxLength: 100),
                    Alies = c.String(maxLength: 50),
                    Payment_term_id = c.Int(),
                    Password = c.String(maxLength: 50),
                    Def_Checkbook = c.Int(),
                    Minimum_order = c.Decimal(precision: 30, scale: 9),
                    Maximum_order = c.Decimal(precision: 30, scale: 9),
                    Shipping_method_id = c.Int(),
                    Tax_group_id = c.Int(),
                    Inactive = c.Boolean(),
                    Customer = c.Boolean(),
                    Revaluate = c.Boolean(),
                    Credit_limit_amount = c.Decimal(precision: 30, scale: 9),
                    Min_payment_amount = c.Decimal(precision: 30, scale: 9),
                    Credit_limit = c.Int(nullable: false),
                    Minimum_payment = c.Int(nullable: false),
                    Payment_per = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CheckBookSetting_table", t => t.Def_Checkbook)
                .ForeignKey("dbo.Receivable_Group_setting", t => t.Group_setting_id)
                .ForeignKey("dbo.Receivable_payment_term", t => t.Payment_term_id)
                .ForeignKey("dbo.Receivable_shipping_method", t => t.Shipping_method_id)
                .ForeignKey("dbo.TaxGroup_table", t => t.Tax_group_id)
                .Index(t => t.Group_setting_id)
                .Index(t => t.Payment_term_id)
                .Index(t => t.Def_Checkbook)
                .Index(t => t.Shipping_method_id)
                .Index(t => t.Tax_group_id);

            CreateTable(
                "dbo.Receivable_Group_setting",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Group_id = c.String(nullable: false, maxLength: 50),
                    Group_description = c.String(maxLength: 200),
                    Currency_id = c.String(maxLength: 50),
                    Payment_terms = c.Int(),
                    Def_Checkbook = c.Int(),
                    Password = c.String(maxLength: 50),
                    Minimum_transaction = c.Decimal(precision: 30, scale: 9),
                    Maximum_transaction = c.Decimal(precision: 30, scale: 9),
                    Shipping_method_id = c.Int(),
                    Tax_group_id = c.Int(),
                    Inactive = c.Boolean(nullable: false),
                    Credit_limit = c.Int(),
                    Minimum_payment = c.Int(),
                    Payment_per = c.Int(),
                    Revaluate = c.Boolean(),
                    Credit_limit_amount = c.Decimal(precision: 30, scale: 9),
                    Min_payment_amount = c.Decimal(precision: 30, scale: 9),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CheckBookSetting_table", t => t.Def_Checkbook)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Receivable_payment_term", t => t.Payment_terms)
                .ForeignKey("dbo.Receivable_shipping_method", t => t.Shipping_method_id)
                .ForeignKey("dbo.TaxGroup_table", t => t.Tax_group_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Payment_terms)
                .Index(t => t.Def_Checkbook)
                .Index(t => t.Shipping_method_id)
                .Index(t => t.Tax_group_id);

            CreateTable(
                "dbo.Receivable_address_info",
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
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Creditor_id, cascadeDelete: false)
                .Index(t => t.Creditor_id);

            CreateTable(
                "dbo.Receivable_aging_date_option",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Date_option = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_aging_period",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 100),
                    From = c.Int(nullable: false),
                    To = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_Assign_void",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Assign_no = c.Int(nullable: false),
                    Vendor_id = c.Int(nullable: false),
                    Trans_doc_type_id = c.Int(nullable: false),
                    Trans_doc_type_id_to = c.Int(nullable: false),
                    Doc_type = c.Int(nullable: false),
                    Doc_Num = c.String(),
                    Currency_id = c.String(maxLength: 50),
                    Applay_date = c.DateTime(nullable: false, storeType: "date"),
                    Creation_date = c.DateTime(),
                    Orginal_amount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Applay_assign = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Unassign_amount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Taken_discount = c.Decimal(nullable: false, precision: 30, scale: 9),
                    Earn_or_lose = c.Decimal(nullable: false, precision: 30, scale: 9),
                    JournalEntry = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Trans_doc_type_id_to, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Vendor_id, cascadeDelete: false)
                .Index(t => t.Vendor_id)
                .Index(t => t.Trans_doc_type_id)
                .Index(t => t.Trans_doc_type_id_to)
                .Index(t => t.Currency_id);

            CreateTable(
                "dbo.Receivable_bank_info",
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
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Creditor_id, cascadeDelete: false)
                .Index(t => t.Creditor_id);

            CreateTable(
                "dbo.Receivable_genral_setting",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Doc_type = c.Int(nullable: false),
                    Checked = c.Boolean(nullable: false),
                    Next_number = c.String(maxLength: 50),
                    Receviable = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_gl_account",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Account_Receivable_id = c.Int(nullable: false),
                    Purchase_id = c.Int(nullable: false),
                    Taken_discount_id = c.Int(nullable: false),
                    Purchase_variance_id = c.Int(nullable: false),
                    Accrued_purchase_id = c.Int(nullable: false),
                    Returne_id = c.Int(nullable: false),
                    Receivable_Group_setting_id = c.Int(),
                    Creditor_setting_id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Account_Receivable_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Accrued_purchase_id, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Creditor_setting_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_variance_id, cascadeDelete: false)
                .ForeignKey("dbo.Receivable_Group_setting", t => t.Receivable_Group_setting_id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Returne_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Taken_discount_id, cascadeDelete: false)
                .Index(t => t.Account_Receivable_id)
                .Index(t => t.Purchase_id)
                .Index(t => t.Taken_discount_id)
                .Index(t => t.Purchase_variance_id)
                .Index(t => t.Accrued_purchase_id)
                .Index(t => t.Returne_id)
                .Index(t => t.Receivable_Group_setting_id)
                .Index(t => t.Creditor_setting_id);

            CreateTable(
                "dbo.Receivable_legal_info",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Tax_file_no = c.String(maxLength: 200),
                    Tax_Id = c.String(maxLength: 200),
                    Commercial_register = c.String(maxLength: 200),
                    Social_insurance = c.String(maxLength: 200),
                    Vendore_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Vendore_id, cascadeDelete: false)
                .Index(t => t.Vendore_id);

            CreateTable(
                "dbo.Receivable_other_option",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Option = c.Int(nullable: false),
                    Checked = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_password_option",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    HasPassword = c.Boolean(nullable: false),
                    Password = c.String(maxLength: 50),
                    Option = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Receivable_vendore_currencies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Vendore_id = c.Int(nullable: false),
                    Currency_id = c.String(maxLength: 50),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Receivable_vendore_setting", t => t.Vendore_id, cascadeDelete: false)
                .Index(t => t.Vendore_id)
                .Index(t => t.Currency_id);

            CreateTable(
                "dbo.Receivable_void",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Transaction_date = c.DateTime(nullable: false),
                    Posting_date = c.DateTime(nullable: false),
                    Trx_id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Trx_id)
                .Index(t => t.Trx_id);

        }

        public override void Down()
        {
          
        }
    }
}
