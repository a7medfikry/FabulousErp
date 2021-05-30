namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay64 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "JournalEntry", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assign_payable_doc", "JournalEntry");
        }
    }
}
