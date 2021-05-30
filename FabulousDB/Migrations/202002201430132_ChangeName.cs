namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_purchase_request", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Inv_purchase_request", "Pr_date");
        }
        
        public override void Down()
        {
           
        }
    }
}
