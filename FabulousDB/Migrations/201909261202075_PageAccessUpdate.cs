namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PageAccessUpdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.UsersPageAccess", new[] { "Page_Id" });
            AddColumn("dbo.UsersPageAccess", "View", c => c.Boolean(nullable: false));
            AddColumn("dbo.UsersPageAccess", "Edit", c => c.Boolean(nullable: false));
            AddColumn("dbo.UsersPageAccess", "Delete", c => c.Boolean(nullable: false));
            AddColumn("dbo.UsersPageAccess", "Update", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UsersPageAccess", "Page_Id", c => c.Int());
            AlterColumn("dbo.UsersPageAccess", "Page_id", c => c.Int(nullable: false));
            CreateIndex("dbo.UsersPageAccess", "Page_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.UsersPageAccess", new[] { "Page_Id" });
            AlterColumn("dbo.UsersPageAccess", "Page_id", c => c.Int());
            AlterColumn("dbo.UsersPageAccess", "Page_Id", c => c.Int(nullable: false));
            DropColumn("dbo.UsersPageAccess", "Update");
            DropColumn("dbo.UsersPageAccess", "Delete");
            DropColumn("dbo.UsersPageAccess", "Edit");
            DropColumn("dbo.UsersPageAccess", "View");
            CreateIndex("dbo.UsersPageAccess", "Page_Id");
        }
    }
}
