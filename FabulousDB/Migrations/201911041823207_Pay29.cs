namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay29 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types");
            DropIndex("dbo.Payable_payment", new[] { "Transaction_id" });
            AlterColumn("dbo.Payable_payment", "Transaction_id", c => c.Int());
            CreateIndex("dbo.Payable_payment", "Transaction_id");
            AddForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types");
            DropIndex("dbo.Payable_payment", new[] { "Transaction_id" });
            AlterColumn("dbo.Payable_payment", "Transaction_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_payment", "Transaction_id");
            AddForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types", "Id", cascadeDelete: true);
        }
    }
}
