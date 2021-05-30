namespace FabulousDB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class New_assets_transaction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public New_assets_transaction()
        {
            Assets = new HashSet<Asset>();
        }

        public int Id { get; set; }

        public decimal? Acquesation_cost { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_of_orgin { get; set; }

        [StringLength(50)]
        public string Currency_id { get; set; }

        [StringLength(200)]
        public string Reference { get; set; }

        public int? Vendor_id { get; set; }

        public int? Type { get; set; }

        public int? Assets_class_id { get; set; }

        [StringLength(10)]
        public string Company_id { get; set; }

        public int? Gl_transaction_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Transaction_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Posting_date { get; set; }

        public bool IsVoid { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [InverseProperty("New_assets_transaction")]
        public virtual ICollection<Asset> Assets { get; set; }

        public virtual Assets_class Assets_class { get; set; }
    }
}
