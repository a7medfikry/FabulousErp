namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Att2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Attachment_files", "File_type", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Attachment_files", "File_type");
        }
    }
}
