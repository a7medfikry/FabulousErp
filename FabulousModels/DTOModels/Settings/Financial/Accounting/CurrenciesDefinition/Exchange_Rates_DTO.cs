using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.Accounting.CurrenciesDefinition
{
    public class Exchange_Rates_DTO
    {
        public int ExchangeID { get; set; }

        public string EstablishDate { get; set; }

        public double Rate { get; set; }

        public string StartDate { get; set; }

        public string ExpireDate { get; set; }
    }
}
