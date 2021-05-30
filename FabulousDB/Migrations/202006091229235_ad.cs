namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ad : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item_adjustment", "UOM_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_item_adjustment", "UOM_id");
            AddForeignKey("dbo.Inv_item_adjustment", "UOM_id", "dbo.Unit_of_measure", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_adjustment", "UOM_id", "dbo.Unit_of_measure");
            DropIndex("dbo.Inv_item_adjustment", new[] { "UOM_id" });
            DropColumn("dbo.Inv_item_adjustment", "UOM_id");
        }
    }
}
