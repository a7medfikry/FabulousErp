namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BSU7 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Account_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_raw_settings");
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Account_id" });
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Row_id" });
            CreateTable(
                "dbo.Balance_sheet_Relation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Balance_sheet_id = c.Int(nullable: false),
                        Account_id = c.Int(),
                        Row_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Account_id)
                .ForeignKey("dbo.Balance_sheet_raw_settings", t => t.Balance_sheet_id, cascadeDelete: true)
                .ForeignKey("dbo.Balance_sheet_raw_settings", t => t.Row_id)
                .Index(t => t.Balance_sheet_id)
                .Index(t => t.Account_id)
                .Index(t => t.Row_id);
            
            DropColumn("dbo.Balance_sheet_raw_settings", "Account_id");
            DropColumn("dbo.Balance_sheet_raw_settings", "Row_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Balance_sheet_raw_settings", "Row_id", c => c.Int());
            AddColumn("dbo.Balance_sheet_raw_settings", "Account_id", c => c.Int());
            DropForeignKey("dbo.Balance_sheet_Relation", "Row_id", "dbo.Balance_sheet_raw_settings");
            DropForeignKey("dbo.Balance_sheet_Relation", "Balance_sheet_id", "dbo.Balance_sheet_raw_settings");
            DropForeignKey("dbo.Balance_sheet_Relation", "Account_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Balance_sheet_Relation", new[] { "Row_id" });
            DropIndex("dbo.Balance_sheet_Relation", new[] { "Account_id" });
            DropIndex("dbo.Balance_sheet_Relation", new[] { "Balance_sheet_id" });
            DropTable("dbo.Balance_sheet_Relation");
            CreateIndex("dbo.Balance_sheet_raw_settings", "Row_id");
            CreateIndex("dbo.Balance_sheet_raw_settings", "Account_id");
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_raw_settings", "Id");
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Account_id", "dbo.C_CreateAccount_Table", "C_AID");
        }
    }
}
