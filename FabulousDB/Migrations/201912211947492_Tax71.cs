namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax71 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tax", "Doc_type", c => c.Int(nullable:true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tax", "Doc_type", c => c.Int(nullable: false));
        }
    }
}
