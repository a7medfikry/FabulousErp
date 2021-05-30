namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay33 : DbMigration
    {
        public override void Up()
        {
            
            AddForeignKey("dbo.Payable_Group_setting", "Tax_group_id", "dbo.TaxGroup_table", "TG_ID");
            AddForeignKey("dbo.Creditor_setting", "Tax_group_id", "dbo.TaxGroup_table", "TG_ID");
        }

        public override void Down()
        {
        }
    }
}
