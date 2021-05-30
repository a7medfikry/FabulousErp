namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvModif2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_receive_po", "Vendore_id", "dbo.Payable_creditor_setting");
            DropForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting");
            DropIndex("dbo.Inv_receive_po", new[] { "Vendore_id" });
            DropIndex("dbo.Inv_sales_invoice", new[] { "Customer_id" });
            AlterColumn("dbo.Inv_receive_po", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_sales_invoice", "Customer_id", c => c.Int());
            CreateIndex("dbo.Inv_receive_po", "Vendore_id");
            CreateIndex("dbo.Inv_sales_invoice", "Customer_id");
            AddForeignKey("dbo.Inv_receive_po", "Vendore_id", "dbo.Payable_creditor_setting", "Id");
            AddForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting");
            DropForeignKey("dbo.Inv_receive_po", "Vendore_id", "dbo.Payable_creditor_setting");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Customer_id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Vendore_id" });
            AlterColumn("dbo.Inv_sales_invoice", "Customer_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_receive_po", "Vendore_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_sales_invoice", "Customer_id");
            CreateIndex("dbo.Inv_receive_po", "Vendore_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po", "Vendore_id", "dbo.Payable_creditor_setting", "Id", cascadeDelete: true);
        }
    }
}
