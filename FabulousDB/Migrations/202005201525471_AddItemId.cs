namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_stocking", "Item_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_stocking", "Item_id");
            AddForeignKey("dbo.Inv_stocking", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_stocking", "Item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_stocking", new[] { "Item_id" });
            DropColumn("dbo.Inv_stocking", "Item_id");
        }
    }
}
