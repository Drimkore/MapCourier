namespace MapCourier.Models
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
        public DateTime TimeFrameBeginning;
        public DateTime TimeFrameEnding;
        public string? Status; //"waiting"/"busy"/"finished"
        public Mark(string x, string y, int id, string status, DateTime beginning, DateTime ending)
        {
            X = x;
            Y = y;
            ID = id;
            Status = status;
            TimeFrameBeginning = beginning;
            TimeFrameEnding = ending;
        }
        public Mark(string x, string y, int id)
        {
            X = x;
            Y = y;
            ID = id;
            Status = "storage";
        }

        public Mark() { }
        public Mark(Mark mark)
        {
            PastMarkDist = mark.PastMarkDist;
            ID = mark.ID;
            X = mark.X;
            Y = mark.Y;
            TimeFrameBeginning = mark.TimeFrameBeginning;
            TimeFrameEnding = mark.TimeFrameEnding;
            PastMark = new List<Mark>(mark.PastMark);
            NearStorageDist = mark.NearStorageDist;
            NearMarks = new List<Mark>(mark.NearMarks);
            NearMarkDist = mark.NearMarkDist;
            Status = mark.Status;
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
    public class OrdersMarks
    {
        public List<Mark>? Marks { get; set; }
        private int Iteration { get; set; }
        public OrdersMarks(List<Mark> marks)
        {
            Marks = marks;
            Iteration = 0;
        }
        public void Next()
        {
            Iteration++;
        }
        public Mark GetMark()
        {
            return Marks[Iteration];
        }
    }
}
