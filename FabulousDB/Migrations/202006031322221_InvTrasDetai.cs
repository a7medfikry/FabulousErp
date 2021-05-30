namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvTrasDetai : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_transfer_items", "From_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_transfer_items", "item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_transfer_items", "Lot_id", "dbo.Inv_items_serial");
            DropForeignKey("dbo.Inv_transfer_items", "Store_Id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_transfer_items", "To_store_id", "dbo.Inv_store");
            DropIndex("dbo.Inv_transfer_items", new[] { "From_store_id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "To_store_id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "item_id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "Lot_id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "Store_Id" });
            CreateTable(
                "dbo.Inv_transfer_relation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Receive_po_id = c.Int(nullable: false),
                        Main_po_id = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Item_id = c.Int(nullable: false),
                        Transfer_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_receive_po", t => t.Main_po_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_receive_po", t => t.Receive_po_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_po_item_transfer", t => t.Transfer_id, cascadeDelete: false)
                .Index(t => t.Receive_po_id)
                .Index(t => t.Main_po_id)
                .Index(t => t.Item_id)
                .Index(t => t.Transfer_id);
            
            DropTable("dbo.Inv_transfer_items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Inv_transfer_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Transfer_no = c.String(maxLength: 500),
                        Transfer_date = c.DateTime(nullable: false),
                        Posting_date = c.DateTime(nullable: false),
                        From_store_id = c.Int(nullable: false),
                        To_store_id = c.Int(nullable: false),
                        Ref = c.String(maxLength: 500),
                        item_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Unite_cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Lot_id = c.Int(nullable: false),
                        Store_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Inv_transfer_relation", "Transfer_id", "dbo.Inv_po_item_transfer");
            DropForeignKey("dbo.Inv_transfer_relation", "Receive_po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_transfer_relation", "Main_po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_transfer_relation", "Item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_transfer_relation", new[] { "Transfer_id" });
            DropIndex("dbo.Inv_transfer_relation", new[] { "Item_id" });
            DropIndex("dbo.Inv_transfer_relation", new[] { "Main_po_id" });
            DropIndex("dbo.Inv_transfer_relation", new[] { "Receive_po_id" });
            DropTable("dbo.Inv_transfer_relation");
            CreateIndex("dbo.Inv_transfer_items", "Store_Id");
            CreateIndex("dbo.Inv_transfer_items", "Lot_id");
            CreateIndex("dbo.Inv_transfer_items", "item_id");
            CreateIndex("dbo.Inv_transfer_items", "To_store_id");
            CreateIndex("dbo.Inv_transfer_items", "From_store_id");
            AddForeignKey("dbo.Inv_transfer_items", "To_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_transfer_items", "Store_Id", "dbo.Inv_store", "Id");
            AddForeignKey("dbo.Inv_transfer_items", "Lot_id", "dbo.Inv_items_serial", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_transfer_items", "item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_transfer_items", "From_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
        }
    }
}
