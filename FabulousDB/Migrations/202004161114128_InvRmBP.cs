namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvRmBP : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Inv_sales_invoice", "Batch_id");
            DropColumn("dbo.Inv_sales_invoice", "Buyer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_invoice", "Buyer", c => c.String());
            AddColumn("dbo.Inv_sales_invoice", "Batch_id", c => c.Int());
        }
    }
}
