namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Vat_amount", c => c.Decimal(precision: 30, scale: 9,defaultValue:0,defaultValueSql:"0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tax", "Vat_amount");
        }
    }
}
