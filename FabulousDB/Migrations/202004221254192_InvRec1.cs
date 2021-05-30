namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRec1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_item_recipe", "Inv_item_Id", "dbo.Inv_item");
            DropIndex("dbo.Inv_item_recipe", new[] { "Inv_item_Id" });
            DropColumn("dbo.Inv_item_recipe", "Inv_item_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_item_recipe", "Inv_item_Id", c => c.Int());
            CreateIndex("dbo.Inv_item_recipe", "Inv_item_Id");
            AddForeignKey("dbo.Inv_item_recipe", "Inv_item_Id", "dbo.Inv_item", "Id");
        }
    }
}
