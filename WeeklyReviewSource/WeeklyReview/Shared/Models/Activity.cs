namespace WeeklyReview.Shared.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastNameEdit { get; set; }
        public Category Category { get; set; }
    }

}
