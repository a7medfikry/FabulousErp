namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec9 : DbMigration
    {
        public override void Up()
        {
           DropForeignKey("dbo.Payable_payment", "Transaction_p_id", "dbo.Payable_transactions_types");
            AddForeignKey("dbo.Payable_payment", "Transaction_p_id", "Payable_transaction", "Id");
        }

        public override void Down()
        {
        }
    }
}
