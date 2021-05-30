namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class kn2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsersPageAccess", "Page_id", "dbo.Pages");
            DropIndex("dbo.UsersPageAccess", new[] { "Page_id" });
            AlterColumn("dbo.UsersPageAccess", "Page_Id", c => c.Int());
            CreateIndex("dbo.UsersPageAccess", "Page_Id");
            AddForeignKey("dbo.UsersPageAccess", "Page_Id", "dbo.Pages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersPageAccess", "Page_Id", "dbo.Pages");
            DropIndex("dbo.UsersPageAccess", new[] { "Page_Id" });
            AlterColumn("dbo.UsersPageAccess", "Page_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.UsersPageAccess", "Page_id");
            AddForeignKey("dbo.UsersPageAccess", "Page_id", "dbo.Pages", "Id", cascadeDelete: true);
        }
    }
}
