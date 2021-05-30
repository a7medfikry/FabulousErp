using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class InvoiceItems
    {
        public int Id { get; set; }
        public string Trx { get; set; }
        public int PoId { get; set; }
        public List<Inv_receive_po_items> Purchase_items { get; set; }
        public List<Inv_sales_invoice_items> Sales_items { get; set; }
    }
}
