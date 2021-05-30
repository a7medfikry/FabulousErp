namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvReturn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_return_po_items", "Sales_id", c => c.Int(nullable: true));
            CreateIndex("dbo.Inv_receive_return_po_items", "Sales_id");
            AddForeignKey("dbo.Inv_receive_return_po_items", "Sales_id", "dbo.Inv_sales_invoice", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_return_po_items", "Sales_id", "dbo.Inv_sales_invoice");
            DropIndex("dbo.Inv_receive_return_po_items", new[] { "Sales_id" });
            DropColumn("dbo.Inv_receive_return_po_items", "Sales_id");
        }
    }
}
