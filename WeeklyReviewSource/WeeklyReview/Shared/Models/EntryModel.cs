namespace WeeklyReview.Shared.Models
{
    public class EntryModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public List<ActivityModel> Activities { get; set; }
        public bool Deleted { get; set; }

        public EntryModel(int id, DateTime startTime, DateTime endTime, List<ActivityModel> activities, bool deleted)
        {
            Id = id;
            StartTime = startTime;
            EndTime = endTime;
            Activities = activities;
            Deleted = deleted;
        }
    }
}
