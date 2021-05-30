namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inv_item_options : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_item_option",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Inv_item_id = c.Int(nullable: false),
                        Height = c.Single(nullable: false),
                        Width = c.Single(nullable: false),
                        Depth = c.Single(nullable: false),
                        Size_type = c.Int(nullable: false),
                        Wight = c.Single(nullable: false),
                        Wight_type = c.Int(nullable: false),
                        Image = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Inv_item_id, cascadeDelete: true)
                .Index(t => t.Inv_item_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_option", "Inv_item_id", "dbo.Inv_item");
            DropIndex("dbo.Inv_item_option", new[] { "Inv_item_id" });
            DropTable("dbo.Inv_item_option");
        }
    }
}
