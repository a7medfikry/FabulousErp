using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.APIModels
{
    public class TransactionApiData
    {
        public C_GeneralJournalEntry_Table SaveHeader { get; set; }

        public C_SaveTransaction_Table[] SaveTransaction { get; set; }

        public C_SaveAnalytic_Table[] SaveAnalytic { get; set; }

        public C_SaveCostCenter_Table[] SaveCost { get; set; }
    }
}
