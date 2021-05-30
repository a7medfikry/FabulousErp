namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_store", "Store_gl_account_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Inv_store", new[] { "Store_gl_account_id" });
            AlterColumn("dbo.Inv_store", "Store_gl_account_id", c => c.Int());
            CreateIndex("dbo.Inv_store", "Store_gl_account_id");
            AddForeignKey("dbo.Inv_store", "Store_gl_account_id", "dbo.C_CreateAccount_Table", "C_AID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_store", "Store_gl_account_id", "dbo.C_CreateAccount_Table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_store", new[] { "Store_gl_account_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inv_store", "Store_gl_account_id", c => c.Int(nullable: false));
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_store", "Store_gl_account_id");
            AddForeignKey("dbo.Inv_store", "Store_gl_account_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
        }
    }
}
