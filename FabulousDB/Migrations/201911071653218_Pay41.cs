namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pay41 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Creditor_setting", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Creditor_setting", new[] { "Currency_id" });
            CreateTable(
                "dbo.Creditro_currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Vendore_id = c.Int(nullable: false),
                        Currency_id = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Creditor_setting", t => t.Vendore_id, cascadeDelete: false)
                .Index(t => t.Vendore_id)
                .Index(t => t.Currency_id);
            
            DropColumn("dbo.Creditor_setting", "Currency_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Creditor_setting", "Currency_id", c => c.String(maxLength: 50));
            DropForeignKey("dbo.Creditro_currencies", "Vendore_id", "dbo.Creditor_setting");
            DropForeignKey("dbo.Creditro_currencies", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropIndex("dbo.Creditro_currencies", new[] { "Currency_id" });
            DropIndex("dbo.Creditro_currencies", new[] { "Vendore_id" });
            DropTable("dbo.Creditro_currencies");
            CreateIndex("dbo.Creditor_setting", "Currency_id");
            AddForeignKey("dbo.Creditor_setting", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
        }
    }
}
