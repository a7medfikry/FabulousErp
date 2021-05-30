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
    public class C_PurchaseTaxHeader_Table
    {
        [Key, ForeignKey("C_GeneralJournalEntry_Table")]
        public int C_PostingNumber { get; set; }

        public int DocumentType { get; set; }

        public int DocumentNumber { get; set; }

        public DateTime DocumentDate { get; set; }

        [ForeignKey("TaxGroup_Table")]
        public int TG_ID { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string VendorName { get; set; }

        public int TaxRegisterNumber { get; set; }

        public int TaxFileNumber { get; set; }

        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string Address { get; set; }

        public int NationalID { get; set; }

        public int MobileNumber { get; set; }


        public virtual C_GeneralJournalEntry_Table C_GeneralJournalEntry_Table { get; set; }

        public virtual C_TaxSetting_table TaxGroup_Table { get; set; }

        public virtual ICollection<C_PurchaseTaxDetails_Table> C_PurchaseTaxDetails_Tables { get; set; }
    }
}
