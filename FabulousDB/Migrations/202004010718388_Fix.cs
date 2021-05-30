namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fix : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id", c => c.Int());
            CreateIndex("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id");
            AddForeignKey("dbo.Inv_sales_receivs_pos", "Inv_sales_invoice_items_Id", "dbo.Inv_sales_invoice_items", "Id");
        }
    }
}
