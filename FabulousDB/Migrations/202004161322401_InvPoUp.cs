namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPoUp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Page_key", c => c.String());
            AlterColumn("dbo.C_GeneralJournalEntry_Table", "C_PostingKey", c => c.String(nullable: false, maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.C_GeneralJournalEntry_Table", "C_PostingKey", c => c.String(nullable: false, maxLength: 10));
            DropColumn("dbo.Tax", "Page_key");
        }
    }
}
