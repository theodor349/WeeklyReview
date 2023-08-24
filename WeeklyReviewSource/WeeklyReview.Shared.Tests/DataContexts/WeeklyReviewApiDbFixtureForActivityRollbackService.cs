using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.CodeCoverage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Server.Persitance;

namespace WeeklyReview.Shared.Tests.DataContexts
{
    public class WeeklyReviewApiDbFixtureForActivityRollbackService
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        private Guid user1 = Guid.NewGuid();
        public Guid User1 => user1;

        private DateTime _dt;
        public DateTime Dt => _dt;
        public DateTime MaxTime => _dt.AddHours(10);

        public WeeklyReviewDbContext CreateContext()
            => new WeeklyReviewApiDbContext(
            new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public WeeklyReviewApiDbFixtureForActivityRollbackService()
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
                            AddCaseSports(context);
                            AddCaseFoods(context);
                            AddCaseSchool(context);
                            AddCaseTrip(context);
                            context.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void AddCaseTrip(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(8);
            var endTime = startTime.AddHours(1);

            var aSpain = new ActivityModel("Spain", false, User1);
            var aFrance = new ActivityModel("France", false, User1);
            var change = new ActivityChangeModel(aSpain, aFrance, endTime.AddHours(1), User1);
            var e1 = new EntryModel(startTime, endTime, startTime.AddMinutes(1), aSpain, true, User1);
            var e2 = new EntryModel(startTime, endTime, startTime.AddMinutes(2), aFrance, true, User1);

            context.Activity.AddRange(aSpain, aFrance);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2);
        }

        private void AddCaseSchool(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(6);
            var endTime = startTime.AddHours(1);

            var cSchool = new CategoryModel("School", 1, Color.Purple, User1);
            var aMath = new ActivityModel("Math", false, cSchool, User1);
            var aEnglish = new ActivityModel("English", false, cSchool, User1);
            var aArts = new ActivityModel("Arts", false, cSchool, User1);
            var aSpanish = new ActivityModel("Spanish", false, cSchool, User1);
            var change = new ActivityChangeModel(aMath, aEnglish, endTime.AddHours(1), User1);
            var e1 = new EntryModel(startTime, endTime, startTime.AddMinutes(1), aMath, true, User1);
            var e2 = new EntryModel(startTime, endTime, startTime.AddMinutes(2), aEnglish, true, User1);
            var e3 = new EntryModel(startTime, endTime, startTime.AddMinutes(3), new List<ActivityModel>() { aArts, aSpanish }, true, User1);
            var e4 = new EntryModel(startTime, endTime, startTime.AddMinutes(4), aEnglish, false, User1);

            context.Category.Add(cSchool);
            context.Activity.AddRange(aMath, aEnglish, aArts, aSpanish);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2, e3, e4);
        }

        private void AddCaseFoods(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(4);
            var endTime = startTime.AddHours(1);

            var cFood = new CategoryModel("Food", 1, Color.Yellow, User1);
            var aBreakfast = new ActivityModel("Breakfast", false, cFood, User1);
            var aLunch = new ActivityModel("Lunch", false, cFood, User1);
            var aDinner = new ActivityModel("Dinner", false, cFood, User1);
            var aSnack = new ActivityModel("Snack", false, cFood, User1);
            var change = new ActivityChangeModel(aBreakfast, aLunch, endTime.AddHours(1), User1);
            var e1 = new EntryModel(startTime, endTime, startTime.AddMinutes(1), aBreakfast, true, User1);
            var e2 = new EntryModel(startTime, endTime, startTime.AddMinutes(2), aLunch, true, User1);
            var e3 = new EntryModel(startTime, endTime, startTime.AddMinutes(3), new List<ActivityModel>() { aDinner, aSnack }, false, User1);

            context.Category.Add(cFood);
            context.Activity.AddRange(aBreakfast, aLunch, aDinner, aSnack);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2, e3);
        }

        private void AddCaseSports(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(2);
            var endTime = startTime.AddHours(1);

            var cSports = new CategoryModel("Sport", 1, Color.DarkGreen, User1);
            var aBike = new ActivityModel("Bike", false, cSports, User1);
            var aSwim = new ActivityModel("Swim", false, cSports, User1);
            var aRun = new ActivityModel("Run", false, cSports, User1);
            var change = new ActivityChangeModel(aBike, aSwim, endTime.AddHours(1), User1);
            var e1 = new EntryModel(startTime, endTime, startTime.AddMinutes(1), aBike, true, User1);
            var e2 = new EntryModel(startTime, endTime, startTime.AddMinutes(2), aSwim, true, User1);
            var e3 = new EntryModel(startTime, endTime, startTime.AddMinutes(3), new List<ActivityModel>() { aSwim, aRun }, false, User1);

            context.Category.Add(cSports);
            context.Activity.AddRange(aBike, aSwim, aRun);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2, e3);
        }

        private void AddCaseMovies(WeeklyReviewDbContext context)
        {
            var startTime = _dt;
            var endTime = startTime.AddHours(1);

            var cWatching = new CategoryModel("Watching", 1, Color.Orange, User1);
            var aMovie = new ActivityModel("Movie", true, cWatching, User1);
            var aSeries = new ActivityModel("Series", false, cWatching, User1);
            var change = new ActivityChangeModel(aMovie, aSeries, endTime.AddHours(1), User1);
            var e1 = new EntryModel(startTime, endTime, startTime.AddMinutes(1), aMovie, true, User1);
            var e2 = new EntryModel(startTime, endTime, startTime.AddMinutes(2), aSeries, false, User1);

            context.Category.Add(cWatching);
            context.Activity.AddRange(aMovie, aSeries);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2);
        }
    }
}
