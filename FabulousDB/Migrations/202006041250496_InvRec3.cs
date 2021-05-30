namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRec3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_po_item_transfer", "Doc_type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_po_item_transfer", "Doc_type");
        }
    }
}
