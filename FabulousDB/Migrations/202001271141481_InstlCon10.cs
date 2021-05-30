namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstlCon10 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Installments", "Check_book_trx_id", "dbo.C_CheckbookTransactions_table");
            DropIndex("dbo.Installments", new[] { "Check_book_trx_id" });
            AlterColumn("dbo.Installments", "Check_book_trx_id", c => c.Int());
            CreateIndex("dbo.Installments", "Check_book_trx_id");
            AddForeignKey("dbo.Installments", "Check_book_trx_id", "dbo.C_CheckbookTransactions_table", "C_CBT");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Installments", "Check_book_trx_id", "dbo.C_CheckbookTransactions_table");
            DropIndex("dbo.Installments", new[] { "Check_book_trx_id" });
            AlterColumn("dbo.Installments", "Check_book_trx_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Installments", "Check_book_trx_id");
            AddForeignKey("dbo.Installments", "Check_book_trx_id", "dbo.C_CheckbookTransactions_table", "C_CBT", cascadeDelete: true);
        }
    }
}
