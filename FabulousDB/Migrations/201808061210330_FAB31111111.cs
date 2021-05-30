namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAB31111111 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreateGroup_Table",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        GroupID = c.String(nullable: false, maxLength: 50),
                        GroupName = c.String(nullable: false, maxLength: 30),
                        GroupDescription = c.String(nullable: false, maxLength: 50),
                        Date = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.GroupID, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.CreateGroup_Table", new[] { "GroupID" });
            DropTable("dbo.CreateGroup_Table");
        }
    }
}
