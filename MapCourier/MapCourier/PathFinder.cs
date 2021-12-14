using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapCourier
{
    class Mark : IComparable, IComparer<List<Mark>>
    {
        public readonly string X;
        public readonly string Y;
        public int ID;
        public List<Mark> PastMark = new List<Mark>();
        public double NearStorageDist;
        public double PastMarkDist;
        public List<Mark> NearMarks = new List<Mark>();
        public double NearMarkDist;
        public int IterationCount = 0;
        public readonly char Status = 'n'; // 'b' - busy, 'f' - free, 'n' - not needed
        public Mark(string x, string y)
        {
            X = x;
            Y = y;
        }
        public Mark()
        {

        }
        public Mark(Mark mark)
        {
            PastMarkDist = mark.PastMarkDist;
            ID = mark.ID;
            X = mark.X;
            Y = mark.Y;
            PastMark = new List<Mark>(mark.PastMark);
            NearStorageDist = mark.NearStorageDist;
            NearMarks = new List<Mark>(mark.NearMarks);
            NearMarkDist = mark.NearMarkDist;
            IterationCount = mark.IterationCount;
            Status = mark.Status;
        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            try
            {
                var mark = (Mark)obj;
            }
            catch
            {
                throw new ArgumentException("Object is not a Temperature");
            }
            Mark otherMark = obj as Mark;
            if (otherMark != null)
            {
                if (this.ID == otherMark.ID)
                    return 0;
                if (this.ID < otherMark.ID)
                    return 1;
                if (this.ID == otherMark.ID)
                    return -1;
            }
            return 1;
        }


        public int Compare(List<Mark> x, List<Mark> y)
        {
            var a = 0;
            if (x[x.Count - 1].ID == y[y.Count - 1].ID)
            {
                if (x.Count < y.Count)
                    a--;
                if (x.Count > y.Count)
                    a++;
            }
            if (x[x.Count - 1].ID + a < y[y.Count - 1].ID)
                return 1;
            if (x[x.Count - 1].ID + a > y[y.Count - 1].ID)
                return -1;
            else
                return 0;
        }
    }
    class MarksNDistance : IComparer
    {
        public readonly Mark Mark1;
        public readonly Mark Mark2;
        public readonly double Distance;
        public MarksNDistance(Mark mark1, Mark mark2)
        { 
            var 
            Mark1 = mark1;
            Mark2 = mark2;
            Distance = Convert.ToDouble(DistanceFinder.GetDistance(mark1.X, mark1.Y, mark2.X, mark2.Y));
        }

        public int Compare(object x, object y)
        {
            MarksNDistance d1 = (MarksNDistance)x;
            MarksNDistance d2 = (MarksNDistance)y;
            if (d1.Distance < d2.Distance)
                return 1;
            if (d1.Distance > d2.Distance)
                return -1;
            else
                return 0;
        }
    }
    class PathFinder
    {
        private static double Spread = 100;
        public static List<Mark> Result = new List<Mark>();
        public static void FindPath(Mark mark)
        {
            if (mark.PastMark.Count > mark.IterationCount)
            {
                return;
            }
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
                if (mark.PastMark.Count > 1 && mark.PastMark[mark.PastMark.Count - 1].NearMarkDist > distance + Spread)
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
                    if (mark.NearMarkDist - distance < Spread)
                    {
                        for (var j = 0; j < mark.NearMarks.Count; j++)
                        {
                            if (mark.NearMarks[j].PastMarkDist > distance + Spread)
                                mark.NearMarks.Remove(mark.NearMarks[j]);
                        }
                    }
                    ClientMark.PastMarkDist = distance;
                    ClientMark.PastMark = new List<Mark>(mark.PastMark);
                    ClientMark.PastMark.Add(new(mark));
                    ClientMark.IterationCount = mark.IterationCount + 1;
                    mark.NearMarks.Add(ClientMark);
                    mark.NearMarkDist = distance;
                }
            }
            for (var i = 0; i < mark.NearMarks.Count; i++)
            {
                FindPath(mark.NearMarks[i]);
            }
            if (mark.NearMarks.Count == 0 && mark.PastMark.Count > 0)
            {
                Result.Add(new(mark));
            }
        }

        public static List<List<Mark>> GetAllPath(List<Mark> marks)
        {
            List<List<Mark>> result = new List<List<Mark>>();
            for (var i = 0; i < marks.Count; i++)
            {
                var m = marks[i];
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
                resultMarks.Add(nearStorage);
                result.Add(resultMarks);
            }
            return result;
        }

        public static List<List<Mark>> GetBestPaths(List<List<Mark>> marks, int count)
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
}
