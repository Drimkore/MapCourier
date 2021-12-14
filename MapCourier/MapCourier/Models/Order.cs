using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MapCourier.Models
{
    public class Order
    {
        public int id { get; set; }
        public int orderNumder { get; set; }
        //public string orderName { get; set; }
        public string address { get; set; }
        public string addressCoordinateLongitude { get; set; }
        public string addressCoordinateLatitude { get; set; }
        //public DateTime timeOfMakingOrder { get; set; }

    }
}
