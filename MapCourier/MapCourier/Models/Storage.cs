using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapCourier.Models
{
    public class Storage
    {
        public int id { get; set; }
        public string storageName { get; set; }
        public string storageAddress { get; set; }
        public string coordinateLongitude { get; set; }
        public string coordinateLatitude { get; set; }
    }
}
