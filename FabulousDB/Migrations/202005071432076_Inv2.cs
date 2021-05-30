namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inv2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice", "Payable_id", c => c.Int());
            CreateIndex("dbo.Inv_sales_invoice", "Payable_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Payable_id", "dbo.Payable_transaction", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Payable_id", "dbo.Payable_transaction");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Payable_id" });
            DropColumn("dbo.Inv_sales_invoice", "Payable_id");
        }
    }
}
