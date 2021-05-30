using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.GeneralSettings.Tax
{
    public class Tax_DTO
    {
        public string Company_AccountsID { get; set; }
        public string Branch_AccountsID { get; set; }
        public string Factory_AccountsID { get; set; }
        public int C_AID { get; set; }
        public int B_AID { get; set; }
        public int F_AID { get; set; }
        public string CompanyTaxs { get; set; }
        public int CT_ID { get; set; }
        public string TaxType { get; set; }
        public int? AccountID { get; set; }
        public string AccountName { get; set; }
        public string TaxDescribtion { get; set; }
        public double? TaxPercentage { get; set; }
        public double? TaxAmount { get; set; }
        public double? MinTaxable { get; set; }
        public double? MaxTaxable { get; set; }
        public bool? PrintDocument { get; set; }
        public string SelectPrintDocument { get; set; }
        public string Transaction_type { get; set; }
        public int TaxGroupID { get; set; }

    }
}
