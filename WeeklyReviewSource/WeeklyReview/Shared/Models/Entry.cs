namespace WeeklyReview.Shared.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Entered { get; set; }
        public List<Activity> Activities { get; set; } = new List<Activity>();
    }
}
