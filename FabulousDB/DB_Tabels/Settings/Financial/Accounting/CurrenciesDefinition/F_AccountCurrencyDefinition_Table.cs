using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition
{
    public class F_AccountCurrencyDefinition_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int F_ACD_ID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }

        public int F_AID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string Type { get; set; }

        public virtual F_CreateAccount_Table C_CreateAccount_Table { get; set; }

        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }
    }
}
