using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition
{
    public class Currencies_Definition_DTO
    {

        public string CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string CurrencyID { get; set; }

        public string CurrencyName { get; set; }

        public string ISOCode { get; set; }

        public string DisActive { get; set; }

        public string AccountID { get; set; }

        public int C_AID { get; set; }

        public int B_AID { get; set; }

        public int F_AID { get; set; }

        public string AccountName { get; set; }

        public string Type { get; set; }
    }
}
