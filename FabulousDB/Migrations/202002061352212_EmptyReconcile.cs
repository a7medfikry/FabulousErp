namespace FabulousDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmptyReconcile : DbMigration
    {
        public override void Up()
        {
           
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_receive_po_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        UOM_id = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Unit_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount_system_currency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Net_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Table_vat_id = c.Int(nullable: false),
                        Table_vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Vat_id = c.Int(nullable: false),
                        Vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deduct_id = c.Int(nullable: false),
                        Deduct_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Receive_po_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_receive_po",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GR_no = c.Int(nullable: false),
                        Doc_type = c.Int(),
                        PO_id = c.Int(nullable: false),
                        Store_id = c.Int(nullable: false),
                        Site_id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Vendore_id = c.Int(nullable: false),
                        Batch_id = c.Int(),
                        Buyer = c.String(),
                        Vendore_inv_number = c.String(maxLength: 200),
                        Currency_id = c.String(maxLength: 50),
                        Inv_receive_po_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_quotation_request",
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_quotation_receiving",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pr_no_id = c.Int(nullable: false),
                        Currency_id = c.String(maxLength: 50),
                        Qutation_num_id = c.Int(nullable: false),
                        Vendore_id = c.Int(nullable: false),
                        Payment_term = c.Int(nullable: false),
                        Vendore_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_po_items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Item_id = c.Int(nullable: false),
                        UOM_id = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Unit_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount_system_currency = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Net_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Table_vat_id = c.Int(nullable: false),
                        Table_vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Vat_id = c.Int(nullable: false),
                        Vat_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deduct_id = c.Int(nullable: false),
                        Deduct_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Po_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inv_po",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Po_num = c.Int(nullable: false),
                        Po_type = c.Int(),
                        Pr_no_id = c.Int(),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Vendore_id = c.Int(nullable: false),
                        Currency_id = c.String(maxLength: 50),
                        System_rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Transaction_rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Difference = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
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
                        Valudation_method = c.Int(nullable: false),
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
                        Item_group_id = c.Int(),
                        Type = c.Int(nullable: false),
                        Unit_of_measure_id = c.Int(nullable: false),
                        Sales_tax_group_id = c.Int(nullable: false),
                        Purchase_tax_group_id = c.Int(nullable: false),
                        Valudation_method = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Inv_transfer_items", "Store_Id");
            CreateIndex("dbo.Inv_transfer_items", "Lot_id");
            CreateIndex("dbo.Inv_transfer_items", "item_id");
            CreateIndex("dbo.Inv_transfer_items", "To_store_id");
            CreateIndex("dbo.Inv_transfer_items", "From_store_id");
            CreateIndex("dbo.Inv_sales_invoice", "Site_id");
            CreateIndex("dbo.Inv_sales_invoice", "Store_id");
            CreateIndex("dbo.Inv_sales_invoice", "Currency_id");
            CreateIndex("dbo.Inv_sales", "Item_id");
            CreateIndex("dbo.Inv_receive_po_items", "Receive_po_id");
            CreateIndex("dbo.Inv_receive_po_items", "Deduct_id");
            CreateIndex("dbo.Inv_receive_po_items", "Vat_id");
            CreateIndex("dbo.Inv_receive_po_items", "Table_vat_id");
            CreateIndex("dbo.Inv_receive_po_items", "UOM_id");
            CreateIndex("dbo.Inv_receive_po_items", "Item_id");
            CreateIndex("dbo.Inv_store_site", "Store_id");
            CreateIndex("dbo.Inv_receive_po", "Inv_receive_po_Id");
            CreateIndex("dbo.Inv_receive_po", "Currency_id");
            CreateIndex("dbo.Inv_receive_po", "Vendore_id");
            CreateIndex("dbo.Inv_receive_po", "Site_id");
            CreateIndex("dbo.Inv_receive_po", "Store_id");
            CreateIndex("dbo.Inv_receive_po", "PO_id");
            CreateIndex("dbo.Inv_quotation_request", "Vendore_id");
            CreateIndex("dbo.Inv_quotation_request", "Item_id");
            CreateIndex("dbo.Inv_quotation_request", "Pr_no_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Vendore_Id");
            CreateIndex("dbo.Inv_quotation_receiving", "Qutation_num_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Currency_id");
            CreateIndex("dbo.Inv_quotation_receiving", "Pr_no_id");
            CreateIndex("dbo.Inv_purchase", "Item_id");
            CreateIndex("dbo.Inv_pricelist", "Unit_of_measure_id");
            CreateIndex("dbo.Inv_pricelist", "Currency_id");
            CreateIndex("dbo.Inv_pricelist", "Item_id");
            CreateIndex("dbo.Inv_purchase_request", "item_id");
            CreateIndex("dbo.Inv_po_items", "Po_id");
            CreateIndex("dbo.Inv_po_items", "Deduct_id");
            CreateIndex("dbo.Inv_po_items", "Vat_id");
            CreateIndex("dbo.Inv_po_items", "Table_vat_id");
            CreateIndex("dbo.Inv_po_items", "UOM_id");
            CreateIndex("dbo.Inv_po_items", "Item_id");
            CreateIndex("dbo.Inv_po", "Currency_id");
            CreateIndex("dbo.Inv_po", "Vendore_id");
            CreateIndex("dbo.Inv_po", "Pr_no_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Purchase_variance_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Cost_of_GS_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Sales_return_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Sales_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Variancies_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Damage_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Returne_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Inventory_id");
            CreateIndex("dbo.Inv_item_gl_accounts", "Item_group_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Purchase_variance_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Cost_of_GS_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Sales_return_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Sales_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Variancies_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Damage_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Returne_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Inventory_id");
            CreateIndex("dbo.Inv_gorup_gl_accounts", "Item_group_id");
            CreateIndex("dbo.Inv_goods_reciept", "Site_id");
            CreateIndex("dbo.Inv_goods_reciept", "Store_id");
            CreateIndex("dbo.Inv_goods_out", "Site_id");
            CreateIndex("dbo.Inv_goods_out", "Store_id");
            CreateIndex("dbo.Inv_items_serial", "Item_id");
            CreateIndex("dbo.Inv_item_group", "Purchase_tax_group_id");
            CreateIndex("dbo.Inv_item_group", "Sales_tax_group_id");
            CreateIndex("dbo.Inv_item_group", "Unit_of_measure_id");
            CreateIndex("dbo.Inv_item", "Purchase_tax_group_id");
            CreateIndex("dbo.Inv_item", "Sales_tax_group_id");
            CreateIndex("dbo.Inv_item", "Unit_of_measure_id");
            CreateIndex("dbo.Inv_item", "Item_group_id");
            CreateIndex("dbo.Inv_adjustment_items", "Store_Id");
            CreateIndex("dbo.Inv_adjustment_items", "Lot_id");
            CreateIndex("dbo.Inv_adjustment_items", "item_id");
            CreateIndex("dbo.Inv_adjustment_items", "To_store_id");
            CreateIndex("dbo.Inv_adjustment_items", "From_store_id");
            AddForeignKey("dbo.Inv_transfer_items", "To_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_transfer_items", "Store_Id", "dbo.Inv_store", "Id");
            AddForeignKey("dbo.Inv_transfer_items", "Lot_id", "dbo.Inv_items_serial", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_transfer_items", "item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_transfer_items", "From_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_sales_invoice", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_sales_invoice", "Site_id", "dbo.Inv_store_site", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_sales_invoice", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
            AddForeignKey("dbo.Inv_sales", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Vat_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "UOM_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Table_vat_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Receive_po_id", "dbo.Inv_receive_po", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po_items", "Deduct_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po", "Vendore_id", "dbo.Payable_creditor_setting", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po", "Site_id", "dbo.Inv_store_site", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_store_site", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po", "PO_id", "dbo.Inv_po", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_receive_po", "Inv_receive_po_Id", "dbo.Inv_receive_po", "Id");
            AddForeignKey("dbo.Inv_receive_po", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
            AddForeignKey("dbo.Inv_quotation_receiving", "Vendore_Id", "dbo.Payable_creditor_setting", "Id");
            AddForeignKey("dbo.Inv_quotation_receiving", "Qutation_num_id", "dbo.Inv_quotation_request", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_quotation_request", "Vendore_id", "dbo.Payable_creditor_setting", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_quotation_request", "Pr_no_id", "dbo.Inv_purchase_request", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_quotation_request", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_quotation_receiving", "Pr_no_id", "dbo.Inv_purchase_request", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_quotation_receiving", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
            AddForeignKey("dbo.Inv_purchase", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_pricelist", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_pricelist", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_pricelist", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
            AddForeignKey("dbo.Inv_po", "Vendore_id", "dbo.Payable_creditor_setting", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po", "Pr_no_id", "dbo.Inv_purchase_request", "Id");
            AddForeignKey("dbo.Inv_purchase_request", "item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po_items", "Vat_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po_items", "UOM_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po_items", "Table_vat_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po_items", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po_items", "Po_id", "dbo.Inv_po", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po_items", "Deduct_id", "dbo.C_TaxSetting_table", "CT_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_po", "Currency_id", "dbo.CurrenciesDefinition_Table", "CurrencyID");
            AddForeignKey("dbo.Inv_item_gl_accounts", "Variancies_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Sales_return_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Sales_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Returne_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Purchase_variance_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Item_group_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Inventory_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Damage_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_gl_accounts", "Cost_of_GS_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Variancies_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Sales_return_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Sales_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Returne_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Purchase_variance_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Item_group_id", "dbo.Inv_item_group", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Inventory_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Damage_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_gorup_gl_accounts", "Cost_of_GS_id", "dbo.C_CreateAccount_Table", "C_AID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_goods_reciept", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_goods_reciept", "Site_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_goods_out", "Store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_goods_out", "Site_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_adjustment_items", "To_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_adjustment_items", "Store_Id", "dbo.Inv_store", "Id");
            AddForeignKey("dbo.Inv_adjustment_items", "Lot_id", "dbo.Inv_items_serial", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_items_serial", "Item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_adjustment_items", "item_id", "dbo.Inv_item", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item", "Sales_tax_group_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item", "Purchase_tax_group_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item", "Item_group_id", "dbo.Inv_item_group", "Id");
            AddForeignKey("dbo.Inv_item_group", "Unit_of_measure_id", "dbo.Unit_of_measure", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_group", "Sales_tax_group_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_item_group", "Purchase_tax_group_id", "dbo.TaxGroup_table", "TG_ID", cascadeDelete: true);
            AddForeignKey("dbo.Inv_adjustment_items", "From_store_id", "dbo.Inv_store", "Id", cascadeDelete: true);
        }
    }
}
