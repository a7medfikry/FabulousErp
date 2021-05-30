namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CstCenter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cost_center_accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Account_id = c.Int(nullable: false),
                        Cost_center_id = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Account_id, cascadeDelete: true)
                .ForeignKey("dbo.C_CostCenter_Table", t => t.Cost_center_id)
                .Index(t => t.Account_id)
                .Index(t => t.Cost_center_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cost_center_accounts", "Cost_center_id", "dbo.C_CostCenter_Table");
            DropForeignKey("dbo.Cost_center_accounts", "Account_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Cost_center_accounts", new[] { "Cost_center_id" });
            DropIndex("dbo.Cost_center_accounts", new[] { "Account_id" });
            DropTable("dbo.Cost_center_accounts");
        }
    }
}
