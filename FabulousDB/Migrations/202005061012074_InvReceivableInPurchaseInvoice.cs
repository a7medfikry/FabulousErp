namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvReceivableInPurchaseInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Receivable_id", c => c.Int());
            CreateIndex("dbo.Inv_receive_po", "Receivable_id");
            AddForeignKey("dbo.Inv_receive_po", "Receivable_id", "dbo.Receivable_transaction", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po", "Receivable_id", "dbo.Receivable_transaction");
            DropIndex("dbo.Inv_receive_po", new[] { "Receivable_id" });
            DropColumn("dbo.Inv_receive_po", "Receivable_id");
        }
    }
}
