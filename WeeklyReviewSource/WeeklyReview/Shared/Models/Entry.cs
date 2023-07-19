namespace WeeklyReview.Shared.Models
{
    public class Entry : IComparable<Entry>
    {
        public int Id { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Entered { get; set; }
        public List<Activity> Activities { get; set; } = new List<Activity>();

        public int CompareTo(Entry? other)
        {
            return StarTime.CompareTo(other.StarTime);
        }
    }
}
