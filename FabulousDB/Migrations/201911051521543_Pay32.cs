namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay32 : DbMigration
    {
        public override void Up()
        {
            Sql("alter table Payable_Group_setting drop constraint [FK_dbo.Payable_Group_setting_dbo.C_TaxSetting_table_Tax_group_id]");
            Sql("alter table Creditor_setting drop constraint [FK_dbo.Creditor_setting_dbo.C_TaxSetting_table_Tax_group_id]");
        }

        public override void Down()
        {
        }
    }
}
