namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPoSales : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_sales_invoice_items", "Inv_po_item", "dbo.Inv_po_items");
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Inv_po_item" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AddColumn("dbo.Inv_sales_invoice_items", "Inv_po_receive", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_sales_invoice", "Inv_po_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_sales_invoice", "Inv_po_Id");
            CreateIndex("dbo.Inv_sales_invoice_items", "Inv_po_receive");
            AddForeignKey("dbo.Inv_sales_invoice", "Inv_po_Id", "dbo.Inv_receive_po", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Inv_sales_invoice_items", "Inv_po_receive", "dbo.Inv_receive_po", "Id", cascadeDelete: false);
            DropColumn("dbo.Inv_sales_invoice_items", "Inv_po_item");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_invoice_items", "Inv_po_item", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inv_sales_invoice_items", "Inv_po_receive", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_sales_invoice", "Inv_po_Id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "Inv_po_receive" });
            DropIndex("dbo.Inv_sales_invoice", new[] { "Inv_po_Id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_sales_invoice", "Inv_po_Id");
            DropColumn("dbo.Inv_sales_invoice_items", "Inv_po_receive");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_sales_invoice_items", "Inv_po_item");
            AddForeignKey("dbo.Inv_sales_invoice_items", "Inv_po_item", "dbo.Inv_po_items", "Id", cascadeDelete: true);
        }
    }
}
