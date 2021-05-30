using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Post
{
    public class PrintDocument_Table
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PD_ID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string CompanyID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(11)]
        [Required]
        public string Module { get; set; }


        [Column(TypeName ="nvarchar"), MaxLength(50)]
        [Required]
        public string TransactionFormName { get; set; }


        public bool Ask { get; set; }


        public bool PrintDirect { get; set; }


        public bool Analytic { get; set; }


        public bool CostCenter { get; set; }



        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }
    }
}
