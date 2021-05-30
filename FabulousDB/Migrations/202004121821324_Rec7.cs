namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec7 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Inv_receive_item_serial",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Item_id = c.Int(nullable: false),
            //            Serial = c.String(maxLength: 200),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Inv_receive_po_items", t => t.Item_id, cascadeDelete: true)
            //    .Index(t => t.Item_id);
            
            //AddColumn("dbo.Tax", "Final_cost_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //CreateIndex("dbo.Inv_sales_invoice", "Customer_id");
            //CreateIndex("dbo.Inv_sales_invoice", "Receivable_id");
            //AddForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting", "Id", cascadeDelete: true);
            //AddForeignKey("dbo.Inv_sales_invoice", "Receivable_id", "dbo.Receivable_transaction", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_item_serial", "Item_id", "dbo.Inv_receive_po_items");
            DropForeignKey("dbo.Inv_sales_invoice", "Receivable_id", "dbo.Receivable_transaction");
            DropForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting");
            DropIndex("dbo.Inv_receive_item_serial", new[] { "Item_id" });
            DropIndex("dbo.Inv_sales_invoice", new[] { "Receivable_id" });
            DropIndex("dbo.Inv_sales_invoice", new[] { "Customer_id" });
            DropColumn("dbo.Tax", "Final_cost_price");
            DropTable("dbo.Inv_receive_item_serial");
        }
    }
}
