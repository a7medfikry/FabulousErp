namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay76 : DbMigration
    {
        public override void Up()
        {
            Sql("delete from Payable_payment");
            AddColumn("dbo.Payable_payment", "Check_book_transaction_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_payment", "Check_book_transaction_id");
            AddForeignKey("dbo.Payable_payment", "Check_book_transaction_id", "dbo.C_CheckbookTransactions_table", "C_CBT", cascadeDelete: true);
        }
        public override void Down()
        {
            DropForeignKey("dbo.Payable_payment", "Check_book_transaction_id", "dbo.C_CheckbookTransactions_table");
            DropIndex("dbo.Payable_payment", new[] { "Check_book_transaction_id" });
            DropColumn("dbo.Payable_payment", "Check_book_transaction_id");
        }
    }
}
