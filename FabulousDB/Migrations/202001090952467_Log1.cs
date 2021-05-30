namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Log1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Logs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Old_value = c.String(),
                        New_value = c.String(),
                        Entit_name = c.String(maxLength: 200),
                        User_id = c.String(maxLength: 200),
                        Creation_date = c.DateTime(nullable: false),
                    })
                    .Index(x=>x.Entit_name)
                    .Index(x=>x.Creation_date)
                    .Index(x=>x.User_id)
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Logs");
        }
    }
}
