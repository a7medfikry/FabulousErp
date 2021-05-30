namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqeue : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Inv_receive_item_serial", "Serial", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_receive_item_serial", new[] { "Serial" });
        }
    }
}
