namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvAddHasSerial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item", "Has_serial", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_item", "Has_serial");
        }
    }
}
