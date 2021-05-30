namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvProfitUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_receive_po", "Profit_user_id", c => c.String(maxLength: 50));
            AddColumn("dbo.Inv_sales_invoice", "Profit_user_id", c => c.String(maxLength: 50));
            CreateIndex("dbo.Inv_receive_po", "Profit_user_id");
            CreateIndex("dbo.Inv_sales_invoice", "Profit_user_id");
            AddForeignKey("dbo.Inv_sales_invoice", "Profit_user_id", "dbo.CreateAccount_Table", "UserID");
            AddForeignKey("dbo.Inv_receive_po", "Profit_user_id", "dbo.CreateAccount_Table", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_receive_po", "Profit_user_id", "dbo.CreateAccount_Table");
            DropForeignKey("dbo.Inv_sales_invoice", "Profit_user_id", "dbo.CreateAccount_Table");
            DropIndex("dbo.Inv_sales_invoice", new[] { "Profit_user_id" });
            DropIndex("dbo.Inv_receive_po", new[] { "Profit_user_id" });
            DropColumn("dbo.Inv_sales_invoice", "Profit_user_id");
            DropColumn("dbo.Inv_receive_po", "Profit_user_id");
        }
    }
}
