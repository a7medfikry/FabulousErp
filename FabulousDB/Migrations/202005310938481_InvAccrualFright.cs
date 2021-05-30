namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvAccrualFright : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_gorup_gl_accounts", "Accrual_fright_id", c => c.Int());
            AddColumn("dbo.Inv_item_gl_accounts", "Accrual_fright_id", c => c.Int());
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Accrual_fright_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Accrual_fright_id");
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Accrual_fright_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Inv_item_gl_accounts", "Accrual_fright_id", "dbo.C_CreateAccount_Table", "C_AID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_gl_accounts", "Accrual_fright_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Accrual_fright_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Accrual_fright_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Accrual_fright_id" });
            DropColumn("dbo.Inv_item_gl_accounts", "Accrual_fright_id");
            DropColumn("dbo.Inv_gorup_gl_accounts", "Accrual_fright_id");
        }
    }
}
