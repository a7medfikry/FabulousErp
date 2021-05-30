namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay45 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_transaction", "Payment_terms_id", "dbo.Payment_term");
            DropIndex("dbo.Payable_transaction", new[] { "Payment_terms_id" });
            AlterColumn("dbo.Payable_transaction", "Payment_terms_id", c => c.Int());
            CreateIndex("dbo.Payable_transaction", "Payment_terms_id");
            AddForeignKey("dbo.Payable_transaction", "Payment_terms_id", "dbo.Payment_term", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_transaction", "Payment_terms_id", "dbo.Payment_term");
            DropIndex("dbo.Payable_transaction", new[] { "Payment_terms_id" });
            AlterColumn("dbo.Payable_transaction", "Payment_terms_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_transaction", "Payment_terms_id");
            AddForeignKey("dbo.Payable_transaction", "Payment_terms_id", "dbo.Payment_term", "Id", cascadeDelete: true);
        }
    }
}
