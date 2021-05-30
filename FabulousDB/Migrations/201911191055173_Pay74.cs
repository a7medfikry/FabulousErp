namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay74 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_void", "Transaction_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Payable_void", "Posting_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Payable_void", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_void", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Payable_void", "Posting_date");
            DropColumn("dbo.Payable_void", "Transaction_date");
        }
    }
}
