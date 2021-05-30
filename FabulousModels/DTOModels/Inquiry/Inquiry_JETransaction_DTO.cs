using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Inquiry
{
    public class Inquiry_JETransaction_DTO
    {
        public int AID { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public string Date { get; set; }

        public int JournalEntryNumber { get; set; }

        public int PostingNumber { get; set; }

        public double OriginalAmount { get; set; }

        public string Currency { get; set; }

        public double TransactionRate { get; set; }

        public string Describtion { get; set; }

        public double? Debit { get; set; }

        public double? Credit { get; set; }

        public string CurrencyID { get; set; }

        public string Periods { get; set; }

        public double? NetChange { get; set; }

        public double? EndingBalance { get; set; }


        public string BatchCreationDate { get; set; }

        public string BatchID { get; set; }

        public string BatchDescribtion { get; set; }

        public string BatchFrom { get; set; }

        public string UserCreatedBatch { get; set; }
        public string UserCreatedBatchName { get; set; }

        public string UserApprovedBatch { get; set; }
        public string UserApprovedBatchName { get; set; }

        public string ApprovedDate { get; set; }

        public int NoOfTrx { get; set; }

        public double? BatchTotal { get; set; }

        public string PostingKey { get; set; }

        public int? VoidJENum { get; set; }
    }
}
