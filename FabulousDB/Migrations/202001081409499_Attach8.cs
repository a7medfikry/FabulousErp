namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attach8 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Attachment_head", "Relation_id", c => c.Int());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Attachment_head", "Relation_id");
        }
    }
}
