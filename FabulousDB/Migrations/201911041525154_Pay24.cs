namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay24 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_payment", "Journal_number", c => c.Int(nullable: false));
            AddColumn("dbo.Payable_transaction", "Journal_number", c => c.Int(nullable: false));
            DropColumn("dbo.Payable_payment", "Posting_number");
            DropColumn("dbo.Payable_transaction", "Posting_number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Payable_transaction", "Posting_number", c => c.Int(nullable: false));
            AddColumn("dbo.Payable_payment", "Posting_number", c => c.Int(nullable: false));
            DropColumn("dbo.Payable_transaction", "Journal_number");
            DropColumn("dbo.Payable_payment", "Journal_number");
        }
    }
}
