using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MapCourier.Data;
using System;
using System.Linq;
using MapCourier.Models;

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
                if (context.Order.Any() && context.Storage.Any())
                {
                    return;
                }

                context.Order.AddRange(
                    new Order
                    {   address = "Test",
                        Longitude = "1",
                        Latitude = "1",
                        status = "yes"
                    },
                    new Order
                    {   address = "Test",
                        Longitude = "1",
                        Latitude = "1",
                        status = "no"
                    },
                    new Order
                    {   address = "Test",
                        Longitude = "1",
                        Latitude = "1",
                        status = "yes"
                    });
                context.Storage.AddRange(
                    new Storage
                    {
                        storageAddress = "test",
                        storageName = "testName",
                        Latitude = "1",
                        Longitude = "1"
                    },
                    new Storage
                    {
                        storageAddress = "test",
                        storageName = "testName",
                        Latitude = "1",
                        Longitude = "1"
                    },
                    new Storage
                    {
                        storageAddress = "test",
                        storageName = "testName",
                        Latitude = "1",
                        Longitude = "1"
                    }
                    );               
                
                context.SaveChanges();
            }
        }
    }
}
