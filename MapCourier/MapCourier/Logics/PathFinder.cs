using MapCourier.Models;
using MapCourier.Data;
using Microsoft.EntityFrameworkCore;
namespace MapCourier.Controllers
{
    class PathFinder
    {
        private double Spread = 0.03; // разброс в градусах параллелей. 1 = 111 километров => 0.01 = 1.11 километр, далее считать самому
        private List<Mark> Result = new List<Mark>();
        public List<Mark> ClientMarks = new List<Mark>();
        public List<Mark> StorageMarks = new List<Mark>();
        private readonly DateTime PresentTime = /*DateTime.Now;*/ new DateTime(2022, 06, 23, 12, 0, 0);
        private readonly TimeSpan MaxPathTime = new TimeSpan(2, 0, 0);
        private readonly int MaxOrdersCount = 3;

        public void GetData()
        {
            ClientMarks = new List<Mark>();
            StorageMarks = new List<Mark>();
            using (var db = new MapContext())
            {
                var orders = db.Order.Where(o => o.TimeFrameEnding.Date == PresentTime.Date && o.status == "waiting"); //<-не работает как задуманно, жаль, разбраться в чём проблема не было возможности.
                foreach (var o in orders)
                {
                    if(o.TimeFrameEnding.Subtract(PresentTime) > MaxPathTime)
                        continue;
                    if (o.address == null)
                        continue;
                    Mark clientMark = new Mark(o.Latitude, o.Longitude, o.OrderID, o.status, o.TimeFrameBeginning, o.TimeFrameEnding);
                    ClientMarks.Add(clientMark);
                }
                foreach (var s in db.Storage)
                {
                    if (s.storageAddress == null)
                        continue;
                    Mark storageMark = new Mark(s.Latitude, s.Longitude, s.StorageID);
                    StorageMarks.Add(storageMark);
                }
            }
        }
        public void FindPaths(Mark mark)
        {
            if (mark.PastMark.Count > MaxOrdersCount - 1)
            {
                Result.Add(new Mark(mark));
                return;
            }            
            mark.NearMarkDist = double.MaxValue;
            foreach (var cm in ClientMarks)
            {
                Mark ClientMark = new(cm);
                if (mark.ID == ClientMark.ID)
                    continue;
                double distance = DistanceFinder.GetMapsDistance(mark, ClientMark);
                bool flag = false;
                foreach (var m in mark.PastMark)
                {
                    if (m.ID == ClientMark.ID)
                        flag = true;
                }
                if (flag)
                    continue;
                if (distance <= mark.NearMarkDist + Spread)
                {
                    if (mark.NearMarkDist - distance > Spread)
                    {
                        for (var i = 0; i < mark.NearMarks.Count; i++)
                        {
                            if (mark.NearMarks[i].PastMarkDist > distance + Spread)
                            {
                                mark.NearMarks.Remove(mark.NearMarks[i]);
                                i--;
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
            foreach (var nm in mark.NearMarks)
            {
                FindPaths(nm);
            }
            //if (mark.NearMarks.Count == 0 && mark.PastMark.Count > 0)
            //{
            //    Result.Add(new(mark));
            //}
            Result.Add(new(mark));
        }
        public List<List<Mark>> GetAllPaths()
        {
            List<List<Mark>> result = new List<List<Mark>>();
            for (var i = 0; i < Result.Count; i++)
            {
                var mark = Result[i];
                List<Mark> resultMarks = new List<Mark>(mark.PastMark);
                resultMarks.Add(mark);
                mark.NearStorageDist = double.MaxValue;
                Mark nearStorage = null;
                foreach (var s in StorageMarks)
                {
                    var distance = DistanceFinder.GetMapsDistance(mark, s);
                    if (distance <= mark.NearStorageDist)
                    {
                        nearStorage = s;
                        mark.NearStorageDist = distance;
                    }
                }
                if (nearStorage != null)
                    resultMarks.Add(nearStorage);
                if (resultMarks.Count < MaxOrdersCount)
                    continue;
                result.Add(resultMarks);
            }
            return result;
        }

        public List<Mark> GetBestPath(List<List<Mark>> paths)
        {
            if (paths.Count == 0)
                return null;
            SortedDictionary<TimeSpan, List<Mark>> keyValuePairs = new SortedDictionary<TimeSpan, List<Mark>>();
            foreach (var p in paths)
            {
                //double allDistance = 0;
                TimeSpan allTime = new TimeSpan();
                bool timeFlag = false;

                for (var i = 1; i < p.Count; i++)
                {
                    var pathData = DistanceFinder.GetPathData(p[i - 1], p[i]);
                    allTime += pathData.Time;
                    DateTime estimatedTime = PresentTime + allTime;
                    if(p[i].Status == "storage")
                    {
                        continue;
                    }
                    if (estimatedTime > p[i].TimeFrameEnding || estimatedTime < p[i].TimeFrameBeginning - new TimeSpan(0,15,0))
                    {
                        timeFlag = true;
                        break;
                    }              
               
                }
                //foreach (var mark in p)
                //{
                //    distance += mark.PastMarkDist + mark.NearStorageDist;
                //    TimeSpan timeToDelivery = DistanceFinder.GetDeliveryTime(mark.PastMarkDist);
                //    TimeSpan timeToStorage = new TimeSpan();
                //    if(mark.NearStorageDist != 0)
                //    {
                //        timeToStorage = DistanceFinder.GetDeliveryTime(mark.NearStorageDist);
                //    }
                //    allTime += timeToDelivery + timeToStorage;
                //    DateTime estimatedTime = PresentTime + allTime;
                //    if (mark.Status != "storage" || estimatedTime > mark.TimeFrameEnding || estimatedTime < mark.TimeFrameBeginning)
                //    {
                //        timeFlag = true;
                //        break;
                //    }
                //}
                if (timeFlag)
                    continue;
                if (allTime > MaxPathTime)
                    continue;
                while (keyValuePairs.ContainsKey(allTime))
                {
                    allTime += new TimeSpan(0,0,1);
                }
                keyValuePairs.Add(allTime, p);
            }
            int maxMarksCount = 0;
            List<Mark> bestPath = null;
            foreach(var vp in keyValuePairs)
            {
                if(vp.Value.Count > maxMarksCount)
                {
                    maxMarksCount = vp.Value.Count;
                    bestPath = vp.Value;
                }
            }
            //for (var i = 0; i < keyValuePairs.Count; i++)
            //{
            //    var j = keyValuePairs.Count - 1 - i;
            //    result.Add(keyValuePairs.Last().Value);
            //}
            return bestPath;
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
                        start = new Mark(storage.Latitude, storage.Longitude, storage.StorageID);
                    }
                }
                pathFinder.FindPaths(start);
                var allPaths = pathFinder.GetAllPaths();
                if (allPaths.Count == 0)
                    return null;
                var bestPath = pathFinder.GetBestPath(allPaths);
                if (bestPath == null)
                    return null;
                foreach (var m in bestPath)
                {
                    if(m.Status == "waiting")
                    {
                        m.Status = "acceptance";
                        var order = db.Order.FirstOrDefault(o => o.OrderID == m.ID);
                        order.status = "acceptance";
                    }
                    db.SaveChanges();
                }
                return bestPath;
            }
        }
    }
}
