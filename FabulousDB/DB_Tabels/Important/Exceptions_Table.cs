using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Tabels.Important
{
    public class Exceptions_Table
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string URL { get; set; }

        public string Exception { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
