using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    public class Receivable_bank_info
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Cheque Name")]
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        public string Cheque_name { get; set; }
        [DisplayName("Bank Name")]
        [Column(TypeName ="nvarchar"), MaxLength(50)]

        public string Bank_name { get; set; }
        [Column(TypeName ="nvarchar"), MaxLength(100)]

        public string Branch { get; set; }
        [Column(TypeName ="nvarchar"), MaxLength(50)]

        [DisplayName("Account Name")]
        public string Account_name { get; set; }
        [Column(TypeName ="nvarchar"), MaxLength(50)]

        [DisplayName("Account Number")]
        public string Account_number { get; set; }
        [DisplayName("Swift Code")]
        [Column(TypeName ="nvarchar"), MaxLength(100)]

        public string Swift_code { get; set; }
        [DisplayName("Bank Address")]
        [Column(TypeName ="nvarchar"), MaxLength(200)]

        public string Bank_address { get; set; }
        [Column(TypeName ="nvarchar"), MaxLength(100)]

        public string Iban { get; set; }
        [ForeignKey("Creditor")]
        [DisplayName("Creditor Id")]
        public int Creditor_id { get; set; }
        public Receivable_vendore_setting Creditor { get; set; }

    }
}