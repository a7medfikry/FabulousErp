using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class FiscalYear_Table
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        public int? YearID { get; set; }


        public int Period_No { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string Period_Start_Date { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string Period_End_Date { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(7)]
        public string CheckDate { get; set; }

        //public bool? Financial { get; set; }
        //public bool? Purchasing { get; set; }
        //public bool? Sales { get; set; }
        //public bool? Inventory { get; set; }

        public virtual NewFiscalYear_Table NewFiscalYear_Table { get; set; }
        public virtual ICollection<Fiscal_year_area> Fiscal_year_area { get; set; }

    }
}
