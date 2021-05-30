namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PagesTblUpdat : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UsersPageAccess", new[] { "Page_Id" });
            AddColumn("dbo.Pages", "Page_section", c => c.String(maxLength: 50));
            AlterColumn("dbo.UsersPageAccess", "Page_Id", c => c.Int());
            AlterColumn("dbo.UsersPageAccess", "Page_id", c => c.Int(nullable: false));
            CreateIndex("dbo.UsersPageAccess", "Page_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UsersPageAccess", new[] { "Page_Id" });
            AlterColumn("dbo.UsersPageAccess", "Page_id", c => c.Int());
            AlterColumn("dbo.UsersPageAccess", "Page_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Pages", "Page_section");
            CreateIndex("dbo.UsersPageAccess", "Page_Id");
        }
    }
}
