namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Unit_of_measure", "Action", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Unit_of_measure", "Action");
        }
    }
}
