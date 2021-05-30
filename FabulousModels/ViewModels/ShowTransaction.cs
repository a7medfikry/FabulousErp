using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels
{
    public class PrintTransaction
    {
        public ShowHeader ShowHeader2 { get; set; }
        public List<ShowTransaction> ShowGeneralLedger { get; set; }
        public List<AnaylticRpt> ShowAnalytics { get; set; }

    }
    public class RetrieveTransactionData
    {
        public ShowHeader ShowHeader { get; set; }

        public List<ShowHeader> ShowHeader2 { get; set; }

        public List<ShowTransaction> ShowTransactions { get; set; }

        public List<ShowTransaction> ShowGeneralLedger { get; set; }

        public List<ShowAnalytic> ShowAnalytics { get; set; }

        public List<ShowCostCenter> ShowCostCenters { get; set; }

        public CheckbookData CheckbookData { get; set; }

        public List<TransferData> TransferData { get; set; }
    }

    public class ShowHeader
    {
        public string PostingDate { get; set; }
        public string TransactionDate { get; set; }
        public string Reference { get; set; }
        public string CurrencyID { get; set; }
        public double SystemRate { get; set; }
        public double TransactionRate { get; set; }
        public string PostingKey { get; set; }
        public string ISO { get; set; }
        public int? VoidJENum { get; set; }
        public string VoidDate { get; set; }
        public string VoidPostingKey { get; set; }
        public int JENumber { get; set; }
        public string TransactionType { get; set; }
    }

    public class ShowHeader2
    {
        public string PostingDate { get; set; }
        public string TransactionDate { get; set; }
        public string Reference { get; set; }
        public string CurrencyID { get; set; }
        public double SystemRate { get; set; }
        public double TransactionRate { get; set; }
        public string PostingKey { get; set; }
        public string ISO { get; set; }
        public int? VoidJENum { get; set; }
        public string VoidDate { get; set; }
        public string VoidPostingKey { get; set; }
        public int JENumber { get; set; }
        public string TransactionType { get; set; }
    }

    public class ShowTransaction
    {
        public int PostingNumber { get; set; }
        public string Describtion { get; set; }
        public string Document { get; set; }
        public int? AID { get; set; }
        public string AccountID { get; set; }
        public double OriginalAmount { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public string AccountName { get; set; }
    }

    public class ShowAnalytic
    {
        public string AnalyticID { get; set; }

        public int? DistID { get; set; }

        public string DistributionID { get; set; }

        public string DistributionName { get; set; }

        public string Describtion { get; set; }

        public double Amount { get; set; }

        public double Percentage { get; set; }

        public double? Debit { get; set; }

        public double? Credit { get; set; }

        public int? AID { get; set; }
    }

    public class ShowCostCenter
    {
        public string CostCenterID { get; set; }

        public string CostCenterName { get; set; }

        public int? CAID { get; set; }

        public string CostAccountID { get; set; }

        public string CostAccountName { get; set; }

        public int? AID { get; set; }

        public string Describtion { get; set; }

        public double Percentage { get; set; }

        public double Amount { get; set; }

        public double? Debit { get; set; }

        public double? Credit { get; set; }

        public string MainCostCenterID { get; set; }

        public double? CostCenterIDPercentage { get; set; }
    }

    public class TransactionsBatches
    {
        public int CBID { get; set; }

        public string BatchID { get; set; }

        public string BatchLocation { get; set; }
    }

    public class JEntryNumbers
    {
        public int PostingNumber { get; set; }

        public int JournalEntryNum { get; set; }

        public string PostingKey { get; set; }
    }

    public class CheckbookData
    {
        public int C_CBT { get; set; }
        public int? C_CBSID { get; set; }
        public string CheckbookName { get; set; }
        public string Payment_To_Recieved_From { get; set; }
        public int NextDepositNumber { get; set; }
        public int NextWithdrawNumber { get; set; }
        public string C_DocumentType { get; set; }
        public decimal C_Balance { get; set; }
        public string C_DueDate { get; set; }
        public string C_CheckNumber { get; set; }
        public string C_TransactionDate { get; set; }
        public string C_PostingDate { get; set; }
    }

    public class TransferData
    {
        public int C_CBT { get; set; }
        public int? C_CBSID { get; set; }
        public string CheckbookName { get; set; }
        public string CurrencyID { get; set; }
        public double? C_SystemRate { get; set; }
        public double? C_TransactionRate { get; set; }
        public double? C_Difference { get; set; }
        public string C_DocumentType { get; set; }
        public string C_Reference { get; set; }
        public decimal C_Balance { get; set; }
        public decimal? RecieptCheck { get; set; }
        public decimal? PaymentCheck { get; set; }
        public string ISO { get; set; }
    }

    public class AnaylticRpt
    {
        public bool V { get; set; }
        public string Date { get; set; }
        public int JE_num { get; set; }
        public int Posting_Num { get; set; }
        public string Posting_key { get; set; }
        public string Currency { get; set; }
        public double Original_amount { get; set; }
        public double? Balance { get; set; }
        public double Transaction_rate { get; set; }
        public string Description { get; set; }
        public double? Debit { get; set; }
        public double? Credit { get; set; }
        public string Action { get; set; }
        public bool IsBeging { get; set; } = false;

        public string Account_id { get; set; }
        public string Anayaltic_Distribution { get; set; }
        public double Prcentage { get; set; }
        public int? DistId { get; set; }
        public string Dist_name { get; set; }
        public List<AnaylticRpt> Beging { get; set; }
    }
}
