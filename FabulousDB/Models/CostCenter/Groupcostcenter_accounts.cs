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
    public  class Groupcostcenter_accounts
    {
        public int Id { get; set; }
        [ForeignKey("Account")]
        public int Account_id { get; set; }
        [ForeignKey("Group_costcenter")]
        public int Group_id { get; set; }
        public C_CreateAccount_Table Account { get; set; }
        public C_GroupCostCenter_Table Group_costcenter { get; set; }
    }
}
