namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Stoking_assets
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Stoking_assets()
        {
            Stocking_assets_transaction = new HashSet<Stocking_assets_transaction>();
            Stocking_notes = new HashSet<Stocking_notes>();
        }

        public int Id { get; set; }

        public int? Assets_id { get; set; }

        public int? Assets_class_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public DateTime? Added_date { get; set; }

        [StringLength(200)]
        public string Serial { get; set; }

        public virtual Asset Asset { get; set; }

        public virtual Assets_class Assets_class { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stocking_assets_transaction> Stocking_assets_transaction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stocking_notes> Stocking_notes { get; set; }
    }
}
