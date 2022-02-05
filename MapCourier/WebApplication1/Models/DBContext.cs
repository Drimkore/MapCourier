using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class DBContext
    {
        public DbSet<Orders> Orders { get; set; } = null!;
        public DbSet<Storages> Storages { get; set; } = null!;
        public string DbPath { get; }
        public DBContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "StoragesAndOrders.db");
        }        
    }
}
