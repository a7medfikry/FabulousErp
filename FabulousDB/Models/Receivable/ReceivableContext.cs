//using FabulousDB.Migrations;
using FabulousDB.DB_Context;
using FabulousDB.Migrations;
using FabulousErp.Receivable.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;



namespace FabulousDB.DB_Context
{
    public partial class DBContext : DbContext
    {
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
        //    modelBuilder.Conventions.Add(new DecimalPropertyConvention(30, 9));
        //}
        //public ReceivableContext():base("ERPContext")
        //{
        //    Database.SetInitializer<ReceivableContext>(new CreateDatabaseIfNotExists<ReceivableContext>());
        //    //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReceivableContext, RecConfiguration>());
        //}
        //Start General Settings
        public DbSet<Receivable_genral_setting> Receivable_general_settings { get; set; }
        public DbSet<Receivable_aging_period> Receivable_aging_periods { get; set; }
        public DbSet<Receivable_password_option> Receivable_password_Options { get; set; }
        public DbSet<Receivable_other_option> Receivable_other_options { get; set; }
        public DbSet<Receivable_payment_term> Receivable_payment_terms { get; set; }
        public DbSet<Receivable_shipping_method> Receivable_shipping_methods { get; set; }
        //End General Settings

        public DbSet<Receivable_Group_setting> Receivable_Group_settings { get; set; }
        public DbSet<Receivable_vendore_setting> Receivable_vendore_settings { get; set; }
        public DbSet<Receivable_vendore_currencies> Receivable_vendore_currencies { get; set; }
        public DbSet<Receivable_gl_account> Receivable_gl_accounts { get; set; }
        public DbSet<Receivable_address_info> Receivable_address_infos { get; set; }
        public DbSet<Receivable_legal_info> Receivable_legal_infos { get; set; }
        public DbSet<Receivable_bank_info> Receivable_bank_info { get; set; }

        public DbSet<Receivable_transactions_types> Receivable_transactions_types { get; set; }
        public DbSet<Receivable_transaction> Receivable_transactions { get; set; }
        public DbSet<Receivable_payment> Receivable_payments { get; set; }
        public DbSet<Assign_Receivable_doc> Assign_Receivable_docs { get; set; }
        public DbSet<Receivable_aging_date_option> Receivable_aging_date_option { get; set; }
        public DbSet<Receivable_void> Receivable_void { get; set; }
        public DbSet<Receivable_Assign_void> Receivable_assign_void { get; set; }
        public DbSet<FabulousErp.Receivable.Models.Related_rec_trans> Related_rec_trans { get; set; }

        //public static ReceivableContext db()
        //{
        //    return new ReceivableContext();
        //}
    }
    //public partial class ReceivableContext : DbContext
    //{
    //    public override int SaveChanges()
    //    {
    //        if (User() == null)
    //        {
    //            return base.SaveChanges();
    //        }
    //        else
    //        {
    //            ChangeTracker.DetectChanges();
    //            var Added = ChangeTracker.Entries()
    //                        .Where(t => t.State == EntityState.Added)
    //                        .Select(t => t.Entity)
    //                        .ToList();

    //            var Edit = ChangeTracker.Entries()
    //                       .Where(t => t.State == EntityState.Modified)
    //                       .Select(t => t.Entity)
    //                       .ToList();

    //            var Delete = ChangeTracker.Entries()
    //                     .Where(t => t.State == EntityState.Deleted)
    //                     .Select(t => t.Entity)
    //                     .ToList();

    //            List<string> NoLogTable = new List<string>()
    //           {
    //               "Translate"
    //           };
    //            List<Log> Logs = Enumerable.Empty<Log>().ToList();
    //            int Count = 0;
    //            foreach (var entity in Edit.Where(x => !NoLogTable.Contains(x.GetType().Name)))
    //            {
    //                try
    //                {
    //                    using (ReceivableContext db = new ReceivableContext())
    //                    {
    //                        string Name = entity.GetType().Name;
    //                        db.Configuration.ProxyCreationEnabled = false;
    //                        db.Configuration.LazyLoadingEnabled = true;

    //                        var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;

