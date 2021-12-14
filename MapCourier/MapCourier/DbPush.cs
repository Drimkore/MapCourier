using MapCourier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapCourier
{
    public class DbPush
    {
        public static void takeDataStorage(string coordinateX, string coordinateY, string address)
        {
            using (MapContext db = new MapContext())
            {
                Storage dataStorage = new Storage
                {
                    id = 1,
                    storageName = "1", // пока тут будет такое значение
                    coordinateLatitude = coordinateX,
                    coordinateLongitude = coordinateY,
                    storageAddress = address
                };
                db.Storages.Add(dataStorage);
                db.SaveChanges();
            }
        }
        public static void takeDataOrder(string coordinateX, string coordinateY, string address)
        {
            using (MapContext db = new MapContext())
            {
                Order dataOrder = new Order
                {
                    id = 1,
                    address = address,
                    addressCoordinateLatitude = coordinateX,
                    addressCoordinateLongitude = coordinateY,
                    orderNumder = 1 // тоже не меняем, ибо нет смысла пока что.
                };
                db.Orders.Add(dataOrder);
                db.SaveChanges();
            }
        }
    }
}
