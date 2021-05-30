namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inv_itemUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_item_option", "Height", c => c.Single());
            AlterColumn("dbo.Inv_item_option", "Width", c => c.Single());
            AlterColumn("dbo.Inv_item_option", "Depth", c => c.Single());
            AlterColumn("dbo.Inv_item_option", "Wight", c => c.Single());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inv_item_option", "Wight", c => c.Single(nullable: false));
            AlterColumn("dbo.Inv_item_option", "Depth", c => c.Single(nullable: false));
            AlterColumn("dbo.Inv_item_option", "Width", c => c.Single(nullable: false));
            AlterColumn("dbo.Inv_item_option", "Height", c => c.Single(nullable: false));
        }
    }
}
