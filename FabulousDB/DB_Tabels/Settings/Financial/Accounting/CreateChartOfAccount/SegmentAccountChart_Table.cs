using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount
{
    public class SegmentAccountChart_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SegmentID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string AccountChartID { get; set; }


        public int IncreaseSegment { get; set; }


        [Required]
        [Column(TypeName = "nvarchar"),MaxLength(10)]
        public string SegmentName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(2)]
        public string Length { get; set; }


        // Account Chart has many of segments
        public AccountChart_Table AccountChart_Table { get; set; }
    }
}
