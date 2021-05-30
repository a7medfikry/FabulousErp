namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax51 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Item_code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tax", "Item_code");
        }
    }
}
