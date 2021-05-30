namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PBC2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BPC_raw_settings", "Minus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BPC_raw_settings", "Minus");
        }
    }
}
