using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Charts.Internal;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.HeatMap.Internal;
using System.Drawing;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Shared.Models.DTOs;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Client.Services
{
    public class DataService : IDataService
    {
        public List<EntryDto> Entries { get; } = new List<EntryDto>();
        public List<ActivityDto> Activities { get; } = new List<ActivityDto>();
        public List<string> Socials { get; } = new List<string>();
        public List<CategoryDto> Categories { get; } = new List<CategoryDto>();

        public DataService()
        {
            GenerateData();
        }

        public Task<IEnumerable<ActivityDto>> GetActivities() => Task.FromResult((IEnumerable<ActivityDto>)Activities);
        public Task<IEnumerable<CategoryDto>> GetCategories() => Task.FromResult((IEnumerable<CategoryDto>)Categories);
        public Task AddActivities(IEnumerable<ActivityDto> activities) => Task.Run(() => 
        {
            Activities.AddRange(activities);
            GenerateSocials();
        });
        public Task AddCategories(IEnumerable<CategoryDto> categories) => Task.Run(() => Categories.AddRange(categories));
        public Task<CategoryDto> GetDefaultCategory() => Task.FromResult(Categories.First());
        public Task<EntryDto?> GetBeforeEntry(DateTime date) => Task.FromResult(Entries.Where(x => x.StarTime < date).MaxBy(x => x.StarTime));
        public Task<EntryDto?> GetAfterEntry(DateTime date) => Task.FromResult(Entries.Where(x => x.StarTime > date).MinBy(x => x.StarTime));
        public Task<EntryDto?> GetEqualEntry(DateTime date) => Task.FromResult(Entries.FirstOrDefault(x => x.StarTime == date));
        public Task<EntryDto?> GetBeforeOrEqualEntry(DateTime date) => Task.FromResult(Entries.Where(x => x.StarTime <= date).MaxBy(x => x.StarTime));
        public Task AddEntry(EntryDto entry) => Task.Run(() => Entries.Add(entry));
        public Task RemoveEntry(EntryDto entry) => Task.Run(() => Entries.RemoveAll(x => x.Id == entry.Id));
        public Task<IEnumerable<EntryDto>> GetEntries() => Task.FromResult((IEnumerable<EntryDto>)Entries);
        public Task<IEnumerable<string>> GetSocials() => Task.FromResult((IEnumerable<string>)Socials);

        private void GenerateData()
        {
            GenerateCategoriesAndActivities();
            GenerateSocials();
            GenerateEntriesV2();
        }

        private void GenerateSocials()
        {
            Socials.Clear();
            Socials.AddRange(Activities.Where(x => x.Category.Name == "Social").ToList().ConvertAll(x => x.Name.Substring(8)));
            Socials.AddRange(Activities.Where(x => x.Category.Name == "Discord").ToList().ConvertAll(x => x.Name.Substring(9)).Where(x => !Socials.Contains(x)));
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
            date = AddEntry(date, 60 * 9, "Sleep");

            return date;
        }

        private DateTime AddEntry(DateTime start, int minutes, string activity)
        {
            var entry = new EntryDto();
            entry.StarTime = start;
            entry.EndTime = start.AddMinutes(minutes);
            entry.Activities.Add(Activities.First(x => x.Name == activity));

            Entries.Add(entry);
            return start.AddMinutes(minutes);
        }

        private void GenerateCategoriesAndActivities()
        {
            var cat = new CategoryDto("", 0, Color.WhiteSmoke);
            Categories.Add(cat);

            cat = new CategoryDto("Exercise", 100, Color.DarkGreen);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Bike", cat));
            Activities.Add(new ActivityDto("Run", cat));
            Activities.Add(new ActivityDto("Swimming", cat));

            cat = new CategoryDto("Transportation", 10, Color.DeepSkyBlue);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Car", cat));
            Activities.Add(new ActivityDto("Bus", cat));
            Activities.Add(new ActivityDto("Train", cat));
            Activities.Add(new ActivityDto("Airplane", cat));

            cat = new CategoryDto("Sleep", 1000, Color.RebeccaPurple);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Sleep", cat, false));

            cat = new CategoryDto("School", 100, Color.HotPink);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("English", cat));
            Activities.Add(new ActivityDto("Math", cat));
            Activities.Add(new ActivityDto("Physics", cat));
            Activities.Add(new ActivityDto("Programming", cat));
            Activities.Add(new ActivityDto("Classes", cat));

            cat = new CategoryDto("Food", 100, Color.YellowGreen);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Breakfast", cat));
            Activities.Add(new ActivityDto("Lunch", cat));
            Activities.Add(new ActivityDto("Dinner", cat));
            Activities.Add(new ActivityDto("Snacking", cat));

            cat = new CategoryDto("Movie", 100, Color.Orange);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("21 Jump Street", cat));
            Activities.Add(new ActivityDto("Captin America The First Avengers", cat));
            Activities.Add(new ActivityDto("Edge of Tomorrow", cat));

            cat = new CategoryDto("Series", 100, Color.Orange);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Black Sails", cat));
            Activities.Add(new ActivityDto("Designated Survivor", cat));
            Activities.Add(new ActivityDto("Salvation", cat));

            cat = new CategoryDto("Administration", 100000, Color.Green);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Planning", cat));
            Activities.Add(new ActivityDto("Reviewing Week", cat));
            Activities.Add(new ActivityDto("Todolist", cat));

            cat = new CategoryDto("Social", 1000000, Color.Blue);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Theodor Risager", cat));
            Activities.Add(new ActivityDto("Kim Larsen", cat));
            Activities.Add(new ActivityDto("Lene Sørensen", cat));
            Activities.Add(new ActivityDto("John Doe", cat));
            Activities.Add(new ActivityDto("Anders Andersen", cat));

            cat = new CategoryDto("Discord", 1000000, Color.Blue);
            Categories.Add(cat);
            Activities.Add(new ActivityDto("Theodor Risager", cat));
            Activities.Add(new ActivityDto("Lene Sørensen", cat));
            Activities.Add(new ActivityDto("Kim Nielsen", cat));
            Activities.Add(new ActivityDto("Kathrine Hansen", cat));
        }
    }
}
