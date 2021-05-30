namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvSalesVenore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice", "Vendore_id", c => c.Int());
            CreateIndex("dbo.Inv_sales_invoice", "Vendore_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Vendore_id", "dbo.Payable_creditor_setting", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Vendore_id", "dbo.Payable_creditor_setting");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Vendore_id" });
            DropColumn("dbo.Inv_sales_invoice", "Vendore_id");
        }
    }
}
