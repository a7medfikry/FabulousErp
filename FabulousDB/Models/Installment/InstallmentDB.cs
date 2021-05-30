using FabulousDB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulousDB.DB_Context
{
    public partial class DBContext:DbContext
    {
        public DbSet<Installment_contract_invoice> Installment_contract_invoice { get; set; }
        public DbSet<Purchase_Installment_contract_invoice> Purchase_Installment_contract_invoice { get; set; }
    }
}
