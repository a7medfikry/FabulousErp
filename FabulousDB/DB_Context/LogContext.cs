using FabulousDB.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Context
{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string Old_value { get; set; }
        public string New_value { get; set; }
        [MaxLength(200)]
        public string Entity_name { get; set; }
        [MaxLength(200)]
        public string User_id { get; set; }
        [MaxLength(200)]
        public string Action { get; set; }
        
        public DateTime Creation_date { get; set; } = DateTime.Now;
    }
    public class LogContext : DbContext
    {
        public LogContext() : base("LogContext")
        {
            Database.SetInitializer<LogContext>(new CreateDatabaseIfNotExists<LogContext>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<LogContext, LogConfiguration>());
        }
        public DbSet<Log> Logs { get; set; }
    }
}
