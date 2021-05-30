namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Item_type", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tax", "Item_type");
        }
    }
}
