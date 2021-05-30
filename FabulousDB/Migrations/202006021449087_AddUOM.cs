namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUOM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_po_item_transfer", "UOM_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_po_item_transfer", "UOM_id");
            AddForeignKey("dbo.Inv_po_item_transfer", "UOM_id", "dbo.Unit_of_measure", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_po_item_transfer", "UOM_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_po_item_transfer", new[] { "UOM_id" });
            DropColumn("dbo.Inv_po_item_transfer", "UOM_id");
        }
    }
}
