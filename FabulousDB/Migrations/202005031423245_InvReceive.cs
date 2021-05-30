namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvReceive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po_items", "IsReturn", c => c.Boolean(nullable: false,defaultValue:false,defaultValueSql:"0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_receive_po_items", "IsReturn");
        }
    }
}
