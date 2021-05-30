namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Inv_sales_item_serial", "Start_warranty", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Inv_sales_item_serial", "End_warranty", c => c.DateTime(storeType: "date"));
            AlterColumn("dbo.Inv_sales_item_serial", "Expiry_date", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Inv_sales_item_serial", "Expiry_date", c => c.DateTime());
            AlterColumn("dbo.Inv_sales_item_serial", "End_warranty", c => c.DateTime());
            AlterColumn("dbo.Inv_sales_item_serial", "Start_warranty", c => c.DateTime());
        }
    }
}
