namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attach5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Attachment_head", "C_PostingNumber", "dbo.C_GeneralJournalEntry_Table");
            DropIndex("dbo.Attachment_head", new[] { "C_PostingNumber" });
            AlterColumn("dbo.Attachment_head", "C_PostingNumber", c => c.Int());
            CreateIndex("dbo.Attachment_head", "C_PostingNumber");
            AddForeignKey("dbo.Attachment_head", "C_PostingNumber", "dbo.C_GeneralJournalEntry_Table", "C_PostingNumber");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachment_head", "C_PostingNumber", "dbo.C_GeneralJournalEntry_Table");
            DropIndex("dbo.Attachment_head", new[] { "C_PostingNumber" });
            AlterColumn("dbo.Attachment_head", "C_PostingNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.Attachment_head", "C_PostingNumber");
            AddForeignKey("dbo.Attachment_head", "C_PostingNumber", "dbo.C_GeneralJournalEntry_Table", "C_PostingNumber", cascadeDelete: true);
        }
    }
}
