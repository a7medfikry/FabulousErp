namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PayToFixedAssets : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payable_transaction", "Fixed_assets_trx", x => x.Boolean(nullable: false, defaultValue: false, defaultValueSql: "false"));
        }
        
        public override void Down()
        {
             }
    }
}
