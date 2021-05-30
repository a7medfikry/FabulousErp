namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvIteGroup : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_item_group", "Vat_Item_type", c => c.Int(nullable: true));
        }
        public override void Down()
        {
            AlterColumn("dbo.Inv_item_group", "Vat_Item_type", c => c.Int());
        }
    }
}
