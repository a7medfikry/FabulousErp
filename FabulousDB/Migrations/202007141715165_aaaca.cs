namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaaca : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_receive_po", "Posting_date", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Inv_receive_po", "Transaction_date", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Inv_sales_invoice", "Posting_date", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Inv_sales_invoice", "Transaction_date", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inv_sales_invoice", "Transaction_date", c => c.DateTime());
            AlterColumn("dbo.Inv_sales_invoice", "Posting_date", c => c.DateTime());
            AlterColumn("dbo.Inv_receive_po", "Transaction_date", c => c.DateTime());
            AlterColumn("dbo.Inv_receive_po", "Posting_date", c => c.DateTime());
        }
    }
}
