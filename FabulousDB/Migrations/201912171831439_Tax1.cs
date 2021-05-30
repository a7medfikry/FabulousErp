namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Tax1 : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.Tax",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Doc_type = c.Int(nullable: false),
                    Doc_num = c.String(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Vendor_name = c.String(),
                    Tax_reg_num = c.String(),
                    Tax_file_number = c.String(),
                    Address = c.String(),
                    National_id = c.String(),
                    Mobile_number = c.String(),
                    Tax_type = c.Int(nullable: false),
                    Other_tax_type = c.Int(nullable: false),
                    Item_name = c.String(),
                    Unit_of_measure_id = c.Int(nullable: false),
                    Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Unit_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Total_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Total_amount_sys_curr = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Net_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Tbl_vat_id = c.Int(nullable: false),
                    Table_vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Table_after_vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Vat_id = c.Int(nullable: false),
                    Dacutta_id = c.Int(nullable: false),
                    Dacutta_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Dacutta_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Tbl_vat_id, cascadeDelete: false)
                .ForeignKey("dbo.Unit_of_measure", t => t.Unit_of_measure_id, cascadeDelete: false)
                .ForeignKey("dbo.C_TaxSetting_table", t => t.Vat_id, cascadeDelete: false)
                .Index(t => t.Unit_of_measure_id)
                .Index(t => t.Tbl_vat_id)
                .Index(t => t.Vat_id)
                .Index(t => t.Dacutta_id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Tax", "Vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Tax", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Tax", "Tbl_vat_id", "dbo.C_TaxSetting_table");
            DropForeignKey("dbo.Tax", "Dacutta_id", "dbo.C_TaxSetting_table");
            DropIndex("dbo.Tax", new[] { "Dacutta_id" });
            DropIndex("dbo.Tax", new[] { "Vat_id" });
            DropIndex("dbo.Tax", new[] { "Tbl_vat_id" });
            DropIndex("dbo.Tax", new[] { "Unit_of_measure_id" });
            DropTable("dbo.Tax");
        }
    }
}
