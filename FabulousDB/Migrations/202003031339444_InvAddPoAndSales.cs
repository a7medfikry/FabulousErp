namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvAddPoAndSales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice_items", "Inv_po_item", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_sales_invoice_items", "Inv_po_item");
            AddForeignKey("dbo.Inv_sales_invoice_items", "Inv_po_item", "dbo.Inv_po_items", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice_items", "Inv_po_item", "dbo.Inv_po_items");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Inv_po_item" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_sales_invoice_items", "Inv_po_item");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
