namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deprecation_number_of_units
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int? Asset_id { get; set; }

        public int? Deprecation_number_of_unit { get; set; }

        public int? Deprecation_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Deprecation_record Deprecation_record { get; set; }
    }
}
