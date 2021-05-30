namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveReturn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_receive_return_po_items", "Po_items_id", "dbo.Inv_receive_po_items");
            DropForeignKey("dbo.Inv_receive_return_po_items", "Sales_id", "dbo.Inv_sales_invoice");
            DropIndex("dbo.Inv_receive_return_po_items", new[] { "Po_items_id" });
            DropIndex("dbo.Inv_receive_return_po_items", new[] { "Sales_id" });
            DropTable("dbo.Inv_receive_return_po_items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Inv_receive_return_po_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Po_items_id = c.Int(nullable: false),
                        Sales_id = c.Int(),
                        Quantity = c.Single(nullable: false),
                        Unit_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Inv_receive_return_po_items", "Sales_id");
            CreateIndex("dbo.Inv_receive_return_po_items", "Po_items_id");
            AddForeignKey("dbo.Inv_receive_return_po_items", "Sales_id", "dbo.Inv_sales_invoice", "Id");
            AddForeignKey("dbo.Inv_receive_return_po_items", "Po_items_id", "dbo.Inv_receive_po_items", "Id", cascadeDelete: true);
        }
    }
}
