using System.Drawing;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Client.Pages
{
    public partial class WeekView
    {
        public DateTime CurrentDate = DateTime.Now;
        public List<ScheduleViewModel> DataSource { get; set; } = new List<ScheduleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

        private List<Entry> _entries = new List<Entry>();
        private List<Activity> _activities = new List<Activity>();
        private List<Category> _categories = new List<Category>();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            GenerateData();
            GenerateViewModels();
        }

        private void GenerateViewModels()
        {
            foreach (var cat in _categories)
            {
                Categories.Add(new CategoryViewModel(cat));
            }

            foreach (var entry in _entries)
            {
                var s = new ScheduleViewModel();
                s.Subject = entry.Activities.ConvertAll(x => x.Name).Aggregate((x, y) => x + " + " + y);
                s.StartTime = entry.StarTime;
                s.EndTime = entry.EndTime;
                s.CategoryId = entry.Activities.ConvertAll(x => x.Category).MaxBy(x => x.Priority).Id;
                DataSource.Add(s);
            }
        }

        private void GenerateData()
        {
            GenerateCategories();
            GenerateActivities();
            GenerateEntries();
        }

        private void GenerateEntries()
        {
            var date = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
            date = new DateTime(date.Year, date.Month, date.Day);

            var random = new Random();
            for (int i = 1; i <= 100; i++)
            {
                int minutesToAdd = 15 * random.Next(1, 4 * 2);

                var e = new Entry();
                e.Id = i;
                e.StarTime = date;
                e.EndTime = date.AddMinutes(minutesToAdd);
                e.Entered = DateTime.Now;
                for (int a = 0; a < random.Next(1, 4); a++)
                {
                    var activityIndex = random.Next(_activities.Count());
                    e.Activities.Add(_activities.ElementAt(activityIndex));
                }
                _entries.Add(e);

                date = date.AddMinutes(minutesToAdd);
            }
        }

        private void GenerateActivities()
        {
            for (int i = 1; i <= 10; i++)
            {
                var a = new Activity();
                a.Id = i;
                a.Name = "Activity " + i;
                a.LastNameEdit = DateTime.Now;
                a.Category = _categories.ElementAt(new Random().Next(_categories.Count()));
                _activities.Add(a);
            }
        }

        private void GenerateCategories()
        {
            for (int i = 1; i <= 4; i++)
            {
                var c = new Category();
                c.Id = i;
                c.Name = "Category " + 1;
                c.Priority = i;
                c.Color = i switch
                {
                    1 => Color.Red,
                    2 => Color.Green,
                    3 => Color.Blue,
                    4 => Color.Magenta,
                    _ => throw new NotImplementedException(),
                };
                _categories.Add(c);
            }
        }
    }
}