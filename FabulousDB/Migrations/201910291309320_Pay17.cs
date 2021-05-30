namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay17 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AccountCurrencyDefinition_Table", newName: "CurrenciesDefinition_Tables");
            DropForeignKey("dbo.Payable_payment", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropIndex("dbo.Payable_payment", new[] { "Currency_id" });
            DropIndex("dbo.Payable_transaction", new[] { "Currency_id" });
            AlterColumn("dbo.Payable_payment", "Currency_id", c => c.String(maxLength: 50));
            AlterColumn("dbo.Payable_transaction", "Currency_id", c => c.String(maxLength: 50));
            CreateIndex("dbo.Payable_payment", "Currency_id");
            CreateIndex("dbo.Payable_transaction", "Currency_id");
            AddForeignKey("dbo.Payable_payment", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_payment", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Payable_transaction", new[] { "Currency_id" });
            DropIndex("dbo.Payable_payment", new[] { "Currency_id" });
            AlterColumn("dbo.Payable_transaction", "Currency_id", c => c.Int());
            AlterColumn("dbo.Payable_payment", "Currency_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_transaction", "Currency_id");
            CreateIndex("dbo.Payable_payment", "Currency_id");
            AddForeignKey("dbo.Payable_payment", "Currency_id", "dbo.AccountCurrencyDefinition_Table", "ACD_ID", cascadeDelete: true);
            RenameTable(name: "dbo.CurrenciesDefinition_Tables", newName: "AccountCurrencyDefinition_Table");
        }
    }
}
