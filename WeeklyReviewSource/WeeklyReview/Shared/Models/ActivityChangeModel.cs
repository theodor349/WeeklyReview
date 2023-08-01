namespace WeeklyReview.Shared.Models
{
    public class ActivityChangeModel
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public DateTime ChangeDate { get; set; }

        public ActivityChangeModel(int id, int fromId, int toId, DateTime changeDate)
        {
            Id = id;
            FromId = fromId;
            ToId = toId;
            ChangeDate = changeDate;
        }
    }
}
