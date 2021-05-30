namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRemoveUOMaction : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Unit_of_measure", "Action");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Unit_of_measure", "Action", c => c.Int(nullable: false));
        }
    }
}
