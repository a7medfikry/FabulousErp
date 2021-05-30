using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.GeneralSettings.Tax
{
    public class TaxCodeByGroup_DTO
    {
        public int CT_ID { get; set; }

        public string C_Taxcode { get; set; }

        public double? Amount { get; set; }

        public double? Percentage { get; set; }
        public double? C_Taxpercentage { get; set; }

        public int? C_AID { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }
        public double? MinAmount { get; set; }
        public double? MaxAmount { get; set; }
    }
}
