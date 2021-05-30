namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RmInv2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_quotation_request", "FK_dbo.Inv_request_for_quotation_dbo.Inv_purchase_Pr_no_id");
            DropColumn("dbo.Inv_quotation_request", "Pr_no_id");

        }

        public override void Down()
        {
        }
    }
}
