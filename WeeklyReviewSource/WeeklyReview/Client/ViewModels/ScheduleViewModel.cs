namespace WeeklyReview.Client.ViewModels
{
    public class ScheduleViewModel
    {
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
