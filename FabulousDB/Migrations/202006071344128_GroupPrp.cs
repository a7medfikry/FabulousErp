namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupPrp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item_group", "Has_serial", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inv_item_group", "Has_warranty", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inv_item_group", "Has_expiry_date", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inv_item_group", "Martial_or_service", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item_group", "Martial_or_service");
            DropColumn("dbo.Inv_item_group", "Has_expiry_date");
            DropColumn("dbo.Inv_item_group", "Has_warranty");
            DropColumn("dbo.Inv_item_group", "Has_serial");
        }
    }
}
