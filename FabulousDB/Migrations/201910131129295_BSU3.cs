namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSU3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_rows");
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Row_id" });
            AddColumn("dbo.Balance_sheet_raw_settings", "Row_name", c => c.String());
            DropColumn("dbo.Balance_sheet_raw_settings", "Row_id");
            DropTable("dbo.Balance_sheet_rows");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Balance_sheet_rows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Balance_sheet_raw_settings", "Row_id", c => c.Int());
            DropColumn("dbo.Balance_sheet_raw_settings", "Row_name");
            CreateIndex("dbo.Balance_sheet_raw_settings", "Row_id");
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_rows", "Id");
        }
    }
}
