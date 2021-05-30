namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvFixPos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_sales_receivs_pos", "Sales_item_id", "dbo.Inv_sales_invoice_items");
            DropIndex("dbo.Inv_sales_receivs_pos", new[] { "Sales_item_id" });
            RenameColumn(table: "dbo.Inv_sales_receivs_pos", name: "Sales_item_id", newName: "Inv_sales_invoice_items_Id");
            AddColumn("dbo.Inv_sales_receivs_pos", "Sales_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id", c => c.Int());
            CreateIndex("dbo.Inv_sales_receivs_pos", "Sales_id");
            CreateIndex("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id");
            AddForeignKey("dbo.Inv_sales_receivs_pos", "Sales_id", "dbo.Inv_sales_invoice", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id", "dbo.Inv_sales_invoice_items", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id", "dbo.Inv_sales_invoice_items");
            DropForeignKey("dbo.Inv_sales_receivs_pos", "Sales_id", "dbo.Inv_sales_invoice");
            DropIndex("dbo.Inv_sales_receivs_pos", new[] { "Inv_sales_invoice_items_Id" });
            DropIndex("dbo.Inv_sales_receivs_pos", new[] { "Sales_id" });
            AlterColumn("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_sales_receivs_pos", "Sales_id");
            RenameColumn(table: "dbo.Inv_sales_receivs_pos", name: "Inv_sales_invoice_items_Id", newName: "Sales_item_id");
            CreateIndex("dbo.Inv_sales_receivs_pos", "Sales_item_id");
            AddForeignKey("dbo.Inv_sales_receivs_pos", "Sales_item_id", "dbo.Inv_sales_invoice_items", "Id", cascadeDelete: true);
        }
    }
}
