namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Logs", "IX_Entit_name");
            AddColumn("dbo.Logs", "Entity_name", c => c.String(maxLength: 200));
            AddColumn("dbo.Logs", "Action", c => c.String(maxLength: 200));
            DropColumn("dbo.Logs", "Entit_name");
            CreateIndex("dbo.Logs", "Entity_name");
            CreateIndex("dbo.Logs", "Action");
        }

        public override void Down()
        {
            AddColumn("dbo.Logs", "Entit_name", c => c.String(maxLength: 200));
            DropColumn("dbo.Logs", "Action");
            DropColumn("dbo.Logs", "Entity_name");
        }
    }
}
