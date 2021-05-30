using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;

namespace FabulousDB.Models
{
    public class BPC_raw_settings
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        [Required]
        public string Row_name { get; set; }
        [Required]
        public Account_row Type { get; set; }
        public Report_type Report_type { get; set; }
        public Minus Minus { get; set; }
        public int Priority { get; set; }
    }
    public enum Account_row
    {
        Account = 1,
        Row = 2
    }
    public enum Report_type
    {
        BallanceSheet = 1,
        PL = 2,
        CashFlow = 3
    }
    public enum Minus
    {
        [Description("Current Minus Last")]
        Current_Minus_Last = 1,
        [Description("Last Minus Current")]
        Last_Minus_Current = 2
    }
}
