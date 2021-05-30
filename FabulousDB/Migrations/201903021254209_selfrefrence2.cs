namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class selfrefrence2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.C_GeneralJournalEntry_Table", new[] { "VoidJENum_C_PostingNumber" });
            RenameColumn(table: "dbo.C_GeneralJournalEntry_Table", name: "VoidJENum_C_PostingNumber", newName: "PostJENum");
            AlterColumn("dbo.C_GeneralJournalEntry_Table", "PostJENum", c => c.Int(nullable: true));
            CreateIndex("dbo.C_GeneralJournalEntry_Table", "PostJENum");
        }
        
        public override void Down()
        {
            DropIndex("dbo.C_GeneralJournalEntry_Table", new[] { "PostJENum" });
            AlterColumn("dbo.C_GeneralJournalEntry_Table", "PostJENum", c => c.Int());
            RenameColumn(table: "dbo.C_GeneralJournalEntry_Table", name: "PostJENum", newName: "VoidJENum_C_PostingNumber");
            CreateIndex("dbo.C_GeneralJournalEntry_Table", "VoidJENum_C_PostingNumber");
        }
    }
}
