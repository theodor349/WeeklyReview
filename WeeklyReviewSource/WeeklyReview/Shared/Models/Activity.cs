namespace WeeklyReview.Shared.Models
{
    public class Activity
    {
        private static int _ActivityIndex = 1;
        private static int _nextIndex => _ActivityIndex++;

        public Activity()
        {

        }

        public Activity(string name, Category category, bool includeCategory = true)
        {
            Id = _nextIndex;
            Category = category;
            LastNameEdit = DateTime.Now;

            if (includeCategory)
                Name = category.Name + ": " + name;
            else
                Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime LastNameEdit { get; set; }
        public Category Category { get; set; }
    }

}
