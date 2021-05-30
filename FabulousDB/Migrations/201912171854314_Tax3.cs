namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Journal_number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tax", "Journal_number");
        }
    }
}
