using Syncfusion.Blazor.HeatMap.Internal;
using System.Drawing;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Client.Services
{
    public class WeeklyReviewService : IWeeklyReviewService
    {

        public List<Entry> Entries { get; } = new List<Entry>();
        public List<Activity> Activities { get; } = new List<Activity>();
        public List<Category> Categories { get; } = new List<Category>();

        public WeeklyReviewService()
        {
            GenerateData();
        }

        private void GenerateData()
        {
            GenerateCategories();
            GenerateActivities();
            //GenerateEntries();
        }

        public void AddEntry(DateTime date)
        {
            var r = new Random();
            var e = new Entry();
            e.StarTime = date;
            e.EndTime = date.AddHours(2);

            int rNum = (int)r.Next();
            int aIndex = rNum % Activities.Count();
            e.Activities.Add(Activities[aIndex]);
            Entries.Add(e);
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
                    var activityIndex = random.Next(Activities.Count());
                    e.Activities.Add(Activities.ElementAt(activityIndex));
                }
                Entries.Add(e);

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
                a.Category = Categories.ElementAt(new Random().Next(Categories.Count()));
                Activities.Add(a);
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
                Categories.Add(c);
            }
        }
    }
}
