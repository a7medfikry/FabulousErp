namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRmQty : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_quotation_request", "Quantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_quotation_request", "Quantity", c => c.Int(nullable: false));
        }
    }
}
