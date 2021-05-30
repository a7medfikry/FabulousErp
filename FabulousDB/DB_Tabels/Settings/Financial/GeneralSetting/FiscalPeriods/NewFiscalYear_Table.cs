using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods
{
    public class NewFiscalYear_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int YearID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Year { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string Fiscal_Year_ID { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string Fiscal_Year_Start { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string Fiscal_Year_End { get; set; }


        public bool? Closed { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(7)]
        public string CheckDate { get; set; }


        //one to many to fiscal year
        [ForeignKey("YearID")]
        public virtual ICollection<FiscalYear_Table> FiscalYear_Table { get; set; }


        //one to many to fiscal adjusment
        [ForeignKey("YearID")]
        public virtual ICollection<FiscalAdjustment_Table> FiscalAdjustment_Table { get; set; }


        public FiscalDefinition_Table FiscalDefinition_Table { get; set; }


        public virtual ICollection<C_EndingBeginingYear> C_EndingBeginingYears { get; set; }


    }
}
