using FabulousDB.DB_Tabels.Settings.Financial.Accounting.CurrenciesDefinition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account
{
    public class B_CurrencyCreateAccount_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int B_CCAID { get; set; }

        public int B_AID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string CurrencyID { get; set; }


        public virtual B_CreateAccount_Table B_CreateAccount_Table { get; set; }

        public virtual CurrenciesDefinition_Table CurrenciesDefinition_Table { get; set; }
    }
}
