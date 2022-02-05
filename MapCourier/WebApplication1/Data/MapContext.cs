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

        public DbSet<WebApplication1.Models.Order> Order { get; set; }

        public DbSet<WebApplication1.Models.Storage> Storage { get; set; }
    }
}
