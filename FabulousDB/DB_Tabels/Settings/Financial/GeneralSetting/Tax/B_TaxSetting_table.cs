using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Branch.B_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax
{
    public class B_TaxSetting_table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BT_ID { get; set; }

        public int? B_AID { get; set; }

        public int? CT_ID { get; set; }

        [ForeignKey("TaxGroup_Table")]
        public int TG_ID { get; set; }

        public virtual B_CreateAccount_Table B_CreateAccount_Table { get; set; }

        public virtual C_TaxSetting_table C_TaxSetting_Table { get; set; }

        public virtual C_TaxSetting_table TaxGroup_Table { get; set; }
    }
}
