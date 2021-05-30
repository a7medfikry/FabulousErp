using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.Models
{
    public class Inv_item_recipe
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Main_item")]
        public int Main_item_id { get; set; }
        [ForeignKey("Recipe_item")]
        public int Recipe_item_id { get; set; }
        public float Qty { get; set; }
        public Inv_item Recipe_item{ get; set; }
        public Inv_item Main_item { get; set; }
    }
}
