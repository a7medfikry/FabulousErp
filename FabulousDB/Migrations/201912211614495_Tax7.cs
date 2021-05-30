namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Total_vat_amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tax", "Total_vat_amount");
        }
    }
}
