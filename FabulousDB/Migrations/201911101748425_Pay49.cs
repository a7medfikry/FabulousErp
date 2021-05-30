namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay49 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types");
            AddForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transaction", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transaction");
            AddForeignKey("dbo.Payable_payment", "Transaction_id", "dbo.Payable_transactions_types", "Id");
        }
    }
}
