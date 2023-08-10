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
    public class WeeklyReviewApiDbFixture
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        private DateTime _dt;
        public DateTime Dt => _dt;

        public WeeklyReviewDbContext CreateContext()
            => new WeeklyReviewApiDbContext(
            new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public WeeklyReviewApiDbFixture()
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
                            context.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void AddCaseSchool(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(6);
            var endTime = startTime.AddHours(1);

            var cSchool = new CategoryModel("School", 1, Color.Purple);
            var aMath = new ActivityModel("Math", false, cSchool);
            var aEnglish = new ActivityModel("English", false, cSchool);
            var aArts = new ActivityModel("Arts", false, cSchool);
            var aSpanish = new ActivityModel("Spanish", false, cSchool);
            var change = new ActivityChangeModel(aMath, aEnglish, endTime.AddHours(1));
            var e1 = new EntryModel(startTime, endTime, aMath, true);
            var e2 = new EntryModel(startTime, endTime, aEnglish, true);
            var e3 = new EntryModel(startTime, endTime, new List<ActivityModel>() { aArts, aSpanish }, true);
            var e4 = new EntryModel(startTime, endTime, aEnglish, false);

            context.Category.Add(cSchool);
            context.Activity.AddRange(aMath, aEnglish, aArts, aSpanish);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2, e3, e4);
        }

        private void AddCaseFoods(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(4);
            var endTime = startTime.AddHours(1);

            var cFood = new CategoryModel("Food", 1, Color.Yellow);
            var aBreakfast = new ActivityModel("Breakfast", false, cFood);
            var aLunch = new ActivityModel("Lunch", false, cFood);
            var aDinner = new ActivityModel("Dinner", false, cFood);
            var aSnack = new ActivityModel("Snack", false, cFood);
            var change = new ActivityChangeModel(aBreakfast, aLunch, endTime.AddHours(1));
            var e1 = new EntryModel(startTime, endTime, aBreakfast, true);
            var e2 = new EntryModel(startTime, endTime, aLunch, true);
            var e3 = new EntryModel(startTime, endTime, new List<ActivityModel>() { aDinner, aSnack }, false);

            context.Category.Add(cFood);
            context.Activity.AddRange(aBreakfast, aLunch, aDinner, aSnack);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2, e3);
        }

        private void AddCaseSports(WeeklyReviewDbContext context)
        {
            var startTime = _dt.AddHours(2);
            var endTime = startTime.AddHours(1);

            var cSports = new CategoryModel("Sport", 1, Color.DarkGreen);
            var aBike = new ActivityModel("Bike", false, cSports);
            var aSwim = new ActivityModel("Swim", false, cSports);
            var aRun = new ActivityModel("Run", false, cSports);
            var change = new ActivityChangeModel(aBike, aSwim, endTime.AddHours(1));
            var e1 = new EntryModel(startTime, endTime, aBike, true);
            var e2 = new EntryModel(startTime, endTime, aSwim, true);
            var e3 = new EntryModel(startTime, endTime, new List<ActivityModel>() { aSwim, aRun }, false);

            context.Category.Add(cSports);
            context.Activity.AddRange(aBike, aSwim, aRun);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2, e3);
        }

        private void AddCaseMovies(WeeklyReviewDbContext context)
        {
            var startTime = _dt;
            var endTime = startTime.AddHours(1);

            var cWatching = new CategoryModel("Watching", 1, Color.Orange);
            var aMovie = new ActivityModel("Movie", false, cWatching);
            var aSeries = new ActivityModel("Series", false, cWatching);
            var change = new ActivityChangeModel(aMovie, aSeries, endTime.AddHours(1));
            var e1 = new EntryModel(startTime, endTime, aMovie, true);
            var e2 = new EntryModel(startTime, endTime, aSeries, false);

            context.Category.Add(cWatching);
            context.Activity.AddRange(aMovie, aSeries);
            context.ActivityChange.Add(change);
            context.Entry.AddRange(e1, e2);
        }

    }
}
