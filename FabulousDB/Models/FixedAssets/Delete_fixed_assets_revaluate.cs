namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Delete_fixed_assets_revaluate
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string Revaluate_no { get; set; }

        public DateTime? Transaction_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Revaluate_date { get; set; }

        public int? Assets_id { get; set; }

        public decimal? Old_cost { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Old_use_life { get; set; }

        public decimal? Adjustment_cost { get; set; }

        public decimal? Net_profit { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public DateTime Delete_date { get; set; }
    }
}
