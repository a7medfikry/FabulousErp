namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay72 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_void", "Payament_id", "dbo.Payable_payment");
            DropForeignKey("dbo.Payable_void", "Transaction_id", "dbo.Payable_transaction");
            DropIndex("dbo.Payable_void", new[] { "Transaction_id" });
            DropIndex("dbo.Payable_void", new[] { "Payament_id" });
            AddColumn("dbo.Payable_void", "Trx_id", c => c.Int());
            CreateIndex("dbo.Payable_void", "Trx_id");
            AddForeignKey("dbo.Payable_void", "Trx_id", "dbo.Payable_transactions_types", "Id");
            DropColumn("dbo.Payable_void", "Transaction_id");
            DropColumn("dbo.Payable_void", "Payament_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_void", "Payament_id", c => c.Int());
            AddColumn("dbo.Payable_void", "Transaction_id", c => c.Int());
            DropForeignKey("dbo.Payable_void", "Trx_id", "dbo.Payable_transactions_types");
            DropIndex("dbo.Payable_void", new[] { "Trx_id" });
            DropColumn("dbo.Payable_void", "Trx_id");
            CreateIndex("dbo.Payable_void", "Payament_id");
            CreateIndex("dbo.Payable_void", "Transaction_id");
            AddForeignKey("dbo.Payable_void", "Transaction_id", "dbo.Payable_transaction", "Id");
            AddForeignKey("dbo.Payable_void", "Payament_id", "dbo.Payable_payment", "Id");
        }
    }
}
