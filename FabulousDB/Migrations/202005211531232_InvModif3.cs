namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvModif3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_po_item_transfer", "Sales_id", "dbo.Inv_sales_invoice");
            DropIndex("dbo.Inv_po_item_transfer", new[] { "Sales_id" });
            AlterColumn("dbo.Inv_po_item_transfer", "Sales_id", c => c.Int());
            CreateIndex("dbo.Inv_po_item_transfer", "Sales_id");
            AddForeignKey("dbo.Inv_po_item_transfer", "Sales_id", "dbo.Inv_sales_invoice", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_po_item_transfer", "Sales_id", "dbo.Inv_sales_invoice");
            DropIndex("dbo.Inv_po_item_transfer", new[] { "Sales_id" });
            AlterColumn("dbo.Inv_po_item_transfer", "Sales_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_po_item_transfer", "Sales_id");
            AddForeignKey("dbo.Inv_po_item_transfer", "Sales_id", "dbo.Inv_sales_invoice", "Id", cascadeDelete: true);
        }
    }
}
