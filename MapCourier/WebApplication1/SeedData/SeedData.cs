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
                /*if (context.Order.Any() && context.Storage.Any() && context.User.Any())
                {
                    return;
                }*/

                context.Order.AddRange(
                    new Order
                    {   address = "Test",
                        Longitude = "1",
                        Latitude = "1"
                    },
                    new Order
                    {   address = "Test",
                        Longitude = "1",
                        Latitude = "1"
                    },
                    new Order
                    {   address = "Test",
                        Longitude = "1",
                        Latitude = "1"
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
                context.User.AddRange(
                    new User
                    {
                        Login = "test",
                        Password = "test"
                    },
                    new User
                    {
                        Login = "test",
                        Password = "test"
                    },
                    new User
                    {
                        Login = "test",
                        Password = "test"
                    }
                    );
                
                context.SaveChanges();
            }
        }
    }
}
