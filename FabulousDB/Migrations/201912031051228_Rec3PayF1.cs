namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rec3PayF1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Receivable_vendore_setting", "Inactive", c => c.Boolean(nullable: false,defaultValueSql:"0"));
            AlterColumn("dbo.Receivable_vendore_setting", "Customer", c => c.Boolean(nullable: false, defaultValueSql: "0"));
            AlterColumn("dbo.Receivable_vendore_setting", "Revaluate", c => c.Boolean(nullable: false, defaultValueSql: "0"));

            AlterColumn("dbo.Payable_creditor_setting", "Inactive", c => c.Boolean(nullable: false,defaultValueSql:"0"));
            AlterColumn("dbo.Payable_creditor_setting", "Customer", c => c.Boolean(nullable: false, defaultValueSql: "0"));
            AlterColumn("dbo.Payable_creditor_setting", "Revaluate", c => c.Boolean(nullable: false, defaultValueSql: "0"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Receivable_vendore_setting", "Revaluate", c => c.Boolean());
            AlterColumn("dbo.Receivable_vendore_setting", "Customer", c => c.Boolean());
            AlterColumn("dbo.Receivable_vendore_setting", "Inactive", c => c.Boolean());
        }
    }
}
