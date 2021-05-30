using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels
{
    public class IDR
    {
        public string Customer_name { get; set; }
        public int Late_installment { get; set; }
        public decimal Late_installment_amount { get; set; }
        public int Collected_installment { get; set; }
        public decimal Collected_installment_amount { get; set; }
        public int UnDue_installment { get; set; }
        public decimal UnDue_installment_amount { get; set; }
        public string Customer_phone { get; set; }
    }
}
