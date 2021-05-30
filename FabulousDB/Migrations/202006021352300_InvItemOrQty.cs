namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvItemOrQty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po_items", "Orginal_qty", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_receive_po_items", "Orginal_qty");
        }
    }
}
