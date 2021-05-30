using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax
{
    public class TaxGroup_table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TG_ID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        public string TaxGroupID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [Required]
        public string TaxGroupDescribtion { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string C_TaxGrouptype { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(10)]
        [Required]
        [ForeignKey("CompanyMainInfo_Table")]
        public string CompanyID { get; set; }

        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

        public virtual ICollection<C_TaxSetting_table> C_TaxSetting_Tables { get; set; }

        //public virtual ICollection<B_TaxSetting_table> B_TaxSetting_Tables { get; set; }

        //public virtual ICollection<F_TaxSetting_table> F_TaxSetting_Tables { get; set; }

        public virtual ICollection<C_PurchaseTaxHeader_Table> C_PurchaseTaxHeader_Tables { get; set; }
    }
}
