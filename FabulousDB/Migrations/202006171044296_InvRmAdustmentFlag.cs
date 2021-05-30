namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRmAdustmentFlag : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_item_adjustment", "Site_transfer");
        }
        public override void Down()
        {
            AddColumn("dbo.Inv_item_adjustment", "Site_transfer", c => c.Boolean(nullable: false));
        }
    }
}
