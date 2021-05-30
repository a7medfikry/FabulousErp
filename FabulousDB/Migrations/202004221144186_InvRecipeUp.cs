namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRecipeUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item_recipe", "Qty", c => c.Single(nullable: false));
            AddColumn("dbo.Inv_item_recipe", "Inv_item_Id", c => c.Int());
            CreateIndex("dbo.Inv_item_recipe", "Inv_item_Id");
            AddForeignKey("dbo.Inv_item_recipe", "Inv_item_Id", "dbo.Inv_item", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_recipe", "Inv_item_Id", "dbo.Inv_item");
            DropIndex("dbo.Inv_item_recipe", new[] { "Inv_item_Id" });
            DropColumn("dbo.Inv_item_recipe", "Inv_item_Id");
            DropColumn("dbo.Inv_item_recipe", "Qty");
        }
    }
}
