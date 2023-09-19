using System.Diagnostics;
using System.Drawing;
using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Models;
using WeeklyReview.Shared.Models.DTOs;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Client.Services
{
    public interface IClientWeeklyReviewService : IWeeklyReviewService { }
    public interface IApiWeeklyReviewService : IWeeklyReviewService { }

    public class ClientWeeklyReviewService : IClientWeeklyReviewService
    {
        private readonly IWeeklyReviewService _inMemmoryWeeklyReviewService;
        private readonly IApiWeeklyReviewService _apiWeeklyReviewService;
        private bool _isLoggedIn => true;

        public IActivityChangeService ActivityChange => _isLoggedIn ? _apiWeeklyReviewService.ActivityChange : _inMemmoryWeeklyReviewService.ActivityChange;

        public IActivityService Activity => _isLoggedIn ? _apiWeeklyReviewService.Activity : _inMemmoryWeeklyReviewService.Activity;

        public ICategoryService Category => _isLoggedIn ? _apiWeeklyReviewService.Category : _inMemmoryWeeklyReviewService.Category;

        public IEntryService Entry => _isLoggedIn ? _apiWeeklyReviewService.Entry : _inMemmoryWeeklyReviewService.Entry;

        public ClientWeeklyReviewService(IApiWeeklyReviewService apiWeeklyReviewService)
        {
            _apiWeeklyReviewService = apiWeeklyReviewService;

            //if (_inMemmoryWeeklyReviewService.Category.GetAll(UserGuid).Count() == 0)
            //    LoadPlageholderData();
        }

        #region Local Setup
        public Guid UserGuid { get; set; } = new Guid("24fe9480-4e7a-4515-b96c-248171496591");
        private List<string> movies;
        private List<string> series;
        private List<string> foods;
        private List<string> schools;
        private List<string> weeklyReviews;
        private List<string> sports;
        private string sleep;
        private string bath;
        private List<string> calls;
        private Random _random = new Random();

        private void LoadPlageholderData()
        {
            LoadEntryExamples();
            CreateEntries();
            ChangeCategoryColors();
        }

        private void ChangeCategoryColors()
        {
            var cats = _inMemmoryWeeklyReviewService.Category.GetAll(UserGuid);
            foreach (var cat in cats)
            {
                switch (cat.NormalizedName) 
                {
                    case "sleep":
                        ChangeColor(cat, Color.Purple);
                        break;
                    case "bath":
                        ChangeColor(cat, Color.Yellow);
                        break;
                    case "movie":
                        ChangeColor(cat, Color.Orange);
                        break;
                    case "series":
                        ChangeColor(cat, Color.Orange);
                        break;
                    case "food":
                        ChangeColor(cat, Color.Yellow);
                        break;
                    case "school":
                        ChangeColor(cat, Color.Pink);
                        break;
                    case "weekly review":
                        ChangeColor(cat, Color.Green);
                        break;
                    case "sport":
                        ChangeColor(cat, Color.DarkGreen);
                        break;
                    case "call":
                        ChangeColor(cat, Color.LightBlue);
                        break;
                }
            }
        }

        private void ChangeColor(CategoryModel cat, Color color)
        {
            _inMemmoryWeeklyReviewService.Category.ChangeColor(cat.Id, color, UserGuid);
        }

        private void CreateEntries()
        {
            var date = DateTime.Now;
            date = date.AddDays(-(int)date.DayOfWeek + 1);
            date = new DateTime(date.Year, date.Month, date.Day);
            date = date.AddDays(-7);
            date = date.AddHours(7).AddMinutes(30);

            date = GenerateRandomDate(date);
            //for (int i = 0; i < 7; i++)
            //{
            //    date = GenerateRandomDate(date);
            //}

            //for (int i = 0; i < (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Now.DayOfWeek); i++)
            //{
            //    date = GenerateRandomDate(date);
            //}
        }

        private void LoadEntryExamples()
        {
            sleep = "Sleep: Sleep";
            bath = "Bath: Shower";

            movies = new List<string>()
            {
                "Movie: Avengers",
                "Movie: Avatar",
                "Movie: Avatar the way of Water",
                "Movie: Oppenheimer",
                "Movie: Barbie",
            };

            series = new List<string>()
            {
                "Series: Casa de Papel",
                "Series: The Witcher",
                "Series: Fubar",
                "Series: Peaky Blinders",
            };

            foods = new List<string>()
            {
                "Food: Breakfast",
                "Food: Lunch",
                "Food: Dinner",
            };

            schools = new List<string>()
            {
                "School: Math",
                "School: Danish",
                "School: Syntax and Semantics",
            };

            weeklyReviews = new List<string>()
            {
                "Weekly Review: Frontend",
                "Weekly Review: Backend",
                "Weekly Review: Hosting",
            };

            sports = new List<string>()
            {
                "Sport: Bike",
                "Sport: Bachata",
                "Sport: Salsa",
            };

            calls = new List<string>()
            {
                "Call: Mom",
                "Call: Dad",
            };
        }

        private DateTime GenerateRandomDate(DateTime date)
        {
            date = AddEntry(foods[0], date, 30);
            date = AddEntry(sports[0], date, 15);
            var entries = _inMemmoryWeeklyReviewService.Entry.GetAll(UserGuid);
            date = AddEntry(RandomEntry(schools), date, 60 * 7);
            date = AddEntry(sports[0], date, 15);
            date = AddEntry(RandomEntry(schools), date, 45);
            date = AddEntry(RandomEntry(sports), date, 45 );
            date = AddEntry(bath, date, 15);
            date = AddEntry(RandomEntry(calls), date, 75);
            date = AddEntry(foods[1], date, 45);
            date = AddEntry(RandomEntry(series), date, 60);
            date = AddEntry(RandomEntry(weeklyReviews), date, 30);
            date = AddEntry(RandomEntry(movies), date, 105);
            date = AddEntry(sleep, date, 60 * 9);

            return date;
        }

        private string RandomEntry(List<string> list)
        {
            var index = _random.Next(list.Count);
            return list[index];
        }

        private DateTime AddEntry(List<string> entries, DateTime date, int minuteOffset)
        {
            _inMemmoryWeeklyReviewService.Entry.Create(new EnterEntryModel()
            {
                Date = date,
                Entries = entries
            },
            UserGuid);
            return date.AddMinutes(minuteOffset);
        }

        private DateTime AddEntry(string entry, DateTime date, int minuteOffset)
        {
            return AddEntry(new List<string>() { entry }, date, minuteOffset);
        }
        #endregion
    }
}
