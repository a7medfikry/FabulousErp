namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvReturnPurc : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_receive_return_po_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Po_items_id = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Unit_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_receive_po_items", t => t.Po_items_id, cascadeDelete: false)
                .Index(t => t.Po_items_id);
            
            DropColumn("dbo.Inv_receive_po_items", "IsReturn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_po_items", "IsReturn", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Inv_receive_return_po_items", "Po_items_id", "dbo.Inv_receive_po_items");
            DropIndex("dbo.Inv_receive_return_po_items", new[] { "Po_items_id" });
            DropTable("dbo.Inv_receive_return_po_items");
        }
    }
}
