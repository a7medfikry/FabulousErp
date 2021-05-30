using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Transaction.Financial.Company
{
    public class C_PurchaseTaxHeader_tbl
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaxHeader_ID { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(10)]
        public string CompanyID { get; set; }
        public int DocumentNumber { get; set; }
        public string DocumentDate { get; set; }
        public int TaxGroup_ID { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string VendorName { get; set; }
        public int TaxRegister_Number { get; set; }
        public int TaxFile_Number { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [Required]
        public string Address { get; set; }
        public int National_ID { get; set; }
        public int MobileNumber { get; set; }
    }
}
