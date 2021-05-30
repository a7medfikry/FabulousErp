namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BalanceSheetUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Account_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_rows");
            DropIndex("dbo.Balance_sheet_raw_settings", "Ix_Balance_sheet_AccountId");
            DropIndex("dbo.Balance_sheet_raw_settings", "Ix_Balance_sheet_RowId");
            AlterColumn("dbo.Balance_sheet_raw_settings", "Account_id", c => c.Int());
            AlterColumn("dbo.Balance_sheet_raw_settings", "Row_id", c => c.Int());
            CreateIndex("dbo.Balance_sheet_raw_settings", "Account_id");
            CreateIndex("dbo.Balance_sheet_raw_settings", "Row_id");
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Account_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_rows", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_rows");
            DropForeignKey("dbo.Balance_sheet_raw_settings", "Account_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Row_id" });
            DropIndex("dbo.Balance_sheet_raw_settings", new[] { "Account_id" });
            AlterColumn("dbo.Balance_sheet_raw_settings", "Row_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Balance_sheet_raw_settings", "Account_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Balance_sheet_raw_settings", "Row_id", name: "Ix_Balance_sheet_RowId");
            CreateIndex("dbo.Balance_sheet_raw_settings", "Account_id", name: "Ix_Balance_sheet_AccountId");
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Row_id", "dbo.Balance_sheet_rows", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Balance_sheet_raw_settings", "Account_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
        }
    }
}
