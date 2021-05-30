namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGOGR : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Gr_num", c => c.Int(nullable: true));
            AddColumn("dbo.Inv_sales_invoice", "Go_num", c => c.Int(nullable: true));
        }
        
        public override void Down()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_sales_invoice", "Go_num");
            DropColumn("dbo.Inv_receive_po", "Gr_num");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
