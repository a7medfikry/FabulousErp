using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FabulousDB.DB_Tabels.Tax;
using FabulousDB.Models;
//using FabulousDB.Migrations;

namespace FabulousDB.DB_Context
{
    public partial class DBContext : DbContext
    {
        public DbSet<Unit_of_measure> Unit_of_measures { get; set; }
        public DbSet<Tax> Taxs { get; set; }

    }
}
