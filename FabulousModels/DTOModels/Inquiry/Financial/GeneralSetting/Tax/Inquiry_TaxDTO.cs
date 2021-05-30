using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Inquiry.Financial.GeneralSetting.Tax
{
    public class Inquiry_TaxDTO
    {
        public string TaxCode { get; set; }
        public string TaxType { get; set; }
        public string TaxDescribtion { get; set; }
        public double? TaxPercentage { get; set; }
        public string AccountID { get; set; }
    }
}
