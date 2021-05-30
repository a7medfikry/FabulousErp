namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Fixed_assets_disposel
    {
        public int Id { get; set; }

        public int? Disposal_no { get; set; }

        public DateTime? Transaction_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Disposal_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Depreication_up_to_date { get; set; }

        public int? Assets_id { get; set; }

        public decimal? Disposal_amount { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public int? Gl_transaction_id { get; set; }

        [StringLength(50)]
        public string Currency_id { get; set; }

        [Required]
        [StringLength(200)]
        public string Reference { get; set; }

        public virtual Asset Asset { get; set; }
    }
}
