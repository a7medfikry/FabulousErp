namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRec4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_adjustment_items", "From_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_adjustment_items", "item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_adjustment_items", "Lot_id", "dbo.Inv_items_serial");
            DropForeignKey("dbo.Inv_adjustment_items", "Store_Id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_adjustment_items", "To_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_adjustment_relation", "Inv_item_adjustment_Id", "dbo.Inv_item_adjustment");
            DropForeignKey("Inv_adjustment_relation", "FK_dbo.Inv_adjustment_relation_dbo.Inv_adjustment_items_Adjustment_id");
            DropIndex("dbo.Inv_adjustment_items", new[] { "From_store_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "To_store_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "item_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "Lot_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "Store_Id" });
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Adjustment_id" });
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Inv_item_adjustment_Id" });
            DropColumn("dbo.Inv_adjustment_relation", "Adjustment_id");
            RenameColumn(table: "dbo.Inv_adjustment_relation", name: "Inv_item_adjustment_Id", newName: "Adjustment_id");
            AlterColumn("dbo.Inv_adjustment_relation", "Adjustment_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_adjustment_relation", "Adjustment_id");
            AddForeignKey("dbo.Inv_adjustment_relation", "Adjustment_id", "dbo.Inv_item_adjustment", "Id", cascadeDelete: true);
            DropTable("dbo.Inv_adjustment_items");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Inv_adjustment_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Adjustment_no = c.String(),
                        Adjustment_date = c.DateTime(nullable: false),
                        Posting_date = c.DateTime(nullable: false),
                        From_store_id = c.Int(nullable: false),
                        To_store_id = c.Int(nullable: false),
                        Ref = c.String(),
                        item_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Unite_cost = c.Decimal(nullable: false, precision: 20, scale: 4),
                        Lot_id = c.Int(nullable: false),
                        Store_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Inv_adjustment_relation", "Adjustment_id", "dbo.Inv_item_adjustment");
            DropIndex("dbo.Inv_adjustment_relation", new[] { "Adjustment_id" });
            AlterColumn("dbo.Inv_adjustment_relation", "Adjustment_id", c => c.Int());
            RenameColumn(table: "dbo.Inv_adjustment_relation", name: "Adjustment_id", newName: "Inv_item_adjustment_Id");
            AddColumn("dbo.Inv_adjustment_relation", "Adjustment_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_adjustment_relation", "Inv_item_adjustment_Id");
            CreateIndex("dbo.Inv_adjustment_relation", "Adjustment_id");
            CreateIndex("dbo.Inv_adjustment_items", "Store_Id");
            CreateIndex("dbo.Inv_adjustment_items", "Lot_id");
            CreateIndex("dbo.Inv_adjustment_items", "item_id");
            CreateIndex("dbo.Inv_adjustment_items", "To_store_id");
            CreateIndex("dbo.Inv_adjustment_items", "From_store_id");
            AddForeignKey("dbo.Inv_adjustment_relation", "Inv_item_adjustment_Id", "dbo.Inv_item_adjustment", "Id");
            AddForeignKey("dbo.Inv_adjustment_items", "To_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_adjustment_items", "Store_Id", "dbo.Inv_store", "Id");
            AddForeignKey("dbo.Inv_adjustment_items", "Lot_id", "dbo.Inv_items_serial", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_adjustment_items", "item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_adjustment_items", "From_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
        }
    }
}
