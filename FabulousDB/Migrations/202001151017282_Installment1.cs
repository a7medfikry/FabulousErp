namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Installment1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.Custom_installment",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   Custom_no = c.String(maxLength: 200),
                   Percetage = c.Double(nullable: false),
                   Installment_setting_id = c.Int(nullable: false),
               })
               .PrimaryKey(t => t.Id)
               .ForeignKey("dbo.Installment_setting", t => t.Installment_setting_id, cascadeDelete: true)
               .Index(x=>x.Installment_setting_id);

            CreateTable(
                "dbo.Installment_setting",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Plan_id = c.String(maxLength: 200),
                    Type = c.Int(nullable: false),
                    Inactive = c.Boolean(nullable: false),
                    Cash_advanced_percentage = c.Double(nullable: false),
                    Number_of_installment = c.Int(nullable: false),
                    Installment_period = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(x => x.Installment_period)
                .Index(x => x.Type)
                .Index(x => x.Plan_id);

        }

        public override void Down()
        {
           
        }
    }
}
