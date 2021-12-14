using MapCourier.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapCourier
{
    public class Program
    {
        

        public static void Main(string[] args)
        {
            using (MapContext db = new MapContext())
            {
                // создаем два объекта User
                Order initOrder = new Order { 
                    id =1, 
                    address="1", 
                    addressCoordinateLatitude="1", 
                    addressCoordinateLongitude="1", 
                    orderNumder=1 };
                Storage initStorage = new Storage {
                    id = 1, 
                    storageName="1", 
                    coordinateLatitude="1", 
                    coordinateLongitude="1", 
                    storageAddress="1" };

                // добавляем их в бд
               // db.Orders.Add(initOrder);
               // db.Storages.Add(initStorage);
               // db.SaveChanges();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseWebRoot("Views");
                });
    }
}
