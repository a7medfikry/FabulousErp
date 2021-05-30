namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Instllment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Installment_setting", "Increase_by", c => c.Int(nullable: false));
            AddColumn("dbo.Installment_setting", "Increase", c => c.Decimal(nullable: false, precision: 20, scale: 4));
            AddColumn("dbo.Installment_setting", "Penelty_by", c => c.Int(nullable: false));
            AddColumn("dbo.Installment_setting", "Penelty_after_daynum", c => c.Single(nullable: false));
            AddColumn("dbo.Installment_setting", "Penelty", c => c.Decimal(nullable: false, precision: 20, scale: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Installment_setting", "Penelty");
            DropColumn("dbo.Installment_setting", "Penelty_after_daynum");
            DropColumn("dbo.Installment_setting", "Penelty_by");
            DropColumn("dbo.Installment_setting", "Increase");
            DropColumn("dbo.Installment_setting", "Increase_by");
        }
    }
}
