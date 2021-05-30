namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInvFright : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_gorup_gl_accounts", "Fright_id", c => c.Int());
            AddColumn("dbo.Inv_item_gl_accounts", "Fright_id", c => c.Int());
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Fright_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Fright_id");
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Fright_id", "dbo.C_CreateAccount_Table", "C_AID");
            AddForeignKey("dbo.Inv_item_gl_accounts", "Fright_id", "dbo.C_CreateAccount_Table", "C_AID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_gl_accounts", "Fright_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Fright_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Fright_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Fright_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_item_gl_accounts", "Fright_id");
            DropColumn("dbo.Inv_gorup_gl_accounts", "Fright_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
