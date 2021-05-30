using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Inquiry
{
    public class Inquiry_DTO
    {
        public string Analytic_AccountID { get; set; }
        public string AnalyticDistribution_ID { get; set; }
        public string AnalyticDistribution_Name { get; set; }
        public string Cost_CenterID { get; set; }
        public string Cost_CenterName { get; set; }
        public string Cost_AccountID{ get; set; }
        public string Cost_AccountName { get; set; }

        // inquiry Chart Of Accounts (Create_Account_Table)
        public string AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public string AccountGroup { get; set; }
        public string PostingType { get; set; }

        // inquiry Checkbook-Setting
        public string CheckbookID { get; set; }
        public string CheckbookName { get; set; }
        public string CheckbookType { get; set; }
        public bool? CheckbookStatus { get; set; }
        public string CheckbookCurrency { get; set; }
        public string CheckbookAccountID { get; set; }
        public string Branch_Factory_Name { get; set; }

    }
}
