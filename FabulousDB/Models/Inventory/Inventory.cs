using FabulousDB.Models;
using FabulousDB.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Context
{
    public partial class DBContext : DbContext
    {
        //public DbSet<Inv_transfer_items> Inv_transfer_items { get; set; }
        public DbSet<Inv_store_site> Inv_store_site { get; set; }
        public DbSet<Inv_store> Inv_store { get; set; }
        public DbSet<Inv_sales_invoice> Inv_sales_invoice { get; set; }
        public DbSet<Inv_sales_GS> Inv_sales_GS { get; set; }
        public DbSet<Inv_sales> Inv_sales { get; set; }
        public DbSet<Inv_quotation_request> Inv_quotation_request { get; set; }
        public DbSet<Inv_quotation_receiving> Inv_quotation_receiving { get; set; }
        public DbSet<Inv_purchase_request> Inv_purchase_request { get; set; }
        public DbSet<Inv_purchase_request_items> Inv_purchase_request_items { get; set; }
        public DbSet<Inv_quotation_request_item> Inv_quotation_request_item { get; set; }
        public DbSet<Inv_purchase> Inv_purchase { get; set; }
        public DbSet<Inv_pricelist> Inv_pricelist { get; set; }
        public DbSet<Inv_po_GS> Inv_po_GS { get; set; }
        public DbSet<Inv_movment_GS> Inv_movment_GS { get; set; }
        public DbSet<Inv_items_serial> Inv_items_serial { get; set; }
        public DbSet<Inv_item_group> Inv_item_group { get; set; }
        public DbSet<Inv_item_deduct_tax> Inv_item_deduct_tax { get; set; }
        public DbSet<Inv_item_group_deduct_tax> Inv_item_group_deduct_tax { get; set; }
        public DbSet<Inv_item_gl_accounts> Inv_item_gl_accounts { get; set; }
        public DbSet<Inv_gorup_gl_accounts> Inv_gorup_gl_accounts { get; set; }
        public DbSet<Inv_item> Inv_item { get; set; }
        public DbSet<Inv_item_option> Inv_item_option { get; set; }
        public DbSet<Inv_item_store_site> Inv_item_store_sites { get; set; }
        //public DbSet<Inv_goods_reciept> Inv_goods_reciept { get; set; }
        //public DbSet<Inv_goods_out> Inv_goods_out { get; set; }
        public DbSet<Inv_po> Inv_po { get; set; }
        public DbSet<Inv_po_items> Inv_po_items { get; set; }
        public DbSet<Inv_receive_po> Inv_receive_po { get; set; }
        public DbSet<Inv_receive_po_items> Inv_receive_po_items { get; set; }
        public DbSet<Inv_sales_invoice_items> Inv_sales_invoice_items { get; set; }
        public DbSet<Inv_sales_receivs_pos> Inv_sales_receivs_pos { get; set; }
        public DbSet<Inv_receive_item_serial> Inv_receive_item_serial { get; set; }
        public DbSet<Inv_receive_expiry> Inv_receive_expiry { get; set; }
        public DbSet<Inv_sales_item_serial> Inv_sales_item_serial { get; set; }
        public DbSet<Inv_item_recipe> Inv_item_recipe { get; set; }
        public DbSet<Inv_old_receive_item_serial> Inv_old_receive_item_serial { get; set; }
        public DbSet<Inv_stocking> Inv_stocking { get; set; }
        public DbSet<Inv_po_item_transfer> Inv_po_item_transfer { get; set; }
        public DbSet<Inv_transfer_relation> Inv_transfer_relation { get; set; }
        public DbSet<Inv_item_adjustment> Inv_item_adjustment { get; set; }
        public DbSet<Inv_adjustment_relation> Inv_adjustment_relation { get; set; }
        public DbSet<Inv_receivable_num> Inv_receivable_num { get; set; }

    }
}