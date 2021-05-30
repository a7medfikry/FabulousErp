namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatmentTax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Statment_type", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tax", "Statment_type");
        }
    }
}
