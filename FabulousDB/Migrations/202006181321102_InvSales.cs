namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvSales : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice", "Payment_terms_id", c => c.Int());
            CreateIndex("dbo.Inv_sales_invoice", "Payment_terms_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Payment_terms_id", "dbo.Receivable_payment_term", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Payment_terms_id", "dbo.Receivable_payment_term");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Payment_terms_id" });
            DropColumn("dbo.Inv_sales_invoice", "Payment_terms_id");
        }
    }
}
