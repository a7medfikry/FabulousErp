using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.ViewModels
{
    public class BatchApproval
    {
        public int CBID { get; set; }

        public string BatchID { get; set; }

        public string Module { get; set; }

        public string Description { get; set; }

        public string BatchLocation { get; set; }

        public string CreationDate { get; set; }

        public bool Approval { get; set; }

        public bool NotApproval { get; set; }

        public int NumOfTransactions { get; set; }
    }
}
