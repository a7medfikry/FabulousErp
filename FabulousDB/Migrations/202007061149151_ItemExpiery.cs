namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemExpiery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_receive_expiery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Serial_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_receive_item_serial", t => t.Serial_id, cascadeDelete: true)
                .Index(t => t.Serial_id);
            
            DropColumn("dbo.Inv_receive_item_serial", "Expiry_date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_receive_item_serial", "Expiry_date", c => c.DateTime());
            DropForeignKey("dbo.Inv_receive_expiery", "Serial_id", "dbo.Inv_receive_item_serial");
            DropIndex("dbo.Inv_receive_expiery", new[] { "Serial_id" });
            DropTable("dbo.Inv_receive_expiery");
        }
    }
}
