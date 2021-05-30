namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Creditor_setting", "Group_setting_id", "dbo.Payable_Group_setting");
            DropIndex("dbo.Creditor_setting", new[] { "Group_setting_id" });
            AlterColumn("dbo.Creditor_setting", "Group_setting_id", c => c.Int());
            CreateIndex("dbo.Creditor_setting", "Group_setting_id");
            AddForeignKey("dbo.Creditor_setting", "Group_setting_id", "dbo.Payable_Group_setting", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Creditor_setting", "Group_setting_id", "dbo.Payable_Group_setting");
            DropIndex("dbo.Creditor_setting", new[] { "Group_setting_id" });
            AlterColumn("dbo.Creditor_setting", "Group_setting_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Creditor_setting", "Group_setting_id");
            AddForeignKey("dbo.Creditor_setting", "Group_setting_id", "dbo.Payable_Group_setting", "Id", cascadeDelete: true);
        }
    }
}
