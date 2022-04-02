using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MapCourier.Data;

namespace MapCourier.SeedData

{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MapContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MapContext>>()))
            {
                if (context.Roles.Any())
                {
                    return;
                }
                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Name = "Admin"
                    },
                    new IdentityRole
                    {
                        Name = "Operator"
                    },
                    new IdentityRole
                    {
                        Name = "Courier"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
