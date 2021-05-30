namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRemovDueDate : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_receive_po", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_po", "Date", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
