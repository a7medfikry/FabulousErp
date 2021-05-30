using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Receivable.Models
{
    [Table("Receivable_address_info")]
    public class Receivable_address_info
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(500)]
        public string Address { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string City { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string State { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        public string Country { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [DisplayName("Post Code")]
        public string Post_code { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [DisplayName("Phone Number")]
        public string Phone_number { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [DisplayName("Fax")]
        public string Fax { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(100)]
        [DisplayName("Contact Person")]
        public string Contact_person { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [DisplayName("Mobile number")]
        public string Mobile_number { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(50)]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [ForeignKey("Creditor")]
        public int Creditor_id { get; set; }
        public Receivable_vendore_setting Creditor { get; set; }
    }
}
