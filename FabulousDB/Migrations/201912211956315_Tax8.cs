namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tax", "Doc_num", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tax", "Doc_num", c => c.String(nullable: false));
        }
    }
}
