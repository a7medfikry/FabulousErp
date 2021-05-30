namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvItemStoreSite : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_item_store_site",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        Store_id = c.Int(),
                        Site_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store_site", t => t.Site_id)
                .ForeignKey("dbo.Inv_store", t => t.Store_id)
                .Index(t => t.Item_id)
                .Index(t => t.Store_id)
                .Index(t => t.Site_id);
         
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_store_site", "Store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_item_store_site", "Site_id", "dbo.Inv_store_site");
            DropForeignKey("dbo.Inv_item_store_site", "Item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_item_store_site", new[] { "Site_id" });
            DropIndex("dbo.Inv_item_store_site", new[] { "Store_id" });
            DropIndex("dbo.Inv_item_store_site", new[] { "Item_id" });
            DropTable("dbo.Inv_item_store_site");
        }
    }
}
