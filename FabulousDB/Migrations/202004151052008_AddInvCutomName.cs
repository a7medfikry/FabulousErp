namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvCutomName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_invoice_items", "Custom_name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_sales_invoice_items", "Custom_name");
        }
    }
}
