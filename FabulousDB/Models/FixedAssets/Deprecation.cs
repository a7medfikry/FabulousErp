namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Deprecation")]
    public partial class Deprecation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deprecation()
        {
            Deprecation_record = new HashSet<Deprecation_record>();
            Depreication_assets_connection = new HashSet<Depreication_assets_connection>();
            Depreication_assets_id_connection = new HashSet<Depreication_assets_id_connection>();
        }

        public int Id { get; set; }

        public int? Deprecation_no { get; set; }

        public DateTime? Transaction_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime Deprecation_date { get; set; }

        public int? Period { get; set; }

        public bool? Is_assets_class { get; set; }

        public decimal? Acquisition_cost { get; set; }

        public decimal? Depreciation_accumulated { get; set; }

        public decimal? Adjustment_cost { get; set; }

        public decimal Deprecation_rate { get; set; }

        public decimal? Depreication_cost { get; set; }

        public decimal? Special_depreication { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public int? Period_id { get; set; }

        public int? Month { get; set; }

        public int? Year { get; set; }

        public DateTime? Createion_date { get; set; }

        [StringLength(200)]
        public string Jornal_number { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deprecation_record> Deprecation_record { get; set; }

        public virtual Deprecation_periods Deprecation_periods { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Depreication_assets_connection> Depreication_assets_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Depreication_assets_id_connection> Depreication_assets_id_connection { get; set; }
    }
}
