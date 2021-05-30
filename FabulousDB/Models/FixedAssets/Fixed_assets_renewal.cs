namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fixed_assets_renewal
    {
        public int Id { get; set; }

        public int? Renewal_no { get; set; }

        [StringLength(500)]
        public string Descroption { get; set; }
        [ForeignKey("Asset")]
        public int? Assets_id { get; set; }

        public decimal? Renewal_amount { get; set; }

        public decimal? Deprecation_rate { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public int? Gl_transaction_id { get; set; }

        public double? Use_life { get; set; }

        public DateTime? Renwal_date { get; set; }

        [StringLength(20)]
        public string Currency_id { get; set; }

        public virtual Asset Asset { get; set; }
    }
}
