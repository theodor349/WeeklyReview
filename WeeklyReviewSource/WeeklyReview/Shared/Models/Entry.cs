namespace WeeklyReview.Shared.Models
{
    public class Entry : IComparable<Entry>
    {
        private static int _entryIndex = 1;
        private static int _nextIndex => _entryIndex++;

        public int Id { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Entered { get; set; }
        public List<Activity> Activities { get; set; } = new List<Activity>();

        public Entry()
        {
            Id = _nextIndex;
        }

        public int CompareTo(Entry? other)
        {
            return StarTime.CompareTo(other.StarTime);
        }
    }
}
