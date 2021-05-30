namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuotauinItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_quotation_request_item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        item_id = c.Int(nullable: false),
                        Quntaty = c.Double(nullable: false),
                        Quotation_request_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_quotation_request", t => t.Quotation_request_id, cascadeDelete: false)
                .Index(t => t.item_id)
                .Index(t => t.Quotation_request_id);
            
            AddColumn("dbo.Inv_purchase_request", "Po_number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_quotation_request_item", "Quotation_request_id", "dbo.Inv_quotation_request");
            DropForeignKey("dbo.Inv_quotation_request_item", "item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_quotation_request_item", new[] { "Quotation_request_id" });
            DropIndex("dbo.Inv_quotation_request_item", new[] { "item_id" });
            DropColumn("dbo.Inv_purchase_request", "Po_number");
            DropTable("dbo.Inv_quotation_request_item");
        }
    }
}
