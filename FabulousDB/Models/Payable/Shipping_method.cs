using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models; namespace FabulousErp.Payable.Models
{
    public class Payable_shipping_method
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName ="nvarchar"),MaxLength(200)]
        public string Ship_method { get; set; }
        [Column(TypeName  = "nvarchar"),MaxLength(200)]
        public string Description { get; set; }
        [DefaultValue(false)]
        public bool Inactive { get; set; }
    }
}