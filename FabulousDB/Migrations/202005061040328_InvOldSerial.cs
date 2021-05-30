namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvOldSerial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_old_receive_item_serial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Serial_item_id = c.Int(nullable: false),
                        Old_po_id = c.Int(nullable: false),
                        Transfer_date = c.DateTime(nullable: false,defaultValue:DateTime.Now, defaultValueSql: "GetDate()"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_receive_po", t => t.Old_po_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_receive_item_serial", t => t.Serial_item_id, cascadeDelete: false)
                .Index(t => t.Serial_item_id)
                .Index(t => t.Old_po_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_old_receive_item_serial", "Serial_item_id", "dbo.Inv_receive_item_serial");
            DropForeignKey("dbo.Inv_old_receive_item_serial", "Old_po_id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_old_receive_item_serial", new[] { "Old_po_id" });
            DropIndex("dbo.Inv_old_receive_item_serial", new[] { "Serial_item_id" });
            DropTable("dbo.Inv_old_receive_item_serial");
        }
    }
}
