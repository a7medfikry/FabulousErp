using FabulousDB.DB_Tabels.Settings.Financial.GeneralSetting.UserAccount;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FabulousDB.DB_Context
{
    public partial class DBContext : DbContext
    {
        public override int SaveChanges()
        {
            if (User() == null)
            {
                return base.SaveChanges();
            }
            else
            {
                ChangeTracker.DetectChanges();
                var Added = ChangeTracker.Entries()
                            .Where(t => t.State == EntityState.Added)
                            .Select(t => t.Entity)
                            .ToList();

                var Edit = ChangeTracker.Entries()
                           .Where(t => t.State == EntityState.Modified)
                           .Select(t => t.Entity)
                           .ToList();

                var Delete = ChangeTracker.Entries()
                         .Where(t => t.State == EntityState.Deleted)
                         .Select(t => t.Entity)
                         .ToList();

                List<string> NoLogTable = new List<string>()
               {
                   "Translate"
               };
                List<Log> Logs = Enumerable.Empty<Log>().ToList();
                int Count = 0;
                foreach (var entity in Edit.Where(x => !NoLogTable.Contains(x.GetType().Name)))
                {
                    try
                    {
                        using (DBContext db = new DBContext())
                        {
                            string Name = entity.GetType().Name;
                            db.Configuration.ProxyCreationEnabled = false;
                            db.Configuration.LazyLoadingEnabled = true;

                            var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;

                            // Get the mapping between CLR types and metadata OSpace
                            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));
                            // Get metadata for given CLR type
                            var entityMetadata = metadata
                                    .GetItems<EntityType>(DataSpace.OSpace)
                                    .FirstOrDefault(e => objectItemCollection.GetClrType(e) == entity.GetType());
                            if (entityMetadata == null)
                            {
                                entityMetadata = metadata
                                   .GetItems<EntityType>(DataSpace.OSpace)
                                   .FirstOrDefault(e => objectItemCollection.GetClrType(e).Name == entity.GetType().Name.Split('_')[0]);
                            }
                            var PrimaryKeyName = entityMetadata.KeyProperties.Select(p => p.Name).ToArray()[0];
                            this.Configuration.ProxyCreationEnabled = false;
                            int PrimaryKeyValue = Convert.ToInt32(entity.GetType().GetProperty(PrimaryKeyName).GetValue(entity));
                            var ThisOldEntity = db.Set(entity.GetType()).Find(PrimaryKeyValue);
                            Log(ref Logs, ThisOldEntity, entity, "Edit");


                        }
                        //var OtherEntity = ThisOldEntity.GetValue(this);

                    }
                    catch
                    {

                    }
                }
                Count = 0;
                foreach (var entity in Delete.Where(x => !NoLogTable.Contains(x.GetType().Name.Split('_')[0])))
                {
                    try
                    {
                        Log(ref Logs, entity, null, "Delete");
                    }
                    catch
                    {

                    }
                }

                int SavedEntities = 0;
                SavedEntities = base.SaveChanges();
                Count = 0;
                foreach (var entity in Added.Where(x => !NoLogTable.Contains(x.GetType().Name)))
                {
                    try
                    {
                        using (DBContext db = new DBContext())
                        {
                            string Name = entity.GetType().Name;
                            db.Configuration.ProxyCreationEnabled = false;
                            db.Configuration.LazyLoadingEnabled = true;

                            var metadata = ((IObjectContextAdapter)this).ObjectContext.MetadataWorkspace;

                            // Get the mapping between CLR types and metadata OSpace
                            var objectItemCollection = ((ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace));

                            // Get metadata for given CLR type
                            var entityMetadata = metadata
                                    .GetItems<EntityType>(DataSpace.OSpace)
                                    .FirstOrDefault(e => objectItemCollection.GetClrType(e) == entity.GetType());
                            if (entityMetadata == null)
                            {
                                entityMetadata = metadata
                                   .GetItems<EntityType>(DataSpace.OSpace)
                                   .FirstOrDefault(e => objectItemCollection.GetClrType(e).Name == entity.GetType().Name.Split('_')[0]);
                            }
                            var PrimaryKeyName = entityMetadata.KeyProperties.Select(p => p.Name).ToArray()[0];
                            this.Configuration.ProxyCreationEnabled = false;
                            int PrimaryKeyValue = Convert.ToInt32(entity.GetType().GetProperty(PrimaryKeyName).GetValue(entity));
                            var ThisOldEntity = db.Set(entity.GetType()).Find(PrimaryKeyValue);
                            Log(ref Logs, ThisOldEntity, null, "Added");

                        }
                    }
                    catch
                    {

                    }

                }

                if (Logs.Count > 0)
                {
                    // using (HRPEntities db = new HRPEntities())
                    {
                        using (LogContext LDb = new LogContext())
                        {
                            LDb.Logs.AddRange(Logs);
                            LDb.SaveChanges();
                        }
                        //base.SaveChanges();

                        // db.SaveChanges();
                    }
                }
                return SavedEntities;
            }

        }

        private void Log(ref List<Log> Logs, object OldEntity, object NewEntity, string Action)
        {
            var Name = OldEntity.GetType().Name;


            //  OldEntity.GetType()
            //if (Action == "Delete" || Action == "Edit")
            //{
            //    Name = Name.Split('_')[0];
            //}
            try
            {
                var NewValue = Newtonsoft.Json.JsonConvert.SerializeObject(NewEntity, new MyJsonConverter()).Replace(" ", "");
                var OldValue = Newtonsoft.Json.JsonConvert.SerializeObject(OldEntity, new MyJsonConverter()).Replace(" ", "");
                Logs.Add(new Log
                {
                    Entity_name = Name,
                    Action = Action,
                    Old_value = OldValue,
                    New_value = NewValue,
                    Creation_date = DateTime.Now,
                    User_id = User()
                });
            }
            catch
            {

            }


        }

        string GetMacAddress()
        {
            var macAddr = NetworkInterface
                        .GetAllNetworkInterfaces()
                        .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        .Select(nic => nic.GetPhysicalAddress().ToString())
                        .FirstOrDefault();
            return macAddr.ToString();
        }
        string User()
        {
            try
            {
                HttpCookie LoginCookie = HttpContext.Current.Request.Cookies["Login"];

                if (LoginCookie["UserId"] != null)
                {
                    string MyUser = LoginCookie["UserId"];
                    return MyUser;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }

        object placeHolderVariable;


    }


    public class MyJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            Newtonsoft.Json.Linq.JObject jo = new Newtonsoft.Json.Linq.JObject();

            foreach (System.Reflection.PropertyInfo prop in value.GetType().GetProperties())
            {
                if (prop.CanRead)
                {
                    if (prop.ToString().StartsWith("FabulousErp.Payable")
                        || prop.ToString().StartsWith("FabulousErp.Receivable")
                        || prop.ToString().StartsWith("FabulousErp")
                        || prop.ToString().StartsWith("FabulousDB")
                        || prop.GetMethod.ReturnParameter.ParameterType.Name.StartsWith("ICollection"))
                        continue;


                    object propValue = prop.GetValue(value);

                    if (propValue != null)
                    {
                        jo.Add(prop.Name, Newtonsoft.Json.Linq.JToken.FromObject(propValue));
                    }
                }
            }
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(objectType);
        }
    }

}
