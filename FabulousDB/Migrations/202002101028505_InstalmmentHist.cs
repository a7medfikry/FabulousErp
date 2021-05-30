namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstalmmentHist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Installments", "Historical", c => c.Boolean(nullable: true));
        }
        public override void Down()
        {
            DropColumn("dbo.Installments", "Historical");
        }
    }
}
