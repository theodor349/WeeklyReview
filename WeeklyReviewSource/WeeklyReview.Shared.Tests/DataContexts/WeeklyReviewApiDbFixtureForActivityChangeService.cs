using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Server.Persitance;

namespace WeeklyReview.Shared.Tests.DataContexts
{
    public class WeeklyReviewApiDbFixtureForActivityChangeService
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=WrTestActivityChange;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        private Guid user1 = Guid.NewGuid();
        public Guid User1 => user1;
        private CategoryModel _defaultCategory;

        private DateTime _dt;
        public DateTime Dt => _dt;
        public DateTime MaxTime => _dt.AddHours(10);

        public WeeklyReviewDbContext CreateContext()
            => new WeeklyReviewApiDbContext(
            new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public WeeklyReviewApiDbFixtureForActivityChangeService()
        {
            _defaultCategory = new CategoryModel("", 0, Color.White, User1);
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        using (var transaction = context.Database.BeginTransaction())
                        {
                            context.Category.Add(_defaultCategory);
                            context.SaveChanges();
                            AddCaseFoods(context);
                            context.SaveChanges();
                            AddCaseSports(context);
                            context.SaveChanges();
                            AddCaseTravel(context);
                            context.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void AddCaseFoods(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(0);
            var endTime = startTime.AddHours(1);

            var aLunch = new ActivityModel("Lunch", false, _defaultCategory, User1);
            var aDinner = new ActivityModel("Dinner", false, _defaultCategory, User1);
            var e1 = new EntryModel(startTime.AddHours(0), endTime.AddHours(0), startTime.AddHours(0).AddMinutes(1), aLunch, true, User1);

            context.Activity.AddRange(aLunch, aDinner);
            context.Entry.AddRange(e1);
        }

        private void AddCaseSports(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(1);
            var endTime = startTime.AddHours(1);

            var aRun = new ActivityModel("Run", false, _defaultCategory, User1);
            var aBike = new ActivityModel("Bike", false, _defaultCategory, User1);
            var e1 = new EntryModel(startTime.AddHours(0), endTime.AddHours(0), startTime.AddHours(0).AddMinutes(1), aRun, false, User1);
            var e2 = new EntryModel(startTime.AddHours(2), endTime.AddHours(2), startTime.AddHours(2).AddMinutes(1), aRun, false, User1);

            context.Activity.AddRange(aRun, aBike);
            context.Entry.AddRange(e1, e2);
        }

        private void AddCaseTravel(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(6);
            var endTime = startTime.AddHours(1);

            var aSpain = new ActivityModel("Spain", false, _defaultCategory, User1);
            var aItaly = new ActivityModel("Italy", false, _defaultCategory, User1);
            var afrance = new ActivityModel("France", false, _defaultCategory, User1);
            var e1 = new EntryModel(startTime, endTime, startTime.AddMinutes(1), new List<ActivityModel>() { aItaly, aSpain }, false, User1);

            context.Activity.AddRange(aSpain, aItaly, afrance);
            context.Entry.AddRange(e1);
        }
    }
}
