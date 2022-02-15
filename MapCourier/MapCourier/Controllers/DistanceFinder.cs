using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapCourier.Models;
using System.Net;

namespace MapCourier.Controllers
{
    class DistanceFinder
    {
        public static string GetDistance(string x1, string y1, string x2, string y2)//Возвращает дистанцию по координатам 2-х точек.
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
        }
        public static double GetMapsDistance(Mark mark1, Mark mark2)
        {
            var mark1X = Convert.ToDouble(mark1.X.Replace('.', ','));
            var mark1Y = Convert.ToDouble(mark1.Y.Replace('.', ','));
            var mark2X = Convert.ToDouble(mark2.X.Replace('.', ','));
            var mark2Y = Convert.ToDouble(mark2.Y.Replace('.', ','));
            var lengthX = mark1X - mark2X;
            var lengthY = mark1Y - mark2Y;
            return Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
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

