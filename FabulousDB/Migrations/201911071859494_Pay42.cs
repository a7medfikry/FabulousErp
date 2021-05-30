namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay42 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting");
            DropIndex("dbo.Payable_gl_account", new[] { "Payable_Group_setting_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Creditor_setting_id" });
            AlterColumn("dbo.Payable_gl_account", "Payable_Group_setting_id", c => c.Int());
            AlterColumn("dbo.Payable_gl_account", "Creditor_setting_id", c => c.Int());
            CreateIndex("dbo.Payable_gl_account", "Payable_Group_setting_id");
            CreateIndex("dbo.Payable_gl_account", "Creditor_setting_id");
            AddForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting", "Id");
            AddForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting");
            DropForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting");
            DropIndex("dbo.Payable_gl_account", new[] { "Creditor_setting_id" });
            DropIndex("dbo.Payable_gl_account", new[] { "Payable_Group_setting_id" });
            AlterColumn("dbo.Payable_gl_account", "Creditor_setting_id", c => c.Int(nullable: false));
            AlterColumn("dbo.Payable_gl_account", "Payable_Group_setting_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Payable_gl_account", "Creditor_setting_id");
            CreateIndex("dbo.Payable_gl_account", "Payable_Group_setting_id");
            AddForeignKey("dbo.Payable_gl_account", "Payable_Group_setting_id", "dbo.Payable_Group_setting", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Payable_gl_account", "Creditor_setting_id", "dbo.Creditor_setting", "Id", cascadeDelete: true);
        }
    }
}
