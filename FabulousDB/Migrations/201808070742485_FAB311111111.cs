namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAB311111111 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountGroup_Table",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 50),
                        GroupID = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.CreateAccount_Table", "DateOfAssignGroup", c => c.String(maxLength: 20));
            AddColumn("dbo.CreateGroup_Table", "DisActive", c => c.Boolean());
            AddColumn("dbo.CreateGroup_Table", "Deleted", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreateGroup_Table", "Deleted");
            DropColumn("dbo.CreateGroup_Table", "DisActive");
            DropColumn("dbo.CreateAccount_Table", "DateOfAssignGroup");
            DropTable("dbo.AccountGroup_Table");
        }
    }
}
