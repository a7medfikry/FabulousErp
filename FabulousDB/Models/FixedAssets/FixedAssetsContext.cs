namespace FabulousDB.DB_Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using FabulousDB.Models;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class DBContext : DbContext
    {
        
        public virtual DbSet<Additional_information> Additional_information { get; set; }
        public virtual DbSet<Asset> Assets { get; set; }
        public virtual DbSet<Assets_accounts> Assets_accounts { get; set; }
        public virtual DbSet<Assets_class> Assets_class { get; set; }
        public virtual DbSet<Assets_main> Assets_main { get; set; }
        public virtual DbSet<Assets_part_serial> Assets_part_serial { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Delete_assets_main> Delete_assets_main { get; set; }
        public virtual DbSet<Delete_fixed_assets_disposel> Delete_fixed_assets_disposel { get; set; }
        public virtual DbSet<Deleted_assets> Deleted_assets { get; set; }
        public virtual DbSet<Deleted_assets_class> Deleted_assets_class { get; set; }
        public virtual DbSet<Deleted_assets_serial> Deleted_assets_serial { get; set; }
        public virtual DbSet<Deleted_fixed_assets_renewal> Deleted_fixed_assets_renewal { get; set; }
        public virtual DbSet<Deprecation> Deprecations { get; set; }
        public virtual DbSet<Deprecation_number_of_units> Deprecation_number_of_units { get; set; }
        public virtual DbSet<Deprecation_periods> Deprecation_periods { get; set; }
        public virtual DbSet<Deprecation_record> Deprecation_record { get; set; }
        public virtual DbSet<Deprecation_Setting> Deprecation_Setting { get; set; }
        public virtual DbSet<Deprecation_temp> Deprecation_temp { get; set; }
        public virtual DbSet<Deprecation_temp_record> Deprecation_temp_record { get; set; }
        public virtual DbSet<Depreication_assets_connection> Depreication_assets_connection { get; set; }
        public virtual DbSet<Depreication_assets_id_connection> Depreication_assets_id_connection { get; set; }
        public virtual DbSet<Fixed_assets_disposel> Fixed_assets_disposel { get; set; }
        public virtual DbSet<Fixed_assets_renewal> Fixed_assets_renewal { get; set; }
        public virtual DbSet<Fixed_assets_revaluate> Fixed_assets_revaluate { get; set; }
        public virtual DbSet<New_assets_transaction> New_assets_transaction { get; set; }
        public virtual DbSet<Stocking_assets_transaction> Stocking_assets_transaction { get; set; }
        public virtual DbSet<Stocking_notes> Stocking_notes { get; set; }
        public virtual DbSet<Stoking_assets> Stoking_assets { get; set; }
        public virtual DbSet<Delete_fixed_assets_revaluate> Delete_fixed_assets_revaluate { get; set; }


    }
}
