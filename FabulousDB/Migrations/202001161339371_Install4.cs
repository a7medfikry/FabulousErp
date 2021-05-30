namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Install4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Custom_installment", "Percetage", c => c.Single(nullable: false));
            AlterColumn("dbo.Installment_setting", "Cash_advanced_percentage", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Installment_setting", "Cash_advanced_percentage", c => c.Double(nullable: false));
            AlterColumn("dbo.Custom_installment", "Percetage", c => c.Double(nullable: false));
        }
    }
}
