namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRecipe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_item_recipe",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Main_item_id = c.Int(nullable: false),
                        Recipe_item_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Main_item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.Recipe_item_id, cascadeDelete: false)
                .Index(t => t.Main_item_id)
                .Index(t => t.Recipe_item_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_recipe", "Recipe_item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_item_recipe", "Main_item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_item_recipe", new[] { "Recipe_item_id" });
            DropIndex("dbo.Inv_item_recipe", new[] { "Main_item_id" });
            DropTable("dbo.Inv_item_recipe");
        }
    }
}
