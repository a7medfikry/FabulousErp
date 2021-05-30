namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveForeignKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inv_item_group", "FK_Inv_item_group_C_TaxSetting_table_Tax_table_type_id");
            DropForeignKey("dbo.Inv_item_group", "FK_Inv_item_group_C_TaxSetting_table_Tax_type_id");
        }
        
        public override void Down()
        {
        }
    }
}
