using MapCourier.Models;
using MapCourier.Data;
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
            foreach (var c in ClientMarks)
            {
                Mark ClientMark = new(c);
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
                if (distance <= mark.NearMarkDist)
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
            foreach (var m in mark.NearMarks)
            {
                FindPaths(m);
            }
            if (mark.NearMarks.Count == 0 && mark.PastMark.Count > 0)
            {
                Result.Add(new(mark));
            }
        }
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
                    if(o.status != "waiting")
                    {
                        continue;
                    }
                    Mark clientMark = new Mark(o.Latitude, o.Longitude, o.OrderID, o.status);
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
                double distance = 0;
                foreach (var j in i)
                {
                    distance += j.PastMarkDist + j.NearStorageDist;
                }
                while (keyValuePairs.ContainsKey(distance))
                {
                    distance += 0.1e-15;
                }
                keyValuePairs.Add(distance, i);
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
                //if (!db.Storage.Any())
                //    new NullReferenceException();
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
                var bestPath = pathFinder.GetBestPaths(allPaths, 1)[0];
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
