#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class MapContext : DbContext
    {
        public MapContext (DbContextOptions<MapContext> options)
            : base(options)
        {
        }
        public MapContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MapContext-f6e8ab39-44a6-4771-a619-ba60e2669d6a");
            }
        }
        public DbSet<WebApplication1.Models.Order> Order { get; set; }

        public DbSet<WebApplication1.Models.Storage> Storage { get; set; }
    }
}
