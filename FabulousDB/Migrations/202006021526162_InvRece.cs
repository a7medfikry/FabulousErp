namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRece : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_receive_po", "Transfer_num");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_po", "Transfer_num", c => c.Int());
        }
    }
}
