using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_legal_info
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [RegularExpression("^[0-9/-]+$")]
        [Column(TypeName ="nvarchar"),MaxLength(200)]
        [DisplayName("Tax file no")]
        public string Tax_file_no { get; set; }
        [RegularExpression("^[0-9/-]+$")]
        [Column(TypeName ="nvarchar"), MaxLength(200)]
        [DisplayName("Tax Id")]

        public string Tax_Id { get; set; }
        [RegularExpression("^[0-9/-]+$")]
        [Column(TypeName ="nvarchar"), MaxLength(200)]
        [DisplayName("C.R.")]
        public string Commercial_register { get; set; }
        [RegularExpression("^[0-9/-]+$")]
        [Column(TypeName ="nvarchar"), MaxLength(200)]
        [DisplayName("Social Insurance")]
        public string Social_insurance { get; set; }
        [ForeignKey("Creditor")]
        public int Creditor_id { get; set; }
        public Payable_creditor_setting Creditor { get; set; }
    }
}