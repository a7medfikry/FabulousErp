namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "FK_dbo.Inv_sales_invoice_dbo.Receivable_vendore_setting_Customer_id");
            CreateIndex("dbo.Inv_sales_invoice", "Customer_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Customer_id" });
        }
    }
}
