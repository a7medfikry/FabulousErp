namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InventoryPoNum : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_po", "Po_num", c => c.Int(nullable: true));
        }

        public override void Down()
        {
           
        }
    }
}
