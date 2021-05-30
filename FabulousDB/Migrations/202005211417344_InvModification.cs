namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvModification : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_receive_po_items", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_sales_invoice_items", "UOM_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_receive_po_items", new[] { "UOM_id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "UOM_id" });
            AddColumn("dbo.Inv_po_item_transfer", "Orginal_Qty", c => c.Single(nullable: false));
            AddColumn("dbo.Inv_po_item_transfer", "Po_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_po_item_transfer", "Sales_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po_items", "UOM_id", c => c.Int());
            AlterColumn("dbo.Inv_sales_invoice_items", "UOM_id", c => c.Int());
            CreateIndex("dbo.Inv_receive_po_items", "UOM_id");
            CreateIndex("dbo.Inv_sales_invoice_items", "UOM_id");
            CreateIndex("dbo.Inv_po_item_transfer", "Po_id");
            CreateIndex("dbo.Inv_po_item_transfer", "Sales_id");
            AddForeignKey("dbo.Inv_po_item_transfer", "Po_id", "dbo.Inv_receive_po", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Inv_po_item_transfer", "Sales_id", "dbo.Inv_sales_invoice", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Inv_receive_po_items", "UOM_id", "dbo.Unit_of_measure", "Id");
            AddForeignKey("dbo.Inv_sales_invoice_items", "UOM_id", "dbo.Unit_of_measure", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice_items", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_receive_po_items", "UOM_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_po_item_transfer", "Sales_id", "dbo.Inv_sales_invoice");
            DropForeignKey("dbo.Inv_po_item_transfer", "Po_id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_po_item_transfer", new[] { "Sales_id" });
            DropIndex("dbo.Inv_po_item_transfer", new[] { "Po_id" });
            DropIndex("dbo.Inv_sales_invoice_items", new[] { "UOM_id" });
            DropIndex("dbo.Inv_receive_po_items", new[] { "UOM_id" });
            AlterColumn("dbo.Inv_sales_invoice_items", "UOM_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po_items", "UOM_id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_po_item_transfer", "Sales_id");
            DropColumn("dbo.Inv_po_item_transfer", "Po_id");
            DropColumn("dbo.Inv_po_item_transfer", "Orginal_Qty");
            CreateIndex("dbo.Inv_sales_invoice_items", "UOM_id");
            CreateIndex("dbo.Inv_receive_po_items", "UOM_id");
            AddForeignKey("dbo.Inv_sales_invoice_items", "UOM_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "UOM_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
        }
    }
}
