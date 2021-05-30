using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class InvCustSalesRpt
    {
        public Doc_type Doc_type { get; set; }
        public string Doc_no { get; set; }
        public string Customer_name { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public DateTime Date { get; set; }
    }
    public class SalesAndDebit
    {
      public List<InvCustSalesRpt> Sales { get; set; }
      public List<InvCSRpt> Debit { get; set; }
    }
}
