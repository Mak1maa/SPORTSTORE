using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportStore.Domain.Entities;
using System.Data.Entity;
using System.Data.SqlClient;

namespace SportStore.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public DbSet<Products> Products { get; set; }
    }
}