namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay6 : DbMigration
    {
        public override void Up()
        {
          //  RenameTable(name: "dbo.Group_setting", newName: "Payable_Group_setting");
            RenameTable(name: "dbo.Genral_setting", newName: "Payable_genral_setting");
            RenameTable(name: "dbo.Gl_account", newName: "Payable_gl_account");
            RenameTable(name: "dbo.UsersPageAccess", newName: "UsersPageAccesses");
         //   DropForeignKey("dbo.Group_setting", "Shipping_method", "dbo.Shippint_method");
         //   DropIndex("dbo.Payable_Group_setting", new[] { "Shipping_method" });
            RenameColumn(table: "dbo.Payable_gl_account", name: "Group_setting_id", newName: "Payable_Group_setting_id");
            RenameIndex(table: "dbo.Payable_gl_account", name: "IX_Group_setting_id", newName: "IX_Payable_Group_setting_id");
            CreateTable(
                "dbo.Shipping_method",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Ship_method = c.String(maxLength: 200),
                    Description = c.String(maxLength: 200),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Assign_payable_doc",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    VendorId = c.Int(name: "Vendor Id", nullable: false),
                    Trans_doc_type_id = c.Int(nullable: false),
                    Documenttype = c.Int(name: "Document type", nullable: false),
                    ApplayDate = c.DateTime(name: "Applay Date", nullable: false, storeType: "date"),
                    OrginalAmount = c.Decimal(name: "Orginal Amount", nullable: false, precision: 20, scale: 4),
                    ApplayAssign = c.Decimal(name: "Applay Assign", nullable: false, precision: 20, scale: 4),
                    Unassignamount = c.Decimal(name: "Unassign amount", nullable: false, precision: 20, scale: 4),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: true)
                .ForeignKey("dbo.Creditor_setting", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.VendorId)
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
                .ForeignKey("dbo.C_CheckBookSetting_table", t => t.CheckBook, cascadeDelete: true)
                .ForeignKey("dbo.AccountCurrencyDefinition_Table", t => t.CurrencyId, cascadeDelete: true)
                .ForeignKey("dbo.Payable_transactions_types", t => t.Transaction_id, cascadeDelete: true)
                .ForeignKey("dbo.Creditor_setting", t => t.VendorId, cascadeDelete: true)
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
                    Description = c.String(),
                    TransactionDate = c.DateTime(name: "Transaction Date", nullable: false, storeType: "date"),
                    PostingDate = c.DateTime(name: "Posting Date", nullable: false, storeType: "date"),
                    SystemRate = c.Decimal(name: "System Rate", nullable: false, precision: 20, scale: 4),
                    TransactionRate = c.Decimal(name: "Transaction Rate", nullable: false, precision: 20, scale: 4),
                    Creditor = c.Int(nullable: false),
                    PONumber = c.String(name: "P.O Number", maxLength: 200),
                    VDocumentNumber = c.String(name: "V.Document Number", maxLength: 200),
                    DocumentDate = c.DateTime(name: "Document Date", nullable: false, storeType: "date"),
                    PaymentTerm = c.Int(name: "Payment Term", nullable: false),
                    ShippingMethod = c.Int(name: "Shipping Method", nullable: false),
                    Purchase = c.Decimal(nullable: false, precision: 20, scale: 4),
                    TakenDiscount = c.Decimal(name: "Taken Discount", nullable: false, precision: 20, scale: 4),
                    Tax = c.Decimal(nullable: false, precision: 20, scale: 4),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Creditor_setting", t => t.Creditor, cascadeDelete: true)
                .ForeignKey("dbo.Payment_term", t => t.PaymentTerm, cascadeDelete: true)
                .ForeignKey("dbo.Shipping_method", t => t.ShippingMethod, cascadeDelete: true)
                .ForeignKey("dbo.Payable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: true)
                .Index(t => t.Trans_doc_type_id)
                .Index(t => t.Creditor)
                .Index(t => t.PaymentTerm)
                .Index(t => t.ShippingMethod);

            AddColumn("dbo.Payable_Group_setting", "Shipping_method_id", c => c.Int(nullable: false));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Reciept", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Payment", c => c.Decimal(precision: 20, scale: 4));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Balance", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.C_BankReconcile_table", "Bank_Statment_Ending_Balance", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Minimum_order", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Creditor_setting", "Maximum_order", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Minimum_transaction", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payable_Group_setting", "Maximum_transaction", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AlterColumn("dbo.Payment_term", "Amount", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            CreateIndex("dbo.Payable_Group_setting", "Shipping_method_id");
            AddForeignKey("dbo.Payable_Group_setting", "Shipping_method_id", "dbo.Shipping_method", "Id", cascadeDelete: false);
            DropColumn("dbo.Payable_Group_setting", "Shipping_method");
            DropTable("dbo.Shippint_method");
        }

        public override void Down()
        {
            CreateTable(
                "dbo.Shippint_method",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Shipping_method = c.String(maxLength: 200),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Payable_Group_setting", "Shipping_method", c => c.Int(nullable: false));
            DropForeignKey("dbo.Payable_transaction", "Trans_doc_type_id", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Payable_transaction", "Shipping Method", "dbo.Shipping_method");
            DropForeignKey("dbo.Payable_transaction", "Payment Term", "dbo.Payment_term");
            DropForeignKey("dbo.Payable_transaction", "Creditor", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_payment", "Vendor Id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Payable_payment", "Currency Id", "dbo.AccountCurrencyDefinition_Table");
            DropForeignKey("dbo.Payable_payment", "Check Book", "dbo.C_CheckBookSetting_table");
            DropForeignKey("dbo.Assign_payable_doc", "Vendor Id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Assign_payable_doc", "Trans_doc_type_id", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Payable_Group_setting", "Shipping_method_id", "dbo.Shipping_method");
            DropIndex("dbo.Payable_transaction", new[] { "Shipping Method" });
            DropIndex("dbo.Payable_transaction", new[] { "Payment Term" });
            DropIndex("dbo.Payable_transaction", new[] { "Creditor" });
            DropIndex("dbo.Payable_transaction", new[] { "Trans_doc_type_id" });
            DropIndex("dbo.Payable_payment", new[] { "Currency Id" });
            DropIndex("dbo.Payable_payment", new[] { "Check Book" });
            DropIndex("dbo.Payable_payment", new[] { "Transaction_id" });
            DropIndex("dbo.Payable_payment", new[] { "Vendor Id" });
            DropIndex("dbo.Assign_payable_doc", new[] { "Trans_doc_type_id" });
            DropIndex("dbo.Assign_payable_doc", new[] { "Vendor Id" });
            DropIndex("dbo.Payable_Group_setting", new[] { "Shipping_method_id" });
            AlterColumn("dbo.Payment_term", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Payable_Group_setting", "Maximum_transaction", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Payable_Group_setting", "Minimum_transaction", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Creditor_setting", "Maximum_order", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Creditor_setting", "Minimum_order", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.C_BankReconcile_table", "Bank_Statment_Ending_Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Balance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Payment", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.C_CheckbookTransactions_table", "C_Reciept", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Payable_Group_setting", "Shipping_method_id");
            DropTable("dbo.Payable_transaction");
            DropTable("dbo.Payable_payment");
            DropTable("dbo.Payable_transactions_types");
            DropTable("dbo.Assign_payable_doc");
            DropTable("dbo.Shipping_method");
            RenameIndex(table: "dbo.Payable_gl_account", name: "IX_Payable_Group_setting_id", newName: "IX_Group_setting_id");
            RenameColumn(table: "dbo.Payable_gl_account", name: "Payable_Group_setting_id", newName: "Group_setting_id");
            CreateIndex("dbo.Payable_Group_setting", "Shipping_method");
            AddForeignKey("dbo.Group_setting", "Shipping_method", "dbo.Shippint_method", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.UsersPageAccesses", newName: "UsersPageAccess");
            RenameTable(name: "dbo.Payable_gl_account", newName: "Gl_account");
            RenameTable(name: "dbo.Payable_genral_setting", newName: "Genral_setting");
            RenameTable(name: "dbo.Payable_Group_setting", newName: "Group_setting");
        }
    }
}
