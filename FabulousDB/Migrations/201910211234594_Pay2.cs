namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Password_option",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HasPassword = c.Boolean(nullable: false),
                        Option = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Password_option");
        }
    }
}
