using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount
{
    public class AccountGroupOfChart_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string AccountGroupChartID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string AccountGroupChartName { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Index(IsUnique =true)]
        [Required]
        public string AccountChartID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(12)]
        [Required]
        public string EstablishDate { get; set; }



        public virtual AccountChart_Table AccountChart_Table { get; set; }


        public virtual ICollection<ChartGroupContent_Table> ChartGroupContent_Table { get; set; }


    }
}
