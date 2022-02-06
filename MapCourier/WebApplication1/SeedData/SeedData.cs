using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using System;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.SeedData

{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MapContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MapContext>>()))
            {
                //if (context.Order.Any() && context.Storage.Any())
                //    return;

                context.Order.AddRange(
                    new Order
                    {   address = "Test",
                        addressCoordinateLongitude = "1",
                        addressCoordinateLatitude = "1"
                    });
                context.Storage.AddRange(
                    new Storage
                    {
                        storageAddress = "test",
                        storageName = "testName",
                        coordinateLatitude = "1",
                        coordinateLongitude = "1"
                    });

                //context.SaveChanges();
            }
        }
    }
}

