using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Server.Persitance;

namespace WeeklyReview.Shared.Tests.DataContexts
{
    public class WeeklyReviewApiDbFixtureForEntryAdderService
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        private List<Guid> users = new()
        {
            Guid.NewGuid(), // 0 (Not Used)
            Guid.NewGuid(), // 1
            Guid.NewGuid(), // 2
            Guid.NewGuid(), // 3
            Guid.NewGuid(), // 4
            Guid.NewGuid(), // 5
        };
        public List<Guid> Users => users;

        private DateTime _dt;
        public DateTime Dt => _dt;
        public DateTime MaxTime => _dt.AddHours(6);

        public WeeklyReviewDbContext CreateContext()
            => new WeeklyReviewApiDbContext(
            new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public WeeklyReviewApiDbFixtureForEntryAdderService()
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
                            AddCaseMovies(context);
                            AddCaseFoods(context);
                            AddCaseSports(context);
                            AddCaseVisit(context);
                            AddCaseSchool(context);
                            context.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void AddCaseMovies(WeeklyReviewDbContext context)
        {
            var user = users[1];

            var aSeries = new ActivityModel("Series", false, user);
            var aMovie = new ActivityModel("Movie", false, user);

            context.Activity.AddRange(aMovie, aSeries);
        }

        private void AddCaseFoods(WeeklyReviewDbContext context)
        {
            var user = users[2];
            var startTime = _dt;
            var endTime = startTime.AddHours(4);

            var aBreakfast = new ActivityModel("Breakfast", false, user);
            var aLunch = new ActivityModel("Lunch", false, user);
            var aDinner = new ActivityModel("Dinner", false, user);
            var e1 = new EntryModel(startTime, endTime, startTime, aLunch, true, user);
            var e2 = new EntryModel(startTime, endTime, startTime, aBreakfast, false, user);
            var e3 = new EntryModel(startTime.AddHours(4), null, startTime, aLunch, false, user);

            context.Activity.AddRange(aBreakfast, aLunch, aDinner);
            context.Entry.AddRange(e1, e2, e3);
        }

        private void AddCaseSports(WeeklyReviewDbContext context)
        {
            var user = users[3];
            var startTime = _dt;
            var endTime = startTime.AddHours(4);

            var aRun = new ActivityModel("Run", false, user);
            var aBike = new ActivityModel("Bike", false, user);
            var aSwim = new ActivityModel("Swim", false, user);
            var e1 = new EntryModel(startTime, endTime, startTime, aRun, false, user);
            var e2 = new EntryModel(endTime, endTime.AddHours(2), startTime, aBike, false, user);

            context.Activity.AddRange(aRun, aBike, aSwim);
            context.Entry.AddRange(e1, e2);
        }

        private void AddCaseVisit(WeeklyReviewDbContext context)
        {
            var user = users[4];
            var startTime = _dt;
            var endTime = startTime.AddHours(4);

            var aParents = new ActivityModel("Parents", false, user);
            var aBrother = new ActivityModel("Brother", false, user);
            var aSister = new ActivityModel("Sister", false, user);
            var e1 = new EntryModel(startTime, endTime, startTime, aParents, false, user);
            var e2 = new EntryModel(endTime, endTime.AddHours(2), startTime, new List<ActivityModel>() { aBrother, aSister }, false, user);

            context.Activity.AddRange(aParents, aBrother, aSister);
            context.Entry.AddRange(e1, e2);
        }

        private void AddCaseSchool(WeeklyReviewDbContext context)
        {
            var user = users[5];
            var startTime = _dt;
            var endTime = startTime.AddHours(4);

            var aEnglish = new ActivityModel("English", false, user);
            var aDanish = new ActivityModel("Danish", false, user);
            var aMath = new ActivityModel("Math", false, user);
            var e1 = new EntryModel(startTime, endTime, startTime, aEnglish, false, user);
            var e2 = new EntryModel(endTime, endTime.AddHours(2), startTime, aDanish, false, user);

            context.Activity.AddRange(aEnglish, aDanish, aMath);
            context.Entry.AddRange(e1, e2);
        }

    }
}
