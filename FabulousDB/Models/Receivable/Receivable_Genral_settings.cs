using FabulousDB.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FabulousErp.Receivable.Models
{
    [Table("Receivable_genral_setting")]
    public class Receivable_genral_setting
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
        public string Receviable { get; set; }
    }

}