namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShowUnitPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_sales_GS", "Show_unit_price", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Inv_sales_GS", "Show_unit_price");
        }
    }
}
