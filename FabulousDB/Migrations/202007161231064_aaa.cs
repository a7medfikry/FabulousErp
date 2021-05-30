namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aaa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Posting_number", c => c.Int(nullable: false));
            AddColumn("dbo.Inv_sales_invoice", "Posting_number", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_sales_invoice", "Posting_number");
            DropColumn("dbo.Inv_receive_po", "Posting_number");
        }
    }
}
