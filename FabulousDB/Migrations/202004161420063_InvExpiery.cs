namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvExpiery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_item_serial", "Expiry_date", c => c.DateTime());
            AlterColumn("dbo.Inv_receive_item_serial", "Start_warranty", c => c.DateTime());
            AlterColumn("dbo.Inv_receive_item_serial", "End_warranty", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inv_receive_item_serial", "End_warranty", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Inv_receive_item_serial", "Start_warranty", c => c.DateTime(nullable: false));
            DropColumn("dbo.Inv_receive_item_serial", "Expiry_date");
        }
    }
}
