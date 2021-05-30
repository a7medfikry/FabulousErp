namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReciveSerial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_receive_item_serial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        Serial = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_receive_po_items", t => t.Item_id, cascadeDelete: false)
                .Index(t => t.Item_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_item_serial", "Item_id", "dbo.Inv_receive_po_items");
            DropIndex("dbo.Inv_receive_item_serial", new[] { "Item_id" });
            DropTable("dbo.Inv_receive_item_serial");
        }
    }
}
