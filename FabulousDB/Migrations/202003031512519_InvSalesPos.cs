namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvSalesPos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_sales_receivs_pos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Po_id = c.Int(nullable: false),
                        Sales_item_id = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Po_id)
                .Index(t => t.Sales_item_id);
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_invoice_items", "Inv_po_receive", c => c.Int(nullable: false));
            DropIndex("dbo.Inv_sales_receivs_pos", new[] { "Sales_item_id" });
            DropIndex("dbo.Inv_sales_receivs_pos", new[] { "Po_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropTable("dbo.Inv_sales_receivs_pos");
            RenameColumn(table: "dbo.Inv_sales_receivs_pos", name: "Sales_item_id", newName: "Inv_po_receive");
            RenameColumn(table: "dbo.Inv_sales_receivs_pos", name: "Po_id", newName: "Inv_po_Id");
            CreateIndex("dbo.Inv_sales_invoice_items", "Inv_po_receive");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
