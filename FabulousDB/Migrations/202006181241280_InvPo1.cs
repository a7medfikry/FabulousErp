namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPo1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Payment_terms_id", c => c.Int());
            AddColumn("dbo.Inv_receive_po", "Shipping_method_id", c => c.Int());
            AddColumn("dbo.Inv_receive_po", "Shipping_methods_Id", c => c.Int());
            CreateIndex("dbo.Inv_receive_po", "Payment_terms_id");
            CreateIndex("dbo.Inv_receive_po", "Shipping_methods_Id");
            AddForeignKey("dbo.Inv_receive_po", "Payment_terms_id", "dbo.Payable_payment_term", "Id");
            AddForeignKey("dbo.Inv_receive_po", "Shipping_methods_Id", "dbo.Payable_shipping_method", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po", "Shipping_methods_Id", "dbo.Payable_shipping_method");
            DropForeignKey("dbo.Inv_receive_po", "Payment_terms_id", "dbo.Payable_payment_term");
            DropIndex("dbo.Inv_receive_po", new[] { "Shipping_methods_Id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Payment_terms_id" });
            DropColumn("dbo.Inv_receive_po", "Shipping_methods_Id");
            DropColumn("dbo.Inv_receive_po", "Shipping_method_id");
            DropColumn("dbo.Inv_receive_po", "Payment_terms_id");
        }
    }
}
