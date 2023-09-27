using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Server.Persitance;

namespace WeeklyReview.Shared.Tests.DataContexts
{
    public class WeeklyReviewApiDbFixtureForEntryParserService
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=WrTestEntryParser;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        private List<Guid> users = new()
        {
            Guid.NewGuid(), // 0 (Not Used)
            Guid.NewGuid(), // 1
            Guid.NewGuid(), // 2
            Guid.NewGuid(), // 3
            Guid.NewGuid(), // 4
        };
        public List<Guid> Users => users;

        private DateTime _dt;
        public DateTime Dt => _dt;
        public DateTime MaxTime => _dt.AddHours(10);

        public WeeklyReviewDbContext CreateContext()
            => new WeeklyReviewApiDbContext(
            new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public WeeklyReviewApiDbFixtureForEntryParserService()
        {
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
                            AddCaseUser1(context);
                            context.SaveChanges();
                            AddCaseUser2(context);
                            context.SaveChanges();
                            AddCaseUser3(context);
                            context.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private async void AddCaseUser1(WeeklyReviewDbContext context)
        {
            var user = users[1];
            var defaultCategory = new CategoryModel("", 0, Color.White, user);

            var cSports = new CategoryModel("Sports", 1, Color.Green, user);
            var cExercise = new CategoryModel("Exercise", 1, Color.Green, user);
            var cCompetitions = new CategoryModel("Difficult Competions", 1, Color.Green, user);
            var aSwim = new ActivityModel("Swim", false, defaultCategory, user);
            var aBike = new ActivityModel("Sports: Bike", false, cSports, user);
            var aDance = new ActivityModel("Dance", false, defaultCategory, user);
            var aRun = new ActivityModel("Exercise: Run", false, cExercise, user);
            var aRace = new ActivityModel("Difficult Competions: Participating in Race", false, cExercise, user);

            context.Category.AddRange(defaultCategory, cSports, cExercise, cCompetitions);
            context.Activity.AddRange(aSwim, aBike, aDance, aRun, aRace);
        }

        private void AddCaseUser2(WeeklyReviewDbContext context)
        {
            var user = users[2];
            var defaultCategory = new CategoryModel("", 0, Color.White, user);

            var cSports = new CategoryModel("Sports", 1, Color.Green, user);
            var aBike = new ActivityModel("Sports: Bike", false, defaultCategory, user);

            context.Category.AddRange(defaultCategory, cSports);
            context.Activity.AddRange(aBike);
        }

        private void AddCaseUser3(WeeklyReviewDbContext context)
        {
        }
    }
}
