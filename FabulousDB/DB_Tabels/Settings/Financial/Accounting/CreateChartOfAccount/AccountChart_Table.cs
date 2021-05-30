using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Analytic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.Accounting.CreateChartOfAccount
{
    public class AccountChart_Table
    {
        [Key]
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string AccountChartID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"),MaxLength(50)]
        public string AccountChartName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string EstablishDate { get; set; }

        [Required]
        [Column( TypeName = "nvarchar"), MaxLength(3)]
        public string AccountLength { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string NumberOfSegment { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string MainSegment { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        public string Separate { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string MoveUserID { get; set; }


        // Account Chart has many of segments
        public virtual ICollection<SegmentAccountChart_Table> SegmentAccountChart_Table { get; set; }


        // Account Chart can relation with many of company
        public virtual ICollection<CompanyChartAccount_Table> CompanyChartAccount_Table { get; set; }


        public virtual ICollection<AccountGroupOfChart_Table> AccountGroupOfChart_Table { get; set; }


        public virtual ICollection<C_CreateAccount_Table> C_CreateAccount_Table { get; set; }

    }
}
