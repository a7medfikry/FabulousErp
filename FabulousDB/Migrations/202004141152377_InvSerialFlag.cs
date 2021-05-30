namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvSerialFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_item_serial", "Sold", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_receive_item_serial", "Sold");
        }
    }
}
