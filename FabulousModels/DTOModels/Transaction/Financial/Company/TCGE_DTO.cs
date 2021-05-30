using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Transaction.Financial.Company
{
    public class TCGE_DTO
    {
        public double Rate { get; set; }

        public int ExchangeID { get; set; }

        public string AccountName { get; set; }

        public string CurrencyID { get; set; }

        public string AccountType { get; set; }

        public bool? SupportDocument { get; set; }

        public string C_AnalyticAccountID { get; set; }

        public string CostCenterType { get; set; }

        public string C_CostCenterID { get; set; }

        public string C_CostCenterName { get; set; }

        public string C_CostCenterGroupID { get; set; }

        public double? MaiximumAmount { get; set; }

        public double? MinimumAmount { get; set; }


        public int C_CBID { get; set; }

        public string C_BatchCreationDate { get; set; }


        public int? C_DistID { get; set; }
        public string C_AccountDistributionID { get; set; }

        public int? C_DistID2 { get; set; }
        public string C_AccountDistributionID2 { get; set; }
        public string C_AccountDistributionName { get; set; }


        public int C_CAID { get; set; }
        public string C_CostAccountID { get; set; }

        public int? C_CAID2 { get; set; }
        public string C_CostAccountID2 { get; set; }
        public string C_CostAccountName { get; set; }


        public string Percentage { get; set; }

        public string CostCenterName { get; set; }
        public string CostCenterIDPercentage { get; set; }


        public int C_AID { get; set; }

        public string AccountID { get; set; }


        public int JournalEntryNumber { get; set; }

        public int PostingNumber { get; set; }
    }
}
