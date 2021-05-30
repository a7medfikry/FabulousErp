using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_password_option
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DefaultValue(false)]
        public bool HasPassword { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        public string Password { get; set; }
        public Password_optionEnum Option { get; set; }

    }
}