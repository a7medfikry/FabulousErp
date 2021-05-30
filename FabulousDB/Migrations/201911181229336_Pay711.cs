namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay711 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Payable_Assign_void",
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
                .ForeignKey("dbo.Payable_transactions_types", t => t.Trans_doc_type_id, cascadeDelete: false)
                .ForeignKey("dbo.Payable_transactions_types", t => t.Trans_doc_type_id_to, cascadeDelete: false)
                .ForeignKey("dbo.Creditor_setting", t => t.Vendor_id, cascadeDelete: false)
                .Index(t => t.Vendor_id)
                .Index(t => t.Trans_doc_type_id)
                .Index(t => t.Trans_doc_type_id_to)
                .Index(t => t.Currency_id);
            
            CreateTable(
                "dbo.Payable_void",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false,defaultValueSql:"GetDate()"),
                        Transaction_id = c.Int(),
                        Payament_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Payable_payment", t => t.Payament_id)
                .ForeignKey("dbo.Payable_transaction", t => t.Transaction_id)
                .Index(t => t.Transaction_id)
                .Index(t => t.Payament_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_void", "Transaction_id", "dbo.Payable_transaction");
            DropForeignKey("dbo.Payable_void", "Payament_id", "dbo.Payable_payment");
            DropForeignKey("dbo.Payable_Assign_void", "Vendor_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_Assign_void", "Trans_doc_type_id_to", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Payable_Assign_void", "Trans_doc_type_id", "dbo.Payable_transactions_types");
            DropForeignKey("dbo.Payable_Assign_void", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Payable_void", new[] { "Payament_id" });
            DropIndex("dbo.Payable_void", new[] { "Transaction_id" });
            DropIndex("dbo.Payable_Assign_void", new[] { "Currency_id" });
            DropIndex("dbo.Payable_Assign_void", new[] { "Trans_doc_type_id_to" });
            DropIndex("dbo.Payable_Assign_void", new[] { "Trans_doc_type_id" });
            DropIndex("dbo.Payable_Assign_void", new[] { "Vendor_id" });
            DropTable("dbo.Payable_void");
            DropTable("dbo.Payable_Assign_void");
        }
    }
}
