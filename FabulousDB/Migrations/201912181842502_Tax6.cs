namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tax6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.C_TaxSetting_table", "Transaction_type", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.C_TaxSetting_table", "Transaction_type");
        }
    }
}
