namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemTrans : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_po_item_transfer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        From_store_id = c.Int(nullable: false),
                        From_site_id = c.Int(nullable: false),
                        To_store_id = c.Int(nullable: false),
                        To_site_id = c.Int(nullable: false),
                        Transfer_qty = c.Single(nullable: false),
                        Site_transfer = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_store_site", t => t.From_site_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.From_store_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store_site", t => t.To_site_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.To_store_id, cascadeDelete: false)
                .Index(t => t.Item_id)
                .Index(t => t.From_store_id)
                .Index(t => t.From_site_id)
                .Index(t => t.To_store_id)
                .Index(t => t.To_site_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_po_item_transfer", "To_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_po_item_transfer", "To_site_id", "dbo.Inv_store_site");
            DropForeignKey("dbo.Inv_po_item_transfer", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_po_item_transfer", "From_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_po_item_transfer", "From_site_id", "dbo.Inv_store_site");
            DropIndex("dbo.Inv_po_item_transfer", new[] { "To_site_id" });
            DropIndex("dbo.Inv_po_item_transfer", new[] { "To_store_id" });
            DropIndex("dbo.Inv_po_item_transfer", new[] { "From_site_id" });
            DropIndex("dbo.Inv_po_item_transfer", new[] { "From_store_id" });
            DropIndex("dbo.Inv_po_item_transfer", new[] { "Item_id" });
            DropTable("dbo.Inv_po_item_transfer");
        }
    }
}
