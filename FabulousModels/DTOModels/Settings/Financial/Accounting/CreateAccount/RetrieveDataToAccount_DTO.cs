using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Settings.Financial.Accounting.CreateAccount
{
    public class RetrieveDataToAccount_DTO
    {
        public string AAccountID { get; set; }
        public string CostCenterID { get; set; }
        public string MainCostCenterID { get; set; }
        public int AccountGroupID { get; set; }
        public string AccountGroupName { get; set; }
        public string C_AccountID { get; set; }
        public int C_AID { get; set; }
        public string C_AccountName { get; set; }
        public string B_AccountID { get; set; }
        public int B_AID { get; set; }
        public string F_AccountID { get; set; }
        public int F_AID { get; set; }
        public string TFactory { get; set; }

        //------------------------------------------------------
        public string AccountName { get; set; }
        public string AccountsGroup { get; set; }
        public bool? DisActive { get; set; }
        public string Currency { get; set; }
        public string AccountType { get; set; }
        public string PostingType { get; set; }
        public bool? ReconcileAccount { get; set; }
        public bool? AllowAdjusment { get; set; }
        public bool? Reevaluate { get; set; }
        public bool? ConsolidationAccount { get; set; }
        public bool? SupportDocument { get; set; }
        public bool? FinancialArea { get; set; }
        public bool? SalesArea { get; set; }
        public bool? PurshacingArea { get; set; }
        public bool? InventoryArea { get; set; }
        public string AreaID { get; set; }
        public double? MaximumAmountPerTransaction { get; set; }
        public double? MinimumAmountPerTransaction { get; set; }
        public string CostCenterType { get; set; }
        public string R_AnalyticAccountID { get; set; }
        public string R_CostCenterID { get; set; }
        public string R_CostCenterGroupID { get; set; }

        //-----------------------------------------------------------

        public string DistAccountID { get; set; }
        public int C_DistID { get; set; }
        public int B_DistID { get; set; }
        public int F_DistID { get; set; }
        public string CostAccountID { get; set; }
        public int C_CAID { get; set; }
        public int ID { get; set; }
        public string C_DistAccountID { get; set; }
        public string C_DistAccountName { get; set; }
        public string C_CostAccountID { get; set; }
        public string C_CostAccountName { get; set; }
        public string Percentage { get; set; }
        public string CostCenterName { get; set; }
        public string CostCenterPercentage { get; set; }
        public string AccountChartID { get; set; }
        public string BranchID { get; set; }
        public string CurrencyID { get; set; }
        public string ISOCode { get; set; }
        public int GroupID { get; set; }

        // ---------------------------------------------------------------
        public string C_Prefix { get; set; }

    }
}
