using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MapCourier.Data;

public class MapContext : IdentityDbContext
{
    public MapContext(DbContextOptions<MapContext> options)
        : base(options)
    {
    }

    public MapContext()
        {

        }

        public DbSet<MapCourier.Models.Order> Order { get; set; }

        public DbSet<MapCourier.Models.Storage> Storage { get; set; }

        public DbSet<MapCourier.Models.Delivery> Delivery { get; set; }
        public DbSet<MapCourier.Models.DeliveryLog> DeliveryLog { get; set; }
}
