namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPostDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Posting_date", c => c.DateTime());
            AddColumn("dbo.Inv_receive_po", "Transaction_date", c => c.DateTime());
            AddColumn("dbo.Inv_sales_invoice", "Posting_date", c => c.DateTime());
            AddColumn("dbo.Inv_sales_invoice", "Transaction_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_sales_invoice", "Transaction_date");
            DropColumn("dbo.Inv_sales_invoice", "Posting_date");
            DropColumn("dbo.Inv_receive_po", "Transaction_date");
            DropColumn("dbo.Inv_receive_po", "Posting_date");
        }
    }
}
