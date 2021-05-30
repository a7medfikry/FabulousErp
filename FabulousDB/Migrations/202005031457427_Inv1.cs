namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inv1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_receive_return_po_items", "Total");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_return_po_items", "Total", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
