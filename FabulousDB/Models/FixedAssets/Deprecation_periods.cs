namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deprecation_periods
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deprecation_periods()
        {
            Deprecations = new HashSet<Deprecation>();
            Deprecation_temp = new HashSet<Deprecation_temp>();
        }

        public int Id { get; set; }

        [StringLength(20)]
        public string text { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Period_start { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Period_end { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deprecation> Deprecations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deprecation_temp> Deprecation_temp { get; set; }
    }
}
