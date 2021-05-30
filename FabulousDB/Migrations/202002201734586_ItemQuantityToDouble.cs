namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemQuantityToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_purchase_request_items", "Quntaty", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inv_purchase_request_items", "Quntaty", c => c.Int(nullable: false));

        }
    }
}
