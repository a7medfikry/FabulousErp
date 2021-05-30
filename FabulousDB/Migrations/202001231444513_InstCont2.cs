namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstCont2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Installment_contract", "Contract_no", c => c.String(maxLength: 200));
            AlterColumn("dbo.Installment_contract", "Desc", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Installment_contract", "Desc", c => c.String());
            AlterColumn("dbo.Installment_contract", "Contract_no", c => c.String());
        }
    }
}
