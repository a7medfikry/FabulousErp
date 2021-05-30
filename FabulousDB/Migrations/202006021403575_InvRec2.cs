namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRec2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_receive_po_items", "Orginal_qty");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_po_items", "Orginal_qty", c => c.Single(nullable: false));
        }
    }
}
