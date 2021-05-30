namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice", "Receivable_id", c => c.Int());
            AddColumn("dbo.Inv_sales_invoice_items", "Cost_price", c => c.Decimal(precision: 18, scale: 2));
          
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_sales_invoice_items", "Cost_price");
            DropColumn("dbo.Inv_sales_invoice", "Receivable_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
