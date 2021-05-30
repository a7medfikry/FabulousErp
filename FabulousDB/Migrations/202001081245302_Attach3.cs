namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attach3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachment_files", "File_key", c => c.String());
            DropColumn("dbo.Attachment_files", "File_type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Attachment_files", "File_type", c => c.Int());
            DropColumn("dbo.Attachment_files", "File_key");
        }
    }
}
