using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount
{
    public class ChartGroupContent_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string AccountName { get; set; }


        public double? AccountFrom { get; set; }


        public double? AccountTo { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string AccountFromWithSep { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string AccountToWithSep { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string AccountGroupChartID { get; set; }



        public virtual AccountGroupOfChart_Table AccountGroupOfChart_Table { get; set; }
    }
}
