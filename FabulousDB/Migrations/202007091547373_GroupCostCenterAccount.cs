namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupCostCenterAccount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groupcostcenter_accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account_id = c.Int(nullable: false),
                        Group_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Account_id, cascadeDelete: false)
                .ForeignKey("dbo.C_GroupCostCenter_Table", t => t.Group_id, cascadeDelete: false)
                .Index(t => t.Account_id)
                .Index(t => t.Group_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Groupcostcenter_accounts", "Group_id", "dbo.C_GroupCostCenter_Table");
            DropForeignKey("dbo.Groupcostcenter_accounts", "Account_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Groupcostcenter_accounts", new[] { "Group_id" });
            DropIndex("dbo.Groupcostcenter_accounts", new[] { "Account_id" });
            DropTable("dbo.Groupcostcenter_accounts");
        }
    }
}
