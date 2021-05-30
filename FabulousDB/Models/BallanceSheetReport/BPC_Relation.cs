using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class BPC_Relation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("BalanceSheet")]
        public int Balance_sheet_id { get; set; }
        [ForeignKey("Account")]
        public int? Account_id { get; set; }
        [ForeignKey("Row")]
        public int? Row_id { get; set; }
        public C_CreateAccount_Table Account { get; set; }
        public BPC_raw_settings Row { get; set; }
        public BPC_raw_settings BalanceSheet { get; set; }
        public Report_type Report_type { get; set; }


    }
}
