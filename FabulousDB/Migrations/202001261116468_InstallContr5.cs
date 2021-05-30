namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstallContr5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Installment_contract", "IsPay", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Installment_contract", "IsPay");
        }
    }
}
