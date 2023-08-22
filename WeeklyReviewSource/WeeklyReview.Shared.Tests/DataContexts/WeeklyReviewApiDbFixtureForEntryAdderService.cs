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

        private Guid user1 = Guid.NewGuid();
        private Guid user2 = Guid.NewGuid();
        public Guid User1 => user1;
        public Guid User2 => user2;

        private DateTime _dt;
        public DateTime Dt => _dt;
        public DateTime MaxTime => _dt.AddHours(10);

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
