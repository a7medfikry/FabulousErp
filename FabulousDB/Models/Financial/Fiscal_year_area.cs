using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.FiscalPeriods;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousDB.Models
{
    public class Fiscal_year_area
    {
        [Key]
        public int Id { get; set; }
        public string Area_name { get; set; }
        public bool Allowed { get; set; }
        public bool Allow_adjust { get; set; }

        [ForeignKey("FiscalYear_Table")]
        public int? FiscalYear_Table_id { get; set; }
        [ForeignKey(nameof(FiscalAdjustment_Table))]
        public int? FiscalAdjustment_Table_id { get; set; }
        public FiscalYear_Table FiscalYear_Table { get; set; }
        public FiscalAdjustment_Table FiscalAdjustment_Table { get; set; }
    }
}