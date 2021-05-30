using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyFactoryInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Factory.Accounting
{
    public class F_CreateBatch_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int F_CBID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string FactoryID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(11)]
        [Required]
        public string F_Module { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(5)]
        [Required]
        public string F_BatchID { get; set; }


        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string F_BatchDescription { get; set; }


        public bool? Approval { get; set; }

        public bool? NotApproval { get; set; }



        public virtual CompanyFactoryInfo_Table CompanyFactoryInfo_Table { get; set; }
    }
}
