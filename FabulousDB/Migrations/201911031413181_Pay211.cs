namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay211 : DbMigration
    {
        public override void Up()
        {
            Sql("alter table Creditor_setting " +
                "drop constraint [FK_dbo.Creditor_setting_dbo.AccountCurrencyDefinition_Table_Currency_id]");
            DropIndex("dbo.Creditor_setting", new[] { "Currency_id" });
            AlterColumn("dbo.Creditor_setting", "Currency_id", c => c.String(maxLength: 50));
            CreateIndex("dbo.Creditor_setting", "Currency_id");
            AddForeignKey("dbo.Creditor_setting", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Creditor_setting", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Creditor_setting", new[] { "Currency_id" });
            AlterColumn("dbo.Creditor_setting", "Currency_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Creditor_setting", "Currency_id");
            AddForeignKey("dbo.Creditor_setting", "Currency_id", "dbo.CurrenciesDefinition_Tables", "ACD_ID", cascadeDelete: true);
        }
    }
}
