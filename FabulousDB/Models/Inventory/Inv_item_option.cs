using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_item_option
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Item")]
        public int Inv_item_id { get; set; }
        public float? Height { get; set; }
        public float? Width { get; set; }
        public float? Depth { get; set; }
        public Size_type Size_type { get; set; } 
        public float? Wight { get; set; }
        public Wight_type Wight_type { get; set; }
        [MaxLength(300)]
        public string Image { get; set; }
        public Inv_item Item { get; set; }
    }
    public enum Size_type
    {
        Cm=1,
        Meter=2
    }
    public enum Wight_type
    {
        Gm=1,
        Kilo=2
    }
}
