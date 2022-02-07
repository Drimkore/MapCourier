using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Mark : IComparer<List<Mark>>
    {
        public readonly string? X;
        public readonly string? Y;
        public readonly int? ID;
        public List<Mark> PastMark = new List<Mark>();
        public double NearStorageDist;
        public double PastMarkDist;
        public List<Mark> NearMarks = new List<Mark>();
        public double NearMarkDist;
      //  public readonly char Status = 'n'; // 'b' - busy, 'f' - free, 'n' - not needed
        public Mark(string x, string y, int id)
        {
            X = x;
            Y = y;
            ID = id;
        }

        public Mark() { }
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
            //Status = mark.Status;
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
}
