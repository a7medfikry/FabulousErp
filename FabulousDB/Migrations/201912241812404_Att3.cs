namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Att3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Attachment_files", "File_type", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Attachment_files", "File_type", c => c.Int(nullable: false));
        }
    }
}
