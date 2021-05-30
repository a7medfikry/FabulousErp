namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tax", "Tax_group_id", c => c.Int(defaultValue:0,defaultValueSql:"0"));
            AlterColumn("dbo.Tax", "Unit_of_measure_id", c => c.Int(defaultValue: 0, defaultValueSql: "0"));
            AlterColumn("dbo.Tax", "Tbl_vat_id", c => c.Int(defaultValue: 0, defaultValueSql: "0"));
            AlterColumn("dbo.Tax", "Vat_id", c => c.Int(defaultValue: 0, defaultValueSql: "0"));
            AlterColumn("dbo.Tax", "Dacutta_id", c => c.Int(defaultValue: 0, defaultValueSql: "0"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tax", "Dacutta_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tax", "Vat_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tax", "Tbl_vat_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tax", "Unit_of_measure_id", c => c.Int(nullable: false));
        }
    }
}
