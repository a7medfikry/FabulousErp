namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvNum : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_receivable_num",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Checkbook_id = c.Int(nullable: false),
                        Inv_sales_id = c.Int(nullable: false),
                        Inv_num_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CheckbookTransactions_table", t => t.Checkbook_id, cascadeDelete: true)
                .ForeignKey("dbo.Receivable_transactions_types", t => t.Inv_num_id, cascadeDelete: true)
                .ForeignKey("dbo.Inv_sales_invoice", t => t.Inv_sales_id, cascadeDelete: true)
                .Index(t => t.Checkbook_id)
                .Index(t => t.Inv_sales_id)
                .Index(t => t.Inv_num_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receivable_num", "Inv_sales_id", "dbo.Inv_sales_invoice");
            DropForeignKey("dbo.Inv_receivable_num", "Inv_num_id", "dbo.Receivable_transactions_types");
            DropForeignKey("dbo.Inv_receivable_num", "Checkbook_id", "dbo.C_CheckbookTransactions_table");
            DropIndex("dbo.Inv_receivable_num", new[] { "Inv_num_id" });
            DropIndex("dbo.Inv_receivable_num", new[] { "Inv_sales_id" });
            DropIndex("dbo.Inv_receivable_num", new[] { "Checkbook_id" });
            DropTable("dbo.Inv_receivable_num");
        }
    }
}
