namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Logs", "Action", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Logs", "Action", c => c.String());
        }
    }
}
