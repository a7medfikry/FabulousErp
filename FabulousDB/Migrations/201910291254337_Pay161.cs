namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay161 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_transaction", "Currency_id", c => c.Int());
            CreateIndex("dbo.Payable_transaction", "Currency_id");
            AddForeignKey("dbo.Payable_transaction", "Currency_id", "dbo.AccountCurrencyDefinition_Table", "ACD_ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_transaction", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropIndex("dbo.Payable_transaction", new[] { "Currency_id" });
            DropColumn("dbo.Payable_transaction", "Currency_id");
        }
    }
}
