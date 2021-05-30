namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_transaction", "Currency_id", "dbo.AccountCurrencyDefinition_Table");
            DropIndex("dbo.Payable_transaction", new[] { "Currency_id" });
            DropColumn("dbo.Payable_transaction", "Currency_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_transaction", "Currency_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_transaction", "Currency_id");
            AddForeignKey("dbo.Payable_transaction", "Currency_id", "dbo.AccountCurrencyDefinition_Table", "ACD_ID", cascadeDelete: true);
        }
    }
}
