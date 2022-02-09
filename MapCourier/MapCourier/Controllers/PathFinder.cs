using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapCourier.Models;
using MapCourier.Data;
using Microsoft.EntityFrameworkCore;
namespace MapCourier.Controllers
{
    class PathFinder
    {
        private double Spread = 0.01; // разброс в градусах параллелей. 1 = 111 километров => 0.01 = 1.11 километр, далее считать самому
        private List<Mark> Result = new List<Mark>();
        public List<Mark> ClientMarks = new List<Mark>();
        public List<Mark> StorageMarks = new List<Mark>();

        public void FindPaths(Mark mark)
        {
            if (mark.PastMark.Count > 2)
            {
                Result.Add(new Mark(mark));
                return;
            }
            mark.NearMarkDist = double.MaxValue;
            foreach (var i in ClientMarks)
            {
                Mark ClientMark = new(i);
                double distance = DistanceFinder.GetMapsDistance(mark, ClientMark);
                if (distance == 0)
                    continue;
                bool flag = false;
                foreach (var j in mark.PastMark)
                {
                    if (j.ID == ClientMark.ID)
                        flag = true;
                }
                if (flag)
                    continue;
                if (distance <= mark.NearMarkDist)
                {
                    if (mark.NearMarkDist - distance > Spread)
                    {
                        for (var j = 0; j < mark.NearMarks.Count; j++)
                        {
                            if (mark.NearMarks[j].PastMarkDist > distance + Spread)
                            {
                                mark.NearMarks.Remove(mark.NearMarks[j]);
                                j--;
                            }
                        }
                    }
                    ClientMark.PastMarkDist = distance;
                    ClientMark.PastMark = new List<Mark>(mark.PastMark);
                    ClientMark.PastMark.Add(new(mark));
                    mark.NearMarks.Add(ClientMark);
                    mark.NearMarkDist = distance;
                }
            }
            for (var i = 0; i < mark.NearMarks.Count; i++)
            {
                FindPaths(mark.NearMarks[i]);
            }
            if (mark.NearMarks.Count == 0 && mark.PastMark.Count > 0)
            {
                Result.Add(new(mark));
            }
        }
        MapContext mapContext = new MapContext();
        public void GetData()
        {
            ClientMarks = new List<Mark>();
            StorageMarks = new List<Mark>();
            using (var db = new MapContext())
            {
                foreach (var o in db.Order)
                {
                    if (o.address == null)
                        continue;
                    if(o.delivered != "waiting")
                    {
                        continue;
                    }
                    Mark clientMark = new Mark(o.Latitude, o.Longitude, o.id, o.delivered);
                    ClientMarks.Add(clientMark);
                }
                foreach (var s in db.Storage)
                {
                    if (s.storageAddress == null)
                        continue;
                    Mark storageMark = new Mark(s.Latitude, s.Longitude, s.id);
                    StorageMarks.Add(storageMark);
                }
            }
        }
        public List<List<Mark>> GetAllPaths()
        {
            List<List<Mark>> result = new List<List<Mark>>();
            for (var i = 0; i < Result.Count; i++)
            {
                var m = Result[i];
                List<Mark> resultMarks = new List<Mark>(m.PastMark);
                resultMarks.Add(m);
                m.NearStorageDist = double.MaxValue;
                Mark nearStorage = null;
                foreach (var s in StorageMarks)
                {
                    var distance = DistanceFinder.GetMapsDistance(m, s);
                    if (distance <= m.NearStorageDist)
                    {
                        nearStorage = s;
                        m.NearStorageDist = distance;
                    }
                }
                if (nearStorage != null)
                    resultMarks.Add(nearStorage);
                result.Add(resultMarks);
            }
            return result;
        }

        public List<List<Mark>> GetBestPaths(List<List<Mark>> marks, int count)
        {
            marks.Sort(new Mark());
            List<List<Mark>> result = new List<List<Mark>>();
            SortedDictionary<double, List<Mark>> keyValuePairs = new SortedDictionary<double, List<Mark>>();
            foreach (var i in marks)
            {
                double c = 0;
                foreach (var j in i)
                {
                    c += j.PastMarkDist + j.NearStorageDist;
                }
                keyValuePairs.Add(c, i);
            }
            for (var i = 0; i < count; i++)
            {
                result.Add(keyValuePairs.ElementAt(i).Value);
            }
            return result;
        }
    }
    public class FinalResult
    {
        public static List<Mark> GetResultPath(string x, string y)
        {
            PathFinder pathFinder = new PathFinder();
            pathFinder.GetData();

            Mark start = new Mark();
            using (var db = new MapContext())
            {
                int minDist = int.MaxValue;
                foreach (var storage in db.Storage)
                {
                    var bufferDist = DistanceFinder.GetMapsDistance(x, y, storage.Latitude, storage.Longitude);
                    if (bufferDist < minDist)
                    {
                        start = new Mark(storage.Latitude, storage.Longitude, storage.id);
                    }
                }
                pathFinder.FindPaths(start);
                var allPaths = pathFinder.GetAllPaths();
                var bestPath = pathFinder.GetBestPaths(allPaths, 1)[0];
                foreach (var m in bestPath)
                {
                    if(m.Status == "waiting")
                    {
                        var order = db.Order.FirstOrDefault(o => o.id == m.ID);
                        order.delivered = "busy";
                    }
                    db.SaveChanges();
                }
                return bestPath;
            }
        }
    }
}
