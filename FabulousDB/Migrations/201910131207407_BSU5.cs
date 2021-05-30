namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSU5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Balance_sheet_raw_settings", "Row_name", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Balance_sheet_raw_settings", "Row_name", c => c.String(maxLength: 200));
        }
    }
}
