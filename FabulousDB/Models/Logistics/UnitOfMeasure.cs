using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace FabulousDB.Models
{
    public class Unit_of_measure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        [Required]
        [DisplayName("Unit Of Measure")]
        public string Unit_id { get; set; }
        [DisplayName("Quantity Dicimal")]
        [Range(0,9)]
        public int Quantity_dicimal { get; set; }
        [DisplayName("Equivalante")]
        [ForeignKey("Equivalante")]
        public int? Equivalante_id { get; set; }
        public float Equivalante_quantity { get; set; } = 1;
        public Unit_of_measure Equivalante { get; set; }
        //public MultiOrDivide Action { get; set; }
    }
    public enum MultiOrDivide
    {
        Multipli=1,
        Divide = 2
    }
}
