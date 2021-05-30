namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InstallmentUpdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Installments", "Paid", c => c.Boolean(nullable: false,defaultValue:true,defaultValueSql:"0"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Installments", "Paid");
        }
    }
}
