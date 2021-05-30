namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class selfrefrence : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.C_GeneralJournalEntry_Table", "VoidJENum_C_PostingNumber", c => c.Int());
            CreateIndex("dbo.C_GeneralJournalEntry_Table", "VoidJENum_C_PostingNumber");
            AddForeignKey("dbo.C_GeneralJournalEntry_Table", "VoidJENum_C_PostingNumber", "dbo.C_GeneralJournalEntry_Table", "C_PostingNumber");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.C_GeneralJournalEntry_Table", "VoidJENum_C_PostingNumber", "dbo.C_GeneralJournalEntry_Table");
            DropIndex("dbo.C_GeneralJournalEntry_Table", new[] { "VoidJENum_C_PostingNumber" });
            DropColumn("dbo.C_GeneralJournalEntry_Table", "VoidJENum_C_PostingNumber");
        }
    }
}
