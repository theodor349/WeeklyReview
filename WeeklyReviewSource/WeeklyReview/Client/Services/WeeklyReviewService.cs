using Syncfusion.Blazor.HeatMap.Internal;
using System.Drawing;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Shared.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WeeklyReview.Client.Services
{
    public class WeeklyReviewService : IWeeklyReviewService
    {

        public List<Entry> Entries { get; } = new List<Entry>();
        public List<Activity> Activities { get; } = new List<Activity>();
        public List<string> Socials { get; } = new List<string>();
        public List<Category> Categories { get; } = new List<Category>();

        public WeeklyReviewService()
        {
            GenerateData();
        }

        private void GenerateData()
        {
            GenerateCategoriesAndActivities();
            GenerateSocials();
            GenerateEntriesV2();
        }

        private void GenerateSocials()
        {
            Socials.AddRange(Activities.Where(x => x.Category.Name == "Social").ToList().ConvertAll(x => x.Name.Substring(8)));
            Socials.AddRange(Activities.Where(x => x.Category.Name == "Discord").ToList().ConvertAll(x => x.Name.Substring(9)).Where(x => !Socials.Contains(x)));
        }

        public void AddEntry(DateTime date, List<string> activities)
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

        private void GenerateEntriesV2()
        {
            var date = DateTime.Now;
            date = date.AddDays(-(int)date.DayOfWeek + 1);
            date = new DateTime(date.Year, date.Month, date.Day);
            date = date.AddDays(-7);
            date = date.AddHours(7).AddMinutes(30);

            for (int i = 0; i < 7; i++)
            {
                date = GenerateMonday(date);
            }

            for (int i = 0; i < (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Now.DayOfWeek); i++)
            {
                date = GenerateMonday(date);
            }
        }

        private DateTime GenerateMonday(DateTime date)
        {
            date = AddEntry(date, 30, "Food: Breakfast");
            date = AddEntry(date, 15, "Exercise: Bike");
            date = AddEntry(date, 60 * 7, "School: Classes");
            date = AddEntry(date, 15, "Exercise: Bike");
            date = AddEntry(date, 60, "School: Math");
            date = AddEntry(date, 45, "Exercise: Run");
            date = AddEntry(date, 75, "Social: Theodor Risager");
            date = AddEntry(date, 45, "Food: Dinner");
            date = AddEntry(date, 60, "Series: Salvation");
            date = AddEntry(date, 30, "Administration: Planning");
            date = AddEntry(date, 105, "Movie: Captin America The First Avengers");
            date = AddEntry(date, 60 * 9, "Sleep: Sleep");

            return date;
        }

        private DateTime AddEntry(DateTime start, int minutes, string activity)
        {
            var entry = new Entry();
            entry.StarTime = start;
            entry.EndTime = start.AddMinutes(minutes);
            entry.Activities.Add(Activities.First(x => x.Name == activity));

            Entries.Add(entry);
            return start.AddMinutes(minutes);
        }

        private void GenerateCategoriesAndActivities()
        {
            var cat = new Category("Exercise", 100, Color.DarkGreen);
            Categories.Add(cat);
            Activities.Add(new Activity("Bike", cat));
            Activities.Add(new Activity("Run", cat));
            Activities.Add(new Activity("Swimming", cat));

            cat = new Category("Transportation", 10, Color.DeepSkyBlue);
            Categories.Add(cat);
            Activities.Add(new Activity("Car", cat));
            Activities.Add(new Activity("Bus", cat));
            Activities.Add(new Activity("Train", cat));
            Activities.Add(new Activity("Airplane", cat));

            cat = new Category("Sleep", 1000, Color.RebeccaPurple);
            Categories.Add(cat);
            Activities.Add(new Activity("Sleep", cat));

            cat = new Category("School", 100, Color.HotPink);
            Categories.Add(cat);
            Activities.Add(new Activity("English", cat));
            Activities.Add(new Activity("Math", cat));
            Activities.Add(new Activity("Physics", cat));
            Activities.Add(new Activity("Programming", cat));
            Activities.Add(new Activity("Classes", cat));

            cat = new Category("Food", 100, Color.YellowGreen);
            Categories.Add(cat);
            Activities.Add(new Activity("Breakfast", cat));
            Activities.Add(new Activity("Lunch", cat));
            Activities.Add(new Activity("Dinner", cat));
            Activities.Add(new Activity("Snacking", cat));

            cat = new Category("Movie", 100, Color.Orange);
            Categories.Add(cat);
            Activities.Add(new Activity("21 Jump Street", cat));
            Activities.Add(new Activity("Captin America The First Avengers", cat));
            Activities.Add(new Activity("Edge of Tomorrow", cat));

            cat = new Category("Series", 100, Color.Orange);
            Categories.Add(cat);
            Activities.Add(new Activity("Black Sails", cat));
            Activities.Add(new Activity("Designated Survivor", cat));
            Activities.Add(new Activity("Salvation", cat));

            cat = new Category("Administration", 100000, Color.Green);
            Categories.Add(cat);
            Activities.Add(new Activity("Planning", cat));
            Activities.Add(new Activity("Reviewing Week", cat));
            Activities.Add(new Activity("Todolist", cat));

            cat = new Category("Social", 1000000, Color.Blue);
            Categories.Add(cat);
            Activities.Add(new Activity("Theodor Risager", cat));
            Activities.Add(new Activity("Kim Larsen", cat));
            Activities.Add(new Activity("Lene Sørensen", cat));
            Activities.Add(new Activity("John Doe", cat));
            Activities.Add(new Activity("Anders Andersen", cat));

            cat = new Category("Discord", 1000000, Color.Blue);
            Categories.Add(cat);
            Activities.Add(new Activity("Theodor Risager", cat));
            Activities.Add(new Activity("Lene Sørensen", cat));
            Activities.Add(new Activity("Kim Nielsen", cat));
            Activities.Add(new Activity("Kathrine Hansen", cat));
        }
    }
}
