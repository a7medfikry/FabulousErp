namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvItems : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_receive_po_items", "Total");
        }
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_po_items", "Total", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
    }
}
