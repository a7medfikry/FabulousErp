namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvPosEdit1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_sales_receivs_pos", "FK_dbo.Inv_sales_receivs_pos_dbo.Inv_item_Item_id");
            AddForeignKey("dbo.Inv_sales_receivs_pos", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: false);

        }

        public override void Down()
        {
        }
    }
}
