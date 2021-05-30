namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay47 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aging_date_option",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date_option = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.Aging_period", "Date_option");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Aging_period", "Date_option", c => c.Int(nullable: false));
            DropTable("dbo.Aging_date_option");
        }
    }
}
