namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec7 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Receivable_payment", "Trans_doc_type_id", "dbo.Receivable_transactions_types");
            //AddColumn("dbo.Receivable_payment", "Receivable_transactions_types_Id", c => c.Int());
            //CreateIndex("dbo.Receivable_payment", "Receivable_transactions_types_Id");
            //AddForeignKey("dbo.Receivable_payment", "Receivable_transactions_types_Id", "dbo.Receivable_transactions_types", "Id");

            AddColumn("dbo.Receivable_payment", "Transaction_p_id", c => c.Int());
            CreateIndex("dbo.Receivable_payment", "Transaction_p_id");
            AddForeignKey("dbo.Receivable_payment", "Transaction_p_id", "dbo.Receivable_transactions_types", "Id");

            DropForeignKey("dbo.Payable_payment", "FK_dbo.Payable_payment_dbo.Payable_transaction_Transaction_p_id");
            DropIndex("dbo.Payable_payment", "IX_Transaction_p_id");
            DropColumn("dbo.Payable_payment", "Transaction_p_id");
            AddColumn("dbo.Payable_payment", "Transaction_p_id", c => c.Int());
            CreateIndex("dbo.Payable_payment", "Transaction_p_id");
            AddForeignKey("dbo.Payable_payment", "Transaction_p_id", "dbo.Payable_transactions_types", "Id");

        }

        public override void Down()
        {
            DropForeignKey("dbo.Receivable_payment", "Receivable_transactions_types_Id", "dbo.Receivable_transactions_types");
            DropForeignKey("dbo.Receivable_payment", "Transaction_p_id", "dbo.Receivable_transactions_types");
            DropIndex("dbo.Receivable_payment", new[] { "Receivable_transactions_types_Id" });
            DropIndex("dbo.Receivable_payment", new[] { "Transaction_p_id" });
            DropColumn("dbo.Receivable_payment", "Receivable_transactions_types_Id");
            DropColumn("dbo.Receivable_payment", "Transaction_p_id");
            AddForeignKey("dbo.Receivable_payment", "Trans_doc_type_id", "dbo.Receivable_transactions_types", "Id", cascadeDelete: true);
        }
    }
}
