namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Assign_payable_doc", "Doc_Num", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Assign_payable_doc", "Doc_Num");
        }
    }
}
