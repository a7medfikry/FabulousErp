namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmInv : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_quotation_request","FK_Inv_quotation_request_Inv_purchase_request_Pr_no_id");
            DropForeignKey("dbo.Inv_quotation_request", "Pr_no_id", "dbo.Inv_purchase_request");
            DropIndex("dbo.Inv_quotation_request", new[] { "Pr_no_id" });
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_quotation_request", "Pr_no_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_request", "Pr_no_id");
            AddForeignKey("dbo.Inv_quotation_request", "Pr_no_id", "dbo.Inv_purchase_request", "Id", cascadeDelete: true);
        }
    }
}
