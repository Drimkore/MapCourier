using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace MapCourier.Models
{
    public class DBContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Storage> Storages { get; set; } = null!;
        public DBContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=App_Data\\StoragesAndOrders.db");
        }

        

    }
}
