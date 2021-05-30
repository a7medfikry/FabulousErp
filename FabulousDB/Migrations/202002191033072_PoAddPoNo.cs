namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PoAddPoNo : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AddColumn("dbo.Inv_po_GS", "Next_po_no", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            DropColumn("dbo.Inv_movment_GS", "Next_po_no");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_movment_GS", "Next_po_no", c => c.Int(nullable: false));
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_po_GS", "Next_po_no");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
