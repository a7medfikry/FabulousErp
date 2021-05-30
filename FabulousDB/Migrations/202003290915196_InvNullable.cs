namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvNullable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_quotation_request", "Po_id", "dbo.Inv_purchase_request");
            DropIndex("dbo.Inv_quotation_request", new[] { "Po_id" });
            AlterColumn("dbo.Inv_quotation_request", "Po_id", c => c.Int());
            CreateIndex("dbo.Inv_quotation_request", "Po_id");
            AddForeignKey("dbo.Inv_quotation_request", "Po_id", "dbo.Inv_purchase_request", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_quotation_request", "Po_id", "dbo.Inv_purchase_request");
            DropIndex("dbo.Inv_quotation_request", new[] { "Po_id" });
            AlterColumn("dbo.Inv_quotation_request", "Po_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_request", "Po_id");
            AddForeignKey("dbo.Inv_quotation_request", "Po_id", "dbo.Inv_purchase_request", "Id", cascadeDelete: true);
        }
    }
}
