using FabulousDB.DB_Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FabulousErp
{
    public sealed class MyDbContext
    {
        private static readonly Lazy<DBContext> lazy =
      new Lazy<DBContext>(() => new DBContext());

        public static DBContext Instance { get { return lazy.Value; } }

        private MyDbContext()
        {
        }

    }

}