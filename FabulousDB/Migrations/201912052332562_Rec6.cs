namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_payment", "Transaction_p_id", c => c.Int());
            CreateIndex("dbo.Payable_payment", "Transaction_p_id");
            AddForeignKey("dbo.Payable_payment", "Transaction_p_id", "dbo.Payable_transaction", "Id");
        }

        public override void Down()
        {
        }
    }
}
