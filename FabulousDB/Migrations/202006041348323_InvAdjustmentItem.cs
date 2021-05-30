namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvAdjustmentItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_item_adjustment",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Item_id = c.Int(nullable: false),
                    Damage_qty = c.Single(nullable: false),
                    Loss_qty = c.Single(nullable: false),
                    Earn_qty = c.Single(nullable: false),
                    Damage_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Loss_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Earn_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Desc = c.String(),
                    Adjustment_date = c.DateTime(nullable: false),
                    Site_transfer = c.Boolean(nullable: false),
                    Po_id = c.Int(nullable: false),
                    Adjustment_num = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_receive_po", t => t.Po_id, cascadeDelete: false)
                .Index(t => t.Item_id)
                .Index(t => t.Po_id);

            CreateTable(
                "dbo.Inv_adjustment_relation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Receive_po_id = c.Int(nullable: false),
                        Main_po_id = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Item_id = c.Int(nullable: false),
                        Adjustment_id = c.Int(nullable: false),
                        Inv_item_adjustment_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_adjustment_items", t => t.Adjustment_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_receive_po", t => t.Main_po_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_receive_po", t => t.Receive_po_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item_adjustment", t => t.Inv_item_adjustment_Id)
                .Index(t => t.Receive_po_id)
                .Index(t => t.Main_po_id)
                .Index(t => t.Item_id)
                .Index(t => t.Adjustment_id)
                .Index(t => t.Inv_item_adjustment_Id);
            
            DropColumn("dbo.Inv_receive_po_items", "Amount_system_currency");
            DropColumn("dbo.Inv_po_item_transfer", "Doc_type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_po_item_transfer", "Doc_type", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_receive_po_items", "Amount_system_currency", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropForeignKey("dbo.Inv_adjustment_relation", "Inv_item_adjustment_Id", "dbo.Inv_item_adjustment");
            DropForeignKey("dbo.Inv_item_adjustment", "Po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_item_adjustment", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_adjustment_relation", "Receive_po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_adjustment_relation", "Main_po_id", "dbo.Inv_receive_po");
            DropForeignKey("dbo.Inv_adjustment_relation", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_adjustment_relation", "Adjustment_id", "dbo.Inv_adjustment_items");
            DropIndex("dbo.Inv_item_adjustment", new[] { "Po_id" });
            DropIndex("dbo.Inv_item_adjustment", new[] { "Item_id" });
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Inv_item_adjustment_Id" });
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Adjustment_id" });
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Item_id" });
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Main_po_id" });
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Receive_po_id" });
            DropTable("dbo.Inv_item_adjustment");
            DropTable("dbo.Inv_adjustment_relation");
        }
    }
}
