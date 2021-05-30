namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class invetoryKeyUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_item", "Item_group_id", c => c.Int(nullable: true));

        }

        public override void Down()
        {
       }
    }
}
