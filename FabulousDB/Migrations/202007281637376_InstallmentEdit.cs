namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstallmentEdit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Installment_setting", "Penelty_days", c => c.Single(nullable: false));
            DropColumn("dbo.Installment_setting", "Penelty_after_daynum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Installment_setting", "Penelty_after_daynum", c => c.Single(nullable: false));
            DropColumn("dbo.Installment_setting", "Penelty_days");
        }
    }
}
