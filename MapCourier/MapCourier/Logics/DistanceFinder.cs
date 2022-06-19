using MapCourier.Models;
using System.Net;
using Newtonsoft.Json;
namespace MapCourier.Controllers
{
    class DistanceFinder
    {
        /*public static string GetDistance(string x1, string y1, string x2, string y2)//Возвращает дистанцию по координатам 2-х точек.
        {
            var url = "https://catalog.api.2gis.com/carrouting/6.0.0/global?key=rurbbn3446";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            string result;
            httpRequest.ContentType = "application/json";

            var data = @"{
   ""points"": [
       {
           ""type"": ""pedo"",
           ""x"": " + x1 + @",
           ""y"": " + y1 + @"
       },
       {
           ""type"": ""pedo"",
           ""x"": " + x2 + @",
           ""y"": " + y2 + @"
       }
   ]
}";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            var parce = @"""total_distance"":";
            result = result.Remove(0, result.IndexOf(parce) + parce.Length);
            result = result.Remove(result.IndexOf(','));
            return result;
        }*/
        public class PathData
        {
            public double Distance { get; set; }
            public TimeSpan Time;
            public PathData(string distance, string time)
            {
                Distance = double.Parse(distance.Replace('.', ','));
                Time = new TimeSpan(0, 0, (int)double.Parse(time.Replace('.', ',')));
            }
        }
        public class Response
        {
            public Routes[] routes { get; set; }
        }
        public class Routes
        {
            public string duration { get; set; }
            public string distance { get; set; }
        }
        public static PathData GetPathData(Mark start, Mark finish)
        {
            string url = "https://api.mapbox.com/directions/v5/mapbox/driving/" + start.X + "," + start.Y + ";" + finish.X + "," + finish.Y +"?&access_token=pk.eyJ1Ijoic3ZlbmFuIiwiYSI6ImNrems0ZnJxeDNnY2EydW8xcDg0cTZrbDQifQ.eBbv3xNxhyEG65wGR8cRSA";
            var parsed = JsonConvert.DeserializeObject<Response>(new WebClient().DownloadString(url));
            var route = parsed.routes[0];
            var result = new PathData(route.distance, route.duration);
            return result;
        }
        public static TimeSpan GetDeliveryTime(double distance)
        {
            distance *= 111;
            var hours = distance / 6;
            int minutes = (Int32)(60 * (distance % 1));
            var time = new TimeSpan((int)hours, minutes, 0);
            return time;
        }
        public static double GetMapsDistance(Mark mark1, Mark mark2)
        {
            return GetMapsDistance(mark1.X, mark1.Y, mark2.X, mark2.Y);
        }
        public static double GetMapsDistance(string x1, string y1, string x2, string y2)
        {
            var mark1X = Convert.ToDouble(x1.Replace('.',','));
            var mark1Y = Convert.ToDouble(y1.Replace('.',','));
            var mark2X = Convert.ToDouble(x2.Replace('.',','));
            var mark2Y = Convert.ToDouble(y2.Replace('.',','));
            var lengthX = mark1X - mark2X;
            var lengthY = mark1Y - mark2Y;
            return Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
        }
    }
}

