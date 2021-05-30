namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item", "Has_warranty", c => c.Boolean(nullable: false,defaultValue:false));
            AddColumn("dbo.Inv_item", "Has_expiry_date", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item", "Has_expiry_date");
            DropColumn("dbo.Inv_item", "Has_warranty");
        }
    }
}
