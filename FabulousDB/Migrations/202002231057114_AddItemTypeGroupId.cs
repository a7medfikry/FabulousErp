namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItemTypeGroupId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item_group", "Vat_Item_type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item_group", "Vat_Item_type");
        }
    }
}
