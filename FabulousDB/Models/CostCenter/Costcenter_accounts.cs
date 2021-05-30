using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_CostCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public  class Cost_center_accounts
    {
        public int Id { get; set; }
        [ForeignKey("Account")]
        public int Account_id { get; set; }
        [ForeignKey("Cost_center")]
        public string Cost_center_id { get; set; }
        public C_CreateAccount_Table Account { get; set; }
        public C_CostCenter_Table Cost_center { get; set; }
    }
}
