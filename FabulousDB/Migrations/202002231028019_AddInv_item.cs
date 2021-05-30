namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInv_item : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item", "Vat_Item_type", c => c.Int(nullable:true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item", "Vat_Item_type");
        }
    }
}
