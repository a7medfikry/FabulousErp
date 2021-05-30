using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Factory.F_Account
{
    public class F_CurrencyCreateAccount_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int F_CCAID { get; set; }

        public int F_AID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }


        public virtual F_CreateAccount_Table F_CreateAccount_Table { get; set; }

        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }
    }
}
