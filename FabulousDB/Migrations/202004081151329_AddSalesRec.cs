namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSalesRec : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Inv_sales_invoice", "Receivable_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Receivable_id", "dbo.Receivable_transaction", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Receivable_id", "dbo.Receivable_transaction");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Receivable_id" });
        }
    }
}
