namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSU4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Balance_sheet_raw_settings", "Row_id", c => c.Int());
            AlterColumn("dbo.Balance_sheet_raw_settings", "Row_name", c => c.String(maxLength: 200));
            CreateIndex("dbo.Balance_sheet_raw_settings", "Row_id");
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_raw_settings", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_raw_settings");
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Row_id" });
            AlterColumn("dbo.Balance_sheet_raw_settings", "Row_name", c => c.String());
            DropColumn("dbo.Balance_sheet_raw_settings", "Row_id");
        }
    }
}
