namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPurReqItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_purchase_request", "item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_purchase_request", new[] { "item_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            CreateTable(
                "dbo.Inv_purchase_request_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        item_id = c.Int(nullable: false),
                        Quntaty = c.Int(nullable: false),
                        Purchase_request_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_purchase_request", t => t.Purchase_request_id, cascadeDelete: false)
                .Index(t => t.item_id)
                .Index(t => t.Purchase_request_id);
            
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            DropColumn("dbo.Inv_purchase_request", "item_id");
            DropColumn("dbo.Inv_purchase_request", "Quntaty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_purchase_request", "Quntaty", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_purchase_request", "item_id", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inv_purchase_request_items", "Purchase_request_id", "dbo.Inv_purchase_request");
            DropForeignKey("dbo.Inv_purchase_request_items", "item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_purchase_request_items", new[] { "Purchase_request_id" });
            DropIndex("dbo.Inv_purchase_request_items", new[] { "item_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropTable("dbo.Inv_purchase_request_items");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_purchase_request", "item_id");
            AddForeignKey("dbo.Inv_purchase_request", "item_id", "dbo.Inv_item", "Id", cascadeDelete: false);
        }
    }
}
