using FabulousDB.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using FabulousDB.Models;
namespace FabulousErp.Payable.Models
{
    public class Payable_genral_setting
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Document Type")]
        public Doc_type Doc_type { get; set; }
        [DefaultValue(true)]
        public bool Checked { get; set; }
        [DisplayName("Next Number")]
        [Column(TypeName ="nvarchar"),MaxLength(50)]
        public string Next_number { get; set; }
    }
 
}