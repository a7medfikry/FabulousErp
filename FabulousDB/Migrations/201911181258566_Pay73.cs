namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay73 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_transaction", "Shipping_method_id", "dbo.Shipping_method");
            DropIndex("dbo.Payable_transaction", new[] { "Shipping_method_id" });
            AlterColumn("dbo.Payable_transaction", "Shipping_method_id", c => c.Int());
            CreateIndex("dbo.Payable_transaction", "Shipping_method_id");
            AddForeignKey("dbo.Payable_transaction", "Shipping_method_id", "dbo.Shipping_method", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_transaction", "Shipping_method_id", "dbo.Shipping_method");
            DropIndex("dbo.Payable_transaction", new[] { "Shipping_method_id" });
            AlterColumn("dbo.Payable_transaction", "Shipping_method_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_transaction", "Shipping_method_id");
            AddForeignKey("dbo.Payable_transaction", "Shipping_method_id", "dbo.Shipping_method", "Id", cascadeDelete: true);
        }
    }
}
