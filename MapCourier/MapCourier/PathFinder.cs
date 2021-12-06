using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapCourier
{
    public class PathFinder
    {
        List<Mark> ClientMarks = new List<Mark>();
        List<Mark> StorageMarks = new List<Mark>();
        private static List<Mark> GetTestMarks(int count)
        {
            var test = new List<Mark>();
            Random random = new Random();
            for(var i = 0; i < count; i++)
            {
                random.Next(1, 100);
                var x = random.ToString();
                random.Next(1, 100);
                var y = random.ToString();
                test.Add(new Mark(x, y));
            }
            return test;
        }
        public List<Mark> FindPath(double maxRad)
        {
            var starMark = new Mark("50", "50");
            double x = Convert.ToDouble(starMark.X);
            double y = Convert.ToDouble(starMark.Y);
            var path = new List<Mark>();
            var nearMarks = new List<Mark>();

            for (double r = 1; r < maxRad && nearMarks.Count <= 3; r++)
            {
                nearMarks.Clear();
                foreach (var i in ClientMarks)
                {
                    var markX = Convert.ToDouble(i.X);
                    var markY = Convert.ToDouble(i.Y);
                    var lengthX = markX - x;
                    var lengthY = markY - y;
                    if (Math.Sqrt(lengthX * lengthX + lengthY * lengthY) <= r)
                        nearMarks.Add(i);
                }
            }
            foreach (var i in nearMarks)
            {
                var markX = Convert.ToDouble(i.X);
                var markY = Convert.ToDouble(i.Y);
                var nearStorageDist = double.MaxValue;
                foreach (var s in StorageMarks)
                {
                    var storageX = Convert.ToDouble(s.X);
                    var storageY = Convert.ToDouble(s.Y);
                    var lengthX = markX - storageX;
                    var lengthY = markY - storageY;
                    var distance = Math.Sqrt(lengthX * lengthX + lengthY * lengthY);
                    if (distance < nearStorageDist)
                    {
                        nearStorageDist = distance;
                        i.NearStorage = s;
                    }
                }
                i.NearStorageDist = Convert.ToInt32(FindDistance.GetDistance(i.X, i.Y, i.NearStorage.X, i.NearStorage.Y));
            }

            //Нужна графы, нихачу графы, попозже графы
            return path;
        }
    }
}
