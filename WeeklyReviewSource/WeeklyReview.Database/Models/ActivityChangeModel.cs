namespace WeeklyReview.Database.Models
{
    public class ActivityChangeModel
    {
        public int Id { get; set; }
        public ActivityModel Source { get; set; }
        public ActivityModel Destination { get; set; }
        public DateTime ChangeDate { get; set; }

        private ActivityChangeModel()
        {
            
        }

        public ActivityChangeModel(ActivityModel source, ActivityModel destination, DateTime changeDate)
        {
            Source = source;
            Destination = destination;
            ChangeDate = changeDate;
        }
    }
}
