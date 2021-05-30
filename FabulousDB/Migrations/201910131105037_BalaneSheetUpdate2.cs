namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BalaneSheetUpdate2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Balance_sheet_raw_settings", "Row_name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Balance_sheet_raw_settings", "Row_name", c => c.String());
        }
    }
}
