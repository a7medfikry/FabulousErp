using FabulousDB.DB_Tabels.Settings.Financial.Accounting._Company.C_Account;
using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.C_B_F_Setup.CompanyInformation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax
{
    public class C_TaxSetting_table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CT_ID { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(15)]
        public string C_Taxcode { get; set; }

        //[Required]
        //[Column(TypeName = "nvarchar"), MaxLength(30)]
        //public string C_Taxtype { get; set; }

        [Required]
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        public string C_Taxdescribtion { get; set; }

        public double? C_Taxpercentage { get; set; }

        public double? C_TaxAmount { get; set; }

        public double? C_MinTaxable { get; set; }

        public double? C_MaxTaxable { get; set; }

        public bool? C_Printdocument { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string C_Selectprintdocument { get; set; }
        public int? C_AID { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(10)]
       // [ForeignKey("CompanyMainInfo_Table")]
        public string CompanyID { get; set; }


        [ForeignKey("TaxGroup_Table")]
        public int TG_ID { get; set; }

        public Transaction_type Transaction_type { get; set; }
        public virtual C_CreateAccount_Table C_CreateAccount_Table { get; set; }
        public virtual CompanyMainInfo_Table CompanyMainInfo_Table { get; set; }

        //public virtual ICollection<B_TaxSetting_table> B_TaxSetting_Tables { get; set; }

        //public virtual ICollection<F_TaxSetting_table> F_TaxSetting_Tables { get; set; }

        public virtual TaxGroup_table TaxGroup_Table { get; set; }

    }
    public enum TaxGroup_enum
    {
        Purchasing=1,
        Sales=2,
        General =3
    }
    public enum Transaction_type
    {
        None = 0,
        Supplies = 1,
        Service = 2,
        Consultant = 3
    }
}
