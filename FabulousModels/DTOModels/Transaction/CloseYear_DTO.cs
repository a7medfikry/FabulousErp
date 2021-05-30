using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousModels.DTOModels.Transaction
{
    public class CloseYear_DTO
    {

        public List<PLAccounts> PLAccounts { get; set; }

        public List<PLAccounts> BallanceSheetAccounts { get; set; }

        public HeaderData HeaderData { get; set; }
        
    }
    public class PLAccounts
    {
        public int AID { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public double? Ballance { get; set; }
    }

    public class HeaderData
    {
        public string LastDayInYear { get; set; }

        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public int? NextYearID { get; set; }

        public string NextYearName { get; set; }

        public string FirstDayInNextYear { get; set; }
    }

}
