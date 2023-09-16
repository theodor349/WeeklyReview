namespace WeeklyReview.Database.Models
{
    public class EntryModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime RecordedTime{ get; set; }
        public TimeSpan Duration => EndTime.HasValue ? EndTime.Value - StartTime : TimeSpan.Zero;
        public List<ActivityModel> Activities { get; set; }
        public bool Deleted { get; set; }
        public Guid UserGuid { get; set; }

        public EntryModel()
        {
            
        }

        public EntryModel(DateTime startTime, DateTime? endTime, DateTime recordedTime, List<ActivityModel> activities, bool deleted, Guid userGuid)
        {
            StartTime = startTime;
            EndTime = endTime;
            RecordedTime = recordedTime;
            Activities = activities;
            Deleted = deleted;
            UserGuid = userGuid;
        }

        public EntryModel(DateTime startTime, DateTime? endTime, DateTime recordedTime, ActivityModel activity, bool deleted, Guid userGuid)
        {
            StartTime = startTime;
            EndTime = endTime;
            RecordedTime = recordedTime;
            Activities = new() { activity };
            Deleted = deleted;
            UserGuid = userGuid;
        }
    }
}
