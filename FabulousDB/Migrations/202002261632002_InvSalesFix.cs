namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvSalesFix : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AddColumn("dbo.Inv_sales_invoice", "PO_id", c => c.Int());
            AddColumn("dbo.Inv_sales_invoice", "Buyer", c => c.String());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_sales_invoice", "Doc_type", c => c.Int());
            AlterColumn("dbo.Inv_sales_invoice", "Batch_id", c => c.Int());
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_sales_invoice", "PO_id");
            AddForeignKey("dbo.Inv_sales_invoice", "PO_id", "dbo.Inv_po", "Id");
            DropColumn("dbo.Inv_receive_po", "GR_no");
            DropColumn("dbo.Inv_sales_invoice", "Doc_no");
            DropColumn("dbo.Inv_sales_invoice", "Transaction_date");
            DropColumn("dbo.Inv_sales_invoice", "Posting_date");
            DropColumn("dbo.Inv_sales_invoice", "Sales_person");
            DropColumn("dbo.Inv_sales_invoice", "So_no");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_invoice", "So_no", c => c.String(maxLength: 500));
            AddColumn("dbo.Inv_sales_invoice", "Sales_person", c => c.String());
            AddColumn("dbo.Inv_sales_invoice", "Posting_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inv_sales_invoice", "Transaction_date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Inv_sales_invoice", "Doc_no", c => c.String(maxLength: 500));
            AddColumn("dbo.Inv_receive_po", "GR_no", c => c.Int(nullable: false));
            DropForeignKey("dbo.Inv_sales_invoice", "PO_id", "dbo.Inv_po");
            DropIndex("dbo.Inv_sales_invoice", new[] { "PO_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            AlterColumn("dbo.Inv_sales_invoice", "Batch_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_sales_invoice", "Doc_type", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_sales_invoice", "Buyer");
            DropColumn("dbo.Inv_sales_invoice", "PO_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
