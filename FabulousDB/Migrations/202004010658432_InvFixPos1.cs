namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvFixPos1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Inv_sales_receivs_pos", "IX_Inv_sales_invoice_items_Id");
            DropForeignKey("Inv_sales_receivs_pos", "FK_dbo.Inv_sales_receivs_pos_dbo.Inv_sales_invoice_items_Inv_sales_invoice_items_Id");
            DropColumn("Inv_sales_receivs_pos", "Inv_sales_invoice_items_id");
        }
        
        public override void Down()
        {
        }
    }
}
