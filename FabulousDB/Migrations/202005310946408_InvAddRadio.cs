namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvAddRadio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item", "Martial_or_service", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item", "Martial_or_service");
        }
    }
}
