namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inv_adjustment_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Adjustment_no = c.String(),
                        Adjustment_date = c.DateTime(nullable: false),
                        Posting_date = c.DateTime(nullable: false),
                        From_store_id = c.Int(nullable: false),
                        To_store_id = c.Int(nullable: false),
                        Ref = c.String(),
                        item_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Unite_cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Lot_id = c.Int(nullable: false),
                        Store_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_store", t => t.From_store_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_items_serial", t => t.Lot_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.Store_Id)
                .ForeignKey("dbo.Inv_store", t => t.To_store_id, cascadeDelete: false)
                .Index(t => t.From_store_id)
                .Index(t => t.To_store_id)
                .Index(t => t.item_id)
                .Index(t => t.Lot_id)
                .Index(t => t.Store_Id);
            
            CreateTable(
                "dbo.Inv_store",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Store_id = c.String(maxLength: 200),
                        Store_name = c.String(maxLength: 500),
                        Address = c.String(maxLength: 500),
                        City = c.String(maxLength: 200),
                        State = c.String(maxLength: 200),
                        Country = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 200),
                        Mobile = c.String(maxLength: 200),
                        Fax = c.String(maxLength: 200),
                        Contact_person = c.String(maxLength: 200),
                        Email = c.String(maxLength: 200),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.String(maxLength: 200),
                        Description = c.String(maxLength: 500),
                        Short_description = c.String(maxLength: 200),
                        Item_group_id = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Unit_of_measure_id = c.Int(nullable: false),
                        Sales_tax_group_id = c.Int(nullable: false),
                        Purchase_tax_group_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item_group", t => t.Item_group_id, cascadeDelete: false)
                .ForeignKey("dbo.TaxGroup_table", t => t.Purchase_tax_group_id, cascadeDelete: false)
                .ForeignKey("dbo.TaxGroup_table", t => t.Sales_tax_group_id, cascadeDelete: false)
                .ForeignKey("dbo.Unit_of_measure", t => t.Unit_of_measure_id, cascadeDelete: false)
                .Index(t => t.Item_group_id)
                .Index(t => t.Unit_of_measure_id)
                .Index(t => t.Sales_tax_group_id)
                .Index(t => t.Purchase_tax_group_id);
            
            CreateTable(
                "dbo.Inv_item_group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_group_id = c.String(maxLength: 200),
                        Desc = c.String(maxLength: 500),
                        Type = c.Int(nullable: false),
                        Unit_of_measure_id = c.Int(nullable: false),
                        Sales_tax_group_id = c.Int(nullable: false),
                        Purchase_tax_group_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaxGroup_table", t => t.Purchase_tax_group_id, cascadeDelete: false)
                .ForeignKey("dbo.TaxGroup_table", t => t.Sales_tax_group_id, cascadeDelete: false)
                .ForeignKey("dbo.Unit_of_measure", t => t.Unit_of_measure_id, cascadeDelete: false)
                .Index(t => t.Unit_of_measure_id)
                .Index(t => t.Sales_tax_group_id)
                .Index(t => t.Purchase_tax_group_id);
            
            CreateTable(
                "dbo.Inv_items_serial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        Desc = c.String(maxLength: 500),
                        Lot_Desc = c.String(),
                        Lot_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Serial_number_from = c.String(),
                        Serial_number_To = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .Index(t => t.Item_id);
            
            CreateTable(
                "dbo.Inv_goods_out",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gr_no = c.String(),
                        Request_no = c.Int(nullable: false),
                        Store_id = c.Int(nullable: false),
                        Site_id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Vendor_doc = c.String(),
                        Po_no = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_store", t => t.Site_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.Store_id, cascadeDelete: false)
                .Index(t => t.Store_id)
                .Index(t => t.Site_id);
            
            CreateTable(
                "dbo.Inv_goods_reciept",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Gr_no = c.String(),
                        Pr_no_id = c.Int(nullable: false),
                        Store_id = c.Int(nullable: false),
                        Site_id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Vendor_doc = c.String(),
                        Po_no = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_store", t => t.Site_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.Store_id, cascadeDelete: false)
                .Index(t => t.Store_id)
                .Index(t => t.Site_id);
            
            CreateTable(
                "dbo.Inv_gorup_gl_accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Desc = c.String(),
                        Item_group_id = c.Int(nullable: false),
                        Inventory_id = c.Int(nullable: false),
                        Returne_id = c.Int(nullable: false),
                        Damage_id = c.Int(nullable: false),
                        Variancies_id = c.Int(nullable: false),
                        Sales_id = c.Int(nullable: false),
                        Sales_return_id = c.Int(nullable: false),
                        Cost_of_GS_id = c.Int(nullable: false),
                        Purchase_variance_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Cost_of_GS_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Damage_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Inventory_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item_group", t => t.Item_group_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_variance_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Returne_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Sales_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Sales_return_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Variancies_id, cascadeDelete: false)
                .Index(t => t.Item_group_id)
                .Index(t => t.Inventory_id)
                .Index(t => t.Returne_id)
                .Index(t => t.Damage_id)
                .Index(t => t.Variancies_id)
                .Index(t => t.Sales_id)
                .Index(t => t.Sales_return_id)
                .Index(t => t.Cost_of_GS_id)
                .Index(t => t.Purchase_variance_id);
            
            CreateTable(
                "dbo.Inv_item_gl_accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Desc = c.String(),
                        Item_group_id = c.Int(nullable: false),
                        Inventory_id = c.Int(nullable: false),
                        Returne_id = c.Int(nullable: false),
                        Damage_id = c.Int(nullable: false),
                        Variancies_id = c.Int(nullable: false),
                        Sales_id = c.Int(nullable: false),
                        Sales_return_id = c.Int(nullable: false),
                        Cost_of_GS_id = c.Int(nullable: false),
                        Purchase_variance_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Cost_of_GS_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Damage_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Inventory_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.Item_group_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Purchase_variance_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Returne_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Sales_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Sales_return_id, cascadeDelete: false)
                .ForeignKey("dbo.C_CreateAccount_Table", t => t.Variancies_id, cascadeDelete: false)
                .Index(t => t.Item_group_id)
                .Index(t => t.Inventory_id)
                .Index(t => t.Returne_id)
                .Index(t => t.Damage_id)
                .Index(t => t.Variancies_id)
                .Index(t => t.Sales_id)
                .Index(t => t.Sales_return_id)
                .Index(t => t.Cost_of_GS_id)
                .Index(t => t.Purchase_variance_id);
            
            CreateTable(
                "dbo.Inv_movment_GS",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Next_gr_no = c.Int(nullable: false),
                        Next_goods_no = c.Int(nullable: false),
                        Allow_adjustment = c.Boolean(nullable: false),
                        Next_adjustment_no = c.Int(nullable: false),
                        Allow_Transfer = c.Boolean(nullable: false),
                        Next_transfer_no = c.Int(nullable: false),
                        Adjustment_password = c.String(maxLength: 200),
                        Transfer_password = c.String(maxLength: 200),
                        Automatic_safety_stock = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_po_GS",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Next_po_no = c.Int(nullable: false),
                        Allow_receiv_without_inv = c.Boolean(nullable: false),
                        Allow_receiv_part_of_po = c.Boolean(nullable: false),
                        Allow_View_jv = c.Boolean(nullable: false),
                        Show_items_cost_in_receiving = c.Boolean(nullable: false),
                        passwords_for_unhold_check = c.Boolean(nullable: false),
                        passwords_for_unhold = c.String(maxLength: 200),
                        Allow_generate_automatic_po = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_pricelist",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        Desc = c.String(maxLength: 500),
                        Price_lvl = c.Int(nullable: false),
                        Currency_id = c.String(maxLength: 50),
                        Unit_of_measure_id = c.Int(nullable: false),
                        Start_quantity = c.Single(nullable: false),
                        End_quantity = c.Single(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Unit_of_measure", t => t.Unit_of_measure_id, cascadeDelete: false)
                .Index(t => t.Item_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Unit_of_measure_id);
            
            CreateTable(
                "dbo.Inv_purchase",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Request = c.String(),
                        Date = c.DateTime(nullable: false),
                        Within_days = c.Int(nullable: false),
                        Delivery_date = c.DateTime(nullable: false),
                        Send_to = c.Int(nullable: false),
                        Item_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .Index(t => t.Item_id);
            
            CreateTable(
                "dbo.Inv_purchase_request",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PR_no = c.String(),
                        Pr_date = c.DateTime(nullable: false),
                        Within_days = c.Int(nullable: false),
                        item_id = c.Int(nullable: false),
                        Quntaty = c.Int(nullable: false),
                        Within_days_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.item_id, cascadeDelete: false)
                .Index(t => t.item_id);
            
            CreateTable(
                "dbo.Inv_quotation_receiving",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pr_no_id = c.Int(nullable: false),
                        Currency_id = c.String(maxLength: 50),
                        Qutation_num_id = c.Int(nullable: false),
                        Vendore_id = c.Int(nullable: false),
                        Payment_term = c.Int(nullable: false)
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Inv_purchase", t => t.Pr_no_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_request_for_quotation", t => t.Qutation_num_id, cascadeDelete: false)
                .ForeignKey("dbo.Payable_creditor_setting", t => t.Vendore_id)
                .Index(t => t.Pr_no_id)
                .Index(t => t.Currency_id)
                .Index(t => t.Qutation_num_id)
                .Index(t => t.Vendore_id);
            
            CreateTable(
                "dbo.Inv_request_for_quotation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Request_for_qut_no = c.String(),
                        Date = c.DateTime(nullable: false),
                        Pr_no_id = c.Int(nullable: false),
                        Within_days = c.Int(nullable: false),
                        Item_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Vendore_id = c.Int(nullable: false),
                        Delivery_Date = c.DateTime(nullable: false),
                        Request_from = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_purchase", t => t.Pr_no_id, cascadeDelete: false)
                .ForeignKey("dbo.Payable_creditor_setting", t => t.Vendore_id, cascadeDelete: false)
                .Index(t => t.Pr_no_id)
                .Index(t => t.Item_id)
                .Index(t => t.Vendore_id);
            
            CreateTable(
                "dbo.Inv_sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Request = c.String(),
                        Date = c.DateTime(nullable: false),
                        Sales_person = c.String(),
                        Within_days = c.Int(nullable: false),
                        Item_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Delivery_Date = c.DateTime(nullable: false),
                        Request_from = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_item", t => t.Item_id, cascadeDelete: false)
                .Index(t => t.Item_id);
            
            CreateTable(
                "dbo.Inv_sales_GS",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Allow_insert_item_no_enough_store = c.Boolean(nullable: false),
                        Allow_proforma_inv = c.Boolean(nullable: false),
                        Allow_View_jv = c.Boolean(nullable: false),
                        Override_price_in_price_list = c.Boolean(nullable: false),
                        Allow_price_lower_cost = c.Boolean(nullable: false),
                        Allow_edit_sales_price = c.Boolean(nullable: false),
                        passwords_for_unhold_check = c.Boolean(nullable: false),
                        passwords_for_unhold = c.String(maxLength: 200),
                        Allow_generate_automatic_po = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_sales_invoice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Doc_type = c.Int(nullable: false),
                        Doc_no = c.String(maxLength: 500),
                        Batch_id = c.Int(nullable: false),
                        Transaction_date = c.DateTime(nullable: false),
                        Posting_date = c.DateTime(nullable: false),
                        Sales_person = c.String(),
                        Currency_id = c.String(maxLength: 50),
                        Customer_id = c.Int(nullable: false),
                        So_no = c.String(maxLength: 500),
                        Store_id = c.Int(nullable: false),
                        Site_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CurrenciesDefinition_Table", t => t.Currency_id)
                .ForeignKey("dbo.Inv_store_site", t => t.Site_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.Store_id, cascadeDelete: false)
                .Index(t => t.Currency_id)
                .Index(t => t.Store_id)
                .Index(t => t.Site_id);
            AddForeignKey("dbo.Inv_sales_invoice", "Customer_id", "dbo.Receivable_vendore_setting", "Id", cascadeDelete:false);

            CreateTable(
                "dbo.Inv_store_site",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Site_id = c.String(maxLength: 200),
                        Site_name = c.String(maxLength: 500),
                        Store_id = c.Int(nullable: false),
                        Inactive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_store", t => t.Store_id, cascadeDelete: false)
                .Index(t => t.Store_id);
            
            CreateTable(
                "dbo.Inv_transfer_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Transfer_no = c.String(maxLength: 500),
                        Transfer_date = c.DateTime(nullable: false),
                        Posting_date = c.DateTime(nullable: false),
                        From_store_id = c.Int(nullable: false),
                        To_store_id = c.Int(nullable: false),
                        Ref = c.String(maxLength: 500),
                        item_id = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Unite_cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Lot_id = c.Int(nullable: false),
                        Store_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inv_store", t => t.From_store_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_item", t => t.item_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_items_serial", t => t.Lot_id, cascadeDelete: false)
                .ForeignKey("dbo.Inv_store", t => t.Store_Id)
                .ForeignKey("dbo.Inv_store", t => t.To_store_id, cascadeDelete: false)
                .Index(t => t.From_store_id)
                .Index(t => t.To_store_id)
                .Index(t => t.item_id)
                .Index(t => t.Lot_id)
                .Index(t => t.Store_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inv_transfer_items", "To_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_transfer_items", "Store_Id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_transfer_items", "Lot_id", "dbo.Inv_items_serial");
            DropForeignKey("dbo.Inv_transfer_items", "item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_transfer_items", "From_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_sales_invoice", "Store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_sales_invoice", "Site_id", "dbo.Inv_store_site");
            DropForeignKey("dbo.Inv_store_site", "Store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_sales_invoice", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropForeignKey("dbo.Inv_sales", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_quotation_receiving", "Vendore_Id", "dbo.Payable_creditor_setting");
            DropForeignKey("dbo.Inv_quotation_receiving", "Qutation_num_id", "dbo.Inv_request_for_quotation");
            DropForeignKey("dbo.Inv_request_for_quotation", "Vendore_id", "dbo.Payable_creditor_setting");
            DropForeignKey("dbo.Inv_request_for_quotation", "Pr_no_id", "dbo.Inv_purchase");
            DropForeignKey("dbo.Inv_request_for_quotation", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_quotation_receiving", "Pr_no_id", "dbo.Inv_purchase");
            DropForeignKey("dbo.Inv_quotation_receiving", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropForeignKey("dbo.Inv_purchase_request", "item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_purchase", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_pricelist", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_pricelist", "Currency_id", "dbo.CurrenciesDefinition_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Variancies_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Sales_return_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Sales_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Returne_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Purchase_variance_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Item_group_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Inventory_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Damage_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_item_gl_accounts", "Cost_of_GS_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Variancies_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Sales_return_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Sales_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Returne_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Purchase_variance_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Item_group_id", "dbo.Inv_item_group");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Inventory_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Damage_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_gorup_gl_accounts", "Cost_of_GS_id", "dbo.C_CreateAccount_Table");
            DropForeignKey("dbo.Inv_goods_reciept", "Store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_goods_reciept", "Site_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_goods_out", "Store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_goods_out", "Site_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_adjustment_items", "To_store_id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_adjustment_items", "Store_Id", "dbo.Inv_store");
            DropForeignKey("dbo.Inv_adjustment_items", "Lot_id", "dbo.Inv_items_serial");
            DropForeignKey("dbo.Inv_items_serial", "Item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_adjustment_items", "item_id", "dbo.Inv_item");
            DropForeignKey("dbo.Inv_item", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_item", "Sales_tax_group_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item", "Purchase_tax_group_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item", "Item_group_id", "dbo.Inv_item_group");
            DropForeignKey("dbo.Inv_item_group", "Unit_of_measure_id", "dbo.Unit_of_measure");
            DropForeignKey("dbo.Inv_item_group", "Sales_tax_group_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_item_group", "Purchase_tax_group_id", "dbo.TaxGroup_table");
            DropForeignKey("dbo.Inv_adjustment_items", "From_store_id", "dbo.Inv_store");
            DropIndex("dbo.Inv_transfer_items", new[] { "Store_Id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "Lot_id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "item_id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "To_store_id" });
            DropIndex("dbo.Inv_transfer_items", new[] { "From_store_id" });
            DropIndex("dbo.Inv_store_site", new[] { "Store_id" });
            DropIndex("dbo.Inv_sales_invoice", new[] { "Site_id" });
            DropIndex("dbo.Inv_sales_invoice", new[] { "Store_id" });
            DropIndex("dbo.Inv_sales_invoice", new[] { "Currency_id" });
            DropIndex("dbo.Inv_sales", new[] { "Item_id" });
            DropIndex("dbo.Inv_request_for_quotation", new[] { "Vendore_id" });
            DropIndex("dbo.Inv_request_for_quotation", new[] { "Item_id" });
            DropIndex("dbo.Inv_request_for_quotation", new[] { "Pr_no_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Vendore_Id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Qutation_num_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Currency_id" });
            DropIndex("dbo.Inv_quotation_receiving", new[] { "Pr_no_id" });
            DropIndex("dbo.Inv_purchase_request", new[] { "item_id" });
            DropIndex("dbo.Inv_purchase", new[] { "Item_id" });
            DropIndex("dbo.Inv_pricelist", new[] { "Unit_of_measure_id" });
            DropIndex("dbo.Inv_pricelist", new[] { "Currency_id" });
            DropIndex("dbo.Inv_pricelist", new[] { "Item_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Purchase_variance_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Cost_of_GS_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Sales_return_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Sales_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Variancies_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Damage_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Returne_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Inventory_id" });
            DropIndex("dbo.Inv_item_gl_accounts", new[] { "Item_group_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Purchase_variance_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Cost_of_GS_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Sales_return_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Sales_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Variancies_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Damage_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Returne_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Inventory_id" });
            DropIndex("dbo.Inv_gorup_gl_accounts", new[] { "Item_group_id" });
            DropIndex("dbo.Inv_goods_reciept", new[] { "Site_id" });
            DropIndex("dbo.Inv_goods_reciept", new[] { "Store_id" });
            DropIndex("dbo.Inv_goods_out", new[] { "Site_id" });
            DropIndex("dbo.Inv_goods_out", new[] { "Store_id" });
            DropIndex("dbo.Inv_items_serial", new[] { "Item_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Purchase_tax_group_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Sales_tax_group_id" });
            DropIndex("dbo.Inv_item_group", new[] { "Unit_of_measure_id" });
            DropIndex("dbo.Inv_item", new[] { "Purchase_tax_group_id" });
            DropIndex("dbo.Inv_item", new[] { "Sales_tax_group_id" });
            DropIndex("dbo.Inv_item", new[] { "Unit_of_measure_id" });
            DropIndex("dbo.Inv_item", new[] { "Item_group_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "Store_Id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "Lot_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "item_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "To_store_id" });
            DropIndex("dbo.Inv_adjustment_items", new[] { "From_store_id" });
            DropTable("dbo.Inv_transfer_items");
            DropTable("dbo.Inv_store_site");
            DropTable("dbo.Inv_sales_invoice");
            DropTable("dbo.Inv_sales_GS");
            DropTable("dbo.Inv_sales");
            DropTable("dbo.Inv_request_for_quotation");
            DropTable("dbo.Inv_quotation_receiving");
            DropTable("dbo.Inv_purchase_request");
            DropTable("dbo.Inv_purchase");
            DropTable("dbo.Inv_pricelist");
            DropTable("dbo.Inv_po_GS");
            DropTable("dbo.Inv_movment_GS");
            DropTable("dbo.Inv_item_gl_accounts");
            DropTable("dbo.Inv_gorup_gl_accounts");
            DropTable("dbo.Inv_goods_reciept");
            DropTable("dbo.Inv_goods_out");
            DropTable("dbo.Inv_items_serial");
            DropTable("dbo.Inv_item_group");
            DropTable("dbo.Inv_item");
            DropTable("dbo.Inv_store");
            DropTable("dbo.Inv_adjustment_items");
        }
    }
}
