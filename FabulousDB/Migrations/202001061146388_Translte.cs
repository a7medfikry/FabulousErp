namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Translte : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Translate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(maxLength: 200),
                        English = c.String(maxLength: 200),
                        Arabic = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(x=>x.Key)
                .Index(x=>x.Arabic)
                .Index(x=>x.English)
                ;
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Translate");
        }
    }
}
