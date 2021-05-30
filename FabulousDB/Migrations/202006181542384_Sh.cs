namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice", "Shipping_method_id", c => c.Int());
            CreateIndex("dbo.Inv_sales_invoice", "Shipping_method_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Shipping_method_id", "dbo.Receivable_shipping_method", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Shipping_method_id", "dbo.Receivable_shipping_method");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Shipping_method_id" });
            DropColumn("dbo.Inv_sales_invoice", "Shipping_method_id");
        }
    }
}
