namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay31 : DbMigration
    {
        public override void Up()
        {
            Sql("alter table Payable_Group_setting drop constraint [FK_dbo.Payable_Group_setting_dbo.AccountCurrencyDefinition_Table_Currency_id]");
           // DropForeignKey("dbo.Payable_Group_setting", "Currency_id", "dbo.CurrenciesDefinition_Tables");
            DropIndex("dbo.Payable_Group_setting", new[] { "Currency_id" });
            AlterColumn("dbo.Payable_Group_setting", "Currency_id", c => c.String(maxLength: 50));
            CreateIndex("dbo.Payable_Group_setting", "Currency_id");
            AddForeignKey("dbo.Payable_Group_setting", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_Group_setting", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Payable_Group_setting", new[] { "Currency_id" });
            AlterColumn("dbo.Payable_Group_setting", "Currency_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_Group_setting", "Currency_id");
            AddForeignKey("dbo.Payable_Group_setting", "Currency_id", "dbo.CurrenciesDefinition_Tables", "ACD_ID", cascadeDelete: true);
        }
    }
}
