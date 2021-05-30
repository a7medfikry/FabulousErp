namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deleted_assets_class
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Class_id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int? Deprecation_method { get; set; }

        public decimal? Deperecation_rate { get; set; }

        public bool Active { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public DateTime Deleted_date { get; set; }
    }
}
