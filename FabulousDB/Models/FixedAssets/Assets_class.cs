namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Assets_class
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Assets_class()
        {
            Assets = new HashSet<Asset>();
            Assets_accounts = new HashSet<Assets_accounts>();
            Assets_main = new HashSet<Assets_main>();
            Depreication_assets_connection = new HashSet<Depreication_assets_connection>();
            New_assets_transaction = new HashSet<New_assets_transaction>();
            Stoking_assets = new HashSet<Stoking_assets>();
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asset> Assets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assets_accounts> Assets_accounts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Assets_main> Assets_main { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Depreication_assets_connection> Depreication_assets_connection { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<New_assets_transaction> New_assets_transaction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stoking_assets> Stoking_assets { get; set; }
    }
}