    //                        // Get the mapping between CLR types and metadata OSpace
    //                        var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));
    //                        // Get metadata for given CLR type
    //                        var entityMetadata = metadata
    //                                .GetItems<EntityType>(DataSpace.OSpace)
    //                                .FirstOrDefault(e => objectItemCollection.GetClrType(e) == entity.GetType());
    //                        if (entityMetadata == null)
    //                        {
    //                            entityMetadata = metadata
    //                               .GetItems<EntityType>(DataSpace.OSpace)
    //                               .FirstOrDefault(e => objectItemCollection.GetClrType(e).Name == entity.GetType().Name.Split('_')[0]);
    //                        }
    //                        var PrimaryKeyName = entityMetadata.KeyProperties.Select(p => p.Name).ToArray()[0];
    //                        this.Configuration.ProxyCreationEnabled = false;
    //                        int PrimaryKeyValue = Convert.ToInt32(entity.GetType().GetProperty(PrimaryKeyName).GetValue(entity));
    //                        var ThisOldEntity = db.Set(entity.GetType()).Find(PrimaryKeyValue);
    //                        Log(ref Logs, ThisOldEntity, entity, "Edit");


    //                    }
    //                    //var OtherEntity = ThisOldEntity.GetValue(this);

    //                }
    //                catch
    //                {

    //                }
    //            }
    //            Count = 0;
    //            foreach (var entity in Delete.Where(x => !NoLogTable.Contains(x.GetType().Name.Split('_')[0])))
    //            {
    //                try
    //                {
    //                    Log(ref Logs, entity, null, "Delete");
    //                }
    //                catch
    //                {

    //                }
    //            }

    //            int SavedEntities = 0;

    //                SavedEntities = base.SaveChanges();

    //            Count = 0;
    //            foreach (var entity in Added.Where(x => !NoLogTable.Contains(x.GetType().Name)))
    //            {
    //                try
    //                {
    //                    using (ReceivableContext db = new ReceivableContext())
    //                    {
    //                        string Name = entity.GetType().Name;
    //                        db.Configuration.ProxyCreationEnabled = false;
    //                        db.Configuration.LazyLoadingEnabled = true;

    //                        var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;

    //                        // Get the mapping between CLR types and metadata OSpace
    //                        var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

    //                        // Get metadata for given CLR type
    //                        var entityMetadata = metadata
    //                                .GetItems<EntityType>(DataSpace.OSpace)
    //                                .FirstOrDefault(e => objectItemCollection.GetClrType(e) == entity.GetType());
    //                        if (entityMetadata == null)
    //                        {
    //                            entityMetadata = metadata
    //                               .GetItems<EntityType>(DataSpace.OSpace)
    //                               .FirstOrDefault(e => objectItemCollection.GetClrType(e).Name == entity.GetType().Name.Split('_')[0]);
    //                        }
    //                        var PrimaryKeyName = entityMetadata.KeyProperties.Select(p => p.Name).ToArray()[0];
    //                        this.Configuration.ProxyCreationEnabled = false;
    //                        int PrimaryKeyValue = Convert.ToInt32(entity.GetType().GetProperty(PrimaryKeyName).GetValue(entity));
    //                        var ThisOldEntity = db.Set(entity.GetType()).Find(PrimaryKeyValue);
    //                        Log(ref Logs, ThisOldEntity, null, "Added");

    //                    }
    //                }
    //                catch
    //                {

    //                }

    //            }

    //            if (Logs.Count > 0)
    //            {
    //                // using (HRPEntities db = new HRPEntities())
    //                {
    //                    using (LogContext LDb = new LogContext())
    //                    {
    //                        LDb.Logs.AddRange(Logs);
    //                        LDb.SaveChanges();
    //                    }
    //                    //base.SaveChanges();

    //                    // db.SaveChanges();
    //                }
    //            }
    //            return SavedEntities;
    //        }

    //    }

    //    private void Log(ref List<Log> Logs, object OldEntity, object NewEntity, string Action)
    //    {
    //        var Name = OldEntity.GetType().Name;


    //        //  OldEntity.GetType()
    //        //if (Action == "Delete" || Action == "Edit")
    //        //{
    //        //    Name = Name.Split('_')[0];
    //        //}
    //        try
    //        {
    //            var NewValue = Newtonsoft.Json.JsonConvert.SerializeObject(NewEntity, new MyJsonConverter()).Replace(" ", "");
    //            var OldValue = Newtonsoft.Json.JsonConvert.SerializeObject(OldEntity, new MyJsonConverter()).Replace(" ", "");
    //            Logs.Add(new Log
    //            {
    //                Entity_name = Name,
    //                Action = Action,
    //                Old_value = OldValue,
    //                New_value = NewValue,
    //                Creation_date = DateTime.Now,
    //                User_id = User()
    //            });
    //        }
    //        catch
    //        {

    //        }


    //    }

    //    string User()
    //    {
    //        try
    //        {
    //            HttpCookie LoginCookie = HttpContext.Current.Request.Cookies["Login"];

    //            if (LoginCookie["UserId"] != null)
    //            {
    //                string MyUser = LoginCookie["UserId"];
    //                return MyUser;
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //        catch
    //        {
    //            return null;
    //        }

    //    }
    //}
}