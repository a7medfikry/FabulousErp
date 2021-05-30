namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deprecation_record
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deprecation_record()
        {
            Deprecation_number_of_units = new HashSet<Deprecation_number_of_units>();
        }

        public int Id { get; set; }

        public int? Asset_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public decimal? Assets_acquisition_cost { get; set; }

        public decimal? Renewal_amount { get; set; }

        public decimal? Disposal_amount { get; set; }

        public decimal? Total { get; set; }

        public decimal? Beginning_deprecation_accumulated { get; set; }

        public decimal? Depreication { get; set; }

        public decimal? Renewal_depreication { get; set; }

        public decimal? Disposal_depreication { get; set; }

        public decimal? Ending_deprecication_accumulated { get; set; }

        public decimal? Net_assets_cost { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public int? Deprecation_id { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Deprecation Deprecation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deprecation_number_of_units> Deprecation_number_of_units { get; set; }
    }
}
