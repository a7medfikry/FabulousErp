using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.Accounting.Analytic
{
    public class Analytic_To_Account_DTO
    {
        public int AID { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public string AnalyticID { get; set; }

        public string AnalyticAccountID { get; set; }

        public string AnalyticAccountName { get; set; }
    }
}
