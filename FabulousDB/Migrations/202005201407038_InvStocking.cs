namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvStocking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_stocking",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Posting_num = c.String(),
                        Po_id = c.Int(nullable: false),
                        Diffrence = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Actual = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Damage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Unit_cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_receive_po", t => t.Po_id, cascadeDelete: false)
                .Index(t => t.Po_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_stocking", "Po_id", "dbo.Inv_receive_po");
            DropIndex("dbo.Inv_stocking", new[] { "Po_id" });
            DropTable("dbo.Inv_stocking");
        }
    }
}
