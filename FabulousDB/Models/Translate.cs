using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Translate
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        public string Key { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        public string English { get; set; }
        [Column(TypeName = "nvarchar"), MaxLength(200)]
        public string Arabic { get; set; }
    }
    public enum Langs
    {
        English,
        Arabic
    }
}
