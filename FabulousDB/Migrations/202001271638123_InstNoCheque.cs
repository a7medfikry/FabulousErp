namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstNoCheque : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Installments", "Cheque_Date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Installments", "Cheque_Date", c => c.DateTime(nullable: false));
        }
    }
}
