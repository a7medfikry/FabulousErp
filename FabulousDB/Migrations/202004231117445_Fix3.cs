namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Assets_main", "Assets_number", c => c.String(maxLength: 200));
        }

        public override void Down()
        {
            AddColumn("dbo.Assets_main", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Assets_main", "Assets_number", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Assets_main", "Description", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
