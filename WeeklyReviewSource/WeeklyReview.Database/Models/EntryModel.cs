namespace WeeklyReview.Database.Models
{
    public class EntryModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime RecordedTime{ get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public List<ActivityModel> Activities { get; set; }
        public bool Deleted { get; set; }

        private EntryModel()
        {
            
        }

        public EntryModel(DateTime startTime, DateTime endTime, DateTime recordedTime, List<ActivityModel> activities, bool deleted)
        {
            StartTime = startTime;
            EndTime = endTime;
            RecordedTime = recordedTime;
            Activities = activities;
            Deleted = deleted;
        }

        public EntryModel(DateTime startTime, DateTime endTime, DateTime recordedTime, ActivityModel activity, bool deleted)
        {
            StartTime = startTime;
            EndTime = endTime;
            RecordedTime = recordedTime;
            Activities = new() { activity };
            Deleted = deleted;
        }
    }
}
