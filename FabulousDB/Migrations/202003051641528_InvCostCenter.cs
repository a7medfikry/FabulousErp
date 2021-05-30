namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvCostCenter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inv_item", "Cost_center_id", c => c.String(maxLength: 20));
            AddColumn("dbo.Inv_item", "Cost_center_account_id", c => c.Int());
            AddColumn("dbo.Inv_item_group", "Cost_center_id", c => c.String(maxLength: 20));
            AddColumn("dbo.Inv_item_group", "Cost_center_account_id", c => c.Int());
            CreateIndex("dbo.Inv_item", "Cost_center_id");
            CreateIndex("dbo.Inv_item", "Cost_center_account_id");
            CreateIndex("dbo.Inv_item_group", "Cost_center_id");
            CreateIndex("dbo.Inv_item_group", "Cost_center_account_id");
            AddForeignKey("dbo.Inv_item", "Cost_center_id", "dbo.C_CostCenter_Table", "C_CostCenterID");
            AddForeignKey("dbo.Inv_item", "Cost_center_account_id", "dbo.C_CostCenterAccounts_Table", "C_CAID");
            AddForeignKey("dbo.Inv_item_group", "Cost_center_id", "dbo.C_CostCenter_Table", "C_CostCenterID");
            AddForeignKey("dbo.Inv_item_group", "Cost_center_account_id", "dbo.C_CostCenterAccounts_Table", "C_CAID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_item_group", "Cost_center_account_id", "dbo.C_CostCenterAccounts_Table");
            DropForeignKey("dbo.Inv_item_group", "Cost_center_id", "dbo.C_CostCenter_Table");
            DropForeignKey("dbo.Inv_item", "Cost_center_account_id", "dbo.C_CostCenterAccounts_Table");
            DropForeignKey("dbo.Inv_item", "Cost_center_id", "dbo.C_CostCenter_Table");
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_item_group", new[] { "Cost_center_account_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Cost_center_id" });
            DropIndex("dbo.Inv_item", new[] { "Cost_center_account_id" });
            DropIndex("dbo.Inv_item", new[] { "Cost_center_id" });
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_id", c => c.Int());
            AlterColumn("dbo.Inv_quotation_receiving", "Vendore_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inv_item_group", "Cost_center_account_id");
            DropColumn("dbo.Inv_item_group", "Cost_center_id");
            DropColumn("dbo.Inv_item", "Cost_center_account_id");
            DropColumn("dbo.Inv_item", "Cost_center_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
        }
    }
}
