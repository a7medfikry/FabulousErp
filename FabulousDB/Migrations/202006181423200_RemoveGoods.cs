namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveGoods : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_goods_out", "Site_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_goods_out", "Store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_goods_reciept", "Site_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_goods_reciept", "Store_id", "dbo.Inv_store");
            DropIndex("dbo.Inv_goods_out", new[] { "Store_id" });
            DropIndex("dbo.Inv_goods_out", new[] { "Site_id" });
            DropIndex("dbo.Inv_goods_reciept", new[] { "Store_id" });
            DropIndex("dbo.Inv_goods_reciept", new[] { "Site_id" });
            DropTable("dbo.Inv_goods_out");
            DropTable("dbo.Inv_goods_reciept");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Inv_goods_reciept",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gr_no = c.String(),
                        Pr_no_id = c.Int(nullable: false),
                        Store_id = c.Int(nullable: false),
                        Site_id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Vendor_doc = c.String(),
                        Po_no = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_goods_out",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gr_no = c.String(),
                        Request_no = c.Int(nullable: false),
                        Store_id = c.Int(nullable: false),
                        Site_id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Vendor_doc = c.String(),
                        Po_no = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Inv_goods_reciept", "Site_id");
            CreateIndex("dbo.Inv_goods_reciept", "Store_id");
            CreateIndex("dbo.Inv_goods_out", "Site_id");
            CreateIndex("dbo.Inv_goods_out", "Store_id");
            AddForeignKey("dbo.Inv_goods_reciept", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_goods_reciept", "Site_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_goods_out", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_goods_out", "Site_id", "dbo.Inv_store", "Id", cascadeDelete: true);
        }
    }
}
