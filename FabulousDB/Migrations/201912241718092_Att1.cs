namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Att1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachment_files",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        File = c.String(maxLength: 200),
                        Attachment_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attachment_head", t => t.Attachment_id, cascadeDelete: true)
                .Index(t => t.Attachment_id);
            
            CreateTable(
                "dbo.Attachment_head",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Page = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attachment_files", "Attachment_id", "dbo.Attachment_head");
            DropIndex("dbo.Attachment_files", new[] { "Attachment_id" });
            DropTable("dbo.Attachment_head");
            DropTable("dbo.Attachment_files");
        }
    }
}
