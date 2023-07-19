namespace WeeklyReview.Shared.Models
{
    public class Activity
    {
        private static int _ActivityIndex = 1;
        private static int _nextIndex => _ActivityIndex++;

        public Activity()
        {

        }

        public Activity(string name, Category category)
        {
            Id = _nextIndex;
            Name = category.Name + ": " + name;
            Category = category;
            LastNameEdit = DateTime.Now;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastNameEdit { get; set; }
        public Category Category { get; set; }
    }

}
