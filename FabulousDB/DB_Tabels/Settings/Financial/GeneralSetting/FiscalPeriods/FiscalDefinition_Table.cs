using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class FiscalDefinition_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string Fiscal_Year_ID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Fiscal_Year_Name { get; set; }
        
        public int Number_Of_Periods { get; set; }

        public int Number_Of_Adjustment_Periods { get; set; }

        
        //one to many to company fiscal
        public ICollection<CompanyFiscalYear_Table> CompanyFiscalYear_Table { get; set; }


        [ForeignKey("Fiscal_Year_ID")]
        public ICollection<NewFiscalYear_Table> NewFiscalYear_Table { get; set; }

    }
}
