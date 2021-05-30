using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class InvCSRpt
    {
        public DateTime Date { get; set; }
        public string Customer_name { get;set;}
        public decimal Amount { get;set;}
    }
}
