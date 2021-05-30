namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvSerial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_sales_item_serial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        Sales_id = c.Int(nullable: false),
                        Serial = c.String(maxLength: 200),
                        Start_warranty = c.DateTime(nullable: false),
                        End_warranty = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_sales_invoice_items", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_sales_invoice", t => t.Sales_id, cascadeDelete: false)
                .Index(t => t.Item_id)
                .Index(t => t.Sales_id);
            
            AddColumn("dbo.Inv_receive_item_serial", "Po_id", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_receive_item_serial", "Start_warranty", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inv_receive_item_serial", "End_warranty", c => c.DateTime(nullable: false));
            CreateIndex("dbo.Inv_receive_item_serial", "Po_id");
            AddForeignKey("dbo.Inv_receive_item_serial", "Po_id", "dbo.Inv_receive_po", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_item_serial", "Sales_id", "dbo.Inv_sales_invoice");
            DropForeignKey("dbo.Inv_sales_item_serial", "Item_id", "dbo.Inv_sales_invoice_items");
            DropForeignKey("dbo.Inv_receive_item_serial", "Po_id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_sales_item_serial", new[] { "Sales_id" });
            DropIndex("dbo.Inv_sales_item_serial", new[] { "Item_id" });
            DropIndex("dbo.Inv_receive_item_serial", new[] { "Po_id" });
            DropColumn("dbo.Inv_receive_item_serial", "End_warranty");
            DropColumn("dbo.Inv_receive_item_serial", "Start_warranty");
            DropColumn("dbo.Inv_receive_item_serial", "Po_id");
            DropTable("dbo.Inv_sales_item_serial");
        }
    }
}
