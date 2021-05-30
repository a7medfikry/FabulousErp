using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.Tax;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company.Accounting
{
    public class C_PurchaseTaxDetails_Table
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int C_PTDID { get; set; }

        [ForeignKey("C_PurchaseTaxHeader_Table")]
        public int C_PostingNumber { get; set; }

        public int TaxType { get; set; }

        public int? TaxTableType { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string ItemName { get; set; }

        public int ItemType { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(20)]
        public string UnitOfMeasure { get; set; }

        public double Quantity { get; set; }

        public double UnitPrice { get; set; }

        public double TotalAmount { get; set; }

        public double TotalAmountBySystemCurrency { get; set; }

        public double? Discount { get; set; }

        public double NetAmount { get; set; }

        [ForeignKey("TableVatIDR")]
        public int? TableVatID { get; set; }

        public double? TableVatAmount { get; set; }

        public double? TotalAfterVatTable { get; set; }

        [ForeignKey("VatIDR")]
        public int? VatID { get; set; }

        public double? VatAmount { get; set; }

        [ForeignKey("DecuttaTaxIDR")]
        public int? DecuttaTaxID { get; set; }

        public double? DacuttaAmount { get; set; }


        public virtual C_PurchaseTaxHeader_Table C_PurchaseTaxHeader_Table { get; set; }

        public virtual C_TaxSetting_table TableVatIDR { get; set; }

        public virtual C_TaxSetting_table VatIDR { get; set; }

        public virtual C_TaxSetting_table DecuttaTaxIDR { get; set; }
    }
}
