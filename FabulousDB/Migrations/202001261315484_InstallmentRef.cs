namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstallmentRef : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Installments", "Refrence", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Installments", "Refrence");
        }
    }
}
