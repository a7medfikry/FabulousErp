namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Additional_information
    {
        public int Id { get; set; }

        public int? Assets_id { get; set; }

        [StringLength(200)]
        public string Field_name { get; set; }

        public virtual Asset Asset { get; set; }
    }
}
