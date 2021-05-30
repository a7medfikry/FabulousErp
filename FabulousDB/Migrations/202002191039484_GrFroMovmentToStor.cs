namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GrFroMovmentToStor : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AddColumn("dbo.Inv_store", "Next_goods_no", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            DropColumn("dbo.Inv_movment_GS", "Next_goods_no");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_movment_GS", "Next_goods_no", c => c.Int(nullable: false));
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_store", "Next_goods_no");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
