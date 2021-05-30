namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Assets_main
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Assets_main()
        {
            Assets = new HashSet<Asset>();
        }

        public int Id { get; set; }

        public int Assets_class_id { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int Number_of_parts { get; set; }

        public bool Inactive { get; set; }

        [StringLength(200)]
        public string Assets_number { get; set; }

        public int? Assets_custom_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asset> Assets { get; set; }

        public virtual Assets_class Assets_class { get; set; }
    }
}
