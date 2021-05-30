using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.Inventory
{
    public class MonthlySalesRpt
    {
        public decimal Sales { get; set; }
        public decimal Discount { get; set; }
        public string Col { get; set; }
        public Doc_type Doc_type { get; set; }
        public int Similer { get; set; }
    }
}
