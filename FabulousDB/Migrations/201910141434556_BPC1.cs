namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BPC1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Balance_sheet_Relation", newName: "BPC_Relation");
            RenameTable(name: "dbo.Balance_sheet_raw_settings", newName: "BPC_raw_settings");
            AddColumn("dbo.BPC_Relation", "Report_type", c => c.Int(nullable: false));
            AddColumn("dbo.BPC_raw_settings", "Report_type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BPC_raw_settings", "Report_type");
            DropColumn("dbo.BPC_Relation", "Report_type");
            RenameTable(name: "dbo.BPC_raw_settings", newName: "Balance_sheet_raw_settings");
            RenameTable(name: "dbo.BPC_Relation", newName: "Balance_sheet_Relation");
        }
    }
}
