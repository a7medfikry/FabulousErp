using FabulousDB.DB_Context;
using FabulousDB.Migrations;
using FabulousErp.Payable.Models;
//using FabulousDB.Migrations;
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
        //public PayableContext():base("ERPContext")
        //{
        //    Database.SetInitializer<PayableContext>(new CreateDatabaseIfNotExists<PayableContext>());
        //    //Database.SetInitializer(new MigrateDatabaseToLatestVersion<PayableContext, PayConfiguration>());
        //}
        //Start General Settings
        public DbSet<Payable_genral_setting> General_settings { get; set; }
        public DbSet<Payable_aging_period> Aging_periods { get; set; }
        public DbSet<Payable_password_option> Password_Options { get; set; }
        public DbSet<Payable_other_option> Other_options { get; set; }
        public DbSet<Payable_payment_term> Payment_terms { get; set; }
        public DbSet<Payable_shipping_method> Shipping_methods { get; set; }
        //End General Settings

        public DbSet<Payable_Group_setting> Payable_Group_settings { get; set; }
        public DbSet<Payable_creditor_setting> Payable_creditor_setting { get; set; }

        public DbSet<Payable_creditor_currencies> Creditro_currencies { get; set; }
        public DbSet<Payable_gl_account> Payable_gl_accounts { get; set; }
        public DbSet<Payable_address_info> Payable_address_infos { get; set; }
        public DbSet<Payable_legal_info> Legal_infos { get; set; }
        public DbSet<Payable_bank_info> Bank_info { get; set; }
        public DbSet<Payable_transactions_types> Payable_transactions_types { get; set; }
        public DbSet<Payable_transaction> Payable_transactions { get; set; }
        public DbSet<Payable_payment> Payable_payments { get; set; }
        public DbSet<Assign_payable_doc> Assign_payable_docs { get; set; }
        public DbSet<Payable_aging_date_option> Aging_date_option { get; set; }
        public DbSet<Payable_void> Payable_void { get; set; }
        public DbSet<Payable_Assign_void> Assign_void { get; set; }
        public DbSet<FabulousErp.Payable.Models.Related_pay_trans> Related_pay_trans { get; set; }
   

    }
    //public partial class DBContext : DbContext
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
    //                    using (PayableContext db = new PayableContext())
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

    //            SavedEntities = base.SaveChanges();

    //            Count = 0;
    //            foreach (var entity in Added.Where(x => !NoLogTable.Contains(x.GetType().Name)))
    //            {
    //                try
    //                {
    //                    using (PayableContext db = new PayableContext())
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