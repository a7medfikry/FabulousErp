namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlanceSheet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Balance_sheet_rows",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Balance_sheet_raw_settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Row_name = c.String(),
                        Type = c.Int(nullable: false),
                        Account_id = c.Int(nullable: false),
                        Row_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Account_id, cascadeDelete: true)
                .ForeignKey("dbo.Balance_sheet_rows", t => t.Row_id, cascadeDelete: true)
                .Index(t => t.Account_id)
                .Index(t => t.Row_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_rows");
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Account_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Row_id" });
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Account_id" });
            DropTable("dbo.Balance_sheet_raw_settings");
            DropTable("dbo.Balance_sheet_rows");
        }
    }
}
